using System.Windows.Input;
using ConAuction3.ViewModels;

namespace ConAuction3.Views {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow {
        private AuctionVM viewModel;

        public MainWindow() {
            InitializeComponent();

            Loaded += (_, __) => {
                viewModel = new ViewModels.AuctionVM();
                DataContext = viewModel;
            };
        }

        private void Control_OnMouseDoubleClickCustomer(object sender, MouseButtonEventArgs e) {
            if (viewModel.ShowCustomerCommand.CanExecute(null)) {
                viewModel.ShowCustomerCommand.Execute(null);
            }
        }

        private void Control_OnMouseDoubleClickProduct(object sender, MouseButtonEventArgs e) {
            if (viewModel.ShowProductCommand.CanExecute(null)) {
                viewModel.ShowProductCommand.Execute(null);
            }
        }
    }
}
