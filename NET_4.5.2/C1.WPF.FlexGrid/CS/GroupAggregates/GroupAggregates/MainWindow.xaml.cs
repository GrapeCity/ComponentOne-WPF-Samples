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

namespace GroupAggregates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // populate Aggregate combobox
            var aggregates = new Aggregate[] 
            {
                Aggregate.None, 
                Aggregate.Sum, 
                Aggregate.Average, 
                Aggregate.Maximum, 
                Aggregate.Minimum, 
                Aggregate.Count 
            };

            foreach (var agg in aggregates)
            {
                _cmbAggregates.Items.Add(agg);
            }
            _cmbAggregates.SelectedIndex = 1; // Aggregate.Sum

            // build data source
            var view = Product.GetProducts(200);

            // set up grouping
            var gd = view.GroupDescriptions;
            gd.Add(new PropertyGroupDescription("Total"));  // grand total group
            gd.Add(new PropertyGroupDescription("Line"));   // group by product line
            gd.Add(new PropertyGroupDescription("Color"));  // group by product color
            gd.Add(new PropertyGroupDescription("Price"));  // group by price range
            var pgdPrice = gd[gd.Count - 1] as PropertyGroupDescription;
            pgdPrice.Converter = new PriceRangeConverter();

            // assign data source to grid
            _flex.ItemsSource = view;

            // provide choices in grid editors
            _flex.Columns["Line"].ValueConverter = new ColumnValueConverter(Product.GetLines(), true);
            _flex.Columns["Color"].ValueConverter = new ColumnValueConverter(Product.GetColors(), false);

            // needed to show aggregates
            _flex.AreRowGroupHeadersFrozen = false;
        }

        private void _cmbAggregates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (string s in "Price,Cost,Weight,Volume".Split(','))
            {
                _flex.Columns[s].GroupAggregate = (Aggregate)_cmbAggregates.SelectedItem;
            }
        }
    }
}
