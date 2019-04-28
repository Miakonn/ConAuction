using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using ConAuction3.DataModels;
using ConAuction3.Views;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ConAuction3.ViewModels {
    class AuctionVM : INotifyPropertyChanged {
        #region Attributes

        public CustomerListVM CustomersVm { get; private set; }

        public ProductListVM ProductsVm { get; private set; }

        private readonly DbAccess _dbAccess;

        public MyCommand NewCustomerCommand { get; }
        public MyCommand ShowCustomerCommand { get; }
        public MyCommand NewProductCommand { get; }
        public MyCommand ShowProductCommand { get; }
        public MyCommand DeleteProductCommand { get; }
        public MyCommand SellProductCommand { get; }
        public MyCommand ExportProductsCommand { get; }
        public MyCommand CancelCommand { get; }
        public MyCommand UpdateCommand { get; }

        // Sort commands
        public ParameterCommand SortCustomerCommand { get; }
        public ParameterCommand SortProductCommand { get; }

        private Customer _selectedCustomer;
        private Product _selectedProduct;
        private OpMode _currentMode;
        private bool _filterJumbleOnly;
        private bool _filterUnsoldOnly;

        // ReSharper disable once UnusedMember.Global
        public List<ComboBoxItemOpMode> OpEnumList =>
            new List<ComboBoxItemOpMode> {
                new ComboBoxItemOpMode {ValueMode = OpMode.Initializing, ValueString = "Välj mod"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Receiving, ValueString = "Inlämning"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Showing, ValueString = "Visning"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Auctioning, ValueString = "Auktion"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Paying, ValueString = "Utbetalning"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Overhead, ValueString = "Projektor"},
            };

        public OpMode CurrentMode {
            get => _currentMode;
            set {
                _currentMode = value;
                UpdateAll();
                OnPropertyChanged("FilterCheckVisible");
                OnPropertyChanged("EditingButtonsVisible");
            }
        }

        // ReSharper disable once UnusedMember.Global
        public ICollectionView Customers => CustomersVm.CustomerView;

        // ReSharper disable once UnusedMember.Global
        public ICollectionView Products => ProductsVm.ProductView;

        public Customer SelectedCustomer {
            get => _selectedCustomer;

            set {
                _selectedCustomer = value;
                if (ProductsVm != null) {
                    if (_selectedCustomer != null && CurrentMode == OpMode.Receiving) {
                        ProductsVm.FilterById(_selectedCustomer.Id);
                    }
                    else {
                        ProductsVm.NoFilter();
                    }
                }

                OnPropertyChanged("Products");
                OnPropertyChanged("StatusAuctionCount");
                OnPropertyChanged("StatusJumbleCount");
            }
        }

        public Product SelectedProduct {
            get => _selectedProduct;

            set {
                _selectedProduct = value;

                if (_selectedProduct != null) {
                    _selectedCustomer = CustomersVm.GetCustomerFromId(_selectedProduct.CustomerId);
                }

                OnPropertyChanged("SelectedProduct");
            }
        }

        // ReSharper disable once UnusedMember.Global
        public string StatusAuctionCount => $"Antal auktion: {ProductsVm.TotalCountAuction}";

        // ReSharper disable once UnusedMember.Global
        public string StatusJumbleCount => $"Antal loppis: {ProductsVm.TotalCountJumble}";

        // ReSharper disable once UnusedMember.Global
        public bool FilterCheckVisible => CurrentMode == OpMode.Showing;

        // ReSharper disable once UnusedMember.Global
        public bool EditingButtonsVisible => CurrentMode == OpMode.Receiving;

        // ReSharper disable once UnusedMember.Global
        public bool FilterJumbleOnly {
            get => _filterJumbleOnly;
            set {
                _filterJumbleOnly = value;
                ProductsVm.Filter(_filterJumbleOnly, _filterUnsoldOnly);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool FilterUnsoldOnly {
            get => _filterUnsoldOnly;
            set {
                _filterUnsoldOnly = value;
                ProductsVm.Filter(_filterJumbleOnly, _filterUnsoldOnly);
            }
        }

        #endregion

        public AuctionVM() {
            _dbAccess = new DbAccess();

            TryConnectDataBase();

            UpdateAll();

            NewCustomerCommand = new MyCommand(NewCustomer, NewCustomer_CanExecute);
            ShowCustomerCommand = new MyCommand(ShowCustomer, ShowCustomer_CanExecute);
            NewProductCommand = new MyCommand(NewProduct, NewProduct_CanExecute);
            ShowProductCommand = new MyCommand(ShowProduct, ShowProduct_CanExecute);
            DeleteProductCommand = new MyCommand(DeleteProduct, DeleteProduct_CanExecute);
            SellProductCommand = new MyCommand(SellProduct, SellProduct_CanExecute);
            ExportProductsCommand = new MyCommand(ExportProducts, ExportProducts_CanExecute);
            UpdateCommand = new MyCommand(UpdateAll);
            CancelCommand = new MyCommand(ExitProgram);

            SortCustomerCommand = new ParameterCommand(CustomersVm.SortBy);
            SortProductCommand = new ParameterCommand(ProductsVm.SortBy);

            CurrentMode = OpMode.Initializing;
        }

        private void UpdateAll() {
            UpdateCustomers();
            UpdateProducts();
        }

        public void UpdateCustomers() {
            var selectedLastCustomer = SelectedCustomer;
            CustomersVm = new CustomerListVM(_dbAccess.ReadAllCustomers());
            OnPropertyChanged("Customers");
            OnPropertyChanged("StatusTotalCount");

            SelectedCustomer = selectedLastCustomer;
        }

        private void TryConnectDataBase() {
            bool fStarted;
            do {
                fStarted = _dbAccess.InitDB();
                if (!fStarted) {
                    var res = MessageBox.Show("Vill du försöka kontakta databasen igen?", null, MessageBoxButtons.RetryCancel);
                    if (res != DialogResult.Retry) {
                        Application.Exit();
                        Environment.Exit(-1);
                    }
                }
            } while (!fStarted);
        }

        public void UpdateProducts() {
            var selectedLastProduct = SelectedProduct;
            ProductsVm = new ProductListVM(_dbAccess.ReadAllProducts());
            OnPropertyChanged("Customers");
            OnPropertyChanged("StatusTotalCount");
            OnPropertyChanged("Products");

            SelectedProduct = selectedLastProduct;
        }

        #region COMMANDS

        public void ExitProgram() {
            Application.Exit();
            Environment.Exit(1);
        }

        public bool NewCustomer_CanExecute() {
            return CurrentMode == OpMode.Receiving;
        }

        public void NewCustomer() {
            var inputDialog = new CustomerDlg(new Customer());
            if (inputDialog.ShowDialog() == true) {
                var customer = inputDialog.Result;

                _dbAccess.InsertNewCustomerToDB(customer);
            }
            UpdateCustomers();
            UpdateProducts();
        }

        public bool ShowCustomer_CanExecute() {
            return CurrentMode == OpMode.Receiving && SelectedCustomer != null;
        }

        public void ShowCustomer() {
            if (SelectedCustomer == null) {
                return;
            }

            var inputDialog = new CustomerDlg(SelectedCustomer);
            if (inputDialog.ShowDialog() == true) {
                var customer = inputDialog.Result;
                _dbAccess.SaveCustomerToDB(customer);
            }
            UpdateCustomers();
            UpdateProducts();
        }

        public bool NewProduct_CanExecute() {
            return CurrentMode == OpMode.Receiving && SelectedCustomer != null;
        }

        public void NewProduct() {
            if (SelectedCustomer == null) {
                return;
            }

            var inputDialog = new ProductDlg(new Product(), SelectedCustomer, ProductsVm);
            if (inputDialog.ShowDialog() == true) {
                var product = inputDialog.Result;

                var idCreated = _dbAccess.InsertNewProductToDB(product);
                UpdateCustomers();
                UpdateProducts();

                SelectedProduct = ProductsVm.GetProductFromId(idCreated);
            }

            OnPropertyChanged("StatusTotalCount");
        }

        public bool ShowProduct_CanExecute() {
            return CurrentMode == OpMode.Receiving && SelectedProduct != null;
        }

        public void ShowProduct() {
            if (SelectedProduct == null) {
                return;
            }

            var customer = SelectedCustomer ?? CustomersVm.GetCustomerFromId(SelectedProduct.CustomerId);
            var inputDialog = new ProductDlg(SelectedProduct, customer, null);
            if (inputDialog.ShowDialog() == true) {
                var product = inputDialog.Result;

                _dbAccess.SaveProductToDB(product);
            }
            UpdateAll();
            OnPropertyChanged("StatusTotalCount");
        }

        public bool DeleteProduct_CanExecute() {
            return CurrentMode == OpMode.Receiving && SelectedProduct != null;
        }

        public void DeleteProduct() {
            if (SelectedProduct == null) {
                return;
            }

            var result = MessageBox.Show("Vill du radera objektet?", "Objekt");
            if (result == DialogResult.OK) {
                _dbAccess.DeleteProductToDB(SelectedProduct.Id);
            }
            UpdateAll();
        }

        public bool SellProduct_CanExecute() {
            return CurrentMode == OpMode.Showing && SelectedProduct != null && SelectedProduct.IsJumble;
        }

        public void SellProduct() {
            if (SelectedProduct == null) {
                return;
            }

            if (SelectedProduct.SoldForFixedPrice()) {
                _dbAccess.SaveProductToDB(SelectedProduct);
            }

            UpdateAll();
        }

        public bool ExportProducts_CanExecute() {
            return CurrentMode == OpMode.Showing;
        }

        public void ExportProducts() {
            if (CurrentMode == OpMode.Showing) {
                using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    openFileDialog.FileName = "ConAuction.json";
                    openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        //Get the path of specified file
                        var filePath = openFileDialog.FileName;
                        var text = ProductsVm.ExportProductsToJson();
                        File.WriteAllText(filePath, text);
                    }
                }


            }
            else {
                var text = ProductsVm.ExportCommaSeparated();

                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                path = Path.Combine(path, "ConAuctionSold.txt");
                File.WriteAllText(path, text);
            }
            ProductsVm.ExportProductsToJson();
        }


        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
