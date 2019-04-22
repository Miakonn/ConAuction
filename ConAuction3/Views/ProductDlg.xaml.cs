using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ConAuction3.DataModels;
using ConAuction3.ViewModels;

namespace ConAuction3.Views {
	/// <summary>
	/// Interaction logic for ProductDlg.xaml
	/// </summary>
	public partial class ProductDlg {
		private readonly long _id;
        private readonly Customer _customer;
        private readonly ProductListVM _productListVm;

		public ProductDlg(Product product, Customer customer, ProductListVM productListVm) {
			InitializeComponent();
            if (customer == null) {
                return;
            }

			_id = product.Id;
            _customer = customer;
            _productListVm = productListVm;
            Label.Text = product.Label;

            Customer.Text = customer.NumberAndName;
            Type.ItemsSource = ProductTypeList;

            btnDialogOk.IsEnabled = false;
            SetFields(product);
            VerifyAllFieldsFilledIn();

            CopyPrevious.IsEnabled = _productListVm != null && _productListVm.CountForCustomer(customer.Id) > 0;
        }
        
        private void SetFields(Product product) {
            Type.Text = product.Type;
            ProductName.Text = product.Name;
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
					CustomerId = _customer.Id,
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

        private void CopyPrevious_OnClick(object sender, RoutedEventArgs e) {
            var previousProducts = _productListVm.ProductsForCustomer(_customer.Id);
            var maxId = previousProducts.Max(p => p.Id);
            var previous = previousProducts.Find(p => p.Id == maxId);
            SetFields(previous);
        }

        private void VerifyAllFieldsFilledIn() {
            var ok = !string.IsNullOrWhiteSpace(Type.Text) && !string.IsNullOrWhiteSpace(ProductName.Text) && !string.IsNullOrWhiteSpace(Description.Text);
            btnDialogOk.IsEnabled = ok;
        }

        private void FieldsChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
            VerifyAllFieldsFilledIn();
        }

        private void OnTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) {
            VerifyAllFieldsFilledIn();
        }
    }
}
