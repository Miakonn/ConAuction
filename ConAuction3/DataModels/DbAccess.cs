using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ConAuction3.DataModels {
    internal class DbAccess {
        private readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];

        private MySqlConnection DbConnection;

        private readonly string Year = ConfigurationManager.AppSettings["Year"];

        private static DbAccess _instance;

        public static DbAccess Instance => _instance ?? (_instance = new DbAccess());

        public bool InitDb() {
            try {
                // Initialize mysql connection
                DbConnection = new MySqlConnection(ConnectionString);
                DbConnection.Open();
                var fOk = DbConnection.Ping();
                if (!fOk) {
                    MessageBox.Show("Cannot connect to server.");
                    return false;
                }
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        #region Customer
        public List<Customer> ReadAllCustomers() {
            var customers = new List<Customer>(500);
            var query = "select id,name,phone,comment,finished,swish,shortname,timestamp from Customer;";
            MySqlCommand cmdDatabase = new MySqlCommand(query, DbConnection);

            try {
                using (MySqlDataReader reader = cmdDatabase.ExecuteReader()) {
                    while (reader.Read()) {
                        var customer = new Customer {
                            Id = reader.GetInt32("id"),
                            Name = reader.GetStringOrDefault("name"),
                            ShortName = reader.GetStringOrDefault("shortname"),
                            Phone = reader.GetStringOrDefault("phone"),
                            Comment = reader.GetStringOrDefault("comment"),
                            Swish = reader.GetBooleanOrDefault("swish"),
                            Finished = reader.GetBooleanOrDefault("finished")
                        };
                        customers.Add(customer);
                    }
                    return customers;
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error: \r\n" + ex);
                return new List<Customer>();
            }
        }

        public int InsertNewCustomerToDb(Customer customer) {
            var sqlTran = DbConnection.BeginTransaction();

            var command = DbConnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DbConnection;
            try {
                // Set the INSERT command and parameter.
                command.CommandText =
                    "INSERT INTO bid (Name, Phone, Comment, Swish, Date, TimeStamp) VALUES (@Name,@ShortName,@Phone,@Comment,@Swish,Now(),Now());";
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@ShortName", customer.ShortName);
                command.Parameters.AddWithValue("@Phone", customer.Phone);
                command.Parameters.AddWithValue("@Comment", customer.Comment);
                command.Parameters.AddWithValue("@Swish", customer.Swish);

                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();
                // If has last inserted id, add a parameter to hold it.
                if (command.LastInsertedId != 0L) {
                    command.Parameters.Add(new MySqlParameter("newId", command.LastInsertedId));
                }

                // Commit the transaction.
                sqlTran.Commit();
                return Convert.ToInt32(command.Parameters["@newId"].Value);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);

                try {
                    sqlTran.Rollback();
                }
                catch (Exception exRollback) {
                    MessageBox.Show(exRollback.Message);
                }

                return 0;
            }
        }

        public void SaveCustomerToDb(Customer customer) {
            customer.Phone = customer.Phone.Replace(" ", "");
            var sqlTran = DbConnection.BeginTransaction();

            var command = DbConnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DbConnection;
            try {
                command.CommandText = "UPDATE Customer SET Name=@Name, ShortName=@ShortName, Phone=@Phone, Comment=@Comment, Date=Now(), Finished=@Finished, Swish=@Swish, Timestamp=Now() WHERE id=@id;";
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@ShortName", customer.ShortName);
                command.Parameters.AddWithValue("@Phone", customer.Phone);
                command.Parameters.AddWithValue("@Comment", customer.Comment);
                command.Parameters.AddWithValue("@Finished", customer.Finished);
                command.Parameters.AddWithValue("@Swish", customer.Swish);
                command.Parameters.AddWithValue("@id", customer.Id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                // Commit the transaction.
                sqlTran.Commit();
                Trace.WriteLine("Update Customer : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                try {
                    sqlTran.Rollback();
                }
                catch (Exception exRollback) {
                    MessageBox.Show(exRollback.Message);
                }
            }
        }

        public void DeleteCustomerFromDb(int id) {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            try {
                command.CommandText = "DELETE FROM Customer WHERE Id=@id;";
                command.Parameters.AddWithValue("@id", id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Delete Customer : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Product
        public List<Product> ReadAllProducts() {
            var products = new List<Product>(900);
            var query =
                "SELECT Id, Label, Name, Type, Description, Note, Price, CustomerId, TimeStamp, FixedPrice, LabelPrinted, PartsNo, Buyer FROM Product WHERE year=" +
                Year;
            var cmdDatabase = new MySqlCommand(query, DbConnection);
            try {
                using (var reader = cmdDatabase.ExecuteReader()) {
                    while (reader.Read()) {
                        var product = new Product {
                            Id = reader.GetInt64("Id"),
                            Label = reader.GetIntOrDefault("Label"),
                            Name = reader.GetStringOrDefault("Name"),
                            Type = reader.GetStringOrDefault("Type"),
                            Description = reader.GetStringOrDefault("Description"),
                            Note = reader.GetStringOrDefault("Note"),
                            Price = reader.GetIntOrDefault("Price"),
                            FixedPrice = reader.GetIntOrDefault("FixedPrice"),
                            CustomerId = reader.GetInt32("CustomerId"),
                            LabelPrinted = reader.GetBoolean("LabelPrinted"),
                            PartsNo = reader.GetInt32("PartsNo"),
                            Buyer = reader.GetStringOrDefault("Buyer"),
                        };
                        products.Add(product);
                    }
                    return products;
                }
            }

            catch (Exception ex) {
                MessageBox.Show("Error: \r\n" + ex);
                return new List<Product>();
            }
        }
        
        public long InsertNewProductToDb(Product prod) {
            var sqlTran = DbConnection.BeginTransaction();

            var command = DbConnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DbConnection;
            try {
                command.CommandText =  $"select count(*) from Product where Label={prod.Label} and Year={Year}";
                var result = (long)command.ExecuteScalar();
                if (result > 0L) {
                    throw new Exception("Objektnumret redan använt!");
                }
                
                command.CommandText =
                    "INSERT INTO Product (Label, Name, Description,Type,  Note, Price, FixedPrice, CustomerId, LabelPrinted, PartsNo, Buyer, Year, timestamp)" +
                    "VALUES (@Label, @Name, @Description, @Type, @Note,@price, @Fixedprice, @CustomerId , @LabelPrinted, @PartsNo, @Year, Now());";
                command.Parameters.AddWithValue("@Label", prod.Label);
                command.Parameters.AddWithValue("@Name", prod.Name);
                command.Parameters.AddWithValue("@Description", prod.Description);
                command.Parameters.AddWithValue("@Price", prod.Price);
                command.Parameters.AddWithValue("@FixedPrice", prod.FixedPrice);
                command.Parameters.AddWithValue("@Type", prod.Type);
                command.Parameters.AddWithValue("@Note", prod.Note);
                command.Parameters.AddWithValue("@CustomerId", prod.CustomerId);
                command.Parameters.AddWithValue("@LabelPrinted", prod.LabelPrinted);
                command.Parameters.AddWithValue("@PartsNo", prod.PartsNo);
                command.Parameters.AddWithValue("@Year", Year);

                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                var idCreated = command.LastInsertedId;

                // Commit the transaction.
                sqlTran.Commit();

                return idCreated;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);

                try {
                    sqlTran.Rollback();

                }
                catch (Exception exRollback) {
                    MessageBox.Show(exRollback.Message);
                }
            }
            return 0;
        }

        public void SaveProductToDb(Product prod) {
            var sqlTran = DbConnection.BeginTransaction();

            var command = DbConnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DbConnection;
            try {
                command.CommandText =
                    "UPDATE Product SET Name=@Name, Description=@Description, Type=@Type, Note=@Note, FixedPrice=@FixedPrice, LabelPrinted=@LabelPrinted, PartsNo=@PartsNo, Timestamp=Now() WHERE id=@id;";
                command.Parameters.AddWithValue("@Name", prod.Name);
                command.Parameters.AddWithValue("@Description", prod.Description);
                command.Parameters.AddWithValue("@FixedPrice", prod.FixedPrice);
                command.Parameters.AddWithValue("@Type", prod.Type);
                command.Parameters.AddWithValue("@Note", prod.Note);
                command.Parameters.AddWithValue("@LabelPrinted", prod.LabelPrinted);
                command.Parameters.AddWithValue("@PartsNo", prod.PartsNo);
                command.Parameters.AddWithValue("@id", prod.Id);

                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                // Commit the transaction.
                sqlTran.Commit();
                Trace.WriteLine("Update Product : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                try {
                    sqlTran.Rollback();
                }
                catch (Exception exRollback) {
                    MessageBox.Show(exRollback.Message);
                }
            }
        }

        public void SaveProductPriceToDb(long id, int price) {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            try {
                command.CommandText = "UPDATE Product SET Price=@Price, Timestamp=Now() WHERE id=@id;";
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@id", id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Update Price : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveProductBuyerToDb(long id, string buyer) {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            try {
                command.CommandText = "UPDATE Product SET Buyer=@Buyer, Timestamp=Now() WHERE id=@id;";
                command.Parameters.AddWithValue("@Buyer", buyer);
                command.Parameters.AddWithValue("@id", id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Update Buyer : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveProductPrintedToDb(Product product) {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            try {
                command.CommandText = "UPDATE Product SET LabelPrinted=@LabelPrinted, Timestamp=Now() WHERE id=@id;";
                command.Parameters.AddWithValue("@LabelPrinted", product.LabelPrinted);
                command.Parameters.AddWithValue("@id", product.Id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Update LabelPrinted : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveProductWithNewLabelToDb(Product product) {
            var sqlTran = DbConnection.BeginTransaction();

            var command = DbConnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DbConnection;
            try {
                command.CommandText = $"select count(*) from Product where Label={product.Label} and Year={Year}";
                var result = (long)command.ExecuteScalar();
                if (result > 0L) {
                    throw new Exception("Objektnumret redan använt!");
                }
                command.CommandText =
                    "UPDATE Product SET Name=@Name, Label=@Label, Description=@Description, Type=@Type, Note=@Note, FixedPrice=@FixedPrice, LabelPrinted=@LabelPrinted, PartsNo=@PartsNo, Timestamp=Now() WHERE id=@id;";
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@Label", product.Label);
                command.Parameters.AddWithValue("@Description", product.Description);
                command.Parameters.AddWithValue("@FixedPrice", product.FixedPrice);
                command.Parameters.AddWithValue("@Type", product.Type);
                command.Parameters.AddWithValue("@Note", product.Note);
                command.Parameters.AddWithValue("@LabelPrinted", product.LabelPrinted);
                command.Parameters.AddWithValue("@PartsNo", product.PartsNo);
                command.Parameters.AddWithValue("@id", product.Id);

                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                // Commit the transaction.
                sqlTran.Commit();
                
                Trace.WriteLine("Updated with new label! : SUCCESS!");
            }

            catch (Exception ex) {
                MessageBox.Show(ex.Message);

                try {
                    sqlTran.Rollback();

                }
                catch (Exception exRollback) {
                    MessageBox.Show(exRollback.Message);
                }
            }
        }

        public void DeleteProductFromDb(long id) {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            try {
                command.CommandText = "DELETE FROM Product WHERE Id=@id;";
                command.Parameters.AddWithValue("@id", id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Delete Product : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Bid
        public void InsertNewBidToDb(Bid bid) {
            var sqlTran = DbConnection.BeginTransaction();

            try {
                var command = DbConnection.CreateCommand();
                command.Transaction = sqlTran;
                command.Connection = DbConnection;

                // Set the INSERT command and parameter.
                command.CommandText =
                    "INSERT INTO bid (CustomerId, MaxBid, ProductId, Year) VALUES (@CustomerId,@MaxBid,@ProductId,@Year);";
                command.Parameters.AddWithValue("@CustomerId", bid.CustomerId);
                command.Parameters.AddWithValue("@MaxBid", bid.MaxBid);
                command.Parameters.AddWithValue("@ProductId", bid.ProductId);
                command.Parameters.AddWithValue("@Year", Year);

                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();
                sqlTran.Commit();

                Trace.WriteLine("Insert Bid : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                try {
                    sqlTran.Rollback();
                }
                catch (Exception exRollback) {
                    MessageBox.Show(exRollback.Message);
                }
            }
        }

        public void SaveBidToDb(Bid bid) {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            try {
                command.CommandText = "UPDATE Bid SET MaxBid=@MaxBid, ProductId=@ProductId WHERE id=@id;";
                command.Parameters.AddWithValue("@MaxBid", bid.MaxBid);
                command.Parameters.AddWithValue("@ProductId", bid.ProductId);
                command.Parameters.AddWithValue("@id", bid.Id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Update Bid : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteBidFromDb(int id) {
            var command = DbConnection.CreateCommand();
            command.Connection = DbConnection;
            try {
                command.CommandText = "Delete from Bid Where Id=@id;";
                command.Parameters.AddWithValue("@id", id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Delete Bid : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Bid> ReadAllBids() {
            var bids = new List<Bid>(200);
            var query =
                "select Id, CustomerId, MaxBid, ProductId from Bid where year=" + Year;
            var cmdDatabase = new MySqlCommand(query, DbConnection);
            try {
                using (var reader = cmdDatabase.ExecuteReader()) {
                    while (reader.Read()) {
                        var product = new Bid {
                            Id = reader.GetInt32("Id"),
                            CustomerId = reader.GetInt32("CustomerId"),
                            ProductId = reader.GetInt32("ProductId"),
                            MaxBid = reader.GetIntOrDefault("MaxBid"),
                        };
                        bids.Add(product);
                    }
                    return bids;
                }
            }

            catch (Exception ex) {
                MessageBox.Show("Error: \r\n" + ex);
                return new List<Bid>();
            }
        }
        #endregion
    }

    public static class DbReaderExtensions {
        public static string GetStringOrDefault(this MySqlDataReader reader, string name) {
            var ordinal = reader.GetOrdinal(name);
            return reader.IsDBNull(ordinal) ? "" : reader.GetString(ordinal);
        }

        public static bool? GetBooleanOrDefault(this MySqlDataReader reader, string name) {
            var ordinal = reader.GetOrdinal(name);
            return reader.IsDBNull(ordinal) ? (bool?) null : reader.GetBoolean(ordinal);
        }

        public static int GetIntOrDefault(this MySqlDataReader reader, string name) {
            var ordinal = reader.GetOrdinal(name);
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
        }
    }

}
