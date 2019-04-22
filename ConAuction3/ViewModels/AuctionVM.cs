using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using ConAuction3.DataModels;
using ConAuction3.Views;

namespace ConAuction3.ViewModels  {

	public enum OpMode {
		Initializing = 0,
		Receiving = 1,
		Showing = 2,
		Auctioning = 3,
		Paying = 4,
		Overhead = 5
	}

	  public class ComboBoxItemOpMode {
        public OpMode ValueMode{ get; set; }
        public string ValueString { get; set; }
    }

    class AuctionVM : INotifyPropertyChanged {
		private  CustomerListVM _customersVM;

		private ProductListVM _products;

		private readonly DbAccess _dbAccess;

		private ObservableCollection<Customer> _customersObserved;
		
		private ObservableCollection<Product>  _productsObserved;


		public MyCommand NewCustomerCommand { get; }
		public ICommand ShowCustomerCommand { get; }
		public ICommand NewProductCommand { get; }
		public ICommand ShowProductCommand { get; }
        public ICommand DeleteProductCommand { get; }
        public ICommand SortCustomerCommand { get; }
        public ICommand CancelCommand { get; }

        public int counter = 0;
		private Customer _selectedCustomer;
		private Product _selectedProduct;

        public List<ComboBoxItemOpMode> OpEnumList =>
            new List<ComboBoxItemOpMode>() {
                new ComboBoxItemOpMode {ValueMode = OpMode.Initializing, ValueString = "Välj mod"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Receiving, ValueString = "Inlämning"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Showing, ValueString = "Visning"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Auctioning, ValueString = "Auktion"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Paying, ValueString = "Utbetalning"},
                new ComboBoxItemOpMode {ValueMode = OpMode.Overhead, ValueString = "Projektor"},
            };

        public OpMode CurrentMode { get; set; }
		
		public ObservableCollection<Customer> Customers => _customersVM.ObservableCustomers;

        public ObservableCollection<Product> Products {
			get {
				if (SelectedCustomer != null) {
					return new ObservableCollection<Product>(_products.ProductList.Where(p => p.CustomerId == SelectedCustomer.Id));
				}
				else {
					return new ObservableCollection<Product>(_products.ProductList);
				}
			}
		}

		public Customer SelectedCustomer {
			get => _selectedCustomer;

            set {
				_selectedCustomer = value; 
				OnPropertyChanged("Products");
			}
		}

		public Product SelectedProduct {
			get => _selectedProduct;

            set {
				_selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        // ReSharper disable once UnusedMember.Global
        public string StatusAuctionCount => $"Antal auktion: {_products.TotalCountAuction}";

        // ReSharper disable once UnusedMember.Global
        public string StatusJumbleCount => $"Antal loppis: {_products.TotalCountJumble}";

        public string StatusMsgTotal => $"?";
        
        public string StatusMsgArchiving => $"?";

        public AuctionVM() {
			_dbAccess = new DbAccess();
		
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
            
			UpdateCustomers();
			UpdateProducts();
            
            NewCustomerCommand = new MyCommand(NewCustomer, NewCustomer_CanExecute);
			ShowCustomerCommand = new MyCommand(ShowCustomer, ShowCustomer_CanExecute);
			NewProductCommand = new MyCommand(NewProduct, NewProduct_CanExecute);
			ShowProductCommand = new MyCommand(ShowProduct, ShowProduct_CanExecute);
            DeleteProductCommand = new MyCommand(DeleteProduct, DeleteProduct_CanExecute);

            SortCustomerCommand = new MyCommand(SortCustomer, () => true);
            CancelCommand = new MyCommand(ExitProgram, () => true);

            CurrentMode = OpMode.Receiving;
		}

        public void ExitProgram()
        {
            Application.Exit();
            Environment.Exit(1);
        }


        public void SortCustomer()
        {
          Trace.WriteLine("dfskjhsdlk");
        }


        public void UpdateCustomers() {
            var selectedLastCustomer = SelectedCustomer;
			_customersVM =  new CustomerListVM(_dbAccess.ReadAllCustomers());
			OnPropertyChanged("Customers");
			OnPropertyChanged("StatusTotalCount");
            OnPropertyChanged("Products");

            SelectedCustomer = selectedLastCustomer;
        }

		public void UpdateProducts() {
            var selectedLastProduct = SelectedProduct;
			_products = new ProductListVM(_dbAccess.ReadAllProducts());
			OnPropertyChanged("Customers");
			OnPropertyChanged("StatusTotalCount");
            OnPropertyChanged("Products");

            SelectedProduct = selectedLastProduct;
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

        public bool NewCustomer_CanExecute() {
			return CurrentMode == OpMode.Receiving;
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

        public bool ShowCustomer_CanExecute()
        {
            return CurrentMode == OpMode.Receiving && SelectedCustomer != null;
        }

        public void NewProduct() {
            if (SelectedCustomer == null) {
                return;
            }

            var inputDialog = new ProductDlg(new Product(), SelectedCustomer);
			if (inputDialog.ShowDialog() == true) {
				var product = inputDialog.Result;

				var idCreated = _dbAccess.InsertNewProductToDB(product);
                UpdateCustomers();
                UpdateProducts();

                SelectedProduct = _products.GetProductFromId(idCreated);
            }

			OnPropertyChanged("StatusTotalCount");
		}

		public bool NewProduct_CanExecute() {
			return CurrentMode == OpMode.Receiving &&  SelectedCustomer != null;
		}

		public void ShowProduct() {
            if (SelectedProduct == null) {
                return;
            }

            var customer = SelectedCustomer ?? _customersVM.GetCustomerFromId(SelectedProduct.CustomerId);
            var inputDialog = new ProductDlg(SelectedProduct, customer);
			if (inputDialog.ShowDialog() == true) {
				var product = inputDialog.Result;

				_dbAccess.SaveProductToDB(product);
			}
			UpdateCustomers();
			OnPropertyChanged("StatusTotalCount");
		}

		public bool ShowProduct_CanExecute() {
			return SelectedProduct != null;
		}

        public void DeleteProduct() {
            if (SelectedProduct == null) {
                return;
            }

            var result = MessageBox.Show("Vill du radera objektet?", "Objekt");
            if (result == DialogResult.OK) {
                _dbAccess.DeleteProductToDB(SelectedProduct.Id);
            }
            UpdateCustomers();
            UpdateProducts();
        }

        public bool DeleteProduct_CanExecute()
        {
            return CurrentMode == OpMode.Receiving && SelectedProduct != null;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}


	public class MyCommand : ICommand {
		public delegate void ExecuteMethod();
		private readonly Action _execute;
		private readonly Func<bool> _canExecute;

		public MyCommand(Action exec) {
			_execute = exec;
			_canExecute = null;
		}

		public MyCommand(Action exec, Func<bool> canExecute) {
			_execute = exec;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter) {
			if (_canExecute != null) {
				return _canExecute();
			}
			return true;
		}

		public void Execute(object parameter) {
			_execute();
		}

		public void RaiseExecuteChanged() {
			CommandManager.InvalidateRequerySuggested();
		}

		public event EventHandler CanExecuteChanged { 
			add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
	}

}
