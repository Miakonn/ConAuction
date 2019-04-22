using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ConAuction3.DataModels;

namespace ConAuction3.ViewModels
{
    class CustomerListVM {
        private readonly List<Customer> _customers;
        private readonly ICollectionView _customerView;

        public CustomerListVM(List<Customer> customers) {
            _customers = customers;
            _customerView = CollectionViewSource.GetDefaultView(customers);
        }
        
        public ICollectionView CustomerView => _customerView;

        public Customer GetCustomerFromId(int id) {
            return _customers.Find(c => c.Id == id);
        }

        public void SortBy(string propertyName) {
            var direction = ListSortDirection.Ascending;
            var sortCurrent = _customerView.SortDescriptions.FirstOrDefault();
            if (sortCurrent != null && sortCurrent.PropertyName == propertyName && sortCurrent.Direction == ListSortDirection.Ascending) {
                direction = ListSortDirection.Descending;
            }
            _customerView.SortDescriptions.Clear();        
            _customerView.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }
    }
}
