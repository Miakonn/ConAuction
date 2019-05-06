using System.Windows;

namespace ConAuction3.Views {
    public partial class PromptDialog {
        public enum InputType {
            Text,
            Password
        }

        private readonly InputType _inputType;

        public PromptDialog(string question, string title, string defaultValue = "", InputType inputType = InputType.Text) {
            InitializeComponent();
            Loaded += PromptDialog_Loaded;
            txtQuestion.Text = question;
            Title = title;
            txtResponse.Text = defaultValue;
            _inputType = inputType;
            if (_inputType == InputType.Password) {
                txtResponse.Visibility = Visibility.Collapsed;
            }
            else {
                txtPasswordResponse.Visibility = Visibility.Collapsed;
            }
        }

        void PromptDialog_Loaded(object sender, RoutedEventArgs e) {
            if (_inputType == InputType.Password) {
                txtPasswordResponse.Focus();
            }
            else {
                txtResponse.Focus();
            }
        }

        public static string Prompt(string question, string title, string defaultValue = "", InputType inputType = InputType.Text) {
            var inst = new PromptDialog(question, title, defaultValue, inputType);
            inst.ShowDialog();
            return inst.DialogResult == true ? inst.ResponseText : null;
        }

        public string ResponseText => _inputType == InputType.Password ? txtPasswordResponse.Password : txtResponse.Text;

        private void ButtonOk_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
