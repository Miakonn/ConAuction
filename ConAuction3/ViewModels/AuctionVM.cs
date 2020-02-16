using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ConAuction3.DataModels;
using ConAuction3.Utilities;
using ConAuction3.Views;
using Application = System.Windows.Forms.Application;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ConAuction3.ViewModels {
    class AuctionVM : INotifyPropertyChanged {
        #region Attributes

        public CustomerListVM CustomersVm { get; private set; }

        public ProductListVM ProductsVm { get; private set; }
        
        public MyCommand NewCustomerCommand { get; }
        public MyCommand ShowCustomerCommand { get; }
        public MyCommand PayCustomerCommand { get; }
        public MyCommand UndoPayCustomerCommand { get; }
        public MyCommand NewProductCommand { get; }
        public MyCommand ShowProductCommand { get; }
        public MyCommand DeleteProductCommand { get; }
        public MyCommand SellProductCommand { get; }
        public MyCommand UndoSoldProductCommand { get; }
        public MyCommand ExportProductsCommand { get; }
        public MyCommand CancelCommand { get; }
        public MyCommand UpdateCommand { get; }
        public MyCommand GotoProductCommand { get; }
        public MyCommand SendSmsCommand { get; }

        // Sort commands
        public ParameterCommand SortCustomerCommand { get; }
        public ParameterCommand SortProductCommand { get; }

        private Customer _selectedCustomer;
        private Product _selectedProduct;
        private OpMode _currentMode;
        private bool _filterJumbleOnly;
        private bool _filterUnsoldOnly;
        private bool _filterCustomerActiveOnly;
        private bool _filterCustomerFinishedOnly;
        private bool _isProductSold;

        private string _LastSmsMessage =
            "Auktionen är avslutad. Du kan hämta ut dina pengar mellan kl 9 och 13 under lördagen. /LinCon auktionen";

        private bool _labelPage1;
        private bool _labelPage2;
        private bool _labelPage3;


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
                if (_currentMode == OpMode.Overhead) {
                    try {
                        var dlg = new ProductDisplayDlg(ProductsVm);
                        dlg.Show();
                        _currentMode = OpMode.Initializing;
                    }
                    catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                    }
                    return;
                }
                
                UpdateAll();
                OnPropertyChanged(nameof(ModeIsShowing));
                OnPropertyChanged(nameof(ModeIsAuctioning));
                OnPropertyChanged(nameof(ModeIsPaying));
                OnPropertyChanged(nameof(ModeIsReceiving));
                OnPropertyChanged(nameof(ModeIsReceivingOrShowing));
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
                    if (CurrentMode == OpMode.Receiving || CurrentMode == OpMode.Paying) {
                        if (_selectedCustomer != null) {
                            ProductsVm.FilterById(_selectedCustomer.Id);
                        }
                        else{
                            ProductsVm.NoFilter();
                        }
                    }
                    _selectedProduct = null;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
                OnPropertyChanged(nameof(SelectedCustomer));
                OnPropertyChanged(nameof(SelectedUnsoldCount));
                OnPropertyChanged(nameof(SelectedAmount));
                OnPropertyChanged(nameof(SelectedNetAmount));
                OnPropertyChanged(nameof(SelectedName));
                OnPropertyChanged(nameof(StatusObjectCount));
                OnPropertyChanged(nameof(PayCustomerCanExecute));
                OnPropertyChanged(nameof(UndoPayCustomerCanExecute));
            }
        }

        public Product SelectedProduct {
            get => _selectedProduct;

            set {
                _selectedProduct = value;

                if (_selectedProduct != null) {
                    OnPropertyChanged("SelectedProduct");
                    _selectedCustomer = CustomersVm.GetCustomerFromId(_selectedProduct.CustomerId);
                    OnPropertyChanged("SelectedCustomer");
                }
                else {
                    OnPropertyChanged("SelectedProduct");
                }
            }
        }

        // ReSharper disable once UnusedMember.Global
        public string StatusCountAuction => $"Inlämnade: {ProductsVm.CountAuction}";

        // ReSharper disable once UnusedMember.Global
        public string StatusCountJumble => $"Inlämnade: {ProductsVm.CountJumble}";

        // ReSharper disable once UnusedMember.Global
        public string StatusCountSoldAuction => ProductsVm.CountSoldAuction > 0 ? $"Sålda: {ProductsVm.CountSoldAuction}" : "";

        // ReSharper disable once UnusedMember.Global
        public string StatusCountSoldJumble => ProductsVm.CountSoldJumble > 0 ? $"Sålda: {ProductsVm.CountSoldJumble}" : "";

        // ReSharper disable once UnusedMember.Global
        public string StatusAmountSoldAuction => ProductsVm.AmountSoldAuction > 0 ? $"Summa: {ProductsVm.AmountSoldAuction}:-" : "";

        // ReSharper disable once UnusedMember.Global
        public string StatusAmountSoldJumble => ProductsVm.AmountSoldJumble > 0 ? $"Summa: {ProductsVm.AmountSoldJumble}:-" : "";

        // ReSharper disable once UnusedMember.Global
        public string StatusLeftToPay => ModeIsPaying ? $"Utbetala: {CustomersVm.LeftToPay()}:-" : "";

        // ReSharper disable once UnusedMember.Global
        public string StatusProfit => ModeIsPaying && ProductsVm.Profit > 0 ? $"Intäkt: {ProductsVm.Profit}:-" : "";

        // ReSharper disable once UnusedMember.Global
        public string StatusObjectCount => ModeIsReceiving && SelectedCustomer != null ? ProductsVm.CustomerStatus(SelectedCustomer.Id) : "";

        // ReSharper disable once UnusedMember.Global
        public bool ModeIsReceivingOrShowing => CurrentMode == OpMode.Receiving || CurrentMode == OpMode.Showing;

        // ReSharper disable once UnusedMember.Global
        public bool ModeIsShowing => CurrentMode == OpMode.Showing;
        
        // ReSharper disable once UnusedMember.Global
        public bool ModeIsPaying => CurrentMode == OpMode.Paying;

        // ReSharper disable once UnusedMember.Global
        public bool ModeIsReceiving => CurrentMode == OpMode.Receiving;

        // ReSharper disable once UnusedMember.Global
        public bool ModeIsAuctioning => CurrentMode == OpMode.Auctioning;
        
        // ReSharper disable once UnusedMember.Global
        public string SelectedUnsoldCount => SelectedCustomer != null ? ProductsVm.NoOfUnsoldForCustomer(SelectedCustomer.Id).ToString() : "";
        
        // ReSharper disable once UnusedMember.Global
        public string SelectedAmount => SelectedCustomer != null ? ProductsVm.TotalAmountForCustomer(SelectedCustomer.Id).ToString() : "";

        // ReSharper disable once UnusedMember.Global
        public string SelectedNetAmount => SelectedCustomer != null ? ProductsVm.NetAmountForCustomer(SelectedCustomer.Id).ToString() : "";

        // ReSharper disable once UnusedMember.Global
        public string SelectedName => SelectedCustomer != null ? SelectedCustomer.NumberAndName : "";

        // ReSharper disable once UnusedMember.Global
        public string VersionStr => "V" + Assembly.GetEntryAssembly()?.GetName().Version;

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

        // ReSharper disable once UnusedMember.Global
        public bool FilterCustomerActiveOnly {
            get => _filterCustomerActiveOnly;
            set {
                _filterCustomerActiveOnly = value;
                CustomersVm.Filter(_filterCustomerActiveOnly, _filterCustomerFinishedOnly);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool FilterCustomerFinishedOnly {
            get => _filterCustomerFinishedOnly;
            set {
                _filterCustomerFinishedOnly = value;
                CustomersVm.Filter(true, _filterCustomerFinishedOnly);
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool LabelPage1 {
            get => _labelPage1;
            set {
                _labelPage1 = value;
                SavingAppSettings();
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool LabelPage2 {
            get => _labelPage2;
            set {
                _labelPage2 = value;
                SavingAppSettings();
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool LabelPage3 {
            get => _labelPage3;
            set {
                _labelPage3 = value;
                SavingAppSettings();
            }
        }

        // ReSharper disable once UnusedMember.Global
        public bool IsProductSold {
            get => _isProductSold;
            set {
                _isProductSold = value;
                OnPropertyChanged("IsProductSold");
            }
        }
        
        #endregion

        public AuctionVM() {
            TryConnectDataBase();

            UpdateAll();

            NewCustomerCommand = new MyCommand(NewCustomer, NewCustomer_CanExecute);
            ShowCustomerCommand = new MyCommand(ShowCustomer, ShowCustomer_CanExecute);
            PayCustomerCommand = new MyCommand(PayCustomer, PayCustomer_CanExecute);
            UndoPayCustomerCommand = new MyCommand(UndoPayCustomer, UndoPayCustomer_CanExecute);
            NewProductCommand = new MyCommand(NewProduct, NewProduct_CanExecute);
            ShowProductCommand = new MyCommand(ShowProduct, ShowProduct_CanExecute);
            DeleteProductCommand = new MyCommand(DeleteProduct, DeleteProduct_CanExecute);
            GotoProductCommand = new MyCommand(GotoProduct, GotoProduct_CanExecute);
            SellProductCommand = new MyCommand(SellProduct, SellProduct_CanExecute);
            UndoSoldProductCommand = new MyCommand(UndoSoldProduct, UndoSoldProduct_CanExecute);
            ExportProductsCommand = new MyCommand(ExportProducts, ExportProducts_CanExecute);
            SendSmsCommand = new MyCommand(SendSms, SendSms_CanExecute);
            UpdateCommand = new MyCommand(UpdateAll);
            CancelCommand = new MyCommand(ExitProgram);

            SortCustomerCommand = new ParameterCommand(CustomersVmSortBy);
            SortProductCommand = new ParameterCommand(ProductsVmSortBy);

            CurrentMode = OpMode.Initializing;

            _labelPage1 = Boolean.Parse(ConfigurationManager.AppSettings["LabelPage1"]);
            _labelPage2 = Boolean.Parse(ConfigurationManager.AppSettings["LabelPage2"]);
            _labelPage3 = Boolean.Parse(ConfigurationManager.AppSettings["LabelPage3"]);

        }

        private void UpdateAll() {
            var selectedLastCustomerId = SelectedCustomer?.Id;
            var selectedLastProductId = SelectedProduct?.Id;
            if (CurrentMode == OpMode.Auctioning) {
                selectedLastCustomerId = null;
            }
            
            CustomersVm = new CustomerListVM(DbAccess.Instance.ReadAllCustomers(), this);
            ProductsVm = new ProductListVM(DbAccess.Instance.ReadAllProducts());
            
            ResetFilter();

            OnPropertyChanged(nameof(StatusCountAuction));
            OnPropertyChanged(nameof(StatusCountJumble));
            OnPropertyChanged(nameof(StatusAmountSoldAuction));
            OnPropertyChanged(nameof(StatusAmountSoldJumble));
            OnPropertyChanged(nameof(StatusCountSoldAuction));
            OnPropertyChanged(nameof(StatusCountSoldJumble));
            OnPropertyChanged(nameof(StatusLeftToPay));
            OnPropertyChanged(nameof(StatusProfit));
            OnPropertyChanged(nameof(Customers));
            OnPropertyChanged(nameof(Products));
            OnPropertyChanged(nameof(VersionStr));


            if (selectedLastProductId.HasValue && selectedLastCustomerId.HasValue) {
                SelectedProduct = ProductsVm.GetProductFromId(selectedLastProductId.Value);
                SelectedCustomer = CustomersVm.GetCustomerFromId(selectedLastCustomerId.Value);
            }
            if (selectedLastProductId.HasValue) {
                SelectedProduct = ProductsVm.GetProductFromId(selectedLastProductId.Value);
            }
            else if (selectedLastCustomerId.HasValue) {
                SelectedCustomer = CustomersVm.GetCustomerFromId(selectedLastCustomerId.Value);
            }
        }

        private void ResetFilter() {
            if (CurrentMode == OpMode.Showing) {
                ProductsVm.Filter(_filterJumbleOnly, _filterUnsoldOnly);
            }
            else if (CurrentMode == OpMode.Paying) {
                CustomersVm.Filter(true, _filterCustomerFinishedOnly);
            }
            else if (CurrentMode == OpMode.Auctioning) {
                ProductsVm.FilterOnlyAuction();
            }
            CustomersVm.SortBy("Name");
            ProductsVm.SortBy("Label");
        }


        private void TryConnectDataBase() {
            bool fStarted;
            do {
                fStarted = DbAccess.Instance.InitDB();
                if (!fStarted) {
                    var res = MessageBox.Show("Vill du försöka kontakta databasen igen?", null, MessageBoxButtons.RetryCancel);
                    if (res != DialogResult.Retry) {
                        Application.Exit();
                        Environment.Exit(-1);
                    }
                }
            } while (!fStarted);
        }

        #region COMMANDS

        public void ExitProgram() {
            Application.Exit();
            Environment.Exit(1);
        }

        private void CustomersVmSortBy(string propertyName) {
            CustomersVm.SortBy(propertyName);
            OnPropertyChanged(nameof(Products));
        }

        private void ProductsVmSortBy(string propertyName) {
            ProductsVm.SortBy(propertyName);
            OnPropertyChanged(nameof(Customers));
        }

        public bool NewCustomer_CanExecute() {
            return CurrentMode == OpMode.Receiving;
        }

        public void NewCustomer() {
            var inputDialog = new CustomerDlg(new Customer());

            int customerId = 0;
            if (inputDialog.ShowDialog() == true) {
                var customer = inputDialog.Result;

                customerId = DbAccess.Instance.InsertNewCustomerToDb(customer);
            }
            UpdateAll();
            if (customerId > 0) {
                SelectedCustomer = CustomersVm.GetCustomerFromId(customerId);
            }
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
                DbAccess.Instance.SaveCustomerToDb(customer);
            }
            UpdateAll();
        }

        public bool PayCustomerCanExecute => PayCustomer_CanExecute();

        public bool PayCustomer_CanExecute() {
            return CurrentMode == OpMode.Paying && SelectedCustomer != null && !SelectedCustomer.IsFinished;
        }

        public void PayCustomer() {
            if (SelectedCustomer == null) {
                return;
            }

            if (!SelectedCustomer.IsFinished) {
                SelectedCustomer.Finished = true;
                DbAccess.Instance.SaveCustomerToDb(SelectedCustomer);
            }

            UpdateAll();
        }

        public bool UndoPayCustomerCanExecute => UndoPayCustomer_CanExecute();

        public bool UndoPayCustomer_CanExecute() {
            return CurrentMode == OpMode.Paying && SelectedCustomer != null && SelectedCustomer.IsFinished;
        }

        public void UndoPayCustomer() {
            if (SelectedCustomer == null) {
                return;
            }

            if (SelectedCustomer.IsFinished) {
                SelectedCustomer.Finished = false;
                DbAccess.Instance.SaveCustomerToDb(SelectedCustomer);
            }

            UpdateAll();
        }

        public bool NewProduct_CanExecute() {
            return CurrentMode == OpMode.Receiving && SelectedCustomer != null;
        }

        public void NewProduct() {
            if (SelectedCustomer == null) {
                return;
            }

            var freeLabel = ProductsVm.GetNextFreeLabel(LabelPage1, LabelPage2, LabelPage3);
            if (freeLabel == 0) {
                MessageBox.Show("Ingen nummersida är vald!");
                return;
            }

            var inputDialog = new ProductDlg(new Product(freeLabel), SelectedCustomer, ProductsVm);
            if (inputDialog.ShowDialog() == true) {
                var product = inputDialog.Result;

                var idCreated = DbAccess.Instance.InsertNewProductToDb(product);
                UpdateAll();

                SelectedProduct = ProductsVm.GetProductFromId(idCreated);
            }

            OnPropertyChanged("StatusTotalCount");
        }

        public bool ShowProduct_CanExecute() {
            return (CurrentMode == OpMode.Receiving || CurrentMode == OpMode.Showing) && SelectedProduct != null;
        }

        public void ShowProduct() {
            if (SelectedProduct == null) {
                return;
            }

            var customer = SelectedCustomer ?? CustomersVm.GetCustomerFromId(SelectedProduct.CustomerId);
            var inputDialog = new ProductDlg(SelectedProduct, customer, null);
            if (inputDialog.ShowDialog() == true) {
                var product = inputDialog.Result;
                DbAccess.Instance.SaveProductToDb(product);
            }
            UpdateAll();
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
                DbAccess.Instance.DeleteProductToDb(SelectedProduct.Id);
                SelectedProduct = null;
            }
            UpdateAll();
        }

        public bool SellProduct_CanExecute() {
            return CurrentMode == OpMode.Showing && SelectedProduct != null && SelectedProduct.IsJumble && !SelectedProduct.IsSold;
        }

        public void SellProduct() {
            if (SelectedProduct == null) {
                return;
            }

            if (SelectedProduct.SellForFixedPrice()) {
                DbAccess.Instance.SaveProductPriceToDb(SelectedProduct.Id, SelectedProduct.Price, SelectedProduct.Note);
            }

            UpdateAll();
        }
        
        public bool UndoSoldProduct_CanExecute() {
            return CurrentMode == OpMode.Showing && SelectedProduct != null && SelectedProduct.IsJumble &&  SelectedProduct.IsSold;
        }

        public void UndoSoldProduct() {
            if (SelectedProduct == null) {
                return;
            }

            if (SelectedProduct.UndoSoldFor()) {
                DbAccess.Instance.SaveProductPriceToDb(SelectedProduct.Id, SelectedProduct.Price, SelectedProduct.Note);
            }

            UpdateAll();
        }

        public bool GotoProduct_CanExecute() {
            return true;
        }

        public void GotoProduct() {
            var labelStr = PromptDialog.Prompt("Produktnummer", "Gå till");
            if (int.TryParse(labelStr, out var label)) {
               SelectedProduct = ProductsVm.GetProductFromLabel(label);
            }
        }

        public bool SendSms_CanExecute() {
            return SelectedCustomer != null;
        }

        public void SendSms() {
            if (SelectedCustomer == null) {
                return;
            }
            _LastSmsMessage = PromptDialog.Prompt("Meddelande till " + SelectedCustomer.Name, "Skicka SMS", _LastSmsMessage);

            SendSMS.Send(SelectedCustomer.Phone, _LastSmsMessage);
        }

        public bool ExportProducts_CanExecute() {
            return CurrentMode == OpMode.Showing || CurrentMode == OpMode.Paying;
        }

        public void ExportProducts() {
            ProductsVm.Statistics();
            if (CurrentMode == OpMode.Showing) {
                using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    openFileDialog.CheckFileExists = false;
                    openFileDialog.FileName = "ConAuction.txt";
                    openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        //Get the path of specified file
                        var filePath = openFileDialog.FileName;
                        var text = filePath.EndsWith("json")
                            ? ProductsVm.ExportProductsToJson()
                            : ProductsVm.ExportCommaSeparated();
                        File.WriteAllText(filePath, text);
                    }
                }
            }
            else if (CurrentMode == OpMode.Paying) {
                using (OpenFileDialog openFileDialog = new OpenFileDialog()) {
                    openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    openFileDialog.CheckFileExists = false;
                    openFileDialog.FileName = "ConAuction-kunder.txt";
                    openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK) {
                        //Get the path of specified file
                        var filePath = openFileDialog.FileName;
                        var text = CustomersVm.ExportCommaSeparated();
                        File.WriteAllText(filePath, text);
                    }
                }
            }
        }


        #endregion

        private void SavingAppSettings() {
            try {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                settings["LabelPage1"].Value = LabelPage1.ToString();
                settings["LabelPage2"].Value = LabelPage2.ToString();
                settings["LabelPage3"].Value = LabelPage3.ToString();

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException) {
                MessageBox.Show("Error writing app settings");
            }
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
