using System;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using C1.WPF.LiveLinq;

using C1.LiveLinq;
using C1.LiveLinq.LiveViews;
using C1.Data;
using C1.Data.DataSource;

namespace Orders
{
    public partial class EditCountryOrders : System.Windows.Controls.UserControl
    {
        // A view source of Orders.
        ClientViewSource _viewSource;

        public EditCountryOrders()
        {
            InitializeComponent();

            // Update the displayed page index and count 
            // when they change in EntityViewSource.

            _viewSource = dataSource.ViewSources["Orders"];

            _viewSource.DataView.PropertyChanged += delegate
            {
                pageInfo.Text = string.Format("{0} / {1}", _viewSource.DataView.PageIndex + 1, _viewSource.DataView.PageCount);
            };
        }

        private void DataGrid1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Rebind the data grid, display the order details of the selected order.
            var item = dataGrid1.SelectedItem as Orders.Order;
            if (item != null)
                dataGrid2.ItemsSource = item.Order_Details;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Update the Order filter, display the orders of customes in the selected country.
            var country = comboBox.SelectedValue as string;
            if (country != null)
            {
                // Update the filter value.
                _viewSource.FilterDescriptors[0].Value = country;
                // Manually reload the EntityViewSource because auto-loading is turned off (AutoLoad=False).
                _viewSource.Load();
            }
        }

        private void MoveToPrevPage(object sender, RoutedEventArgs e)
        {
            _viewSource.DataView.MoveToPreviousPage();
        }

        private void MoveToNextPage(object sender, RoutedEventArgs e)
        {
            _viewSource.DataView.MoveToNextPage();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Initially show the customers from USA.
            int usa = comboBox.Items.IndexOf("USA");
            if (usa != -1)
                comboBox.SelectedIndex = usa;
        }
    }
}
