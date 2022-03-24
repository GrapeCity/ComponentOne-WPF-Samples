using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for EditingForm.xaml
    /// </summary>
    public partial class EditingForm : Page
    {
        CellRange selectedRange;
        public EditingForm()
        {
            InitializeComponent();

            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.HeadersVisibility = HeadersVisibility.All;
            grid.IsReadOnly = true;
            grid.MinColumnWidth = 85;
            grid.SelectionMode = C1.WPF.FlexGrid.SelectionMode.Row;
            grid.SelectionChanged += Grid_SelectionChanged;
            grid.DoubleClick += Grid_DoubleClick;
        }

        private void Grid_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.selectedRange != null)
            {
                Customer c = grid.Rows[this.selectedRange.Row].DataItem as Customer;
                if (c != null)
                    new EditCustomerForm(c).ShowDialog();
            }
        }

        private void Grid_SelectionChanged(object sender, CellRangeEventArgs e)
        {
            this.selectedRange = e.CellRange;
        }
    }
}
