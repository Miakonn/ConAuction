using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using System.Reflection;
using MySql.Data.MySqlClient;


namespace ConAuction
{
	public enum OpMode { Receiving, Showing, Selling, Paying};

	public partial class MainForm : Form
	{

		private Customers BaseCustomers = new Customers();
		private OpMode Mode = OpMode.Showing;

		string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
		MySqlConnection DBconnection;
		MySqlDataAdapter DBadapterCustomer;
		System.Data.DataSet DBDataSetCustomer;
		MySqlDataAdapter DBadapterProduct;
		System.Data.DataSet DBDataSetProduct;
		
 
		public MainForm()
		{
			InitializeComponent();

			InitDB();

			LoadFromFile();

			Stream _imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConAuction.LinconAuktionLiten.png");
			Bitmap bitmapLogo = new Bitmap(_imageStream);
			pictureBoxLogo.Image = bitmapLogo;

			this.Icon = Icon.FromHandle(bitmapLogo.GetHicon());
		}

		public void InitDB()
		{
			try {
				//Initialize mysql connection
				DBconnection = new MySqlConnection(ConnectionString);
				DBconnection.Open();

			}
			catch (MySql.Data.MySqlClient.MySqlException ex) {
				switch (ex.Number) {
					case 0:
						MessageBox.Show("Cannot connect to server.  Contact administrator");
						break;
					case 1045:
						MessageBox.Show("Invalid username/password, please try again");
						break;
				}
			}
		}


		public void UpdateCustomerFromDB()
		{
			//prepare adapter to run query
			string query = "select id,name,phone,comment,done from Customer;";

			DBadapterCustomer = new MySqlDataAdapter(query, DBconnection);
			DBDataSetCustomer = new DataSet();
			//get query results in dataset
			DBadapterCustomer.Fill(DBDataSetCustomer);

			DBDataSetCustomer.Tables[0].TableName = "Customer";

			//DBRelationCustomerProd = this.DBDataSetCustomer.Relations.Add("CustProd", DBDataSetCustomer.Tables["customer"].Columns["id"],
			//    DBDataSetCustomer.Tables["product"].Columns["CustomerId"]);

			// Set the UPDATE command and parameters.
			DBadapterCustomer.UpdateCommand = new MySqlCommand(
				"UPDATE customer SET Name=@Name, Phone=@Phone, Comment=@Comment, Date=NOW(), Done=@Done, Timestamp=Now() WHERE id=@id;",
				DBconnection);
			DBadapterCustomer.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
			DBadapterCustomer.UpdateCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 30, "Name");
			DBadapterCustomer.UpdateCommand.Parameters.Add("@Phone", MySqlDbType.VarChar, 15, "Phone");
			DBadapterCustomer.UpdateCommand.Parameters.Add("@Comment", MySqlDbType.VarChar, 100, "Comment");
			DBadapterCustomer.UpdateCommand.Parameters.Add("@Done", MySqlDbType.VarChar, 10, "Done");
			//DBadapter.UpdateCommand.Parameters.Add("@Timestamp", MySqlDbType.DateTime, 10, "Timestamp");
			DBadapterCustomer.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

			// Set the INSERT command and parameter.
			DBadapterCustomer.InsertCommand = new MySqlCommand(
				"INSERT INTO customer VALUES (NULL,@Name,@Phone,Now(),@Comment,Now());",
				DBconnection);
			DBadapterCustomer.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 30, "Name");
			DBadapterCustomer.InsertCommand.Parameters.Add("@Comment", MySqlDbType.VarChar, 100, "Comment");
			DBadapterCustomer.InsertCommand.Parameters.Add("@Phone", MySqlDbType.VarChar, 15, "Phone");

			DBadapterCustomer.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

