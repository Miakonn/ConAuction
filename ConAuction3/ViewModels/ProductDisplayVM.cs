using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConAuction3.Annotations;
using ConAuction3.Utilities;

namespace ConAuction3.ViewModels {
    public class ProductDisplayVM : INotifyPropertyChanged {
        private ProductListVM _productListVm;

        private int CurrentLabel;
        private int MaxLabel;

        public string ProductLabel { get; set; }
        public string ProductType { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }

        public MyCommand NextProductCommand { get; }
        public MyCommand FirstProductCommand { get; }
        public MyCommand PreviousProductCommand { get; }
        public ObjectCommand CloseCommand { get; }

        public ProductDisplayVM(ProductListVM productListVm) {
            _productListVm = productListVm;

            PreviousProductCommand = new MyCommand(PreviousProduct);
            NextProductCommand = new MyCommand(NextProduct);
            FirstProductCommand = new MyCommand(FirstProduct);
            CloseCommand = new ObjectCommand(CloseWindow);

            CurrentLabel = 0;
            MaxLabel = 1000;
            UpdateFields();
        }

        public void NextProduct() {

        }

        public void PreviousProduct() {

        }

        public void FirstProduct() {
            
        }

        public void CloseWindow(Object parameter) {
            var window = parameter as Window;
            window?.Close();
        }

        private void UpdateFields() {
            ProductLabel = string.Empty;
            ProductType = string.Empty;
            ProductName = string.Empty;

            if (CurrentLabel == 0) {
                ProductDescription = ReadStartFile();
            }
            else if (CurrentLabel > MaxLabel ) {
                ProductDescription = ReadEndFile();
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
