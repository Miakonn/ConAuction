using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
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

        public static long TotalSoldAmount(this DataTable table) {
            var obj = table.Compute("Sum(Price)", "Price > 0");
	        return (obj ==  DBNull.Value) ? 0 : (long) obj;
        }

		public static int TotalProfit(this DataTable table) {
			var settingCost = int.Parse(ConfigurationManager.AppSettings["Cost"]);
			var settingCostFixed = int.Parse(ConfigurationManager.AppSettings["CostFixed"]);

			var countFixed = TotalCountFixedPrice(table);
			var countTotal = TotalCount(table);
			var countStd = countTotal - countFixed;

			return (countStd * settingCost + countFixed * settingCostFixed);
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

		public static int GetCustomerIdForProductId(this DataTable table, int productId) {
			var foundRows = table.Select("id = " + productId);
			if (foundRows.Length > 0) {
				return (int)foundRows[0]["CustomerId"];
			}
			return 0;
		}

        public static int GetLastProductIdForCustomer(this DataTable table, int customerId) {
            var foundRows = table.Select("CustomerId = " + customerId);

            return foundRows.Select(row => (int) row["id"]).Concat(new[] {0}).Max();
        }

        public static int TotalAmountForCustomer(this DataTable table, int customerId) {
            var foundRows = table.Select("CustomerId = " + customerId + " and ISNULL(Price, 0) > 0");

            return foundRows.Sum(row => (int) row["Price"]);
        }

        public static string ExportCustomerReceipt(this DataTable table, int customerId, string customerName) {
            var foundRows = table.Select("CustomerId = " + customerId + " and ISNULL(Price, 0) > 0");

            var strB = new StringBuilder();
            strB.AppendLine("<!DOCTYPE html> <meta charset=\"UTF-8\"> ");
            strB.AppendLine("<html>");
            strB.AppendLine("<body>");

            strB.AppendLine("<h1>LinCons auktion</h1>");
            strB.AppendLine("<h2>Linköping " + DateTime.Now.ToString("yyyy-MM-dd HH:mm") + " </h2>");
            strB.AppendLine("<h3>" + customerName + "</h3>");

            strB.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
            strB.AppendLine("<tr>");
            strB.AppendLine("<td align='center'>Id</td>");
            strB.AppendLine("<td align='left'>Namn</td>");
            strB.AppendLine("<td align='left'>Beskrivning</td>");
            strB.AppendLine("<td align='right'>Sålt för</td>");
            strB.AppendLine("<td align='right'>Avgift</td>");
            strB.AppendLine("<tr>");

            var costNormal = int.Parse(ConfigurationManager.AppSettings["Cost"]);
            var costFixed = int.Parse(ConfigurationManager.AppSettings["CostFixed"]);

            foreach (DataRow row in foundRows) {
                bool sold = row["Price"].ToString() != "0";
                bool fixedPrice = row["FixedPrice"].ToString() != "0";

                strB.AppendLine("<tr>");

                strB.AppendLine("<td align='center'>" + row["Label"] + "</td>");
                strB.AppendLine("<td align='left'>" + row["Name"] + "</td>");
                strB.AppendLine("<td align='left'>" + row["Description"] + "</td>");
                strB.AppendLine("<td align='right'>" + (sold ? row["Price"] : "ej sålt") + "</td>");
                int cost = fixedPrice ? costFixed : costNormal;
                strB.AppendLine("<td align='right'>" + cost +"</td>");
                strB.AppendLine("</tr>");
            }

            // table footer & end of html file
            strB.AppendLine("</table><p>");

            strB.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
 
            strB.AppendLine("<tr><td>Summa:</td><td align='right'>" + table.TotalAmountForCustomer(customerId) + " kr </td></tr>");
            strB.AppendLine("<tr><td>Avgift:</td><td align='right'>" + table.TotalCostForCustomer(customerId) + " kr </td></tr>");
            strB.AppendLine("<tr><td>Netto:</td><td align='right'>" + table.NetAmountForCustomer(customerId) + " kr </td></tr>");
            strB.AppendLine("</table>");
            strB.AppendLine("<h3/>Ansvarig för auktionen: " + ConfigurationManager.AppSettings["MailSenderName"] + " - " +ConfigurationManager.AppSettings["MailSenderAddress"] + "</h3>");
            strB.AppendLine("</body></html>");

            return strB.ToString();
        }

        public static int TotalCostForCustomer(this DataTable table, int customerId) {
            var sum = 0;
            try {
                var settingCost = int.Parse(ConfigurationManager.AppSettings["Cost"]);
                var settingCostFixed = int.Parse(ConfigurationManager.AppSettings["CostFixed"]);
                var foundRows = table.Select("CustomerId = " + customerId);

                foreach (var row in foundRows) {
                    if (!string.IsNullOrEmpty(row["FixedPrice"].ToString())) {
                        var isFixedPrice = (int) row["FixedPrice"] > 0;
                        sum += isFixedPrice ? settingCostFixed : settingCost;
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

        private static string WriteJsonObj(string label, string value) {
            label = label.Replace('"', '\'');
            value = value.Replace('"', '\'');
	        if (string.IsNullOrEmpty(value)) {
		        value = "-";
	        }
            return string.Format("\"{0}\":\"{1}\"", label, value);
        }

        public static string ExportProductsToJson(this DataTable table) {
           var strB = new StringBuilder();
            strB.AppendLine("[");

			for(var i=0; i < table.Rows.Count; i++) {
				var row = table.Rows[i];
				if ((int) row["Price"] > 0) {
					continue;
				}
				var suffix = (i != (table.Rows.Count - 1)) ? "," : "";							
                var str = string.Format("{{{0}, {1}, {2}, {3}}}{4}", 
					WriteJsonObj("Label", row["Label"].ToString()),
                    WriteJsonObj("Name", row["Name"].ToString()),
					WriteJsonObj("Type", row["Type"].ToString()),
					WriteJsonObj("Description", row["Description"].ToString()),
					suffix
					);

                str = str.Replace("\r\n", "\\n");
                strB.AppendLine(str);

            }
            strB.AppendLine("]");
            return strB.ToString();
        }

		public static string ExportCommaSeparated(this DataTable table) {
			var strB = new StringBuilder();

			for (var i = 0; i < table.Rows.Count; i++) {
				var row = table.Rows[i];
				var str = string.Format("{0}; {1}; {2}; {3}; {4}",
					row["Label"], row["Name"], row["Type"], row["Description"], row["Price"]);

				str = str.Replace("\r\n", "\\n");
				strB.AppendLine(str);

			}
			return strB.ToString();
		}


		public static string GetCustomerNameFromId(this DataTable table, int customerId) {
			var foundRows = table.Select("id = " + customerId);
			if (foundRows.Length > 0) {
				return foundRows[0]["id"] + ": " + foundRows[0]["Name"];
			}
			return null;		    
	    }

		public static Customer GetCustomerFromId(this DataTable table, int customerId) {
			var foundRows = table.Select("id = " + customerId);
			if (foundRows.Length > 0) {
				return new Customer(foundRows[0]);
			}
			return null;
		}

    }
}