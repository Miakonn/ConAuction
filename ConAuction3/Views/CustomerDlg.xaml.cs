using System.Windows;
using ConAuction3.DataModels;

namespace ConAuction3.Views {
	/// <summary>
	/// Interaction logic for CustomerDlg.xaml
	/// </summary>
	public partial class CustomerDlg {
		private readonly int _customerId;

		public CustomerDlg(Customer customer) {
			InitializeComponent();
			_customerId = customer.Id;
			Id.Content = customer.Id.ToString();
			CustomerName.Text = customer.Name;
            ShortName.Text = customer.ShortName;
			Phone.Text = customer.Phone;
			Comment.Text = customer.Comment;
            Swish.IsChecked = customer.Swish.HasValue && customer.Swish.Value;
        }

		public Customer Result => new Customer(_customerId, CustomerName.Text ,ShortName.Text, Phone.Text, Comment.Text, false, Swish.IsChecked);

        private void OnClick(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
