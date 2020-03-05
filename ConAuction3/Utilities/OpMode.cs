namespace ConAuction3.ViewModels {
    public enum OpMode {
        Initializing = 0,
        Receiving = 1,
        Showing = 2,
        Bidding = 3,
        Auctioning = 4,
        Paying = 5,
        Buyer = 6,
        Overhead = 7
    }


    public class ComboBoxItemOpMode {
        public OpMode ValueMode { get; set; }
        public string ValueString { get; set; }
    }
}