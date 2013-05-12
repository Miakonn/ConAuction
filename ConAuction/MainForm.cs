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

namespace ConAuction
{
	public enum OpMode { Receiving, Showing, Selling, Paying};

	public partial class MainForm : Form
	{

		private Customers BaseCustomers = new Customers();
		private OpMode Mode = OpMode.Showing;
		private string SettingPath = @"C:\ConAuction\Data\ConAuction.txt";
		private DateTime TimeLastBackuped = DateTime.MinValue;

		public MainForm()
		{
			InitializeComponent();

			GetAppConfiguration();

			LoadFromFile();

			Stream _imageStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ConAuction.LinconAuktionLiten.png");
			Bitmap bitmapLogo = new Bitmap(_imageStream);
			pictureBoxLogo.Image = bitmapLogo;

			this.Icon = Icon.FromHandle(bitmapLogo.GetHicon());
		}

		public void GetAppConfiguration()
		{
			SettingPath = ConfigurationManager.AppSettings["Path"];
		}

		private void LoadFromFile()
		{
			try {
				BaseCustomers = Customers.Load(SettingPath);
				BaseCustomers.Sorting();
			}
			catch (Exception ex) {
				MessageBox.Show("Ingen fil hittades, startar utan kunder. " + ex.Message);
			}
			Mode = BaseCustomers.Mode;

			UpdateCustomerList();
			UpdateRadioButtons();

			Refresh();
		}

