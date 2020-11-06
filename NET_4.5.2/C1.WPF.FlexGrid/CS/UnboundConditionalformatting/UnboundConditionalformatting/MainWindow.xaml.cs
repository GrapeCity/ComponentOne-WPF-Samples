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

namespace UnboundConditionalformatting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var flex = new C1FlexGrid();
            flex.CellFactory = new ConditionalFormattingFactory();
            LayoutRoot.Children.Add(flex);

            var list = Product.GetProducts(100);

            foreach (var pi in typeof(Product).GetProperties())
            {
                var c = new Column();
                c.Header = pi.Name;
                c.DataType = pi.PropertyType;
                if (pi.PropertyType == typeof(double))
                {
                    c.Format = "n0";
                }
                flex.Columns.Add(c);
            }

            foreach (var p in list)
            {
                var r = new Row();
                flex.Rows.Add(r);
                foreach (var c in flex.Columns)
                {
                    var pi = typeof(Product).GetProperty(c.Header);
                    r[c] = pi.GetValue(p, null);
                }
            }
        }
    }
}
