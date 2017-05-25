using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace ConAuction {
    public enum OpMode {
        Initializing = 0,
        Receiving = 1,
        Showing = 2,
        Auctioning = 3,
        Paying = 4,
        Overhead = 5
    }

    public partial class MainForm : Form {
	    private ViewModel DataViewModel { get; set; }

	    private OpMode Mode  { get; set; }
	    
        public MainForm() {
            Trace.WriteLine("MainForm");
	        try {
		        InitializeComponent();
		        LoadImage();

		        DataViewModel = new ViewModel();

		        bool fStarted;
		        do {
			        fStarted = DataViewModel.InitDB();
			        if (!fStarted) {
				        var res = MessageBox.Show("Vill du försöka kontakta databasen igen?", null,
					        MessageBoxButtons.RetryCancel);
				        if (res != DialogResult.Retry) {
					        Application.Exit();
					        Environment.Exit(-1);
				        }
			        }
		        } while (!fStarted);

		        Mode = OpMode.Initializing;
		        InitComboBoxMode();
		        TableAutoSizeToggleOnCustomer(dataGridViewCustomers);
		        TableAutoSizeToggleOnProduct(dataGridViewProducts);
		        dataGridViewCustomers.ClearSelection();
		        dataGridViewCustomers.CurrentCell = null;
		        Trace.WriteLine("MainForm - finished");
	        }
			catch (FileLoadException ex) {
				MessageBox.Show(ex.Message);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
        }

        private void InitComboBoxMode() {
            comboBoxMode.Items.Add("Välj läge!");
            comboBoxMode.Items.Add("Inlämning");
            comboBoxMode.Items.Add("Visning");
            comboBoxMode.Items.Add("Auktion");
            comboBoxMode.Items.Add("Utlämning");
            comboBoxMode.Items.Add("OH-projektor");
            comboBoxMode.SelectedIndex = (int) OpMode.Initializing;

            comboBoxMode.MouseWheel += comboBoxMode_MouseWheel;
        }

        private void LoadImage() {
            var imageStream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream("ConAuction.LinconAuktionLiten.png");
            if (imageStream != null) {
                var bitmapLogo = new Bitmap(imageStream);
                pictureBoxLogo.Image = bitmapLogo;

                Icon = Icon.FromHandle(bitmapLogo.GetHicon());
            }
        }

        public void UpdateFromDb()
        {
            Cursor.Current = Cursors.WaitCursor;
            Trace.WriteLine("UpdateFromDB");
            DataViewModel.fUpdatingProductList = true;
            DataViewModel.fUpdatingCustomerList = true;
            TableAutoSizeToggleOffCustomer(dataGridViewCustomers);
            TableAutoSizeToggleOffProduct(dataGridViewProducts);

	        int selectedCustomer = GetSelectedCustomerId();

			//if (DataViewModel.fDataGridCustomerIsChanged && Mode != OpMode.Initializing) {
			//	var res = MessageBox.Show("Vill du spara ändringar innan uppdatering?", null, MessageBoxButtons.YesNo);
			//	if (res == DialogResult.Yes) {
			//		DataViewModel.SaveCustomerToDB();
			//	}
			//}


			dataGridViewCustomers.DataSource = DataViewModel.DataTableCustomer;
			DataViewModel.UpdateCustomerFromDB();
			dataGridViewCustomers.DataSource = DataViewModel.DataTableCustomer;
			if (Mode == OpMode.Initializing) {
                InitCustomerList();
            }
            SetVisibleCustomerList();

            DataViewModel.UpdateProductFromDB();
            if (Mode == OpMode.Initializing) {
                InitProductList();
            }
            SetVisibleProductList();

			SelectCustomerRow(selectedCustomer);

            UpdateAuctionSummary();
            SetProductListHiding();
            UpdateSummaryPerCustomer();
            DataViewModel.fDataGridCustomerIsChanged = false;

            TableAutoSizeToggleOnCustomer(dataGridViewCustomers);
            TableAutoSizeToggleOnProduct(dataGridViewProducts);
            Cursor.Current = Cursors.Default;
            DataViewModel.fUpdatingCustomerList = false;
            DataViewModel.fUpdatingProductList = false;
            Trace.WriteLine("UpdateFromDB -finished");
        }

        private void SetProductListHiding() {
            var selectedCustomerId = GetSelectedCustomerId();
            if (DataViewModel.fUpdatingCustomerList) {
                return;
            }

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Trace.WriteLine("UpdateProductListHiding:" + Mode);
            dataGridViewProducts.ClearSelection();
            dataGridViewProducts.CurrentCell = null;

            dataGridViewProducts.SuspendLayout();
            if (Mode == OpMode.Auctioning || Mode == OpMode.Showing) {
                foreach (DataGridViewRow productRow in dataGridViewProducts.Rows) {
					productRow.Visible = true;
	                SetProductRowColor(productRow);
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
                            var productCustomerId = (int) productRow.Cells["CustomerId"].Value;
                            productRow.Visible = selectedCustomerId == productCustomerId;
                        }
                    }
                    catch {
                        Trace.WriteLine("Internal error");
                    }
                }
            }
            dataGridViewProducts.ResumeLayout();
            stopwatch.Stop();
            Trace.WriteLine("UpdateProductListHiding - finished " + stopwatch.ElapsedMilliseconds);
        }

        private int GetSelectedCustomerId()
        {
            var rows = dataGridViewCustomers.SelectedRows;
            if (rows.Count == 1 && rows[0].Cells["id"].Value != DBNull.Value &&
                rows[0].Cells["id"].Value != null)
            {
                return (int)rows[0].Cells["id"].Value;
            }
            return 0;
        }

		private Customer GetSelectedCustomer() {
			var rows = dataGridViewCustomers.SelectedRows;
			if (rows.Count == 1 && rows[0].Cells["id"].Value != DBNull.Value && rows[0].Cells["id"].Value != null) {
				return new Customer(rows[0]);
			}
			return null;
		}

        private string GetSelectedCustomerColumn(string columnId)
        {
            var rows = dataGridViewCustomers.SelectedRows;
            if (rows.Count == 1 && rows[0].Cells[columnId].Value != DBNull.Value &&
                rows[0].Cells[columnId].Value != null)
            {
                return rows[0].Cells[columnId].Value.ToString();
            }
            return "";
        }

        private void SelectCustomerRow(int customerId) {
	        bool found = false;
            if (Mode != OpMode.Initializing) {
                foreach (DataGridViewRow row in dataGridViewCustomers.Rows) {
                    if (row.Cells["id"].Value != null && row.Cells["id"].Value != DBNull.Value) {
                        var rowId = (int) row.Cells["id"].Value;
	                    if (customerId == rowId) {
		                    if (row.Displayed == false && row.Visible) {
			                    dataGridViewCustomers.FirstDisplayedScrollingRowIndex = row.Index;
		                    }
		                    row.Selected = true;
		                    found = true;
		                    break;
	                    }
                    }
                }
	            if (!found) {
		            int irow = dataGridViewCustomers.Rows.GetLastRow(DataGridViewElementStates.Visible) - 1;
		            var rowLast = dataGridViewCustomers.Rows[irow];
		            if (rowLast.Displayed == false && rowLast.Visible) {
			            dataGridViewCustomers.FirstDisplayedScrollingRowIndex = irow;
		            }
		            rowLast.Selected = true;
	            }
            }
        }

	    private void SelectProductRow(string productId) {
			if (Mode != OpMode.Initializing) {
				foreach (DataGridViewRow row in dataGridViewProducts.Rows) {
					if (row.Cells["id"].Value != null && row.Cells["id"].Value != DBNull.Value) {
						var rowId = ((int)row.Cells["id"].Value).ToString();
						if (productId == rowId) {
							if (row.Displayed == false && row.Visible) {
								dataGridViewProducts.FirstDisplayedScrollingRowIndex = row.Index;
							}
							row.Selected = true;
							break;
						}
					}
				}
			}		    
	    }

        private void SelectCustomerRowBasedOnPhone(string strPhone) {
            if (Mode != OpMode.Initializing) {
                foreach (DataGridViewRow row in dataGridViewCustomers.Rows) {
                    if (row.Cells["Phone"].Value != null && row.Cells["Phone"].Value != DBNull.Value) {
                        if (strPhone == row.Cells["Phone"].Value.ToString()) {
                            row.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        private Product GetSelectedProduct() {
            var rows = dataGridViewProducts.SelectedRows;
            if (rows.Count > 0) {
                return new Product(rows[0]);
            }
            return null;
        }

		private void SelectProductPriceColumns() {
			var rows = dataGridViewProducts.SelectedRows;
			if (rows.Count > 0) {
				rows[0].Cells["Price"].Selected = true;
			}
		}


        private int GetSelectedProductId() {
            var rows = dataGridViewProducts.SelectedRows;
            if (rows.Count == 1) {
                return (int) rows[0].Cells["id"].Value;
            }
            return -1;
        }

        private void InitCustomerList() {
            try {
				Trace.WriteLine("InitCustomerList");
				dataGridViewCustomers.DataSource = DataViewModel.DataTableCustomer;

                if (dataGridViewCustomers.ColumnCount < 5) {
                    return;
                }

                // ReSharper disable PossibleNullReferenceException
                dataGridViewCustomers.Columns["id"].HeaderText = "Id";
                dataGridViewCustomers.Columns["id"].ReadOnly = true;
                dataGridViewCustomers.Columns["Name"].HeaderText = "Namn";
                dataGridViewCustomers.Columns["Phone"].HeaderText = "Mobilnr";
                dataGridViewCustomers.Columns["Comment"].HeaderText = "Not";
                dataGridViewCustomers.Columns["Finished"].HeaderText = "Klar";

                dataGridViewCustomers.Columns["TimeStamp"].Visible = false;
               // ReSharper restore PossibleNullReferenceException
            }
            catch (Exception ex) {
                MessageBox.Show("Error " + ex.Message);
            }
        }

        private void SetVisibleCustomerList() {
            // ReSharper disable once PossibleNullReferenceException
			dataGridViewCustomers.Visible = Mode != OpMode.Auctioning;
	        if (dataGridViewCustomers.ColumnCount < 5) {
		        return;
	        }
	        // ReSharper disable once PossibleNullReferenceException
            dataGridViewCustomers.Columns["Finished"].Visible = Mode == OpMode.Paying;
            dataGridViewCustomers.MultiSelect = Mode == OpMode.Paying;

            if (Mode == OpMode.Paying) {
	            dataGridViewCustomers.CurrentCell = null;
                foreach (DataGridViewRow customerRow in dataGridViewCustomers.Rows) {
                    if (customerRow.Cells["id"].Value != null) {
                        var count = DataViewModel.DataTableProduct.CountForCustomer((int)customerRow.Cells["id"].Value);
                        customerRow.Visible = count > 0;
                    }
                }
            }
        }

        private void UpdateAuctionSummary() {
            textBoxSoldCount.Visible = Mode == OpMode.Auctioning || Mode == OpMode.Paying;
            labelSoldCount.Visible = Mode == OpMode.Auctioning || Mode == OpMode.Paying;
            textBoxAmount.Visible = Mode == OpMode.Auctioning || Mode == OpMode.Paying;
            labelSoldAmount.Visible = Mode == OpMode.Auctioning || Mode == OpMode.Paying;
            buttonSave.Visible = Mode == OpMode.Receiving;
	        buttonSave.Enabled = DataViewModel.fDataGridCustomerIsChanged;
            buttonSendSMS.Visible = Mode == OpMode.Paying;
            buttonSendResult.Visible = (Mode == OpMode.Paying);
            buttonExport.Visible = (Mode == OpMode.Showing);

            if (DataViewModel.fUpdatingCustomerList) {
                return;
            }

            if (DataViewModel.DataTableProduct == null) {
                return;
            }

            var totalcountAuction = DataViewModel.DataTableProduct.TotalCountAuction();
            var totalcountFixed = DataViewModel.DataTableProduct.TotalCountFixedPrice();
            textBoxTotalCount.Text = totalcountAuction + " + " + totalcountFixed;

            if (Mode == OpMode.Auctioning || Mode == OpMode.Paying) {
                var totalSoldCount = DataViewModel.DataTableProduct.TotalSoldCount();
                textBoxSoldCount.Text = totalSoldCount.ToString();
                var totalSoldAmount = DataViewModel.DataTableProduct.TotalSoldAmount();
	            var totalProfit = DataViewModel.DataTableProduct.TotalProfit();
                textBoxAmount.Text = totalSoldAmount + " (" + totalProfit + ")";
            }
	        if (Mode == OpMode.Paying) {
		        textBoxToPay.Text = DataViewModel.LeftToPay().ToString();
	        }
        }

        private void InitProductList() {
            try {
                dataGridViewProducts.DataSource = DataViewModel.DataTableProduct;

                // ReSharper disable PossibleNullReferenceException
                dataGridViewProducts.Columns["Label"].HeaderText = "Id";
                dataGridViewProducts.Columns["Name"].HeaderText = "Namn";
                dataGridViewProducts.Columns["Type"].HeaderText = "Typ";
                dataGridViewProducts.Columns["Description"].HeaderText = "Beskr.";
                dataGridViewProducts.Columns["Note"].HeaderText = "Not";
                dataGridViewProducts.Columns["Price"].HeaderText = "Pris";
                dataGridViewProducts.Columns["FixedPrice"].HeaderText = "LoppisPris";

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
                dataGridViewProducts.Columns["FixedPrice"].Visible = true;
                // ReSharper restore PossibleNullReferenceException
            }
            catch (Exception ex) {
                MessageBox.Show("Error " + ex.Message);
            }
        }

	    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
		private static void TableAutoSizeToggleOnCustomer(DataGridView grid) {
			if (grid.ColumnCount < 5) {
				return;
			}

			grid.Columns["id"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["Comment"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			grid.Columns["Finished"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

	    [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
	    private static void TableAutoSizeToggleOnProduct(DataGridView grid) {
			grid.Columns["Label"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
			grid.Columns["Note"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["Price"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			grid.Columns["FixedPrice"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
		}

        private static void TableAutoSizeToggleOffCustomer(DataGridView grid)
        {
            foreach (DataGridViewColumn col in grid.Columns) {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
        }

		private static void TableAutoSizeToggleOffProduct(DataGridView grid) {
			foreach (DataGridViewColumn col in grid.Columns) {
				col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
			}
		}

        private void SetVisibleProductList() {

	        dataGridViewProducts.Visible = (Mode != OpMode.Initializing);   

            dataGridViewProducts.DataSource = DataViewModel.DataTableProduct;
            // ReSharper disable PossibleNullReferenceException
			//dataGridViewProducts.Columns["FixedPrice"].Visible = Mode != OpMode.Auctioning;
			dataGridViewProducts.Columns["Note"].Visible = Mode == OpMode.Auctioning || Mode == OpMode.Paying;
            dataGridViewProducts.Columns["Note"].ReadOnly = Mode != OpMode.Auctioning;
            dataGridViewProducts.Columns["Price"].ReadOnly = Mode != OpMode.Auctioning;
            dataGridViewProducts.Columns["Price"].Visible = Mode == OpMode.Auctioning || Mode == OpMode.Paying ||
                                                            Mode == OpMode.Showing;
			dataGridViewProducts.Columns["Description"].Visible = Mode == OpMode.Auctioning || Mode == OpMode.Showing;
			dataGridViewProducts.EditMode = Mode == OpMode.Auctioning
                ? DataGridViewEditMode.EditOnKeystrokeOrF2
                : DataGridViewEditMode.EditProgrammatically;
            // ReSharper restore PossibleNullReferenceException
        }

        private void UpdateSummaryPerCustomer() {
            Trace.WriteLine("UpdateSummaryPerCustomer");
            var fShowSummary = Mode == OpMode.Paying;

            textBoxTotalAmount.Visible = fShowSummary;
            labelTotalPerCustomer.Visible = fShowSummary;
            textBoxNetAmount.Visible = fShowSummary;
            labelNetPerCustomer.Visible = fShowSummary;
            textBoxUnsold.Visible = fShowSummary;
            labelUnsoldPerCustomer.Visible = fShowSummary;

            if (fShowSummary) {
                var foundCustomer = GetSelectedCustomerId();
                try {
                    if (foundCustomer > 0 && DataViewModel.DataTableProduct != null) {
                        var totalAmount = DataViewModel.DataTableProduct.TotalAmountForCustomer(foundCustomer);
                        textBoxTotalAmount.Text = totalAmount.ToString();
                        var netAmount = DataViewModel.DataTableProduct.NetAmountForCustomer(foundCustomer);
                        textBoxNetAmount.Text = netAmount.ToString();
                        var noOfUnsold = DataViewModel.DataTableProduct.NoOfUnsoldForCustomer(foundCustomer);
                        textBoxUnsold.Text = noOfUnsold.ToString();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show("Error in UpdateSummaryPerCustomer: " + ex.Message);
                }
            }
            buttonNewProduct.Visible = Mode == OpMode.Receiving;
            buttonNewProduct.Enabled = !DataViewModel.fDataGridCustomerIsChanged;

            buttonNewCustomer.Visible = Mode == OpMode.Receiving;
            buttonNewCustomer.Enabled = !DataViewModel.fDataGridCustomerIsChanged;

            var productCurrent = GetSelectedProduct();

            buttonDeleteProduct.Visible = Mode == OpMode.Receiving;
            buttonDeleteProduct.Enabled = !DataViewModel.fDataGridCustomerIsChanged && productCurrent != null;

            buttonSoldFixedPrice.Visible = Mode == OpMode.Showing;
            buttonSoldFixedPrice.Enabled = !DataViewModel.fDataGridCustomerIsChanged && productCurrent != null &&
                                           productCurrent.IsFixedPrice && !productCurrent.IsSold;

            buttonSearch.Visible = Mode == OpMode.Showing;
        
        }

        public int SearchProductTable(string searchText) {
            dataGridViewProducts.ClearSelection();
            int found = 0;
            foreach (DataGridViewRow row in dataGridViewProducts.Rows) {
                string s1 = (string)row.Cells["Description"].Value;
                string s2 = (string)row.Cells["Name"].Value;

                if ((s1.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    s2.IndexOf(searchText, StringComparison.CurrentCultureIgnoreCase) >= 0) {
                    row.Selected = true;
                    if (found == 0) {
                        dataGridViewProducts.FirstDisplayedScrollingRowIndex = row.Index;
                    }
                    found++;
                }
            }
            MessageBox.Show("Hittade " + found.ToString() + " objekt!");
            return found;
        }

	    private void SetProductRowColor(DataGridViewRow row) {
			var sold = ((int)row.Cells["Price"].Value > 0);
			row.DefaultCellStyle.BackColor = sold ? Color.LightGray : Color.White;
	    }


	    #region  Event handling

        private void dataGridViewCustomers_SelectionChanged(object sender, EventArgs e) {
            if (Mode != OpMode.Initializing) {
                if (!DataViewModel.fUpdatingCustomerList && !DataViewModel.fUpdatingProductList) {
                    Trace.WriteLine("dataGridViewCustomers_SelectionChanged");
                    SetProductListHiding();
                    UpdateSummaryPerCustomer();
                }
            }
        }

        private void dataGridViewCustomers_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F2 && Mode == OpMode.Receiving) {
	            dataGridViewCustomers_CellDoubleClick(sender, null);
            }
			if (e.KeyCode == Keys.Delete && Mode == OpMode.Receiving) {
				var res = MessageBox.Show("Är du säker på att du vill radera kunden?", "ConAuction",
                    MessageBoxButtons.YesNo);
				if (res == DialogResult.Yes) {
					DataViewModel.DeleteCustomerToDB(GetSelectedCustomerId());
				}
				UpdateFromDb();
			}
			if (e.KeyCode == Keys.End) {
				SelectCustomerRow(999999);
			}
			if (e.KeyCode == Keys.Home) {
				SelectCustomerRow(1);
			}
		}

        private void dataGridViewCustomers_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			//if (Mode == OpMode.Receiving) {
			//	DataViewModel.fDataGridCustomerIsChanged = true;
			//	UpdateAuctionSummary();
			//	UpdateSummaryPerCustomer();
			//	if (
			//		string.Compare(dataGridViewCustomers.Columns[e.ColumnIndex].Name, "Name",
			//			StringComparison.OrdinalIgnoreCase) == 0 &&
			//		dataGridViewCustomers.Rows[e.RowIndex].Cells["Name"].Value.ToString() != "") {
			//		dataGridViewCustomers.Rows[e.RowIndex].Cells["Phone"].Selected = true;
			//		dataGridViewCustomers.BeginEdit(false);
			//	}
			//}
	        if (Mode == OpMode.Paying) {
				//DataViewModel.fDataGridCustomerIsChanged = true;
				Customer customer = new Customer(dataGridViewCustomers.Rows[e.RowIndex]);
				DataViewModel.SaveCustomerToDB(customer);
				UpdateAuctionSummary();
				UpdateFromDb();
	        }
        }

        private void dataGridViewCustomers_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (Mode == OpMode.Paying && e.RowIndex >= 0 && e.ColumnIndex >= 0) {
                var cell = dataGridViewCustomers.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.ValueType == typeof (bool)) {
                    if (cell.Value == DBNull.Value) {
                        cell.Value = new bool();
                        cell.Value = true;
                    }
                    else {
                        cell.Value = !(bool) cell.Value;
                    }
					Customer customer = new Customer(dataGridViewCustomers.Rows[e.RowIndex]);
					DataViewModel.SaveCustomerToDB(customer);
					UpdateAuctionSummary();
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e) {
			//var strPhone = GetSelectedCustomerColumn("Phone");
			//DataViewModel.SaveCustomerToDB();
			//UpdateFromDb();
			//SelectCustomerRowBasedOnPhone(strPhone);
        }

        private void buttonNewProduct_Click(object sender, EventArgs e) {
            var customerId = GetSelectedCustomerId();
            if (customerId > 0 && DataViewModel.DataTableProduct != null) {
                var productIdLast = DataViewModel.DataTableProduct.GetLastProductIdForCustomer(customerId);
                Product productLast = null;
                if (productIdLast > 0) {
                    productLast = new Product(DataViewModel.DataTableProduct.GetRowForProductId(productIdLast));
                }

                var productNew = new Product();
	            var customer = GetSelectedCustomer();

                var form = new FormEditProduct(productNew, productLast, Mode, customer.NumberAndName);

                if (form.ShowDialog(this) == DialogResult.OK) {
                    DataViewModel.InsertNewProductToDB(productNew, customerId);
                    UpdateFromDb();
                }
            }
        }

        private void dataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            var productCurrent = GetSelectedProduct();

            if (productCurrent != null) {
                if (Mode == OpMode.Auctioning) {
                    dataGridViewProducts.BeginEdit(true);
                }
                else {
	                int productId = Int32.Parse(productCurrent.Id);
	                var id = DataViewModel.DataTableProduct.GetCustomerIdForProductId(productId);
	                var customer = DataViewModel.DataTableCustomer.GetCustomerFromId(id);
                    var form = new FormEditProduct(productCurrent, null, Mode, customer.NumberAndName);
                    if (form.ShowDialog() == DialogResult.OK) {
                        DataViewModel.SaveProductToDB(productCurrent);
                        UpdateFromDb();
                    }
                }
	            SelectProductRow(productCurrent.Id);
            }
        }

        private void dataGridViewProducts_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.F2 && Mode == OpMode.Auctioning) {
				SelectProductPriceColumns();
				dataGridViewProducts.BeginEdit(false);
            }
			if (e.KeyCode == Keys.F1 && Mode == OpMode.Auctioning) {
				SelectProductPriceColumns();
				dataGridViewProducts.BeginEdit(true);
			}
		}

        private void dataGridViewProducts_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
            if (Mode == OpMode.Auctioning && DataViewModel.DataTableProduct != null &&
                DataViewModel.DataTableProduct.Columns[e.ColumnIndex].ColumnName == "Price") {
                Trace.WriteLine("CellValueChanged row=" + e.RowIndex + " col=" + e.ColumnIndex);

                var row = DataViewModel.DataTableProduct.Rows[e.RowIndex];
	            if (row["Price"] != DBNull.Value) {
		            DataViewModel.SaveProductPriceToDB((int) row["id"], (int) row["Price"], row["Note"].ToString());
					UpdateAuctionSummary();
				}
				else {
					UpdateFromDb();
				}
			}
            if (Mode == OpMode.Auctioning && DataViewModel.DataTableProduct != null &&
                DataViewModel.DataTableProduct.Columns[e.ColumnIndex].ColumnName == "Note") {
                Trace.WriteLine("CellValueChanged row=" + e.RowIndex + " col=" + e.ColumnIndex);

                var row = DataViewModel.DataTableProduct.Rows[e.RowIndex];
	            if (row["Price"] != DBNull.Value) {
		            DataViewModel.SaveProductPriceToDB((int) row["id"], (int) row["Price"], row["Note"].ToString());
		            UpdateAuctionSummary();
	            }
	            else {
		            UpdateFromDb();
	            }
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
			//if (DataViewModel.fDataGridCustomerIsChanged) {
			//	var res = MessageBox.Show("Vill du spara ändringar?", "DB", MessageBoxButtons.YesNo);
			//	if (res == DialogResult.Yes) {
			//		DataViewModel.SaveCustomerToDB();
			//	}
			//}
        }

        private void buttonUpdate_Click(object sender, EventArgs e) {
            UpdateFromDb();
        }

        private void buttonDeleteProduct_Click(object sender, EventArgs e) {
            var productId = GetSelectedProductId();
            if (productId > 0) {
                var res = MessageBox.Show("Är du säker på att du vill radera produkten?", "ConAuction",
                    MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes) {
                    DataViewModel.DeleteProductToDB(productId);
					UpdateFromDb();
                }
            }
        }

        private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e) {
            if (!DataViewModel.fUpdatingProductList && !DataViewModel.fUpdatingCustomerList) {
                Trace.WriteLine("dataGridViewProducts_SelectionChanged");
                UpdateSummaryPerCustomer();
            }
        }

        private void comboBoxMode_SelectedIndexChanged(object sender, EventArgs e) {
            Mode = (OpMode) comboBoxMode.SelectedIndex;
            if (Mode == OpMode.Overhead) {
                var form = new FormProductDisplay(DataViewModel.DataTableProduct);
                form.ShowDialog();
                Mode = OpMode.Initializing;
                comboBoxMode.SelectedIndex = (int) OpMode.Initializing;
            }
            Trace.WriteLine("comboBoxMode_SelectedIndexChanged");

            UpdateFromDb();
        }

        private void buttonSendSMS_Click(object sender, EventArgs e) {
            var rows = dataGridViewCustomers.SelectedRows;

	        if (rows.Count >= 1) {
				var form = new FormSendSMS(rows.Cast<DataGridViewRow>().Where(row => row.Visible).ToList());
                form.ShowDialog(this);
            }
        }

        private void comboBoxMode_MouseWheel(object sender, MouseEventArgs e) {
            ((HandledMouseEventArgs) e).Handled = true;
        }

        private void buttonSoldFixedPrice_Click(object sender, EventArgs e) {
            var productCurrent = GetSelectedProduct();

            if (productCurrent != null) {
                if (Mode == OpMode.Showing) {
                    if (productCurrent.SoldForFixedPrice()) {
                        DataViewModel.SaveProductToDB(productCurrent);
                        UpdateFromDb();
                    }
                }
            }
        }

        private void buttonNewCustomer_Click(object sender, EventArgs e) {
            var customerNew = new Customer();
            var form = new FormEditCustomer(customerNew);

            if (form.ShowDialog(this) == DialogResult.OK) {
                DataViewModel.InsertNewCustomerToDB(customerNew);
                UpdateFromDb();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            var formSearch = new FormSearch(this);
            formSearch.ShowDialog();
        }

        private void buttonSendResult_Click(object sender, EventArgs e) {

            var foundCustomer = GetSelectedCustomerId();
            try {
                if (foundCustomer > 0 && DataViewModel.DataTableProduct != null) {
                    var report = DataViewModel.DataTableProduct.ExportCustomerReceipt(foundCustomer, GetSelectedCustomerColumn("Name"));

                    var form = new FormSendMail(report);
                    form.ShowDialog(this);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonExport_Click(object sender, EventArgs e) {
            var text = DataViewModel.DataTableProduct.ExportProductsToJson();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, "ConAuction.json");
            File.WriteAllText(path, text);
        }

		private void dataGridViewCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {

			Customer customer = GetSelectedCustomer();
			if (customer == null) {
				return;
			}

			var form = new FormEditCustomer(customer);

			if (form.ShowDialog(this) == DialogResult.OK) {
				DataViewModel.SaveCustomerToDB(customer);
				UpdateFromDb();
			}
		}
    }

    #endregion
}