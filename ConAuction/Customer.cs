using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Xml.Serialization;
using System.Configuration;
using System.IO;

namespace ConAuction
{
	[Serializable]
	[XmlType("Customer")]
	[XmlInclude(typeof(Product))]
	public class Customer
	{

		public Customer(string id)
		{
			Id = id;
			ProductList = new List<Product>();
		}

		public Customer()
		{
			ProductList = new List<Product>();
		}

		#region Properties
		[XmlArray("Objects")]
		[XmlArrayItem("Object", typeof(Product))]
		public List<Product> ProductList { get; set; }

		public string Id { get; set; }

		public string Name { get; set; }

		public string PhoneNo { get; set; }
		
		public int NoOfProducts { 
			get {
				return ProductList.Count;
			}
		}


		public bool Finished { get; set; }
		#endregion

		public int NoOfUnsoldProducts()
		{
			int count = 0;
			foreach (Product product in ProductList) {
				if (product.IsSold()) {
					count++;
				}
			}
			return ProductList.Count - count;
		}

		public bool IsUsed()
		{
			return (Name!= null && Name.Length > 0) ;
		}

		public int TotalAmount()
		{
			int count = 0;
			foreach (Product product in ProductList) {
				count += product.Price;
			}
			return count;
		}

		public int NetAmount()
		{
			int SettingCost = 10;
			try {
				SettingCost = Int32.Parse(ConfigurationManager.AppSettings["Cost"]);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			int net = TotalAmount() - NoOfProducts * SettingCost;
			return net;
		}

		public string GetNextFreeProductId()
		{
			for (char id = 'A'; id < 'L'; id++) {
				bool found = false;
				foreach (Product product in ProductList) {
					if (product.Id.EndsWith(id.ToString())) {
						found = true;
						break;
					}
				}
				if (!found) {
					return (Id + id.ToString());
				}
			}
			MessageBox.Show("Slut på ID-nummer!");
			return "";
		}

		public Product GetLastProduct()
		{
			if (ProductList.Count > 0) {
				return ProductList[ProductList.Count - 1];
			}
			return null;
		}



		public string ExportCustomerCSV()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(Id);
			sb.Append(";");
			sb.Append(Name);
			sb.Append(";");
			sb.Append(PhoneNo);
			sb.Append(";");
			sb.Append(NoOfProducts);
			sb.Append(";");
			sb.Append(Finished);
			sb.Append(";");

			return sb.ToString();
		}

		public string ExportProductsCSV()
		{
			StringBuilder sb = new StringBuilder();
			foreach (Product product in ProductList) {
				sb.Append(product.Export());
				sb.AppendLine();
			}
			return sb.ToString();
		}

	}
}
