using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using ConAuction3.Annotations;
using ConAuction3.DataModels;
using ConAuction3.Utilities;

namespace ConAuction3.ViewModels {
    public class ProductDisplayVM : INotifyPropertyChanged {
        private readonly List<Product> _productsAuction;

        private readonly int _numberOfItemsToSell;
        private Product _currentProduct;
        private int _currentIndex;

        public string ProductLabel { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Statistics { get; set; }

        public MyCommand NextProductCommand { get; }
        public MyCommand FirstProductCommand { get; }
        public MyCommand Next10ProductCommand { get; }
        public MyCommand PreviousProductCommand { get; }
        public MyCommand Previous10ProductCommand { get; }
        public ObjectCommand CloseCommand { get; }

        public ProductDisplayVM(ProductListVM productListVm) {
            _productsAuction = productListVm.ProductsToSellAtAuction;

            Previous10ProductCommand = new MyCommand(GotoPrevious10Product);
            PreviousProductCommand = new MyCommand(GotoPreviousProduct);
            NextProductCommand = new MyCommand(GotoNextProduct);
            Next10ProductCommand = new MyCommand(GotoNext10Product);
            FirstProductCommand = new MyCommand(FirstProduct);
            CloseCommand = new ObjectCommand(CloseWindow);

            _currentProduct = null;
            _currentIndex = -1;
            _numberOfItemsToSell = _productsAuction.Count;

            UpdateFields();
        }

        public void GotoNextProduct() {
            _currentIndex++;
            UpdateFields();
        }

        public void GotoPreviousProduct() {
            _currentIndex--;
            UpdateFields();
        }

        public void GotoNext10Product() {
            _currentIndex+= 10;
            UpdateFields();
        }

        public void GotoPrevious10Product() {
            _currentIndex-= 10;
            UpdateFields();
        }

        public void FirstProduct() {
            _currentIndex = 0;
            _currentProduct = _productsAuction[_currentIndex];
            UpdateFields();
        }

        public void CloseWindow(object parameter) {
            var window = parameter as Window;
            window?.Close();
        }

        private void UpdateFields() {
            if (_currentIndex < 0) {
                _currentIndex = -1;
                _currentProduct = null;

                ProductDescription = ReadStartFile();
                ProductLabel = string.Empty;
                ProductType = string.Empty;
                ProductName = $"{_numberOfItemsToSell} objekt ska ropas ut idag.";
                Statistics = string.Empty;
            }
            else if (_currentIndex >= _numberOfItemsToSell) {
                _currentIndex = _numberOfItemsToSell;
                _currentProduct = null;

                ProductDescription = ReadEndFile();
                ProductLabel = string.Empty;
                ProductType = string.Empty;
                ProductName = $"{_numberOfItemsToSell} objekt ropades ut idag. Snittpris blev {AveragePrice()}:-";
                Statistics = "100%";
            }
            else {
                _currentProduct = _productsAuction[_currentIndex];

                ProductLabel = _currentProduct.LabelStr;
                ProductType = _currentProduct.Type;
                ProductName = _currentProduct.Name;
                ProductDescription = _currentProduct.Description;
                Statistics = AuctionedOffPercentage() + "%";
            }

            OnPropertyChanged(nameof(ProductLabel));
            OnPropertyChanged(nameof(ProductType));
            OnPropertyChanged(nameof(ProductName));
            OnPropertyChanged(nameof(ProductDescription));
            OnPropertyChanged(nameof(Statistics));
        }
        
        private static string ReadStartFile() {
            return File.ReadAllText("TextFiles\\ConAuctionRules.txt");
        }

        private static string ReadEndFile() {
            return File.ReadAllText("TextFiles\\ConAuctionFinish.txt");
        }

        private string AveragePrice() {
            var soldNo = _productsAuction.Count(p => p.IsSold);
            var sum = _productsAuction.Sum(p => p.Price);
            return (sum / soldNo).ToString();
        }

        private int AuctionedOffPercentage() {
            return _currentIndex * 100 / _numberOfItemsToSell;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
