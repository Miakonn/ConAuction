using System;
using System.Data;
using System.Windows.Forms;

namespace ConAuction {
    public class Customer {
        public Customer() {}

        public Customer(string id, string name, string phone, string note) {
            Id = id;
            Name = name;
            Phone = phone;
            Note = note;
        }

        public Customer(DataGridViewRow row) {
			Id = row.Cells["id"].Value.ToString();
			Name = row.Cells["Name"].Value != DBNull.Value ? row.Cells["Name"].Value as string : "";
			Phone = row.Cells["Phone"].Value != DBNull.Value ? row.Cells["Phone"].Value as string : "";
			Note = row.Cells["Comment"].Value != DBNull.Value ? row.Cells["Comment"].Value as string : "";
        }

        public Customer(DataRow row) {
			Id = row["id"].ToString();
			Name = row["Name"] != null ? row["Name"] as string : "";
			Phone = row["Phone"] != null ? row["Phone"] as string : "";
			Note = row["Comment"] != null ? row["Comment"] as string : "";
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }

	    public string NumberAndName {
		    get { return Id + " : " + Name; }
	    }
    }
}