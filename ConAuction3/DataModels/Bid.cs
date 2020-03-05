using System;
using ConAuction3.Annotations;

namespace ConAuction3.DataModels
{
    public class Bid
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int MaxBid { get; set; }
        public int ProductId { get; set; }

        public Product BidProduct { get; set; }

        public Customer Bidder { get; set; }

        public Bid() {
        }

        public Bid(int customerId) {
            CustomerId = customerId;
        }

        public Bid(int customerId, int productId, int maxBid) {
            CustomerId = customerId;
            MaxBid = maxBid;
            ProductId = productId;
        }

        [UsedImplicitly]
        public string LabelSortableStr => ProductId.ToString("0000");

        [UsedImplicitly]
        public string MaxBidSortableStr => MaxBid.ToString("0000");

        [UsedImplicitly]
        public string ProductName => BidProduct?.Name;

        public int SoldFor => BidProduct?.Price ?? 0;

        [UsedImplicitly]
        public string SoldForSortableStr => SoldFor.ToString("0000");

        public string BidderShortNameOrDefault => Bidder?.ShortNameOrDefault;

        [UsedImplicitly]
        public bool BidSuccessFul => string.Compare(BidderShortNameOrDefault, BidProduct?.BuyerStr,
            StringComparison.CurrentCultureIgnoreCase)==0;


        public override string ToString() {
            return $"{BidderShortNameOrDefault}:{MaxBid}";
        }
    }
}
