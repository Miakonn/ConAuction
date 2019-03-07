using System;
using System.Data;

namespace ConAuction3.DataModels {
    public class Customer {
        public Customer() {}

        public Customer(int id, string name, string phone, string note, bool? finished) {
            Id = id;
            Name = name;
            Phone = phone;
            Note = note;
	        Finished = finished;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
		public bool? Finished { get; set; }

	    public string NumberAndName {
		    get { return Id + " : " + Name; }
	    }
    }
}