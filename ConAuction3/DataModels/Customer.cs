using ConAuction3.Annotations;

namespace ConAuction3.DataModels {
    public class Customer {
        public Customer() {}

        public Customer(int id, string name, string shortName, string phone, string note, bool? finished, bool? swish) {
            Id = id;
            Name = name;
            ShortName = shortName;
            Phone = phone;
            Note = note;
	        Finished = finished;
            Swish = swish;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
		public bool? Finished { get; set; }
        public bool? Swish { get; set; }
        public string ShortName { get; set; }

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