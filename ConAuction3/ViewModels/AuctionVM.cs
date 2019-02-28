using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConAuction.DataModels;

namespace ConAuction3.ViewModels {
	class AuctionVM {
		private DbAccess _dbAccess;
		private ObservableCollection<Customer> _customers;
		
		private ObservableCollection<Product>  _products;

		public ObservableCollection<Customer> Customers {
			get {
				return _customers;
			}
		}

		public ObservableCollection<Product> Products {
			get {
				return _products;
			}
		}


		public AuctionVM() {
			_dbAccess = new DbAccess();
			_dbAccess.InitDB();
			_customers = new ObservableCollection<Customer>(_dbAccess.ReadAllCustomers());
			_products = new ObservableCollection<Product>(_dbAccess.ReadAllProducts());
		}

	}
}
