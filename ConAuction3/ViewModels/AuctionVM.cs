using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using ConAuction3.DataModels;

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
		private  List<Customer> _customers;

		private ProductListVM _products;


		private ObservableCollection<Customer> _customersObserved;
		
		private ObservableCollection<Product>  _productsObserved;


		public MyCommand NewCustomerCommand { get; private set; }
		public ICommand NewObjectCommand { get; private set; }
		public ICommand SortCommand { get; private set; }

		public int counter = 0;


		public List<ComboBoxItemOpMode> OpEnumList {
			get {
				return new List<ComboBoxItemOpMode>() {
					new ComboBoxItemOpMode {ValueMode = OpMode.Initializing, ValueString = "Välj mod"},
					new ComboBoxItemOpMode {ValueMode = OpMode.Receiving, ValueString = "Inlämning"},
					new ComboBoxItemOpMode {ValueMode = OpMode.Showing, ValueString = "Visning"},
					new ComboBoxItemOpMode {ValueMode = OpMode.Auctioning, ValueString = "Auktion"},
					new ComboBoxItemOpMode {ValueMode = OpMode.Paying, ValueString = "Utbetalning"},
					new ComboBoxItemOpMode {ValueMode = OpMode.Overhead, ValueString = "Projektor"},
				};
			}
		}

		public OpMode CurrentMode { get; set; }
		
		public ObservableCollection<Customer> Customers {
			get {
				return new ObservableCollection<Customer>(_customers);
			}
		}

		public ObservableCollection<Product> Products {
			get {
				return new ObservableCollection<Product>(_products.ProductList);
			}
		}


		public string StatusTotalCount {
			//get { return String.Format("Antal objekt {0} + {1}", _products.TotalCountAuction, _products.TotalCountJumble); }
			get { return String.Format("Antal objekt {0} + {1}", counter, 0); }
		}



		public AuctionVM() {
			var dbAccess = new DbAccess();
			dbAccess.InitDB();
			_customers = dbAccess.ReadAllCustomers();
			_products = new ProductListVM( dbAccess.ReadAllProducts());

			NewCustomerCommand = new MyCommand(NewCustomer, () => CurrentMode == OpMode.Receiving);
			NewObjectCommand = new MyCommand(NewObject, () => CurrentMode == OpMode.Receiving);

			CurrentMode = OpMode.Receiving;
		}

		public void NewCustomer() {
			counter++;
			OnPropertyChanged("StatusTotalCount");
		}

		public bool NewCustomer_CanExecute() {
			return (counter % 2) == 0 ;
		}

		public void NewObject() {
			counter++;
			OnPropertyChanged("StatusTotalCount");
		}

		public bool NewObject_CanExecute() {
			return (counter % 2) == 1;
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
		private Action _execute;
		private Func<bool> _canExcute;

		public MyCommand(Action exec) {
			_execute = exec;
			_canExcute = null;
		}

		public MyCommand(Action exec, Func<bool> canExecute) {
			_execute = exec;
			_canExcute = canExecute;
		}

		public bool CanExecute(object parameter) {
			if (_canExcute != null) {
				return _canExcute();
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
			add { CommandManager.RequerySuggested += value; } 
			remove { CommandManager.RequerySuggested -= value; } 
		}
	}

}
