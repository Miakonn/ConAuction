namespace ConAuction3.DataModels
{
    public class Bid
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int MaxBid { get; set; }
        public int ProductId { get; set; }

        public Product BidProduct { get; set; }

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

        public string LabelSortableStr => ProductId.ToString("0000");

        public string MaxBidSortableStr => MaxBid.ToString("0000");
        
        public string ProductName => BidProduct?.Name;

        public int SoldFor => BidProduct?.Price ?? 0;

        public string SoldForSortableStr => SoldFor.ToString("0000");

        public override string ToString() {
            return $"{CustomerId}:{MaxBid}";
        }
    }
}
