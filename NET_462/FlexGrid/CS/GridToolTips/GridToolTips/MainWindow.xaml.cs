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
using C1.WPF.FlexGrid;

namespace GridToolTips
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _grid.CellFactory = new ToolTipCellFactory();
            var list = new List<Customer>();
            for (int i = 0; i < 50; i++)
            {
                list.Add(new Customer()
                {
                    Name = "Customer " + i.ToString(),
                    Age = 10 + i,
                    Active = i % 3 != 0
                });
            }
            _grid.ItemsSource = list;

            foreach (Column col in _grid.Columns)
            {
                col.Tag = string.Format("tag for column '{0}'", col.ColumnName);
            }

            // hook up event handler
            _grid.PrepareCellForEdit += _grid_PrepareCellForEdit;
        }

        // customize editor by changing the appearance of the selection
        void _grid_PrepareCellForEdit(object sender, CellEditEventArgs e)
        {
            var b = e.Editor as Border;
            var tb = b.Child as TextBox;
            if (tb != null)
            {
                tb.Background = new SolidColorBrush(Colors.Black);
                tb.Foreground = new SolidColorBrush(Colors.White);
            }
        }
    }
}
