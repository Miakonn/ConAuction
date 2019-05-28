using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using ConAuction3.Annotations;
using ConAuction3.DataModels;
using ConAuction3.Utilities;

namespace ConAuction3.ViewModels {
    public class ProductDisplayVM : INotifyPropertyChanged {
        private readonly List<Product> _productsAuction;

        private Product CurrentProduct;
        private int CurrentIndex;

        public string ProductLabel { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

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

            CurrentProduct = null;
            CurrentIndex = -1;
            UpdateFields();
        }

        public void GotoNextProduct() {
            CurrentIndex++;
            UpdateFields();
        }

        public void GotoPreviousProduct() {
            CurrentIndex--;
            UpdateFields();
        }

        public void GotoNext10Product() {
            CurrentIndex+= 10;
            UpdateFields();
        }

        public void GotoPrevious10Product() {
            CurrentIndex-= 10;
            UpdateFields();
        }

        public void FirstProduct() {
            CurrentIndex = 0;
            CurrentProduct = _productsAuction[CurrentIndex];
            UpdateFields();
        }

        public void CloseWindow(object parameter) {
            var window = parameter as Window;
            window?.Close();
        }

        private void UpdateFields() {
            if (CurrentIndex < 0) {
                CurrentIndex = -1;
                CurrentProduct = null;
            }
            else if (CurrentIndex >= _productsAuction.Count) {
                CurrentIndex = _productsAuction.Count;
                CurrentProduct = null;
            }
            else {
                CurrentProduct = _productsAuction[CurrentIndex];
            }
            if (CurrentProduct == null) {
                ProductDescription = CurrentIndex < 0 ? ReadStartFile() : ReadEndFile();
                ProductLabel = string.Empty;
                ProductType = string.Empty;
                ProductName = string.Empty;
            }
            else {
                ProductLabel = CurrentProduct.LabelStr;
                ProductType = CurrentProduct.Type;
                ProductName = CurrentProduct.Name;
                ProductDescription = CurrentProduct.Description;
            }
            
            OnPropertyChanged(nameof(ProductLabel));
            OnPropertyChanged(nameof(ProductType));
            OnPropertyChanged(nameof(ProductName));
            OnPropertyChanged(nameof(ProductDescription));
        }
        
        private string ReadStartFile() {
            return File.ReadAllText("TextFiles\\ConAuctionRules.txt");
        }

        private string ReadEndFile() {
            return File.ReadAllText("TextFiles\\ConAuctionFinish.txt");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
