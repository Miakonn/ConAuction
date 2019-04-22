using System.Windows.Input;
using ConAuction3.ViewModels;

namespace ConAuction3.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow {
        private AuctionVM _viewModel;

        public MainWindow() {
            InitializeComponent();

            Loaded += (_, __) => {
                _viewModel = new AuctionVM();
                DataContext = _viewModel;
            };
        }

        private void Control_OnMouseDoubleClickCustomer(object sender, MouseButtonEventArgs e) {
            if (_viewModel.ShowCustomerCommand.CanExecute(null)) {
                _viewModel.ShowCustomerCommand.Execute(null);
            }
        }

        private void Control_OnMouseDoubleClickProduct(object sender, MouseButtonEventArgs e) {
            if (_viewModel.ShowProductCommand.CanExecute(null)) {
                _viewModel.ShowProductCommand.Execute(null);
            }
        }
    }
}
