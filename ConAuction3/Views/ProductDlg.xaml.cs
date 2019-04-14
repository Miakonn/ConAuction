using System;
using System.Windows;
using ConAuction3.DataModels;

namespace ConAuction3.Views {
	/// <summary>
	/// Interaction logic for ProductDlg.xaml
	/// </summary>
	public partial class ProductDlg {
		private readonly string _id;
		public ProductDlg(Product product, Customer customer) {
			InitializeComponent();

			_id = product.Id;

			Label.Text = product.Label;
			Type.Text = product.Type;
			ProductName.Text = product.Name;
			Customer.Text = customer!=null ? customer.NumberAndName : "unknown?";
			Description.Text = product.Description;
			FixedPrice.Text = product.FixedPrice.ToString();
			Note.Text = product.Note;
		}

		public Product Result {
			get {
				var product = new Product {
					Id = _id, 
					Label = Label.Text,
					Name = ProductName.Text,
					CustomerId = Int32.Parse(Customer.Text),
					Type = Type.Text,
					Description = Description.Text,
					FixedPrice = Int32.Parse(FixedPrice.Text),
					Note = Note.Text
				};
				return product;
			}
		}

		public void OnClick(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}
	}
}
