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
using System.Reflection;


namespace FilterRow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // set this to true to show filtering in unbound mode
        const bool UNBOUND = false;

        public MainWindow()
        {
            InitializeComponent();

            // populate the grid
            if (UNBOUND)
            {
                // add columns
                foreach (PropertyInfo pi in typeof(Product).GetProperties())
                {
                    var c = new C1.WPF.FlexGrid.Column();
                    c.ColumnName = pi.Name;
                    c.DataType = pi.PropertyType;
                    _flex.Columns.Add(c);
                }

                // add rows
                foreach (var item in Product.GetProducts(100))
                {
                    var r = new C1.WPF.FlexGrid.Row();
                    _flex.Rows.Add(r);
                    foreach (PropertyInfo pi in typeof(Product).GetProperties())
                    {
                        r[pi.Name] = pi.GetValue(item, null);
                    }
                }
            }
            else
            {
                _flex.ItemsSource = Product.GetProducts(100);
            }

            // create filter row
            _flex.CellFactory = new FilterRowCellFactory();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var cf = _flex.CellFactory as FilterRowCellFactory;
            cf.UseRegEx = ((CheckBox)sender).IsChecked.Value;

            cf.RefreshFilter(_flex);
        }
    }
}
