using System;
using System.Data;

namespace ConAuction.DataModels {
    public class Product {
        public Product() {}

        public Product(string id, string type, string name, string desc, string limit) {
            Id = id;
            Type = type;
            Name = name;
            Description = desc;
            Note = limit;
        }

        public string Id { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int Price { get; set; }
        public int FixedPrice { get; set; }

        public bool IsSold {
            get { return Price > 0; }
        }

        public bool IsFixedPrice {
            get { return FixedPrice > 0; }
        }

        public bool SoldForFixedPrice() {
            if (IsFixedPrice && !IsSold) {
                Price = FixedPrice;
                return true;
            }
            return false;
        }
    }
}