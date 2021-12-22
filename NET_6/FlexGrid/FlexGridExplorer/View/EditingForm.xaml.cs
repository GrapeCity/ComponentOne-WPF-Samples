using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for EditingForm.xaml
    /// </summary>
    public partial class EditingForm : UserControl
    {
        GridCellRange selectedRange;
        public EditingForm()
        {
            InitializeComponent();
            Tag = AppResources.EditingDescription;
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.HeadersVisibility = GridHeadersVisibility.All;
            grid.IsReadOnly = true;
            grid.MinColumnWidth = 85;
            grid.SelectionMode = GridSelectionMode.Row;
            grid.SelectionChanged += Grid_SelectionChanged;
            grid.CellDoubleTapped += Grid_DoubleClick;
        }

        private void Grid_DoubleClick(object sender, GridInputEventArgs e)
        {
            if (this.selectedRange != null)
            {
                Customer c = grid.Rows[this.selectedRange.Row].DataItem as Customer;
                if (c != null)
                    new EditCustomerForm(c).ShowDialog();
            }
        }

        private void Grid_SelectionChanged(object sender, GridCellRangeEventArgs e)
        {
            this.selectedRange = e.CellRange;
        }
    }
}
