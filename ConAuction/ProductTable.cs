﻿using System;
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

		public static int TotalCountAuction(this DataTable table) {
			return (int)table.Compute("Count(FixedPrice)", "FixedPrice = 0");
		}

		public static int TotalCountFixedPrice(this DataTable table) {
			return (int)table.Compute("Count(FixedPrice)", "FixedPrice > 0");
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

		public static int NoOfSoldForCustomer(this DataTable table, int customerId) {
			return (int)table.Compute("Count(Price)", "Price > 0 and CustomerId=" + customerId.ToString());
		}

		public static int CountForCustomer(this DataTable table, int customerId) {
			return (int)table.Compute("Count(Price)", "CustomerId=" + customerId.ToString());
		}

		public static int NoOfUnsoldForCustomer(this DataTable table, int customerId) {
			return CountForCustomer(table, customerId) - NoOfSoldForCustomer(table, customerId);
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

		public static int TotalCostForCustomer(this DataTable table, int customerId)
		{
			int SettingCost = 10;
			int SettingCostFixed = 5;
			int sum = 0;
			try {
				SettingCost = Int32.Parse(ConfigurationManager.AppSettings["Cost"]);
				SettingCostFixed = Int32.Parse(ConfigurationManager.AppSettings["CostFixed"]);
				DataRow[] foundRows = table.Select("CustomerId = " + customerId.ToString());

				foreach (DataRow row in foundRows) {
					if (!String.IsNullOrEmpty(row["FixedPrice"].ToString())) {
						bool isFixedPrice = ((int)row["FixedPrice"]) > 0;
						sum += (isFixedPrice ? SettingCostFixed : SettingCost);
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
			DataTable changes = table.GetChanges();

			if (changes != null && changes.Rows.Count > 0) {
				return true;
			}
			return false;
		}

	}

}
