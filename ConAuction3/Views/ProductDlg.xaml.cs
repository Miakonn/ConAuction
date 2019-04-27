using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ConAuction3.ViewModels;

using ConAuction3.DataModels;
namespace ConAuction3.Views {
	/// <summary>
	/// Interaction logic for ProductDlg.xaml
	/// </summary>
	public partial class ProductDlg {
		private readonly long _id;
        private readonly Customer _customer;
        private readonly ProductListVM _productListVm;
        private readonly List<string> _dictionary;

		public ProductDlg(Product product, Customer customer, ProductListVM productListVm) {
			InitializeComponent();
            if (customer == null) {
                return;
            }

            DataContext = this;
            
			_id = product.Id;
            _customer = customer;
            _productListVm = productListVm;
            Label.Text = product.Label;

            Customer.Text = customer.NumberAndName;
            Type.ItemsSource = ProductTypeList;

            btnDialogOk.IsEnabled = false;
            SetFields(product);
            VerifyAllFieldsFilledIn();

            CheckBoxJumble.IsChecked = product.IsFixedPrice;

            FixedPricePanel.Visibility = product.IsFixedPrice ? Visibility.Visible : Visibility.Hidden;
            CopyPrevious.IsEnabled = _productListVm != null && _productListVm.CountForCustomer(customer.Id) > 0;



            _dictionary = new List<string>();
            try {
                using (var reader = new StreamReader("ConAuctionDictionary.txt")) {
                    while (!reader.EndOfStream) {
                        var line = reader.ReadLine();
                        _dictionary.Add(line);
                    }
                }
            }
            catch { }
        }
        
        private void SetFields(Product product) {
            Type.Text = product.Type;
            ProductName.Text = product.Name;
            Description.Text = product.Description;
            FixedPrice.Text = product.FixedPriceString;
            Note.Text = product.Note;
        }

        public IEnumerable<string> TitleDictionary => _dictionary;
        
        public Product Result {
			get {
				var product = new Product {
					Id = _id, 
					Label = Label.Text,
					Name = ProductName.Text,
					CustomerId = _customer.Id,
					Type = Type.Text,
					Description = Description.Text,
					Note = Note.Text
				};
                if (CheckBoxJumble.IsChecked.HasValue && CheckBoxJumble.IsChecked.Value) {
                    product.FixedPrice = int.Parse(FixedPrice.Text);
                }
                else {
                    product.FixedPrice = null;
                }

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

        private void FieldsChanged(object sender, SelectionChangedEventArgs e) {
            VerifyAllFieldsFilledIn();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e) {
            VerifyAllFieldsFilledIn();
        }

        private void CheckJumble_OnChecked(object sender, RoutedEventArgs e) {
            FixedPricePanel.Visibility = Visibility.Visible;
        }

        private void CheckJumble_OnUnchecked(object sender, RoutedEventArgs e) {
            FixedPricePanel.Visibility = Visibility.Hidden;
        }
    }
}
