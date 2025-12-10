using System;
using System.Windows.Controls;

namespace RulesManagerExplorer
{
    public partial class DataGridIntegrationDemo : UserControl
    {
        public DataGridIntegrationDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.DataGridIntegrationDescription;

            grid.ItemsSource = Customer.GetCustomerList(100);
            grid.MinColumnWidth = 85;
        }

        private void C1RulesEngine_RulesChanged(object sender, EventArgs e)
        {
            grid?.Items.Refresh();
        }
    }
}
