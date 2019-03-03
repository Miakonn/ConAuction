using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;	
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConAuction3.DataModels;

namespace ConAuction3.ViewModels  {
	class AuctionVM : INotifyPropertyChanged {
		private DbAccess _dbAccess;

		private  List<Customer> _customers;

		private ProductListVM _products;


		private ObservableCollection<Customer> _customersObserved;
		
		private ObservableCollection<Product>  _productsObserved;

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
			get { return String.Format("Antal objekt {0} + {1}", _products.TotalCountAuction, _products.TotalCountJumble); }
		}



		public AuctionVM() {
			_dbAccess = new DbAccess();
			_dbAccess.InitDB();
			_customers = _dbAccess.ReadAllCustomers();
			_products = new ProductListVM( _dbAccess.ReadAllProducts());
		}


		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion
	}
}
