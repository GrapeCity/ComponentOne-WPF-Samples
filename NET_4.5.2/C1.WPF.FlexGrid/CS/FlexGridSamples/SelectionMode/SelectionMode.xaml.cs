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
using System.Collections.ObjectModel;
using C1.WPF.FlexGrid;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for SelectionMode.xaml
    /// </summary>
    public partial class SelectionMode : UserControl
    {
        const int ROW_COUNT = 500;

        public SelectionMode()
        {
            InitializeComponent();
            BindGrid();
        }

        void BindGrid()
        {
            // create PagedCollectionView used as a data source for the
            // FlexGrid and for the MS DataGrid
            var data = new ObservableCollection<Customer>();
            for (int i = 0; i < ROW_COUNT; i++)
            {
                data.Add(new Customer(i));
            }
            var view = new MyCollectionView(data);
            using (view.DeferRefresh())
            {
                view.GroupDescriptions.Clear();
                view.GroupDescriptions.Add(new PropertyGroupDescription("Country"));
                view.GroupDescriptions.Add(new PropertyGroupDescription("Active"));
                var gd = view.GroupDescriptions[0] as PropertyGroupDescription;
                gd.Converter = new CountryInitialConverter();
            }

            // bind grids to ListCollectionView
            _flexSelectionMode.ItemsSource = view;
        }

        // change selection mode
        void _cmbSelectionMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cmbSelectionMode != null && _flexSelectionMode != null)
            {
                _flexSelectionMode.SelectionMode = (C1.WPF.FlexGrid.SelectionMode)_cmbSelectionMode.SelectedIndex;
            }
        }

        // turn merging on and off
        void _chkAllowMergingSelMode_Click(object sender, RoutedEventArgs e)
        {
            var f = _flexSelectionMode;
            if (f != null)
            {
                var allowMerging = ((CheckBox)sender).IsChecked.Value;
                f.AllowMerging = allowMerging
                    ? AllowMerging.Cells
                    : AllowMerging.None;
                f.Columns["Country"].AllowMerging = allowMerging;
                f.Columns["First"].AllowMerging = allowMerging;
                f.Columns["Last"].AllowMerging = allowMerging;
            }
        }

        void _flexSelectionMode_SelectionChanged(object sender, CellRangeEventArgs e)
        {
            var fg = _flexSelectionMode;
            var customers = fg.SelectedItems;
            _lblSelState.Text = string.Format("{0} item{1} selected, {2} active, total weight: {3:n2}",
                customers.Count,
                customers.Count == 1 ? string.Empty : "s",
                (from Customer c in customers where c.Active select c).Count(),
                (from Customer c in customers select c.Weight).Sum());
        }
    }
}
