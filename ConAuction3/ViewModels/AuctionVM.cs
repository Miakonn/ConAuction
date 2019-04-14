using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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


		public MyCommand NewCustomerCommand { get; private set; }
		public ICommand ShowCustomerCommand { get; private set; }
		public ICommand SortCommand { get; private set; }
		public ICommand NewProductCommand { get; private set; }
		public ICommand ShowProductCommand { get; private set; }
	
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
				// OnPropertyChanged("Products");
			}
		}

		public string StatusTotalCount => String.Format("Antal objekt {0} + {1}", counter, 0);


        public AuctionVM() {
			_dbAccess = new DbAccess();
		
            bool fStarted;
            do
            {
                fStarted = _dbAccess.InitDB();
                if (!fStarted)
                {
                    var res = MessageBox.Show("Vill du försöka kontakta databasen igen?", null, MessageBoxButtons.RetryCancel);
                    if (res != DialogResult.Retry)
                    {
                        Application.Exit();
                        Environment.Exit(-1);
                    }
                }
            } while (!fStarted);
            
			UpdateCustomers();
			UpdateProducts();
            
            NewCustomerCommand = new MyCommand(NewCustomer, () => CurrentMode == OpMode.Receiving);
			ShowCustomerCommand = new MyCommand(ShowCustomer, () => CurrentMode == OpMode.Receiving);
			NewProductCommand = new MyCommand(NewProduct, () => CurrentMode == OpMode.Receiving);
			ShowProductCommand = new MyCommand(ShowProduct, () => CurrentMode == OpMode.Receiving);
			

			CurrentMode = OpMode.Receiving;
		}

		public void UpdateCustomers() {
			_customersVM =  new CustomerListVM(_dbAccess.ReadAllCustomers());
			OnPropertyChanged("Customers");
			OnPropertyChanged("StatusTotalCount");
		}

		public void UpdateProducts() {
			_products = new ProductListVM(_dbAccess.ReadAllProducts());
			OnPropertyChanged("Customers");
			OnPropertyChanged("StatusTotalCount");
		}

		public void NewCustomer() {
			var inputDialog = new CustomerDlg(new Customer());
			if (inputDialog.ShowDialog() == true) {
				var customer = inputDialog.Result;

				_dbAccess.InsertNewCustomerToDB(customer);
			}
			UpdateCustomers();
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
		}

		public bool NewCustomer_CanExecute() {
			return true;
		}

		public void NewProduct() {
            if (SelectedCustomer == null) {
                return;
            }
			var inputDialog = new ProductDlg(new Product(), SelectedCustomer);
			if (inputDialog.ShowDialog() == true) {
				var product = inputDialog.Result;

				_dbAccess.InsertNewProductToDB(product);
			}
			UpdateCustomers();
			OnPropertyChanged("StatusTotalCount");
		}

		public bool NewProduct_CanExecute() {
			return SelectedCustomer != null;
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
