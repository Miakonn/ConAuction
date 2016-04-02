using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace ConAuction {
    public static class ProductTable {
        public static int TotalCount(this DataTable table) {
            return table.Rows.Count;
        }

        public static int TotalCountAuction(this DataTable table) {
            return (int) table.Compute("Count(FixedPrice)", "FixedPrice = 0");
        }

        public static int TotalCountFixedPrice(this DataTable table) {
            return (int) table.Compute("Count(FixedPrice)", "FixedPrice > 0");
        }

        public static int TotalSoldCount(this DataTable table) {
            return (int) table.Compute("Count(Price)", "Price > 0");
        }

        public static int TotalSoldAmount(this DataTable table) {
            var obj = table.Compute("Sum(Price)", "Price > 0");
            int amount;
            if (int.TryParse(obj.ToString(), out amount)) {
                return amount;
            }
            return 0;
        }

        public static int NoOfSoldForCustomer(this DataTable table, int customerId) {
            return (int) table.Compute("Count(Price)", "Price > 0 and CustomerId=" + customerId);
        }

        public static int CountForCustomer(this DataTable table, int customerId) {
            return (int) table.Compute("Count(Price)", "CustomerId=" + customerId);
        }

        public static int NoOfUnsoldForCustomer(this DataTable table, int customerId) {
            return CountForCustomer(table, customerId) - NoOfSoldForCustomer(table, customerId);
        }

        public static DataRow GetRowForProductId(this DataTable table, int productId) {
            var foundRows = table.Select("id = " + productId);
            if (foundRows.Length > 0) {
                return foundRows[0];
            }
            return null;
        }

        public static int GetLastProductIdForCustomer(this DataTable table, int customerId) {
            var foundRows = table.Select("CustomerId = " + customerId);

            var rowId = 0;
            foreach (var row in foundRows) {
                rowId = Math.Max(rowId, (int) row["id"]);
            }
            return rowId;
        }

        public static int TotalAmountForCustomer(this DataTable table, int customerId) {
            var foundRows = table.Select("CustomerId = " + customerId + " and ISNULL(Price, 0) > 0");

            var sum = 0;
            foreach (var row in foundRows) {
                var price = (int) row["Price"];
                sum += price;
            }
            return sum;
        }

        public static int TotalCostForCustomer(this DataTable table, int customerId) {
            int SettingCost;
            int SettingCostFixed;
            var sum = 0;
            try {
                SettingCost = int.Parse(ConfigurationManager.AppSettings["Cost"]);
                SettingCostFixed = int.Parse(ConfigurationManager.AppSettings["CostFixed"]);
                var foundRows = table.Select("CustomerId = " + customerId);

                foreach (var row in foundRows) {
                    if (!string.IsNullOrEmpty(row["FixedPrice"].ToString())) {
                        var isFixedPrice = (int) row["FixedPrice"] > 0;
                        sum += isFixedPrice ? SettingCostFixed : SettingCost;
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }

            return sum;
        }

        public static int NetAmountForCustomer(this DataTable table, int customerId) {
            return table.TotalAmountForCustomer(customerId) - table.TotalCostForCustomer(customerId);
        }

        public static bool IsDirty(this DataTable table) {
            var changes = table.GetChanges();

            if (changes != null && changes.Rows.Count > 0) {
                return true;
            }
            return false;
        }
    }
}