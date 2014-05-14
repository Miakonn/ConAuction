using System;
using System.Data;
using System.Text;
using System.IO;
using System.Configuration;
using System.Windows.Forms;

namespace ConAuction
{
	public class ProductTable
	{

		public static int TotalCount(DataTable tableProducts)
		{
			if (tableProducts != null) {
				return tableProducts.Rows.Count;
			}
			return 0;
		}

		public static int TotalSoldCount(DataTable tableProducts)
		{
			if (tableProducts != null) {
				return (int)tableProducts.Compute("Count(Price)", "Price > 0");
			}
			return 0;
		}

		public static int TotalSoldAmount(DataTable tableProducts) {
			if (tableProducts != null) {
				Object obj = tableProducts.Compute("Sum(Price)", "Price > 0");
				int amount=0;
				if (Int32.TryParse(obj.ToString(), out amount)) {
					return amount;
				};
			}
			return 0;
		}


		public static int NoOfUnsoldForCustomer(int customerId, DataTable tableProducts)
		{
			if (tableProducts != null) {
				return (int)tableProducts.Compute("Count(Price)", "Price > 0 and CustomerId=" + customerId.ToString());
			}

			return 0;
		}

		public static int CountForCustomer(int customerId, DataTable tableProducts)
		{
			if (tableProducts != null) {
				return (int)tableProducts.Compute("Count(Price)", "CustomerId=" + customerId.ToString());
			}
			return 0;

		}

		public static DataRow GetRowForProductId(int productId, DataTable tableProducts)
		{
			if (tableProducts != null) {
				DataRow[] foundRows = tableProducts.Select("id = " + productId.ToString());
				if (foundRows.Length > 0) {
					return foundRows[0];
				}
			}
			return null;
		}

		public static int GetLastProductIdForCustomer(int customerId, DataTable tableProducts)
		{
			if (tableProducts == null) {
				return 0;
			}
			DataRow[] foundRows = tableProducts.Select("CustomerId = " + customerId.ToString());

			int rowId = 0;
			foreach (DataRow row in foundRows)	{
				rowId = Math.Max(rowId, (int)row["id"]);
			}
			return rowId;
		}

		public static int TotalAmountForCustomer(int customerId, DataTable tableProducts)
		{
			if (tableProducts == null) {
				return 0;
			}
			DataRow[] foundRows = tableProducts.Select("CustomerId = " + customerId.ToString() + " and ISNULL(Price, 0) > 0");

			int sum = 0;
			foreach (DataRow row in foundRows) {
				int price = (int)row["Price"];
				sum += price;
			}
			return sum;
		}

		public static int NetAmountForCustomer(int customerId, DataTable tableProducts)
		{
			int SettingCost = 10;
			try {
				SettingCost = Int32.Parse(ConfigurationManager.AppSettings["Cost"]);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			int net = TotalAmountForCustomer(customerId, tableProducts) - CountForCustomer(customerId, tableProducts) * SettingCost;
			return net;
		}

	}

}
