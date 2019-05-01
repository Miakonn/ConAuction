using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ConAuction3.DataModels;

namespace ConAuction3.ViewModels
{
    class CustomerListVM {
        private readonly List<Customer> _customers;

        public CustomerListVM(List<Customer> customers) {
            _customers = customers;
            CustomerView = CollectionViewSource.GetDefaultView(customers);
        }
        
        public ICollectionView CustomerView { get; }

        public Customer GetCustomerFromId(int id) {
            return _customers.Find(c => c.Id == id);
        }

        public void SortBy(string propertyName) {
            var direction = ListSortDirection.Ascending;
            var sortCurrent = CustomerView.SortDescriptions.FirstOrDefault();
            if (sortCurrent != null && sortCurrent.PropertyName == propertyName && sortCurrent.Direction == ListSortDirection.Ascending) {
                direction = ListSortDirection.Descending;
            }
            CustomerView.SortDescriptions.Clear();        
            CustomerView.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }

        public List<Customer> CustomersLeftToGetPaid () {
            return _customers.FindAll(c => c.Finished.HasValue && !c.Finished.Value);
        }
    }
}
