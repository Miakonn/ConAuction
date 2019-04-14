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
			Id.Text = customer.Id.ToString();
			CustomerName.Text = customer.Name;
			Phone.Text = customer.Phone;
			Note.Text = customer.Note;
		}

		public Customer Result => new Customer(_customerId, CustomerName.Text, Phone.Text, Note.Text, false);

        private void OnClick(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
