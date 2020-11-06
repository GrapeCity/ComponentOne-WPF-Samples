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
using System.ComponentModel;
using C1.WPF.FlexGrid;

namespace CustomTypeDescriptor
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        ICollectionView _mainView;

        public Window1()
        {
            InitializeComponent();

            // DataTable as source
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Serial", typeof(int));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Current", typeof(bool));
            var rnd = new Random();
            for (int r = 0; r < 100; r++)
            {
                dt.Rows.Add(
                    string.Format("item {0}", r),
                    r,
                    new decimal(rnd.NextDouble() * 10000 + 1000),
                    rnd.NextDouble() < 0.8);
            }
            var dtView = new ListCollectionView(dt.DefaultView);

            // CustomTypeDescriptor as source
            var fd = FinancialData.GetFinancialData();
            var fdView = new ListCollectionView(fd);

            // bind c1 grids
            _flexDataTable.ItemsSource = dtView;
            _flexFinancial.ItemsSource = fdView;

            // save main view to customize in event handlers
            _mainView = fdView;
            _mainView.CollectionChanged += _mainView_CollectionChanged;

            // configure search box
            _search.View = _mainView;
            var ctd = _mainView.CurrentItem as ICustomTypeDescriptor;
            foreach (PropertyDescriptor pd in ctd.GetProperties())
            {
                if (pd.Name == "Symbol" || pd.Name == "Name")
                {
                    _search.FilterProperties.Add(pd);
                }
            }

            // customize group row content
            _flexFinancial.GroupHeaderConverter = new SymbolGroupHeaderConverter();
        }

        // event handlers
        void _chkGroup_Click(object sender, RoutedEventArgs e)
        {
            // toggle grouping
            _mainView.GroupDescriptions.Clear();
            if (_chkGroup.IsChecked.Value)
            {
                _mainView.GroupDescriptions.Add(new PropertyGroupDescription("Symbol"));
                var gd = _mainView.GroupDescriptions[0] as PropertyGroupDescription;
                gd.Converter = new StringInitialConverter();
            }
        }
        void _chkCustomCells_Click(object sender, RoutedEventArgs e)
        {
            if (_chkCustomCells.IsChecked.Value)
            {
                // create custom cell factory
                _flexFinancial.CellFactory = new FinancialCellFactory();

                // make custom columns wider
                foreach (string id in "LastSale,Bid,Ask".Split(','))
                {
                    _flexFinancial.Columns[id].Width = new GridLength(150);
                }
            }
            else
            {
                _flexFinancial.CellFactory = null;
            }
        }
        void _mainView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var count = (from object x in _mainView select x).Count();
            _txtStatus.Text = string.Format("Record Count: {0:n0}", count);

            // note: this works too, but the count includes the group rows:
            //var count = _flexFinancial.Rows.Count;
        }

        // converter used to group strings by their first letter
        class StringInitialConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                return ((string)value)[0].ToString().ToUpper();
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Converter used to customize content of group rows.
        /// </summary>
        class SymbolGroupHeaderConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var gr = parameter as GroupRow;
                var group = gr.Group;
                if (group != null && gr != null && targetType == typeof(string))
                {
                    return string.Format("{0:n0} symbols starting with '{1}'",
                        group.ItemCount,
                        group.Name);
                }

                // default
                return value;
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