			// Set the DELETE command and parameter.
			DBadapterCustomer.DeleteCommand = new MySqlCommand(
				"DELETE FROM customer WHERE Id=@id;", DBconnection);
			DBadapterCustomer.DeleteCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
			//DBadapterCustomer.DeleteCommand.Parameters.Add("@Timestamp", MySqlDbType.DateTime, 4, "Timestamp");
			DBadapterCustomer.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
		}

		public void UpdateProductFromDB()
		{
			try {
				//prepare adapter to run query
				string query = "select id, Label, Name, Type, Description, Note, Price, CustomerId from Product";

				DBadapterProduct = new MySqlDataAdapter(query, DBconnection);
				DBDataSetProduct = new DataSet();
				//get query results in dataset
				DBadapterProduct.Fill(DBDataSetProduct);

				DBDataSetProduct.Tables[0].TableName = "Product";

				//DBRelationProductProd = this.DBDataSetProduct.Relations.Add("CustProd", DBDataSetProduct.Tables["Product"].Columns["id"],
				//    DBDataSetProduct.Tables["product"].Columns["ProductId"]);

				// Set the UPDATE command and parameters.
				DBadapterProduct.UpdateCommand = new MySqlCommand(
					"UPDATE Product SET Name=@Name, Description=@Description, Type=@Type, Note=@Note, Price=@Price, CustomerId=@CustomerId, Timestamp=Now() WHERE id=@id;",
					DBconnection);
				DBadapterProduct.UpdateCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
				DBadapterProduct.UpdateCommand.Parameters.Add("@Description", MySqlDbType.VarChar, 200, "Description");
				DBadapterProduct.UpdateCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 15, "Type");
				DBadapterProduct.UpdateCommand.Parameters.Add("@Note", MySqlDbType.VarChar, 15, "Note");
				DBadapterProduct.UpdateCommand.Parameters.Add("@Price", MySqlDbType.Int16, 10, "Price");
				DBadapterProduct.UpdateCommand.Parameters.Add("@Done", MySqlDbType.Int16, 10, "Done");
				//DBadapter.UpdateCommand.Parameters.Add("@Timestamp", MySqlDbType.DateTime, 10, "Timestamp");
				DBadapterProduct.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;

				// Set the INSERT command and parameter.
				DBadapterProduct.InsertCommand = new MySqlCommand(
					"INSERT INTO Product (Label, Name, Description,Type,  Note, Price, CustomerId, Year, timestamp)" +
				"VALUES (@Label, @Name, @Description, @Type, @Note,@price, @CustomerId, 2014, Now());",
					DBconnection);
				DBadapterProduct.InsertCommand.Parameters.Add("@Label", MySqlDbType.VarChar, 100, "Label");
				DBadapterProduct.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");
				DBadapterProduct.InsertCommand.Parameters.Add("@Description", MySqlDbType.VarChar, 100, "Description");
				DBadapterProduct.InsertCommand.Parameters.Add("@Price", MySqlDbType.Int16, 10, "Price");
				DBadapterProduct.InsertCommand.Parameters.Add("@Type", MySqlDbType.VarChar, 15, "Type");
				DBadapterProduct.InsertCommand.Parameters.Add("@Note", MySqlDbType.VarChar, 15, "Note");
				DBadapterProduct.InsertCommand.Parameters.Add("@CustomerId", MySqlDbType.Int16, 10, "CustomerId");
				DBadapterProduct.InsertCommand.UpdatedRowSource = UpdateRowSource.None;

				// Set the DELETE command and parameter.
				DBadapterProduct.DeleteCommand = new MySqlCommand(
					"DELETE FROM Product WHERE Id=@id;", DBconnection);
				DBadapterProduct.DeleteCommand.Parameters.Add("@id", MySqlDbType.Int16, 4, "id");
				DBadapterProduct.DeleteCommand.UpdatedRowSource = UpdateRowSource.None;
			}
			catch (MySql.Data.MySqlClient.MySqlException ex) {
					MessageBox.Show("SQL error" + ex.ToString());
			}


		}


		private void SaveProductToDB(Product prod, int customerid)
		{
			MySqlTransaction sqlTran = DBconnection.BeginTransaction();

			// Enlist a command in the current transaction.
			MySqlCommand command = DBconnection.CreateCommand();
			command.Transaction = sqlTran;
			command.Connection = DBconnection;

			try {

				command.CommandText = "select max(Label) from Product where Year=2014";
				int maxlabel = (int)command.ExecuteScalar();
				int nextlabel = maxlabel + 1;



				command.CommandText ="INSERT INTO Product (Label, Name, Description,Type,  Note, Price, CustomerId, Year, timestamp)" +
								"VALUES (@Label, @Name, @Description, @Type, @Note,@price, @CustomerId, 2014, Now());";
				command.Parameters.AddWithValue("@Label", nextlabel);
				command.Parameters.AddWithValue("@Name", prod.Name);
				command.Parameters.AddWithValue("@Description", prod.Description);
				command.Parameters.AddWithValue("@Price", prod.Price);
				command.Parameters.AddWithValue("@Type", prod.ProductType);
				command.Parameters.AddWithValue("@Note", prod.Limit);
				command.Parameters.AddWithValue("@CustomerId", customerid);
				command.UpdatedRowSource = UpdateRowSource.None;
				command.ExecuteNonQuery();

				// Commit the transaction.
				sqlTran.Commit();
			}
			catch (Exception ex) {
				// Handle the exception if the transaction fails to commit.
				Console.WriteLine(ex.Message);

				try {
					// Attempt to roll back the transaction.
					sqlTran.Rollback();
				}
				catch (Exception exRollback) {
					// Throws an InvalidOperationException if the connection  
					// is closed or the transaction has already been rolled  
					// back on the server.
					Console.WriteLine(exRollback.Message);
				}
			}

		}



		public void UpdateFromDB()
		{
			UpdateCustomerFromDB();
			UpdateCustomerList();
			UpdateProductFromDB();
			UpdateProductList();
			UpdateProductListHiding();
		}


		private void UpdateProductListHiding()
		{
			DataGridViewRow selectedCustomerRow = GetSelectedCustomerRow();

			dataGridViewProducts.ClearSelection();
			dataGridViewProducts.CurrentCell = null;

			foreach (DataGridViewRow productRow in dataGridViewProducts.Rows) {
				productRow.Selected = false;
				try {
					if (selectedCustomerRow == null) {
						productRow.Visible = false;
					}
					else {
						string s1 = selectedCustomerRow.Cells["id"].Value.ToString();
						if (productRow.Cells["CustomerId"].Value != null) {
							string s2 = productRow.Cells["CustomerId"].Value.ToString();
							productRow.Visible = (String.Compare(s1, s2) == 0);
						}
					}
				}
				catch 
				{}
			}

		}


		private void LoadFromFile()
		{
			UpdateFromDB();
			UpdateCustomerList();
			UpdateRadioButtons();

			Refresh();
		}

		private void Serialize()
		{
			BaseCustomers.Mode = Mode;
			//BaseCustomers.Save(SettingPath);
			//if (DateTime.Now > TimeLastBackuped.AddMinutes(15.0)) {
			//    BaseCustomers.Save(SettingPath + "." + DateTime.Now.ToString("hhMM"));
			//    TimeLastBackuped = DateTime.Now;
			//}
		}

		private void UpdateRadioButtons()
		{
			switch (Mode) {
				case OpMode.Receiving: radioButton1.Checked = true; break;
				case OpMode.Showing: radioButton1.Checked = true; break;
				case OpMode.Selling: radioButton1.Checked = true; break;
				case OpMode.Paying: radioButton1.Checked = true; break;
			}
		}


		private Customer GetSelectedCustomer()
		{
			DataGridViewSelectedRowCollection rows = dataGridViewCustomers.SelectedRows;
			try
			{
				if (rows != null && rows.Count > 0)
				{
					string id = rows[0].Cells[0].Value.ToString();
					Customer foundCustomer = BaseCustomers.CustomerList.Find(x => x.Id == id);
					return foundCustomer;
				}
			}
			catch { }
			return null;
		}

		private DataGridViewRow  GetSelectedCustomerRow()
		{
			DataGridViewSelectedRowCollection rows = dataGridViewCustomers.SelectedRows;
			if (rows != null && rows.Count == 1) {
				 return rows[0];
			}
			return null;
		}

		private Product GetSelectedProduct()
		{
			DataGridViewSelectedRowCollection rows = dataGridViewProducts.SelectedRows;
			Customer currentCustomer = GetSelectedCustomer();

			if (currentCustomer != null && rows != null && rows.Count > 0) {
				string id = rows[0].Cells[0].Value.ToString();
				Product foundProduct = currentCustomer.ProductList.Find(x => x.Id == id);
				return foundProduct;
			}
			return null;
		}

		private void UpdateCustomerList()
		{
			try {

				dataGridViewCustomers.DataSource = DBDataSetCustomer.Tables["Customer"];
				// dataGridViewCustomers.DataSource = BaseCustomers.CustomerList;
				dataGridViewCustomers.Invalidate();
				dataGridViewCustomers.Refresh();

				dataGridViewCustomers.Columns["id"].HeaderText = "Id";
				dataGridViewCustomers.Columns["id"].ReadOnly = true;
				dataGridViewCustomers.Columns["Name"].HeaderText = "Namn";
				dataGridViewCustomers.Columns["Phone"].HeaderText = "Telefon";
				dataGridViewCustomers.Columns["Done"].HeaderText = "OK";


				dataGridViewCustomers.Columns["id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridViewCustomers.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns["Done"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

				dataGridViewCustomers.Columns["Done"].Visible = (Mode == OpMode.Paying);

				UpdateCustomerSummary();
			}
			catch (Exception ex){
				MessageBox.Show("Error " + ex.Message);
			}
		}

		private void UpdateCustomerSummary()
		{
			textBoxTotalCount.Text = BaseCustomers.TotalCount().ToString();

			textBoxSoldCount.Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			labelSoldCount.Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			textBoxSoldCount.Text = BaseCustomers.TotalSoldCount().ToString();


			buttonSave.Visible = (Mode == OpMode.Receiving);
		}

		private void UpdateProductList()
		{
			try {
				dataGridViewProducts.Invalidate();
				dataGridViewProducts.Refresh();
				dataGridViewProducts.DataSource = DBDataSetProduct.Tables["Product"];

				dataGridViewProducts.Columns["Label"].HeaderText = "Id";
				dataGridViewProducts.Columns["Type"].HeaderText = "Typ";
				dataGridViewProducts.Columns["Name"].HeaderText = "Namn";
				dataGridViewProducts.Columns["Description"].HeaderText = "Beskr.";
				dataGridViewProducts.Columns["Note"].HeaderText = "Not";
				dataGridViewProducts.Columns["Price"].HeaderText = "Pris";

				dataGridViewProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridViewProducts.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

				dataGridViewProducts.Columns["Label"].ReadOnly = true;
				//dataGridViewProducts.Columns[1].ReadOnly = true;
				//dataGridViewProducts.Columns[2].ReadOnly = true;
				//dataGridViewProducts.Columns[3].ReadOnly = true;
				//dataGridViewProducts.Columns[4].ReadOnly = true;
				//dataGridViewProducts.Columns[5].ReadOnly = !(Mode == OpMode.Selling);


				//dataGridViewProducts.Columns[4].Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
				//dataGridViewProducts.Columns[5].Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);

				//dataGridViewProducts.EditMode = (Mode == OpMode.Selling) ? DataGridViewEditMode.EditOnKeystrokeOrF2 : DataGridViewEditMode.EditProgrammatically;

				UpdateProductSummary();
				}
			catch (Exception ex) {
				MessageBox.Show("Error " + ex.Message);
			}
		}

		private void UpdateProductSummary()
		{
			Customer foundCustomer = GetSelectedCustomer();
			if (foundCustomer != null) {
				textBoxTotalAmount.Text = foundCustomer.TotalAmount().ToString();
				textBoxNetAmount.Text = foundCustomer.NetAmount().ToString();
				textBoxUnsold.Text = foundCustomer.NoOfUnsoldProducts().ToString();
			}
			buttonSaveProduct.Visible = (Mode == OpMode.Receiving);
		}

		#region  Event handling

		private void dataGridViewCustomers_SelectionChanged(object sender, EventArgs e)
		{
			//UpdateProductList();
			UpdateProductListHiding();
			// Serialize();
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			DBadapterCustomer.Update(DBDataSetCustomer.Tables["Customer"]);
			UpdateCustomerList();
		}







		private void buttonSaveProduct_Click(object sender, EventArgs e)
		{
			DataGridViewRow rowCustomer= GetSelectedCustomerRow();
			if (rowCustomer != null) {
				Product productNew = new Product();
				FormProduct form = new FormProduct(null, productNew, Mode);

				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
					//DataGridViewRow rowProduct = new DataGridViewRow();
					//rowProduct.CreateCells(dataGridViewProducts);

					SaveProductToDB(productNew, (int)rowCustomer.Cells["id"].Value);


					//DataRow row = DBDataSetProduct.Tables[0].NewRow();
					//row["id"] = DBNull.Value;
					//row["Label"] = DBNull.Value;
					//row["Name"] = productNew.Name;
					//row["Description"] = productNew.Description;
					//row["Type"] = productNew.ProductType;
					//row["Price"] = productNew.Price;
					//row["Note"] = productNew.Limit;
					//row["CustomerId"] = rowCustomer.Cells["id"].Value;

					//DBDataSetProduct.Tables[0].Rows.Add(row);

					//DBconnection.BeginTransaction();
					
					//DBadapterProduct.Update(DBDataSetProduct.Tables[0]);


					UpdateFromDB();
				}
			}
		}

		private void dataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			Product productCurrent = GetSelectedProduct();
			Customer customerCurrent = GetSelectedCustomer();

			if (productCurrent != null) {
				if ((Mode == OpMode.Selling)) {
					dataGridViewProducts.BeginEdit(true);
				}
				else {
					FormProduct form = new FormProduct(customerCurrent, productCurrent,  Mode);

					if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						Serialize();
						UpdateProductList();
					}
				}
			}
		}

		private void dataGridViewCustomers_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F2) {
				dataGridViewCustomers.BeginEdit(false);
			}
		}

		private void dataGridViewProducts_KeyDown(object sender, KeyEventArgs e)
		{
			//if (e.KeyCode == Keys.F2) {
			//    dataGridViewProducts.BeginEdit(false);
			//}
		}

		private void dataGridViewProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			UpdateProductSummary();
			Serialize();
		}

		private void dataGridViewCustomers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			UpdateCustomerSummary();
			Serialize();
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			Mode = OpMode.Receiving;
			UpdateFromDB();
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			Mode = OpMode.Showing;
			UpdateFromDB();
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			Mode = OpMode.Selling;
			UpdateFromDB();
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			Mode = OpMode.Paying;
			UpdateFromDB();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			
		}

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			UpdateFromDB();
		}

	}
		#endregion
}
