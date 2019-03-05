using System.Windows;
using ConAuction3.DataModels;

namespace ConAuction3.Views {
	/// <summary>
	/// Interaction logic for CustomerDlg.xaml
	/// </summary>
	public partial class CustomerDlg {
		public CustomerDlg(Customer customer) {
			InitializeComponent();

			CustomerName.Text = customer.Name;
			Phone.Text = customer.Phone;
			Note.Text = customer.Note;
		}

		public Customer Result { get { return new Customer("", CustomerName.Text, Phone.Text, Note.Text, false); } }

		private void OnClick(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
