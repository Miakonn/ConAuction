using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Configuration;
using System.Windows.Forms;

namespace ConAuction
{

	public partial class MainForm : Form
	{

		public int ProductTotalCount(DataTable tableProducts)
		{
			if (tableProducts != null) {
				return tableProducts.Rows.Count;
			}
			return 0;
		}

		public int TotalSoldCount(DataTable tableProducts)
		{
			int count = 0;
			if (tableProducts != null) {
				foreach (DataRow row in tableProducts.Rows) {
					if (row["Price"] != DBNull.Value) {
						int price = (int)row["Price"];
						if (price > 0) {
							count++;
						}
					}
				}
			}
			return count;
		}


		public int NoOfUnsoldProductsForCustomer(int customerId, DataTable tableProducts)
		{
			int count = 0;
			foreach (DataRow row in tableProducts.Rows) {
				int rowCustomerid = (int)row["CustomerId"];
				if (customerId == rowCustomerid) {
					if (row["Price"] == DBNull.Value) {
						count++;
					}
					else {
						int price = (int)row["Price"];
						if (price == 0) {
							count++;
						}
					}
				}
			}
			return count;
		}


		public int NoOfProductsForCustomer(int customerId, DataTable tableProducts)
		{
			int count = 0;
			if (tableProducts != null) {
				foreach (DataRow row in tableProducts.Rows) {
					int rowCustomerid = (int)row["CustomerId"];
					if (customerId == rowCustomerid ) {
						count++;
					}
				}
			}
			return count;

		}

		public int TotalAmountForCustomer(int customerId, DataTable tableProducts)
		{
			int sum = 0;
			if (tableProducts != null) {
				foreach (DataRow row in tableProducts.Rows) {
					int rowCustomerid = (int)row["CustomerId"];
					if (customerId == rowCustomerid && row["Price"] != DBNull.Value) {
						int price = (int)row["Price"];
						if (price > 0) {
							sum+= price;
						}
					}
				}
			}
			return sum;
		}

		public int NetAmountForCustomer(int customerId, DataTable tableProducts)
		{
			int SettingCost = 10;
			try {
				SettingCost = Int32.Parse(ConfigurationManager.AppSettings["Cost"]);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			int net = TotalAmountForCustomer(customerId, tableProducts) - NoOfProductsForCustomer(customerId, tableProducts) * SettingCost;
			return net;
		}

	}

}
