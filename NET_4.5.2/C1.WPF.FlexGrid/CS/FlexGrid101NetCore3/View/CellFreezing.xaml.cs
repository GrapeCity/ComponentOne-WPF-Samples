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
    /// Interaction logic for CellFreezing.xaml
    /// </summary>
    public partial class CellFreezing : Page
    {
        public CellFreezing()
        {
            InitializeComponent();

            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.Columns["Country"].AllowMerging = true;
            grid.MinColumnWidth = 85;
        }
    }
}
