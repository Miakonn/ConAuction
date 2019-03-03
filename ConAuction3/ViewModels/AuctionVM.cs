using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;	
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ConAuction3.DataModels;
using System.Workflow.ComponentModel.Serialization;

namespace ConAuction3.ViewModels  {

	public enum OpMode {
		Initializing = 0,
		Receiving = 1,
		Showing = 2,
		Auctioning = 3,
		Paying = 4,
		Overhead = 5
	}


	class AuctionVM : INotifyPropertyChanged {
		private DbAccess _dbAccess;

		private OpMode _mode;

		private  List<Customer> _customers;

		private ProductListVM _products;


		private ObservableCollection<Customer> _customersObserved;
		
		private ObservableCollection<Product>  _productsObserved;


		public ICommand NewCustomerCommand { get; private set; }
		public ICommand NewObjectCommand { get; private set; }
		public ICommand SortCommand { get; private set; }

		public int counter = 0;

		public string CurrentModeBind {
			get { return ((int)_mode).ToString(); }
			set { _mode = (OpMode) int.Parse(value); }
		}

		public OpMode CurrentMode {
			get { return _mode; }
			set { _mode = value; }
		}


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
			_dbAccess = new DbAccess();
			_dbAccess.InitDB();
			_customers = _dbAccess.ReadAllCustomers();
			_products = new ProductListVM( _dbAccess.ReadAllProducts());

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

		public event EventHandler CanExecuteChanged { 
			add { CommandManager.RequerySuggested += value; } 
			remove { CommandManager.RequerySuggested -= value; } 
		}
	}

}
