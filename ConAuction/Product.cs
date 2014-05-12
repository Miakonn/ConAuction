using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace ConAuction
{
	[Serializable]
	[XmlType("Product")]
	public class Product
	{
		public string Id { get; set; }
		public string Label { get; set; }
		public string Type { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Note { get; set; }
		public int Price { get; set; }



		public Product() { }
		public Product(string id, string type, string name, string desc, string limit) {
			Id = id;
			Type = type;
			Name = name;
			Description = desc;
			Note = limit;
		}

		public Product(DataGridViewRow row)	{
			Id = ((int) row.Cells["id"].Value).ToString();
			Label = ((int)row.Cells["Label"].Value).ToString();
			Type = (string) row.Cells["Type"].Value;
			Name = (string)row.Cells["Name"].Value;
			Description = (string)row.Cells["Description"].Value;
			if (row.Cells["Note"].Value != DBNull.Value) {
				Note = (string)row.Cells["Note"].Value;
			}
			if (row.Cells["Price"].Value != DBNull.Value) {
				Price = (int)row.Cells["Price"].Value;
			}
		}



		public bool IsSold()
		{
			return (Price > 0);
		}

	}
}
