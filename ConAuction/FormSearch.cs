using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConAuction
{
    public partial class FormSearch : Form
    {
        private MainForm _main;

        public FormSearch(MainForm main)
        {
            InitializeComponent();
            _main = main;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            var searchText = textBoxSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchText)) {
                DialogResult = DialogResult.Cancel;
                Close();
            }

            var found = _main.SearchProductTable(searchText);
            if (found > 0) {
                DialogResult = DialogResult.OK;
                Close();

            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            this.buttonSearch.Enabled = !string.IsNullOrEmpty(textBoxSearch.Text);
        }
    }
}
