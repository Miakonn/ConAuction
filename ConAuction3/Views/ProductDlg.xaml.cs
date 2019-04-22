using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ConAuction3.DataModels;

namespace ConAuction3.Views {
	/// <summary>
	/// Interaction logic for ProductDlg.xaml
	/// </summary>
	public partial class ProductDlg {
		private readonly long _id;
        private readonly int _customerId;

		public ProductDlg(Product product, Customer customer) {
			InitializeComponent();
            if (customer == null) {
                return;
            }

			_id = product.Id;
            _customerId = customer.Id;

			Label.Text = product.Label;
            Type.ItemsSource = ProductTypeList;
            Type.Text = product.Type;
            ProductName.Text = product.Name;
			Customer.Text = customer.NumberAndName;
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
					CustomerId = _customerId,
					Type = Type.Text,
					Description = Description.Text,
					FixedPrice = int.Parse(FixedPrice.Text),
					Note = Note.Text
				};
				return product;
			}
		}

        public List<string> ProductTypeList { get; } = new List<string> {
            "Brädspel",
            "Kortspel",
            "Rollspel",
            "Figurspel",
            "Krigsspel",
            "Övrigt"
        };

        public void OnClick(object sender, RoutedEventArgs e) {
			DialogResult = true;
		}

        private static readonly Regex _regex = new Regex("[^0-9]+"); //regex that matches disallowed text
        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }

        private void FixedPrice_OnPreviewTextInput(object sender, TextCompositionEventArgs e) {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
