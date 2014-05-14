using System;
using System.Data;
using System.Text;
using System.IO;
using System.Configuration;
using System.Windows.Forms;

namespace ConAuction
{
	public static class ProductTable
	{

		public static int TotalCount(this DataTable table) {
			return table.Rows.Count;
		}

		public static int TotalSoldCount(this DataTable table) {
			return (int)table.Compute("Count(Price)", "Price > 0");
		}

		public static int TotalSoldAmount(this DataTable table) {
			Object obj = table.Compute("Sum(Price)", "Price > 0");
			int amount = 0;
			if (Int32.TryParse(obj.ToString(), out amount)) {
				return amount;
			};
			return 0;
		}

		public static int NoOfUnsoldForCustomer(this DataTable table, int customerId) {
			return (int)table.Compute("Count(Price)", "Price > 0 and CustomerId=" + customerId.ToString());
		}

		public static int CountForCustomer(this DataTable table, int customerId) {
			return (int)table.Compute("Count(Price)", "CustomerId=" + customerId.ToString());

		}

		public static DataRow GetRowForProductId(this DataTable table, int productId) {
			DataRow[] foundRows = table.Select("id = " + productId.ToString());
			if (foundRows.Length > 0) {
				return foundRows[0];
			}
			return null;
		}

		public static int GetLastProductIdForCustomer(this DataTable table, int customerId) {
			DataRow[] foundRows = table.Select("CustomerId = " + customerId.ToString());

			int rowId = 0;
			foreach (DataRow row in foundRows) {
				rowId = Math.Max(rowId, (int)row["id"]);
			}
			return rowId;
		}

		public static int TotalAmountForCustomer(this DataTable table, int customerId) {
			DataRow[] foundRows = table.Select("CustomerId = " + customerId.ToString() + " and ISNULL(Price, 0) > 0");

			int sum = 0;
			foreach (DataRow row in foundRows) {
				int price = (int)row["Price"];
				sum += price;
			}
			return sum;
		}

		public static int NetAmountForCustomer(this DataTable table, int customerId) {
			int SettingCost = 10;
			try {
				SettingCost = Int32.Parse(ConfigurationManager.AppSettings["Cost"]);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			int net = table.TotalAmountForCustomer(customerId) - table.CountForCustomer(customerId) * SettingCost;
			return net;
		}

	}

}
