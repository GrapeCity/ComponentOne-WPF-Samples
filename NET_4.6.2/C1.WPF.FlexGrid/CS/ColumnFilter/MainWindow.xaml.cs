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

namespace ColumnFilter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // populate grid
            _flex.ItemsSource = Product.GetProducts(2000);

            // this is how you would create a filter in code:
            // (we don't do that here because the filter was created in the XAML page)
            //var f = new C1FlexGridFilter(_flex);
            //f.FilterApplied += C1FlexGridFilter_FilterApplied;

            // show how many items we have in view
            UpdateStatusBar();
        }

        // clear filters
        void _btnClear_Click(object sender, RoutedEventArgs e)
        {
            var f = C1FlexGridFilterService.GetFlexGridFilter(_flex);
            f.Clear();
            f.Apply();
        }

        // show only blue products
        void _btnBlue_Click(object sender, RoutedEventArgs e)
        {
            // get grid filter
            var f = C1FlexGridFilterService.GetFlexGridFilter(_flex);

            // customize color column filter
            var c = _flex.Columns["Color"];
            var cf = f.GetColumnFilter(c);
            if (cf != null)
            {
                cf.ConditionFilter.Clear();
                cf.ValueFilter.Values = new string[] { "Blue" };
            }

            // apply changes
            f.Apply();
        }

        // show only expensive products
        void _btnExpensive_Click(object sender, RoutedEventArgs e)
        {
            // get filter
            var f = C1FlexGridFilterService.GetFlexGridFilter(_flex);

            // customize price column filter
            var c = _flex.Columns["Price"];
            var cf = f.GetColumnFilter(c);
            if (cf != null)
            {
                cf.ValueFilter.Clear();
                var c1 = cf.ConditionFilter.Condition1;
                c1.Operator = ConditionOperator.IsGreaterThanOrEqualTo;
                c1.Parameter = 900.5;
            }

            // apply changes
            f.Apply();
        }

        // toggle ICollectionView vs Row.Visible modes
        void _chkCollectionView_Click(object sender, RoutedEventArgs e)
        {
            var f = C1FlexGridFilterService.GetFlexGridFilter(_flex);
            f.UseCollectionView = _chkCollectionView.IsChecked.Value;
        }

        // save current filter definition
        string _filterDef = string.Empty;
        void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            var f = C1FlexGridFilterService.GetFlexGridFilter(_flex);
            _filterDef = f.FilterDefinition;
        }

        // load filter definition
        void _btnLoad_Click(object sender, RoutedEventArgs e)
        {
            var f = C1FlexGridFilterService.GetFlexGridFilter(_flex);
            f.FilterDefinition = _filterDef;
            f.Apply();
        }

        // update UI when filter is applied
        void C1FlexGridFilter_FilterApplied(object sender, EventArgs e)
        {
            UpdateStatusBar();
        }

        // show how many rows have passed the filter
        void UpdateStatusBar()
        {
            if ((_flex != null) && (_txtStatus != null))
            {
                var cnt = _flex.Rows.Count(r => r.Visible);
                _txtStatus.Text = string.Format("{0:n0} row{1} filtered.", cnt, cnt == 1 ? string.Empty : "s");
            }
        }
    }
}
