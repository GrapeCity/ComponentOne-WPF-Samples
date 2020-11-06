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
using ColumnFooters.Data;

namespace ColumnFooters
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region BoundGrid
            // populate bound grid
            var products = Product.GetProducts(250);
            var view = new ListCollectionView(products);
            _flexBound.ItemsSource = view;

            // add a column footer row to the bound grid
            AddColumnFooter(_flexBound);

            // show aggregate values in subtotal cell
            foreach (var colName in "Price,Cost,Weight,Volume".Split(','))
            {
                var c = _flexBound.Columns[colName];
                c.GroupAggregate = C1.WPF.FlexGrid.Aggregate.Sum;

            }
           
            #endregion

            #region Unbound
            //populate unbound grid
            AddColumn("Name", typeof(string), null);
            AddColumn("Color", typeof(string), null);
            AddColumn("Price", typeof(double), "n2");
            AddColumn("Cost", typeof(double), "n2");
            AddColumn("Weight", typeof(double), "n2");
            AddColumn("Volume", typeof(double), "n2");
            AddColumn("Discontinued", typeof(bool), null);
            AddColumn("Rating", typeof(int), "n2");
            foreach (var p in products)
            {
                var row = new C1.WPF.FlexGrid.Row();
                _flexUnbound.Rows.Add(row);
                row["Name"] = p.Name;
                row["Color"] = p.Color;
                row["Price"] = p.Price;
                row["Cost"] = p.Cost;
                row["Weight"] = p.Weight;
                row["Volume"] = p.Volume;
                row["Discontinued"] = p.Discontinued;
                row["Rating"] = p.Rating;
            }

            // add a column footer row to the unbound grid
            AddColumnFooter(_flexUnbound);

            // show aggregate values in subtotal cell
            foreach (var colName in "Price,Cost,Weight,Volume".Split(','))
            {
                var c = _flexUnbound.Columns[colName];
                c.GroupAggregate = C1.WPF.FlexGrid.Aggregate.Sum;
            } 
            #endregion
        }
        void AddColumn(string name, Type type, string fmt)
        {
            var c = new C1.WPF.FlexGrid.Column();
            c.ColumnName = name;
            c.DataType = type;
            c.Format = fmt;
            _flexUnbound.Columns.Add(c);
        }
        // create group row to show column footer cells 
        // NOTE: 
        //   using a group row because that is read-only and provides
        //   automatic aggregates when the column's GroupAggregate 
        //   property is set.
        void AddColumnFooter(C1.WPF.FlexGrid.C1FlexGrid flex)
        {
            

            flex.ColumnFooters.Columns.Add(new Column());
            var gr = new C1.WPF.FlexGrid.GroupRow();

            // customize appearance of the new row
            gr.FontWeight = FontWeights.Bold;
            gr.Background = new SolidColorBrush(Color.FromArgb(0xff, 0x80, 0x00, 0x00));
            gr.Foreground = new SolidColorBrush(Colors.White);
            // add the row to the ColumnFooters GridPanel
            flex.ColumnFooters.Rows.Add(gr);
            gr[0] = "Totals";
         
        }

        // print bound grid
        void _btnPrintBound_Click(object sender, RoutedEventArgs e)
        {
            _flexBound.Print("Bound FlexGrid with column footers");
        }

        // print unbound grid
        void _btnPrintUnound_Click(object sender, RoutedEventArgs e)
        {
            _flexUnbound.Print("Unbound FlexGrid with column footers");
        }
    }
}
