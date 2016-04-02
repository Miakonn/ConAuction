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

        //public Customer(DataGridViewRow row) {
        //    Id = ((int) row.Cells["id"].Value).ToString();
        //    Name = (string) row.Cells["Name"].Value;
        //    Phone = (string) row.Cells["Phone"].Value;
        //    if (row.Cells["Comment"].Value != DBNull.Value) {
        //        Note = (string) row.Cells["Comment"].Value;
        //    }
        //}

        //public Customer(DataRow row) {
        //    Id = ((int) row["id"]).ToString();
        //    Name = (string) row["Name"];
        //    Phone = (string) row["Phone"];
        //    if (row["Comment"] != null) {
        //        Note = (string) row["Comment"];
        //    }
        //}

        public string Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
    }
}