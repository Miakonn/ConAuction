using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using ConAuction3.DataModels;

namespace ConAuction3.ViewModels
{
    class CustomerListVM {
        private readonly List<Customer> _customers;

        private readonly AuctionVM _auction;

        public CustomerListVM(List<Customer> customers, AuctionVM products) {
            _customers = customers;
            _auction = products;
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
        
        public void Filter(bool activeOnly, bool finishedOnly) {
            CustomerView.Filter = o => o is Customer c && (IsCustomerActive(c) || !activeOnly) && (!c.IsFinished || !finishedOnly);
        }
        
        public void NoFilter() {
            CustomerView.Filter = null;
        }
        
        public IEnumerable<Customer> CustomersLeftToGetPaid () {
            return _customers.FindAll(c => c.Finished.HasValue && !c.Finished.Value);
        }

        public IEnumerable<Product> Products(Customer customer) {
            return _auction.ProductsVm.ProductsForCustomer(customer.Id);
        }

        public bool IsCustomerActive(Customer customer) {
            return Products(customer).Any();
        }

        public int LeftToPay() {
            var customersLeftToGetPaid = CustomersLeftToGetPaid();
            return customersLeftToGetPaid.Sum(c => _auction.ProductsVm.NetAmountForCustomer(c.Id));
        }
        
        public string ExportCommaSeparated() {
            var strB = new StringBuilder();
            var enumerator = CustomerView.GetEnumerator();
            while (enumerator.MoveNext()) {
                if (!(enumerator.Current is Customer customer)) {
                    continue;
                }
                var str = $"{customer.Id}; {customer.Name};";

                str = str.Replace("\r\n", "\\n");
                strB.AppendLine(str);
            }
            return strB.ToString();
        }

    }
}
