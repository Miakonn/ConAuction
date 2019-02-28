using System;
using System.Data;

namespace ConAuction.DataModels {
    public class Customer {
        public Customer() {}

        public Customer(string id, string name, string phone, string note, bool? finished) {
            Id = id;
            Name = name;
            Phone = phone;
            Note = note;
	        Finished = finished;
        }

		//public Customer(DataGridViewRow row) {
		//	Id = row.Cells["id"].Value.ToString();
		//	Name = row.Cells["Name"].Value != DBNull.Value ? row.Cells["Name"].Value as string : "";
		//	Phone = row.Cells["Phone"].Value != DBNull.Value ? row.Cells["Phone"].Value as string : "";
		//	Note = row.Cells["Comment"].Value != DBNull.Value ? row.Cells["Comment"].Value as string : "";
		//	Finished = row.Cells["Finished"].Value != DBNull.Value ? row.Cells["Finished"].Value as bool? : false;
		//}

		//public Customer(DataRow row) {
		//	Id = row["id"].ToString();
		//	Name = row["Name"] != null ? row["Name"] as string : "";
		//	Phone = row["Phone"] != null ? row["Phone"] as string : "";
		//	Note = row["Comment"] != null ? row["Comment"] as string : "";
		//	Finished = row["Finished"] != null ? row["Finished"] as bool? : false;
		//}

        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
		public bool? Finished { get; set; }

	    public string NumberAndName {
		    get { return Id + " : " + Name; }
	    }
    }
}