using System.Windows;
using ConAuction3.ViewModels;

namespace ConAuction3.Views {
    /// <summary>
    /// Interaction logic for ProductDisplayDlg.xaml
    /// </summary>
    public partial class ProductDisplayDlg : Window {

        public ProductDisplayDlg(ProductListVM productListVm) {
            DataContext = new ProductDisplayVM(productListVm);
            InitializeComponent();
        }
    }
}
