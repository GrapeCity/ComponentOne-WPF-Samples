
using System.ComponentModel;

namespace Orders.View
{
    public partial class OrdersView : System.Windows.Controls.UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
        }

        private void OnDataGrid1SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Rebind the data grid, display the order details of the selected order.
            var item = (ViewModel.OrderViewModel)dataGrid1.SelectedItem;
            if (item != null)
            {
                var ecv = item.OrderDetails as IEditableCollectionView;
                if (ecv.IsEditingItem)
                    // to compensate for a TabControl idiosyncrasy
                    ecv.CommitEdit();
                dataGrid2.ItemsSource = item.OrderDetails;
            }
        }
    }
}