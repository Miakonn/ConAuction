using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Configuration;
using System.Windows.Forms;

namespace ConAuction
{
	[XmlRoot("Customers")]
	[XmlInclude(typeof(Customer))] 
	public class Customers
	{
		[XmlArray("Customers")]
		[XmlArrayItem("Customer", typeof(Customer))]
		public List<Customer> CustomerList { get; set; }

		public OpMode Mode { get; set; }


		public Customers()
		{
			CustomerList = new List<Customer>();
		}


		public static Customers Load(string fileName)
		{
			var serializer = new XmlSerializer(typeof(Customers));
			using (TextReader reader = new StreamReader(fileName)) {
				return (Customers)serializer.Deserialize(reader);
			}
		}


        public void Sorting()
        {
            CustomerList.Sort((x,y) => string.Compare(x.Id, y.Id));
        }

		public void Save(string fileName)
		{
			var serializer = new XmlSerializer(typeof(Customers));
			using (TextWriter writer = new StreamWriter(fileName)) {
				serializer.Serialize(writer, this);
			}
			//MakeFree();
		}


		public string ExportCustomersCSV()
		{
			StringBuilder sb = new StringBuilder();

			//// The header
			//foreach (string field in fields)
			//    sb.Append(field).Append(",");
			//sb.AppendLine();

			// The rows
			foreach (Customer customer in this.CustomerList) {
				sb.Append(customer.ExportCustomerCSV());
				sb.AppendLine();
			}

			return sb.ToString();
		}

		public string ExportProductsCSV()
		{
			StringBuilder sb = new StringBuilder();

			//// The header
			//foreach (string field in fields)
			//    sb.Append(field).Append(",");
			//sb.AppendLine();

			// The rows
			foreach (Customer customer in this.CustomerList) {
				sb.Append(customer.ExportProductsCSV());
			}

			return sb.ToString();
		}



		/// <summary>
		/// Exports to a file
		/// </summary>
		public void ExportCustomersToFile(string path)
		{
			File.WriteAllText(path, ExportCustomersCSV());
		}

		/// <summary>
		/// Exports to a file
		/// </summary>
		public void ExportProductsToFile(string path)
		{
			File.WriteAllText(path, ExportProductsCSV());
		}


		//public void MakeFree()
		//{
		//    foreach (Customer customer in CustomerList) {
		//        if (!customer.IsUsed()) {
		//            return;
		//        }
		//    }

		//    string id = GetNextFreeCustomerId();
		//    if (id != "") {
		//        CustomerList.Add(new Customer(id));
		//    }
		//}

		public int TotalCount()
		{
			int count = 0;
			foreach (Customer customer in CustomerList) {
				count += customer.ProductList.Count;
			}
			return count;
		}

		public int TotalSoldCount()
		{
			int count = 0;
			foreach (Customer customer in CustomerList) {
				count += customer.NoOfUnsoldProducts() - customer.ProductList.Count;
			}
			return count;
		}

		public string GetNextFreeCustomerId()
		{
			int SettingStartId = 1;
			int SettingEndId = 99;
			try {
				SettingStartId = Int32.Parse(ConfigurationManager.AppSettings["StartId"]);
				SettingEndId = Int32.Parse(ConfigurationManager.AppSettings["EndId"]);
			}
			catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			for (int id = SettingStartId; id < SettingEndId; id++) {
				bool found = false;
				foreach (Customer customer in CustomerList) {
					if (id.ToString() == customer.Id) {
						found = true;
						break;
					}
				}
				if (!found) {
					return id.ToString();
				}
			}
			return "";
		}

	}

}
