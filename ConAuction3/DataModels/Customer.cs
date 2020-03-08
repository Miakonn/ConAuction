using ConAuction3.Annotations;
using ConAuction3.Utilities;

namespace ConAuction3.DataModels {
    public class Customer {
        public Customer() {}

        public Customer(int id, string name, string shortName, string phone, string comment, bool? finished, bool? swish) {
            Id = id;
            Name = name;
            ShortName = shortName;
            Phone = phone;
            Comment = comment;
	        Finished = finished;
            Swish = swish;
        }

        public int Id { get; set; }

        private string _name;
        public string Name {
            get => _name;
            set => _name = StringHelpers.SetValueWithLimit(value, 45);
        }

        private string _phone;
        public string Phone {
            get => _phone;
            set => _phone = StringHelpers.SetValueWithLimit(value, 20);
        }

        private string _comment;
        public string Comment {
            get => _comment;
            set => _comment = StringHelpers.SetValueWithLimit(value, 45);
        }

        private string _shortName;
        public string ShortName {
            get => _shortName;
            set => _shortName =  StringHelpers.SetValueWithLimit(value, 15);
        }

		public bool? Finished { get; set; }

        public bool? Swish { get; set; }
        
        public string ShortNameOrDefault => string.IsNullOrWhiteSpace(ShortName) ? "B" + Id : ShortName;

        public string NumberAndName => Id + " : " + Name;

        public bool IsFinished {
            get => Finished.HasValue && Finished.Value;
            set => Finished = value;
        }

        [UsedImplicitly]
        public string IsFinishedStr => IsFinished ? "Klar" : "";

        [UsedImplicitly]
        public string PhoneFormatted {
            get {
                if (Phone.Length > 10) {
                    return Phone.Substring(0, 3) + " " + Phone.Substring(3, 3) + " " + Phone.Substring(6, 3) + " " + Phone.Substring(9);
                }
                if (Phone.Length > 8) {
                    return Phone.Substring(0, 3) + " " + Phone.Substring(3, 3) + " " + Phone.Substring(6, 2) + " " + Phone.Substring(8);
                }
                return Phone;
            }
        }

        [UsedImplicitly]
        public string SwishFormatted => Swish.HasValue && Swish.Value ? "✓" : string.Empty;
    }
}