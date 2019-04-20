using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using ConAuction3.DataModels;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace ConAuction3.DataModels
{
    internal class DbAccess
    {
        private readonly string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];


        private MySqlConnection DBconnection;
        //private MySqlDataAdapter DBadapterCustomer;
        // private MySqlDataAdapter DBadapterProduct;

 
        public bool InitDB()
        {
            try {
                // Initialize mysql connection
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
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
			return true;
        }

		//public void UpdateCustomerFromDB()
		//{
		//	try {
		//		//prepare adapter to run query
		//		var query = "select id,name,phone,comment,finished,timestamp from Customer;";

		//		DBadapterCustomer = new MySqlDataAdapter(query, DBconnection);
		//		var DBDataSetCustomer = new DataSet();
		//		//get query results in dataset
		//		DBadapterCustomer.Fill(DBDataSetCustomer);

		//		DBDataSetCustomer.Tables[0].TableName = "Customer";
		//		DataTableCustomer = DBDataSetCustomer.Tables[0];

		//		// Set the UPDATE command and parameters.
		//		DBadapterCustomer.UpdateCommand = new MySqlCommand(
		//			"UPDATE customer SET Name=@Name, Phone=@Phone, Comment=@Comment, Date=NOW(), Finished=@Finished, Timestamp=Now() WHERE id=@id and timestamp=@Timestamp;",
		//			DBconnection);
		//		DBadapterCustomer.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
		//		DBadapterCustomer.UpdateCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 30, "Name");
		//		DBadapterCustomer.UpdateCommand.Parameters.Add("@Phone", MySqlDbType.VarChar, 15, "Phone");
		//		DBadapterCustomer.UpdateCommand.Parameters.Add("@Comment", MySqlDbType.VarChar, 100, "Comment");
		//		DBadapterCustomer.UpdateCommand.Parameters.Add("@Finished", MySqlDbType.UByte, 1, "Finished");
		//		DBadapterCustomer.UpdateCommand.Parameters.Add("@Timestamp", MySqlDbType.DateTime, 10, "Timestamp");
		//		DBadapterCustomer.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

		//		// Set the INSERT command and parameter.
		//		DBadapterCustomer.InsertCommand = new MySqlCommand(
		//			"INSERT INTO customer (Name, Phone, Comment, Date, TimeStamp) VALUES (@Name,@Phone,@Comment,Now(),Now());",
		//			DBconnection);
		//		DBadapterCustomer.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 30, "Name");
		//		DBadapterCustomer.InsertCommand.Parameters.Add("@Phone", MySqlDbType.VarChar, 15, "Phone");
		//		DBadapterCustomer.InsertCommand.Parameters.Add("@Comment", MySqlDbType.VarChar, 100, "Comment");

		//		DBadapterCustomer.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

		//		// Set the DELETE command and parameter.
		//		DBadapterCustomer.DeleteCommand = new MySqlCommand("DELETE FROM customer WHERE Id=@id;", DBconnection);
		//		DBadapterCustomer.DeleteCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
		//		DBadapterCustomer.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
		//	}
		//	catch (MySqlException ex) {
		//		MessageBox.Show(ex.Message);
		//	}
		//	//fUpdatingCustomerList = false;
		//}




		public List<Customer> ReadAllCustomers() {
			var customers = new List<Customer>(900);
			var query = "select id,name,phone,comment,finished,timestamp from Customer;";
			MySqlCommand cmdDatabase = new MySqlCommand(query, DBconnection);

			try {
				using (MySqlDataReader reader = cmdDatabase.ExecuteReader()) {
					while (reader.Read()) {
						var customer = new Customer {
							Id = reader.GetInt32("id"),
							Name = reader.GetStringOrDefault("name"),
							Phone = FormatPhoneNumber(reader.GetStringOrDefault("phone")),
							Note = reader.GetStringOrDefault("comment"),
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


		public List<Product> ReadAllProducts() {
			var products = new List<Product>(900);
			var query =
					"select Id, Label, Name, Type, Description, Note, Price, CustomerId, TimeStamp, FixedPrice from Product where year=" +
					ConfigurationManager.AppSettings["Year"];
			MySqlCommand cmdDatabase = new MySqlCommand(query, DBconnection);
			try {
				using (MySqlDataReader reader = cmdDatabase.ExecuteReader()) {

					while (reader.Read()) {
						var product = new Product {
							Id = reader.GetInt64("Id"),
							Label = FormatPhoneNumber(reader.GetStringOrDefault("Label")),
							Name = reader.GetStringOrDefault("Name"),
							Type = reader.GetStringOrDefault("Type"),
							Description = reader.GetStringOrDefault("Description"),
							Note = reader.GetStringOrDefault("Note"),
							Price = reader.GetIntOrDefault("Price"),
							FixedPrice = reader.GetIntOrDefault("FixedPrice"),
							CustomerId = reader.GetInt32("CustomerId")
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
		
		public string FormatPhoneNumber(string number) {
			if (number.Length > 10) {
				return number.Substring(0, 3) + " " + number.Substring(3, 3) + " " + number.Substring(6, 3) + " " + number.Substring(9);
			}
			if (number.Length > 8) {
				return number.Substring(0, 3) + " " + number.Substring(3, 3) + " " + number.Substring(6, 2) + " " + number.Substring(8);
			}
			return number;
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

		public void SaveCustomerToDB(Customer customer) {
			var sqlTran = DBconnection.BeginTransaction();

			var command = DBconnection.CreateCommand();
			command.Transaction = sqlTran;
			command.Connection = DBconnection;
			try {
				command.CommandText = "UPDATE customer SET Name=@Name, Phone=@Phone, Comment=@Comment, Date=NOW(), Finished=@Finished, Timestamp=Now() WHERE id=@id;";
				command.Parameters.AddWithValue("@Name", customer.Name);
				command.Parameters.AddWithValue("@Phone", customer.Phone);
				command.Parameters.AddWithValue("@Comment", customer.Note);
				command.Parameters.AddWithValue("@Finished", customer.Finished);
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
	
		public void DeleteCustomerToDB(int id) {
			var command = DBconnection.CreateCommand();
			command.Connection = DBconnection;
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

		//public void UpdateProductFromDB()
		//{
		//	try {
		//		//prepare adapter to run query
		//		var query =
		//			"select id, Label, Name, Type, Description, Note, Price, CustomerId, TimeStamp, FixedPrice from Product where year=" +
		//			ConfigurationManager.AppSettings["Year"];

		//		DBadapterProduct = new MySqlDataAdapter(query, DBconnection);
		//		var DBDataSetProduct = new DataSet();
		//		//get query results in dataset
		//		DBadapterProduct.Fill(DBDataSetProduct);

		//		DataTableProduct = DBDataSetProduct.Tables[0];
		//		DataTableProduct.TableName = "Product";
		//	}
		//	catch (MySqlException ex) {
		//		MessageBox.Show(ex.Message);
		//	}
		//}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prod"></param>
        /// <returns>The db id</returns>
        public long InsertNewProductToDB(Product prod)
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

                prod.Label = nextLabel.ToString();

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
                command.Parameters.AddWithValue("@CustomerId", prod.CustomerId);

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

        public void DeleteProductToDB(long id)
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

		//public int LeftToPay() {
		//	var foundRows = DataTableCustomer.Select("ISNULL(finished, false)=false");
		//	var sum = 0;

		//	foreach (var row in foundRows) {
		//		var id = (int)row["id"];
		//		int net = 0; // TODO
		//		//int net = DataTableProduct.NetAmountForCustomer(id);
		//		Trace.WriteLine(id + " " + net);
		//		sum += net;
		//	   }

		//	return sum;
		//}

    }


	public static class DbReaderExtensions {
		public static string GetStringOrDefault(this MySqlDataReader reader, string name) {
			var ordinal = reader.GetOrdinal(name);
			if (reader.IsDBNull(ordinal)) {
				return "";
			}
			else {
				return reader.GetString(ordinal);
			}
		}

		public static bool? GetBooleanOrDefault(this MySqlDataReader reader, string name) {
			var ordinal = reader.GetOrdinal(name);
			if (reader.IsDBNull(ordinal)) {
				return null;
			}
			else {
				return reader.GetBoolean(ordinal);
			}
		}

		public static int GetIntOrDefault(this MySqlDataReader reader, string name) {
			var ordinal = reader.GetOrdinal(name);
			if (reader.IsDBNull(ordinal)) {
				return 0;
			}
			else {
				return reader.GetInt32(ordinal);
			}
		}
	}

}
