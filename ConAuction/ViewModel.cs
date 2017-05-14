using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ConAuction
{
    internal class ViewModel
    {
        private readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        public DataTable DataTableCustomer;
        public DataTable DataTableProduct;

        private MySqlConnection DBconnection;
        private MySqlDataAdapter DBadapterCustomer;
        private MySqlDataAdapter DBadapterProduct;

        public bool fDataGridCustomerIsChanged;
        public bool fUpdatingCustomerList;
        public bool fUpdatingProductList;

        public bool InitDB()
        {
            try {
                //Initialize mysql connection
                DBconnection = new MySqlConnection(ConnectionString);
                DBconnection.Open();
                var fOk = DBconnection.Ping();
                if (!fOk) {
                    MessageBox.Show("Cannot connect to server.");
                    return false;
                }
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public void UpdateCustomerFromDB()
        {
            try {
                //prepare adapter to run query
                var query = "select id,name,phone,comment,finished,timestamp from Customer;";

                DBadapterCustomer = new MySqlDataAdapter(query, DBconnection);
                var DBDataSetCustomer = new DataSet();
                //get query results in dataset
                DBadapterCustomer.Fill(DBDataSetCustomer);

                DBDataSetCustomer.Tables[0].TableName = "Customer";
                DataTableCustomer = DBDataSetCustomer.Tables[0];

                // Set the UPDATE command and parameters.
                DBadapterCustomer.UpdateCommand = new MySqlCommand(
                    "UPDATE customer SET Name=@Name, Phone=@Phone, Comment=@Comment, Date=NOW(), Finished=@Finished, Timestamp=Now() WHERE id=@id and timestamp=@Timestamp;",
                    DBconnection);
                DBadapterCustomer.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
                DBadapterCustomer.UpdateCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 30, "Name");
                DBadapterCustomer.UpdateCommand.Parameters.Add("@Phone", MySqlDbType.VarChar, 15, "Phone");
                DBadapterCustomer.UpdateCommand.Parameters.Add("@Comment", MySqlDbType.VarChar, 100, "Comment");
                DBadapterCustomer.UpdateCommand.Parameters.Add("@Finished", MySqlDbType.UByte, 1, "Finished");
                DBadapterCustomer.UpdateCommand.Parameters.Add("@Timestamp", MySqlDbType.DateTime, 10, "Timestamp");
                DBadapterCustomer.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                // Set the INSERT command and parameter.
                DBadapterCustomer.InsertCommand = new MySqlCommand(
                    "INSERT INTO customer (Name, Phone, Comment, Date, TimeStamp) VALUES (@Name,@Phone,@Comment,Now(),Now());",
                    DBconnection);
                DBadapterCustomer.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 30, "Name");
                DBadapterCustomer.InsertCommand.Parameters.Add("@Phone", MySqlDbType.VarChar, 15, "Phone");
                DBadapterCustomer.InsertCommand.Parameters.Add("@Comment", MySqlDbType.VarChar, 100, "Comment");

                DBadapterCustomer.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                // Set the DELETE command and parameter.
                DBadapterCustomer.DeleteCommand = new MySqlCommand("DELETE FROM customer WHERE Id=@id;", DBconnection);
                DBadapterCustomer.DeleteCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
                DBadapterCustomer.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
            }
            fUpdatingCustomerList = false;
        }

        public void InsertNewCustomerToDB(Customer customer)
        {
            var sqlTran = DBconnection.BeginTransaction();

            var command = DBconnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DBconnection;
            try {
                // Set the INSERT command and parameter.
                command.CommandText =
                    "INSERT INTO customer (Name, Phone, Comment, Date, TimeStamp) VALUES (@Name,@Phone,@Comment,Now(),Now());";
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@Phone", customer.Phone);
                command.Parameters.AddWithValue("@Comment", customer.Note);

                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                // Commit the transaction.
                sqlTran.Commit();
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

        public void UpdateProductFromDB()
        {
            try {
                //prepare adapter to run query
                var query =
                    "select id, Label, Name, Type, Description, Note, Price, CustomerId, TimeStamp, FixedPrice from Product where year=" +
                    ConfigurationManager.AppSettings["Year"];

                DBadapterProduct = new MySqlDataAdapter(query, DBconnection);
                var DBDataSetProduct = new DataSet();
                //get query results in dataset
                DBadapterProduct.Fill(DBDataSetProduct);

                DataTableProduct = DBDataSetProduct.Tables[0];
                DataTableProduct.TableName = "Product";

                // Set the UPDATE command and parameters.
                //DBadapterProduct.UpdateCommand = new MySqlCommand(
                //    "UPDATE Product SET Name=@Name, Description=@Description, Type=@Type, Note=@Note, Price=@Price, Timestamp=Now() WHERE id=@id;",
                //    DBconnection);
                //DBadapterProduct.UpdateCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 45, "name");
                //DBadapterProduct.UpdateCommand.Parameters.Add("@Description", MySqlDbType.VarChar, 250, "Description");
                //DBadapterProduct.UpdateCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 15, "Type");
                //DBadapterProduct.UpdateCommand.Parameters.Add("@Note", MySqlDbType.VarChar, 15, "Note");
                //DBadapterProduct.UpdateCommand.Parameters.Add("@Price", MySqlDbType.Int16, 10, "Price");
                //DBadapterProduct.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
                ////DBadapter.UpdateCommand.Parameters.Add("@Timestamp", MySqlDbType.DateTime, 10, "Timestamp");
                //DBadapterProduct.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

                // Set the INSERT command and parameter.
                //DBadapterProduct.InsertCommand = new MySqlCommand(
                //    "INSERT INTO Product (Name, Description,Type,  Note, Price, CustomerId, Year, timestamp)" +
                //"VALUES (@Name, @Description, @Type, @Note,@price, @CustomerId, 2014, Now());",
                //    DBconnection);
                //DBadapterProduct.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 45, "Name");
                //DBadapterProduct.InsertCommand.Parameters.Add("@Description", MySqlDbType.VarChar, 250, "Description");
                //DBadapterProduct.InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 15, "Type");
                //DBadapterProduct.InsertCommand.Parameters.Add("@Note", MySqlDbType.VarChar, 15, "Note");
                //DBadapterProduct.InsertCommand.Parameters.Add("@Price", MySqlDbType.Int16, 10, "Price");
                //DBadapterProduct.InsertCommand.Parameters.Add("@CustomerId", MySqlDbType.Int16, 10, "CustomerId");
                //DBadapterProduct.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

                // Set the DELETE command and parameter.
                //DBadapterProduct.DeleteCommand = new MySqlCommand(
                //    "DELETE FROM Product WHERE Id=@id;", DBconnection);
                //DBadapterProduct.DeleteCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
                //DBadapterProduct.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void InsertNewProductToDB(Product prod, int customerid)
        {
            var sqlTran = DBconnection.BeginTransaction();

            var command = DBconnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DBconnection;
            try {
                command.CommandText = "select max(Label) from Product where Year=" +
                                      ConfigurationManager.AppSettings["Year"];
                var result = command.ExecuteScalar();
                var nextLabel = 1;
                if (result != DBNull.Value) {
                    nextLabel = (int)command.ExecuteScalar() + 1;
                }

                command.CommandText =
                    "INSERT INTO Product (Label, Name, Description,Type,  Note, Price, FixedPrice, CustomerId, Year, timestamp)" +
                    "VALUES (@Label, @Name, @Description, @Type, @Note,@price, @Fixedprice, @CustomerId ," +
                    ConfigurationManager.AppSettings["Year"] + ", Now());";
                command.Parameters.AddWithValue("@Label", nextLabel);
                command.Parameters.AddWithValue("@Name", prod.Name);
                command.Parameters.AddWithValue("@Description", prod.Description);
                command.Parameters.AddWithValue("@Price", prod.Price);
                command.Parameters.AddWithValue("@FixedPrice", prod.FixedPrice);
                command.Parameters.AddWithValue("@Type", prod.Type);
                command.Parameters.AddWithValue("@Note", prod.Note);
                command.Parameters.AddWithValue("@CustomerId", customerid);

                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                // Commit the transaction.
                sqlTran.Commit();
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

        public void SaveProductToDB(Product prod)
        {
            var sqlTran = DBconnection.BeginTransaction();

            var command = DBconnection.CreateCommand();
            command.Transaction = sqlTran;
            command.Connection = DBconnection;
            try {
                command.CommandText =
                    "UPDATE Product SET Name=@Name, Description=@Description, Type=@Type, Note=@Note, Price=@Price, FixedPrice=@FixedPrice, Timestamp=Now() WHERE id=@id;";
                command.Parameters.AddWithValue("@Name", prod.Name);
                command.Parameters.AddWithValue("@Description", prod.Description);
                command.Parameters.AddWithValue("@Price", prod.Price);
                command.Parameters.AddWithValue("@FixedPrice", prod.FixedPrice);
                command.Parameters.AddWithValue("@Type", prod.Type);
                command.Parameters.AddWithValue("@Note", prod.Note);
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

        public void SaveProductPriceToDB(int id, int price, string note)
        {
            var command = DBconnection.CreateCommand();
            command.Connection = DBconnection;
            try {
                command.CommandText = "UPDATE Product SET Price=@Price, Timestamp=Now(), Note=@Note WHERE id=@id;";
                command.Parameters.AddWithValue("@Price", price);
                command.Parameters.AddWithValue("@Note", note);
                command.Parameters.AddWithValue("@id", id);
                command.UpdatedRowSource = UpdateRowSource.None;
                command.ExecuteNonQuery();

                Trace.WriteLine("Update Price : SUCCESS!");
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteProductToDB(int id)
        {
            var command = DBconnection.CreateCommand();
            command.Connection = DBconnection;
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

        public void SaveCustomerToDB()
        {
            try {
                DBadapterCustomer.Update(DataTableCustomer);
                fDataGridCustomerIsChanged = false;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

	    public int LeftToPay() {
			var foundRows = DataTableCustomer.Select("finished=false");
			var sum = 0;

		    foreach (var row in foundRows) {
			    var id = (int)row["id"];
				sum += DataTableProduct.NetAmountForCustomer(id);
			   }

			return sum;
	    }

    }
}