		private void Serialize()
		{
			BaseCustomers.Mode = Mode;
			BaseCustomers.Save(SettingPath);
			if (DateTime.Now > TimeLastBackuped.AddMinutes(15.0)) {
				BaseCustomers.Save(SettingPath + "." + DateTime.Now.ToString("hhMM"));
				TimeLastBackuped = DateTime.Now;
			}
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
			if (BaseCustomers.CustomerList.Count == 0)
			{
				Customer customer = new Customer(BaseCustomers.GetNextFreeCustomerId());

				BaseCustomers.CustomerList.Add(customer);
			}

			try {
				dataGridViewCustomers.DataSource = null;
				dataGridViewCustomers.DataSource = BaseCustomers.CustomerList;
				dataGridViewCustomers.Invalidate();
				dataGridViewCustomers.Refresh();

				dataGridViewCustomers.Columns[0].HeaderText = "Id";
				dataGridViewCustomers.Columns[1].HeaderText = "Namn";
				dataGridViewCustomers.Columns[2].HeaderText = "Telefon";
				dataGridViewCustomers.Columns[3].HeaderText = "#";
				dataGridViewCustomers.Columns[4].HeaderText = "OK";

				dataGridViewCustomers.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridViewCustomers.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridViewCustomers.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

				dataGridViewCustomers.Columns[4].Visible = (Mode == OpMode.Paying);

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


			buttonNewCustomer.Visible = (Mode == OpMode.Receiving);
		}

		private void UpdateProductList()
		{
			try {
				dataGridViewProducts.DataSource = null;
				Customer foundCustomer = GetSelectedCustomer();
				if (foundCustomer != null && foundCustomer.ProductList.Count > 0)
				{
					dataGridViewProducts.DataSource = foundCustomer.ProductList;
					dataGridViewProducts.Columns[0].HeaderText = "Id";
					dataGridViewProducts.Columns[1].HeaderText = "Typ";
					dataGridViewProducts.Columns[2].HeaderText = "Namn";
					dataGridViewProducts.Columns[3].HeaderText = "Beskr.";
					dataGridViewProducts.Columns[4].HeaderText = "Not";
					dataGridViewProducts.Columns[5].HeaderText = "Pris";

					dataGridViewProducts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
					dataGridViewProducts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
					dataGridViewProducts.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
					dataGridViewProducts.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
					dataGridViewProducts.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
					dataGridViewProducts.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

					dataGridViewProducts.Columns[0].ReadOnly = true;
					dataGridViewProducts.Columns[1].ReadOnly = true;
					dataGridViewProducts.Columns[2].ReadOnly = true;
					dataGridViewProducts.Columns[3].ReadOnly = true;
					dataGridViewProducts.Columns[4].ReadOnly = true;
					dataGridViewProducts.Columns[5].ReadOnly = !(Mode == OpMode.Selling);


					dataGridViewProducts.Columns[4].Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);
					dataGridViewProducts.Columns[5].Visible = (Mode == OpMode.Selling || Mode == OpMode.Paying);

					dataGridViewProducts.EditMode = (Mode == OpMode.Selling) ? DataGridViewEditMode.EditOnKeystrokeOrF2 : DataGridViewEditMode.EditProgrammatically;

					UpdateProductSummary();
				}
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
			buttonNewProduct.Visible = (Mode == OpMode.Receiving);
		}

		#region  Event handling

		private void dataGridViewCustomers_SelectionChanged(object sender, EventArgs e)
		{
			UpdateProductList();
			// Serialize();
		}

		private void buttonNewCustomer_Click(object sender, EventArgs e)
		{
			Customer newCustomer = new Customer(BaseCustomers.GetNextFreeCustomerId());
			BaseCustomers.CustomerList.Add(newCustomer);
			UpdateCustomerList();
		}

		private void buttonNewProduct_Click(object sender, EventArgs e)
		{
			Customer customerCurrent = GetSelectedCustomer();
			if (customerCurrent != null) {
				Product productNew = new Product();
				productNew.Id = customerCurrent.GetNextFreeProductId();
				FormProduct form = new FormProduct(customerCurrent,productNew,  Mode);

				if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
					customerCurrent.ProductList.Add(productNew);
					Serialize();
					UpdateProductList();
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
			UpdateCustomerList();
			UpdateProductList();
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			Mode = OpMode.Showing;
			UpdateCustomerList();
			UpdateProductList();
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			Mode = OpMode.Selling;
			UpdateCustomerList();
			UpdateProductList();
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			Mode = OpMode.Paying;
			UpdateCustomerList();
			UpdateProductList();
		}

		private void buttonExportCustomers_Click(object sender, EventArgs e)
		{
			SaveFileDialog fileDialog = new SaveFileDialog();

			fileDialog.InitialDirectory = "c:\\";
			fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			fileDialog.FilterIndex = 2;
			fileDialog.Title = "Exportera kunder";

			if (fileDialog.ShowDialog() == DialogResult.OK) {
				try {
					BaseCustomers.ExportCustomersToFile(fileDialog.FileName);
				}
				catch (Exception ex) {
					MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
				}

			}
		}

		private void buttonExportProducts_Click(object sender, EventArgs e)
		{
			SaveFileDialog fileDialog = new SaveFileDialog();

			fileDialog.InitialDirectory = "c:\\";
			fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			fileDialog.FilterIndex = 2;
			fileDialog.Title = "Exportera objekt";

			if (fileDialog.ShowDialog() == DialogResult.OK) {
				try {

					BaseCustomers.ExportProductsToFile(fileDialog.FileName);
				}
				catch (Exception ex) {
					MessageBox.Show("Error: Could not save file to disk. Original error: " + ex.Message);
				}

			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Serialize();
		}

		private void buttonImport_Click(object sender, EventArgs e)
		{
			Serialize();

			OpenFileDialog fileDialog = new OpenFileDialog();

			fileDialog.InitialDirectory = "c:\\";
			fileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			fileDialog.FilterIndex = 2;
			fileDialog.Title = "Importera xml-fil";

			if (fileDialog.ShowDialog() == DialogResult.OK) {
				try {
					Customers customersAdd = Customers.Load(fileDialog.FileName);
					if (customersAdd.CustomerList.Count > 0) {
						BaseCustomers.CustomerList.AddRange(customersAdd.CustomerList);
						BaseCustomers.CustomerList.Sort((x, y) => x.Id.CompareTo(y.Id));
						UpdateCustomerList();
					}
				}
				catch (Exception ex) {
					MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
				}
			}


		}

	}
		#endregion
}
