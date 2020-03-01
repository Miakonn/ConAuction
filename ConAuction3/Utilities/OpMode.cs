namespace ConAuction3.ViewModels {
    public enum OpMode {
        Initializing = 0,
        Receiving = 1,
        Showing = 2,
        Auctioning = 3,
        Paying = 4,
        Buyer = 5,
        Overhead = 6
    }


    public class ComboBoxItemOpMode {
        public OpMode ValueMode { get; set; }
        public string ValueString { get; set; }
    }
}