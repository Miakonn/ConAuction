using System;
using System.Data;
using System.Windows.Forms;

namespace ConAuction {
    public class Product {
        public Product() {}

        public Product(string id, string type, string name, string desc, string limit) {
            Id = id;
            Type = type;
            Name = name;
            Description = desc;
            Note = limit;
        }

        public Product(DataGridViewRow row) {
            Id = ((int) row.Cells["id"].Value).ToString();
            Label = ((int) row.Cells["Label"].Value).ToString();
            Type = (string) row.Cells["Type"].Value;
            Name = (string) row.Cells["Name"].Value;
            Description = (string) row.Cells["Description"].Value;
            if (row.Cells["Note"].Value != DBNull.Value) {
                Note = (string) row.Cells["Note"].Value;
            }
            if (row.Cells["Price"].Value != DBNull.Value) {
                Price = (int) row.Cells["Price"].Value;
            }
            if (row.Cells["FixedPrice"].Value != DBNull.Value) {
                FixedPrice = (int) row.Cells["FixedPrice"].Value;
            }
        }

        public Product(DataRow row) {
            Id = ((int) row["id"]).ToString();
            Label = ((int) row["Label"]).ToString();
            Type = (string) row["Type"];
            Name = (string) row["Name"];
            Description = (string) row["Description"];
            if (row["Note"] != null) {
                Note = (string) row["Note"];
            }
            if (row["Price"] != null) {
                Price = (int) row["Price"];
            }
            if (row["FixedPrice"] != null) {
                FixedPrice = (int) row["FixedPrice"];
            }
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