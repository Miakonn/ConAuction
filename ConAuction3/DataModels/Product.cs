namespace ConAuction3.DataModels {
    public class Product {
        public Product() {}

		public Product(long id, string type, string name, string desc, string limit, int customerId) {
            Id = id;
            Type = type;
            Name = name;
            Description = desc;
            Note = limit;
	        CustomerId = customerId;
        }

        public long Id { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int Price { get; set; }
        public int? FixedPrice { get; set; }
		public int CustomerId { get; set; }

        public string FixedPriceString => FixedPrice.HasValue && FixedPrice.Value > 0 ? FixedPrice.Value.ToString() : "";

        public bool IsSold => Price > 0;
        
        public string IsSoldStr => IsSold ? "Såld" : "";

        public bool IsJumble => FixedPrice.HasValue && FixedPrice.Value > 0;

        public bool SoldForFixedPrice() {
            if (FixedPrice.HasValue && !IsSold) {
                Price = FixedPrice.Value;
                return true;
            }
            return false;
        }

        public bool UndoSoldFor() {
            if (FixedPrice.HasValue && IsSold) {
                Price = 0;
                return true;
            }
            return false;
        }
    }
}