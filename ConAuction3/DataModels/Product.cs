using System;
using System.Collections.Generic;
using ConAuction3.Annotations;

namespace ConAuction3.DataModels {
    public class Product : IComparable {
        private string _description;
        private string _name;
        private string _buyer;
        private string _note;
        public long Id { get; set; }
        public int Label { get; set; }
        public string Type { get; set; }
        public bool LabelPrinted { get; set; }
        public int PartsNo { get; set; }

        public List<Bid> Bids { get; set; }
        
        public string Name {
            get => _name;
            set => _name = value.Length > 45 ? value.Substring(0, 45) : value;
        }

        public string Buyer {
            get => _buyer;
            set => _buyer = value.Length > 15 ? value.Substring(0, 15) : value;
        }

        public string Description {
            get => _description;
            set => _description = value.Length > 250 ? value.Substring(0, 250) : value;
        }

        public string Note {
            get => _note;
            set => _note = value.Length > 15 ? value.Substring(0, 15) : value;
        }

        public int Price { get; set; }

        // Used for sorting
        [UsedImplicitly]
        public string PriceSortableStr => Price.ToString("0000");

        public string BarcodeNumber => Label.ToString("0000");

        public int? FixedPrice { get; set; }

        // Used for sorting
        [UsedImplicitly]
        public string FixedPriceSortableStr => FixedPrice.HasValue ? FixedPrice?.ToString("0000") : "";

        public int CustomerId { get; set; }

        public string FixedPriceString => FixedPrice.HasValue && FixedPrice.Value > 0 ? FixedPrice.Value.ToString() : "";

        public bool IsSold => Price > 0;

        public Product() {
            PartsNo = 1;
            Bids = new List<Bid>();
        }

        public Product(int label) {
            Label = label;
            PartsNo = 1;
            Bids = new List<Bid>();
        }

        public string LabelStr => Label.ToString("0000");

        public string BidsStr => string.Join(", ", Bids);

        [UsedImplicitly]
        public string SoldForStr {
            get => IsSold ? Price.ToString() : "";
            set {
                Price = !string.IsNullOrWhiteSpace(value) && int.TryParse(value, out var price) ? price : 0;
                DbAccess.Instance.SaveProductPriceToDb(Id, Price);
            }
        }

        public string BuyerStr {
            get => Buyer;
            set {
                Buyer = value.Length > 15 ? value.Substring(0,14) : value; 
                DbAccess.Instance.SaveProductBuyerToDb(Id, Buyer);
            }
        }

        public bool IsJumble => FixedPrice.HasValue && FixedPrice.Value > 0;

        public bool SellForFixedPrice() {
            if (FixedPrice.HasValue && !IsSold) {
                Price = FixedPrice.Value;
                return true;
            }
            return false;
        }

        public bool UndoSoldFor() {
            if (FixedPrice.HasValue && IsSold) {
                Price = 0;
                Buyer = null;
                return true;
            }
            return false;
        }

        public int CompareTo(object obj) {
            return obj is Product comparePart ? Label.CompareTo(comparePart.Label) : 1;
        }
    }
}