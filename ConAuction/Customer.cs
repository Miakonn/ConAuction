﻿using System;
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


		public Customer(string id, string name, string phoneno)
		{
			Id = id;
			Name = name;
			PhoneNo = phoneno;
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


		public Product GetLastProduct()
		{
			if (ProductList.Count > 0) {
				return ProductList[ProductList.Count - 1];
			}
			return null;
		}

	}
}
