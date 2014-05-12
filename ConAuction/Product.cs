using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ConAuction
{
	[Serializable]
	[XmlType("Product")]
	public class Product
	{
		public Product() { }
		public Product(string id, string type, string name, string desc, string limit) {
			Id = id;
			ProductType = type;
			Name = name;
			Description = desc;
			Limit = limit;
		}	

		public string Id { get; set; }
		public string ProductType { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Limit { get; set; }
		public int Price { get; set; }


		public bool IsSold()
		{
			return (Price > 0);
		}


		public string Export()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\"" + Id + "\"" );
			sb.Append(";");
			sb.Append(Name);
			sb.Append(";");
			sb.Append(ProductType);
			sb.Append(";");
			sb.Append(Description);
			sb.Append(";");
			sb.Append(Limit);
			sb.Append(";");
			sb.Append(Price);
			sb.Append(";");
			return sb.ToString();

		}
	}
}
