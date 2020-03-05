using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using ConAuction3.DataModels;

namespace ConAuction3.ViewModels
{
    class BidListVM
    {
        public List<Bid> _bidList;

        public ICollectionView BidView { get; }
        
        public BidListVM(List<Bid> bids, ProductListVM productsVm, CustomerListVM customersVm) {
            _bidList = bids;

            foreach (var bid in bids) {
                bid.BidProduct = productsVm.GetProductFromLabel(bid.ProductId);
                bid.BidProduct?.Bids.Add(bid);
                bid.Bidder = customersVm.GetCustomerFromId(bid.CustomerId);
            }

            BidView = CollectionViewSource.GetDefaultView(bids);
        }

        public List<Bid> BidsByCustomer(int customerId) {
            return _bidList.FindAll(b => b.CustomerId == customerId);
        }

        public void SortBy(string propertyName) {
            var direction = ListSortDirection.Ascending;
            var sortCurrent = BidView.SortDescriptions.FirstOrDefault();
            if (sortCurrent != null && sortCurrent.PropertyName == propertyName && sortCurrent.Direction == ListSortDirection.Ascending) {
                direction = ListSortDirection.Descending;
            }
            BidView.SortDescriptions.Clear();
            BidView.SortDescriptions.Add(new SortDescription(propertyName, direction));
        }

        public void NoFilter() {
            BidView.Filter = null;
        }

        public void FilterByCustomerId(int customerId) {
            BidView.Filter = o => o is Bid b && b.CustomerId == customerId;
        }

    }
}
