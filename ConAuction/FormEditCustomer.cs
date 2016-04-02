using System;
using System.Windows.Forms;

namespace ConAuction {
    public partial class FormEditCustomer : Form {
        private readonly Customer _customer;

        public FormEditCustomer(Customer customer) {
            InitializeComponent();
            _customer = customer;
            SetCustomerContents(customer);
            EnableDisableButtons();
        }

        private void SetCustomerContents(Customer customer) {
            textBoxCustomerId.Text = customer.Id;
            textBoxCustomerName.Text = customer.Name;
            textBoxCustomerPhone.Text = customer.Phone;
            textBoxCustomerNote.Text = customer.Note;
        }

        private void UpdateProduct() {
            _customer.Name = textBoxCustomerName.Text;
            _customer.Phone = textBoxCustomerPhone.Text;
            _customer.Note = textBoxCustomerNote.Text;
        }

        private void EnableDisableButtons() {
            buttonOk.Enabled = textBoxCustomerName.Text != "" && textBoxCustomerPhone.Text != "";
        }

        private void textBoxCustomerName_TextChanged(object sender, EventArgs e) {
            EnableDisableButtons();
        }

        private void textBoxCustomerPhone_TextChanged(object sender, EventArgs e) {
            EnableDisableButtons();
        }

        private void buttonOk_Click(object sender, EventArgs e) {
            UpdateProduct();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}