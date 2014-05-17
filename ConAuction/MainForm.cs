using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Configuration;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace ConAuction
{
	public enum OpMode { Initializing=0, Receiving=1, Showing=2, Selling=3, Paying=4, Overhead=5};

	public partial class MainForm : Form
	{

		private OpMode Mode = OpMode.Initializing;

		string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
		MySqlConnection DBconnection;

		MySqlDataAdapter DBadapterCustomer;
		MySqlDataAdapter DBadapterProduct;
		DataTable DataTableCustomer = null;
		DataTable DataTableProduct = null;

		bool fUpdatingCustomerList = false;
		bool fDataGridCustomerIsChanged = false;

		public MainForm()
		{
			InitializeComponent();
			LoadImage();

			bool fStarted;
			do {
				fStarted = InitDB();
				if (!fStarted) {
					DialogResult res = MessageBox.Show("Vill du försöka kontakta databasen igen?", null, MessageBoxButtons.RetryCancel);
					if (res != DialogResult.Retry) {
						Application.Exit();
						System.Environment.Exit(-1);
						return;
					}
				}
			} while (!fStarted);

			UpdateFromDB();

			dataGridViewCustomers.ClearSelection();
			dataGridViewCustomers.CurrentCell = null;
			InitComboBoxMode();
		}


		private void InitComboBoxMode() {
			comboBoxMode.Items.Add("Välj mode!");
			comboBoxMode.Items.Add("Inlämning");
			comboBoxMode.Items.Add("Visning");
			comboBoxMode.Items.Add("Auktion");
			comboBoxMode.Items.Add("Utlämning");
			comboBoxMode.Items.Add("OH-projektor");
			comboBoxMode.SelectedIndex = (int)OpMode.Initializing;
		}

		private void LoadImage()
		{
			Stream _imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConAuction.LinconAuktionLiten.png");
			Bitmap bitmapLogo = new Bitmap(_imageStream);
			pictureBoxLogo.Image = bitmapLogo;

			this.Icon = Icon.FromHandle(bitmapLogo.GetHicon());
		}

		private bool InitDB()
		{
			try {
				//Initialize mysql connection
				DBconnection = new MySqlConnection(ConnectionString);
				DBconnection.Open();
				bool fOk = DBconnection.Ping();
				if (!fOk) {
					MessageBox.Show("Cannot connect to server.");
					return false;
				}

			}
			catch (MySql.Data.MySqlClient.MySqlException ex) {
				MessageBox.Show(ex.Message);
				return false;
			}
			return true;
		}

		public void UpdateCustomerFromDB()
		{
			fUpdatingCustomerList = true;
			try {
				//prepare adapter to run query
				string query = "select id,name,phone,comment,finished,timestamp from Customer;";

				DBadapterCustomer = new MySqlDataAdapter(query, DBconnection);
				DataSet DBDataSetCustomer = new DataSet();
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
			catch (MySql.Data.MySqlClient.MySqlException ex) {
				MessageBox.Show(ex.Message);
			}
			fUpdatingCustomerList = false;

		}

		public void UpdateProductFromDB()
		{
			try {
				//prepare adapter to run query
				string query = "select id, Label, Name, Type, Description, Note, Price, CustomerId, TimeStamp from Product";

				DBadapterProduct = new MySqlDataAdapter(query, DBconnection);
				DataSet DBDataSetProduct = new DataSet();
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
			catch (MySql.Data.MySqlClient.MySqlException ex) {
					MessageBox.Show(ex.Message);
			}
		}

		private void InsertNewProductToDB(Product prod, int customerid)
		{
			MySqlTransaction sqlTran = DBconnection.BeginTransaction();

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

		private void SaveProductToDB(Product prod)
		{
			MySqlTransaction sqlTran = DBconnection.BeginTransaction();

			MySqlCommand command = DBconnection.CreateCommand();
			command.Transaction = sqlTran;
			command.Connection = DBconnection;
			try {
				command.CommandText = "UPDATE Product SET Name=@Name, Description=@Description, Type=@Type, Note=@Note, Price=@Price, Timestamp=Now() WHERE id=@id;";
				command.Parameters.AddWithValue("@Name", prod.Name);
				command.Parameters.AddWithValue("@Description", prod.Description);
				command.Parameters.AddWithValue("@Price", prod.Price);
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

		private void SaveProductPriceToDB(int id, int price)
		{
			MySqlCommand command = DBconnection.CreateCommand();
			command.Connection = DBconnection;
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

		private void DeleteProductToDB(int id) {
			MySqlCommand command = DBconnection.CreateCommand();
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

		public void UpdateFromDB()
		{
			int customerId = GetSelectedCustomerId();

			if (fDataGridCustomerIsChanged && Mode != OpMode.Initializing) {
				DialogResult res = MessageBox.Show("Vill du spara ändringar innan uppdatering?", null, MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes) {
					SaveCustomerToDB();
				}
			}

			fUpdatingCustomerList = true;
			UpdateCustomerFromDB();
			if (Mode == OpMode.Initializing) InitCustomerList();
			SetVisibleCustomerList();

			UpdateProductFromDB();
			if (Mode == OpMode.Initializing) InitProductList();
			SetVisibleProductList();

			fUpdatingCustomerList = false;
			UpdateAuctionSummary();
			UpdateProductListHiding();
			UpdateProductSummary();
			fDataGridCustomerIsChanged = false;

			SelectCustomerRow(customerId);
		}

		private void UpdateProductListHiding()
		{
			int selectedCustomerId = GetSelectedCustomerId();
			if (fUpdatingCustomerList) {
				return;
			}

			dataGridViewProducts.ClearSelection();
			dataGridViewProducts.CurrentCell = null;

			if (Mode == OpMode.Selling || Mode == OpMode.Showing) {
				foreach (DataGridViewRow productRow in dataGridViewProducts.Rows) {
					productRow.Visible = true;
				}
			}
			else {
				foreach (DataGridViewRow productRow in dataGridViewProducts.Rows) {
					productRow.Selected = false;
					try {
						if (selectedCustomerId < 1) {
							productRow.Visible = false;
						}
						else {
							int productCustomerId = (int)productRow.Cells["CustomerId"].Value;
							productRow.Visible = (selectedCustomerId == productCustomerId);
						}
					}
					catch {
						Trace.WriteLine("Internal error");
					}
				}
			}
		}

		void SaveCustomerToDB() {
			try {
				DBadapterCustomer.Update(DataTableCustomer);
				fDataGridCustomerIsChanged = false;
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private DataGridViewRow  GetSelectedCustomerRow()
		{
			DataGridViewSelectedRowCollection rows = dataGridViewCustomers.SelectedRows;
			if (rows != null && rows.Count == 1) {
				 return rows[0];
			}
			return null;
		}

		private int GetSelectedCustomerId()
		{
			DataGridViewSelectedRowCollection rows = dataGridViewCustomers.SelectedRows;
			if (rows != null && rows.Count == 1 && rows[0].Cells["id"].Value != DBNull.Value && rows[0].Cells["id"].Value != null) {
				return (int)rows[0].Cells["id"].Value;
			}
			return 0;
		}

		private void SelectCustomerRow(int customerId) {
			if (Mode != OpMode.Initializing) {
				foreach (DataGridViewRow row in dataGridViewCustomers.Rows) {
					if (row.Cells["id"].Value != null && row.Cells["id"].Value != DBNull.Value) {
						int rowId = (int)row.Cells["id"].Value;
						if (customerId == rowId) {
							row.Selected = true;
							break;
						}
					}
				}
			}
		}

		private Product GetSelectedProduct()
		{
			DataGridViewSelectedRowCollection rows = dataGridViewProducts.SelectedRows;
			if (rows != null && rows.Count > 0) {
				return new Product(rows[0]);
			}
			return null;
		}

		private int GetSelectedProductId() {
			DataGridViewSelectedRowCollection rows = dataGridViewProducts.SelectedRows;
			if (rows != null && rows.Count == 1) {
				return (int)rows[0].Cells["id"].Value;
			}
			return -1;
		}

		private void InitCustomerList()
		{
			try {
				dataGridViewCustomers.DataSource = DataTableCustomer;

				if (dataGridViewCustomers.ColumnCount < 5) {
					return;
				}

				dataGridViewCustomers.Columns["id"].HeaderText = "Id";
				dataGridViewCustomers.Columns["id"].ReadOnly = true;
				dataGridViewCustomers.Columns["Name"].HeaderText = "Namn";
				dataGridViewCustomers.Columns["Phone"].HeaderText = "Telefon";
				dataGridViewCustomers.Columns["Comment"].HeaderText = "Not";
				dataGridViewCustomers.Columns["Finished"].HeaderText = "Klar";

				dataGridViewCustomers.Columns["id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns["Comment"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridViewCustomers.Columns["Finished"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;


				dataGridViewCustomers.Columns["TimeStamp"].Visible = false;
			}
			catch (Exception ex){
				MessageBox.Show("Error " + ex.Message);
			}
		}

		private void SetVisibleCustomerList() {
				dataGridViewCustomers.Columns["Finished"].Visible = (Mode == OpMode.Paying);
		}

		private void UpdateAuctionSummary()
		{
			textBoxSoldCount.Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			labelSoldCount.Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			textBoxAmount.Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			labelSoldAmount.Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			buttonSave.Visible = (Mode == OpMode.Receiving);
			buttonSave.Enabled = fDataGridCustomerIsChanged;

			if (fUpdatingCustomerList) {
				return;
			}

			if (DataTableProduct == null) {
				return;
			}

			int totalcount = DataTableProduct.TotalCount();
			textBoxTotalCount.Text = totalcount.ToString();

			if (Mode == OpMode.Selling || Mode == OpMode.Paying) {
				int totalSoldCount = DataTableProduct.TotalSoldCount();
				textBoxSoldCount.Text = totalSoldCount.ToString();
				int totalSoldAmount = DataTableProduct.TotalSoldAmount();
				textBoxAmount.Text = totalSoldAmount.ToString();
			}
		}

		private void InitProductList()
		{
			try {
				dataGridViewProducts.DataSource = DataTableProduct;

				dataGridViewProducts.Columns["Label"].HeaderText = "Id";
				dataGridViewProducts.Columns["Name"].HeaderText = "Namn";
				dataGridViewProducts.Columns["Type"].HeaderText = "Typ";
				dataGridViewProducts.Columns["Description"].HeaderText = "Beskr.";
				dataGridViewProducts.Columns["Note"].HeaderText = "Not";
				dataGridViewProducts.Columns["Price"].HeaderText = "Pris";

				dataGridViewProducts.Columns["Label"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridViewProducts.Columns["Note"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewProducts.Columns["Price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

				dataGridViewProducts.Columns["Label"].ReadOnly = true;
				dataGridViewProducts.Columns["Type"].ReadOnly = true;
				dataGridViewProducts.Columns["Name"].ReadOnly = true;
				dataGridViewProducts.Columns["Description"].ReadOnly = true;
				dataGridViewProducts.Columns["Note"].ReadOnly = true;
				dataGridViewProducts.Columns["id"].ReadOnly = true;
				dataGridViewProducts.Columns["id"].Visible = false;
				dataGridViewProducts.Columns["CustomerId"].Visible = false;
				dataGridViewProducts.Columns["CustomerId"].ReadOnly = true;
				dataGridViewProducts.Columns["TimeStamp"].Visible = false;
				}
			catch (Exception ex) {
				MessageBox.Show("Error " + ex.Message);
			}
		}

		private void SetVisibleProductList() {
			dataGridViewProducts.Columns["Note"].Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			dataGridViewProducts.Columns["Price"].ReadOnly = !(Mode == OpMode.Selling);
			dataGridViewProducts.Columns["Price"].Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
			dataGridViewProducts.EditMode = (Mode == OpMode.Selling) ? DataGridViewEditMode.EditOnKeystrokeOrF2 : DataGridViewEditMode.EditProgrammatically;
		}

		private void UpdateProductSummary()
		{
			int foundCustomer = GetSelectedCustomerId();
			try {
				if (foundCustomer > 0 && DataTableProduct != null) {
					int totalAmount = DataTableProduct.TotalAmountForCustomer(foundCustomer);
					textBoxTotalAmount.Text = totalAmount.ToString();
					int netAmount = DataTableProduct.NetAmountForCustomer(foundCustomer);
					textBoxNetAmount.Text = netAmount.ToString();
					int noOfUnsold = DataTableProduct.NoOfUnsoldForCustomer(foundCustomer);
					textBoxUnsold.Text = noOfUnsold.ToString();
				}
			}
			catch (System.Exception ex)	{
				MessageBox.Show("Error " + ex.Message);
			}

			buttonNewProduct.Visible = (Mode == OpMode.Receiving);
			buttonNewProduct.Enabled = !fDataGridCustomerIsChanged;

			buttonDeleteProduct.Visible = (Mode == OpMode.Receiving);
			buttonDeleteProduct.Enabled = (!fDataGridCustomerIsChanged && GetSelectedProductId() > 0);
		}

		#region  Event handling

		private void dataGridViewCustomers_SelectionChanged(object sender, EventArgs e)
		{
			if (Mode != OpMode.Initializing) {
				if (!fUpdatingCustomerList){
				 
					UpdateProductListHiding();
					UpdateProductSummary();
				}
			}
		}

		private void dataGridViewCustomers_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.F2 && Mode == OpMode.Receiving) {
				dataGridViewCustomers.BeginEdit(false);
			}
		}

		private void dataGridViewCustomers_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			fDataGridCustomerIsChanged = true;
			UpdateAuctionSummary();
			UpdateProductSummary();
		}

		private void dataGridViewCustomers_CellClick(object sender, DataGridViewCellEventArgs e) {
			if (Mode == OpMode.Paying && e.RowIndex >= 0 && e.ColumnIndex >= 0) {
				DataGridViewCell cell = dataGridViewCustomers.Rows[e.RowIndex].Cells[e.ColumnIndex];
				if (cell.ValueType == typeof(bool)) {
					if (cell.Value == DBNull.Value) {
						cell.Value = new bool();
						cell.Value = true;
					}
					else {
						cell.Value = !(bool)cell.Value;
					}
					SaveCustomerToDB();
				}
			}
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			SaveCustomerToDB();
			UpdateFromDB();
		}

		private void buttonNewProduct_Click(object sender, EventArgs e)
		{
			int customerId = GetSelectedCustomerId();
			if (customerId > 0 && DataTableProduct != null) {
				int productIdLast = DataTableProduct.GetLastProductIdForCustomer(customerId);
				Product productLast = null;
				if (productIdLast > 0) {
					productLast = new Product(DataTableProduct.GetRowForProductId(productIdLast));
					
				}

				Product productNew = new Product();
				FormEditProduct form = new FormEditProduct(productNew, productLast, Mode);

				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {

					InsertNewProductToDB(productNew, customerId);
					UpdateFromDB();

					//SelectCustomerRow(customerId);
				}
			}
		}

		private void dataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			Product productCurrent = GetSelectedProduct();

			if (productCurrent != null) {
				if ((Mode == OpMode.Selling)) {
					dataGridViewProducts.BeginEdit(true);
				}
				else {
					FormEditProduct form = new FormEditProduct(productCurrent, null, Mode);
					if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
						SaveProductToDB(productCurrent);
						UpdateFromDB();
					}
				}
			}
		}

		private void dataGridViewProducts_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.F2 && Mode == OpMode.Selling) {
			    dataGridViewProducts.BeginEdit(false);
			}
		}

		private void dataGridViewProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (Mode == OpMode.Selling && DataTableProduct != null && DataTableProduct.Columns[e.ColumnIndex].ColumnName=="Price") {
				Trace.WriteLine("CellValueChanged row=" + e.RowIndex.ToString() + " col=" + e.ColumnIndex.ToString());

				DataRow row = DataTableProduct.Rows[e.RowIndex];
				SaveProductPriceToDB((int)row["id"], (int)row["Price"]);
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (fDataGridCustomerIsChanged) {
				DialogResult res = MessageBox.Show("Vill du spara ändringar?", "DB", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes) {
					SaveCustomerToDB();
				}
			}			
		}

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			UpdateFromDB();
		}


		private void buttonDeleteProduct_Click(object sender, EventArgs e) {
			int productId = GetSelectedProductId();
			if (productId > 0) {
				DialogResult res = MessageBox.Show("Är du säker på att du vill radera produkten?", "ConAuction", MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes) {
					DeleteProductToDB(productId);
				}
			}
		}

		private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e) {
			UpdateProductSummary();
		}

		private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e) {
			Mode = (ConAuction.OpMode) comboBoxMode.SelectedIndex;
			if (Mode == OpMode.Overhead) {
				FormProductDisplay form = new FormProductDisplay(DataTableProduct);
				form.ShowDialog();
				Mode = OpMode.Initializing;
				comboBoxMode.SelectedIndex = (int) OpMode.Initializing;
			}
			UpdateFromDB();
		}

	}
		#endregion
}
