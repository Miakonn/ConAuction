using System.Windows;
using System.Windows.Controls;
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

        private void ListView_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (sender is ListView listView  && e.AddedItems.Count > 0) {
                listView.ScrollIntoView(e.AddedItems[0]);
            }
        }

        private void PriceTb_OnPreviewKeyUp(object sender, KeyEventArgs e) {
            if (!(sender is UIElement element)) {
                return;
            }

            switch (e.Key) {
                case Key.Enter:
                case Key.Down:
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    break;
                case Key.Up:
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                    break;
            }
        }
    }
}
