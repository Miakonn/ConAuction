﻿using System;
using System.Globalization;

namespace ConAuction3.DataModels {
    public class Product : IComparable {
        public long Id { get; set; }
        public int Label { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public int Price { get; set; }
        public int? FixedPrice { get; set; }
		public int CustomerId { get; set; }

        public string FixedPriceString => FixedPrice.HasValue && FixedPrice.Value > 0 ? FixedPrice.Value.ToString() : "";

        public bool IsSold => Price > 0;
        
        public Product() {}

        public Product(int label) {
            Label = label;
        }

        public string LabelStr => Label.ToString("000");

        // ReSharper disable once UnusedMember.Global
        public string SoldForStr {
            get => IsSold ? Price.ToString() : "";
            set {
                if (!string.IsNullOrWhiteSpace(value) && int.TryParse(value, out var price)) {
                    Price = price;
                    DbAccess.Instance.SaveProductPriceToDB(Id, price, Note);
                }
                else {
                    Price = 0;
                }
            }
        }

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

        public int CompareTo(object obj) {
            return obj is Product comparePart ? Label.CompareTo(comparePart.Label) : 1;
        }
    }
}