using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly int _label;
        private readonly bool _labelPrinted;

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
            _label = product.Label;
            _labelPrinted = product.LabelPrinted;
            _customer = customer;
            _productListVm = productListVm;
            Label.Text = product.LabelStr;

            Customer.Text = customer.NumberAndName;
            TypeCombo.ItemsSource = ProductTypeList;
            PartsNoCombo.ItemsSource = PartsNoList;

            BtnDialogOk.IsEnabled = false;
            BtnPrintLabelAndSave.IsEnabled = false;
            SetFields(product);
            VerifyAllFieldsFilledIn();

            BtnCopyPrevious.IsEnabled = _productListVm != null && _productListVm.CountForCustomer(customer.Id) > 0;
            
            _dictionary = new List<string>();
            try {
                using (var reader = new StreamReader("TextFiles\\ConAuctionDictionary.txt")) {
                    while (!reader.EndOfStream) {
                        var line = reader.ReadLine();
                        _dictionary.Add(line);
                    }
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex) {
                Debug.Assert(ex == null);
            }
        }
        
        private void SetFields(Product product) {
            TypeCombo.Text = product.Type;
            ProductName.Text = product.Name;
            Description.Text = product.Description;
            FixedPrice.Text = product.FixedPriceString;
            Note.Text = product.Note;
            CheckBoxJumble.IsChecked = product.IsJumble;
            PartsNoCombo.SelectedIndex = product.PartsNo - 1;
            FixedPricePanel.Visibility = product.IsJumble ? Visibility.Visible : Visibility.Hidden;
        }

        // ReSharper disable once UnusedMember.Global
        public IEnumerable<string> TitleDictionary => _dictionary;
        
        public Product Result {
			get {
				var product = new Product {
					Id = _id, 
					Label = _label,
					Name = ProductName.Text,
					CustomerId = _customer.Id,
					Type = TypeCombo.Text,
					Description = Description.Text,
					Note = Note.Text,
                    PartsNo = PartsNo,
                    LabelPrinted = _labelPrinted
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

        public List<string> PartsNoList { get; } = new List<string> {
            "1",
            "2",
            "3",
            "4"
        };

        public void Save_OnClick(object sender, RoutedEventArgs e) {
            PrintLabel = false;
			DialogResult = true;
            Close();
		}

        public bool PrintLabel { get; private set; }

        private void PrintLabelAndSave_OnClick(object sender, RoutedEventArgs e) {
            PrintLabel = true;
            DialogResult = true;
            Close();
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


        private bool IsJumble => (CheckBoxJumble.IsChecked.HasValue && CheckBoxJumble.IsChecked.Value);

        private int PartsNo => int.Parse(PartsNoCombo.Text);

        private void VerifyAllFieldsFilledIn() {
            var ok =  !string.IsNullOrWhiteSpace(TypeCombo.Text) && !string.IsNullOrWhiteSpace(ProductName.Text);
            if (IsJumble) {
                ok &= !string.IsNullOrWhiteSpace(FixedPrice.Text);
            }
            else {
                ok &= !string.IsNullOrWhiteSpace(Description.Text);
            }

            BtnDialogOk.IsEnabled = ok;
            BtnPrintLabelAndSave.IsEnabled = ok && LabelWriter.Instance.IsPrinterOnline();
        }

        private void FieldsChanged(object sender, SelectionChangedEventArgs e) {
            TypeCombo.Text = TypeCombo.SelectedItem.ToString();
            VerifyAllFieldsFilledIn();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e) {
            VerifyAllFieldsFilledIn();
        }

        private void CheckJumble_OnChecked(object sender, RoutedEventArgs e) {
            FixedPricePanel.Visibility = Visibility.Visible;
            VerifyAllFieldsFilledIn();
        }

        private void CheckJumble_OnUnchecked(object sender, RoutedEventArgs e) {
            FixedPricePanel.Visibility = Visibility.Hidden;
            VerifyAllFieldsFilledIn();
        }
    }
}
