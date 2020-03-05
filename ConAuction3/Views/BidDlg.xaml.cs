using System;
using System.Windows;
using ConAuction3.DataModels;

namespace ConAuction3.Views {
	/// <summary>
	/// Interaction logic for CustomerDlg.xaml
	/// </summary>
	public partial class BidDlg {
		private readonly int _customerId;
        private readonly int _id;

		public BidDlg(Bid bid) {
			InitializeComponent();
			
			ProductId.Text = bid.ProductId.ToString();
            MaxBid.Text = bid.MaxBid.ToString();
            _customerId = bid.CustomerId;
            _id = bid.Id;
        }
        
		public Bid Result {
            get {
                int.TryParse(ProductId.Text, out int productId);
                int.TryParse(MaxBid.Text, out int maxBid);

                var bid = new Bid(_customerId, productId, maxBid) {
                    Id = _id
                };

                return bid;
            }
        }

        private void OnClick(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
