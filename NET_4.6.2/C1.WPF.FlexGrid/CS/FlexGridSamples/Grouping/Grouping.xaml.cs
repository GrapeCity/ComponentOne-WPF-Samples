using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using C1.WPF.FlexGrid;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for Grouping.xaml
    /// </summary>
    public partial class Grouping : UserControl
    {
        const int ROW_COUNT = 500;

        public Grouping()
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
            _flexGroup.ItemsSource = view;
            _flexGroup.KeyDown += _flexGroup_KeyDown;
        }

        // add a new item when the user presses INS
        void _flexGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Insert)
            {
                var view = _flexGroup.ItemsSource as ICollectionView;
                var list = view.SourceCollection as ObservableCollection<Customer>;
                list.Add(new Customer(-1));
            }
        }

        // look for text in the grid
        void _txtFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            // select row in the FlexGrid
            int col = _flexGroup.Columns["Name"].Index;
            int row = FindRow(_flexGroup, _txtFind.Text, _flexGroup.Selection.Row, col, true);
            if (row > -1)
            {
                _flexGroup.Select(row, col);
                _txtFind.Background = new SolidColorBrush(Colors.White);
            }
            else
            {
                _txtFind.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xff, 0xee, 0xee));
            }
        }

        // look for a row that contains some text in a specific column
        int FindRow(C1FlexGrid flex, string text, int startRow, int col, bool wrap)
        {
            startRow = Math.Max(startRow, 0);
            int count = flex.Rows.Count;
            for (int off = 0; off <= count; off++)
            {
                // reached the bottom and not wrapping? quit now
                if (!wrap && startRow + off >= count)
                {
                    break;
                }

                // get text from row
                int row = (startRow + off) % count;
                var content = flex[row, col];

                // found? return row index
                if (content != null &&
                    content.ToString().IndexOf(text, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    return row;
                }
            }

            // not found...
            return -1;
        }

        // group by country or by initial
        void _chkGroupByInitial_Click(object sender, RoutedEventArgs e)
        {
            if (_flexGroup != null)
            {
                var view = _flexGroup.ItemsSource as ICollectionView;
                var gd = view.GroupDescriptions[0] as PropertyGroupDescription;
                gd.Converter = ((CheckBox)sender).IsChecked.Value
                    ? new CountryInitialConverter()
                    : null;
            }
        }

        // turn merging on and off
        void _chkAllowMerging_Click(object sender, RoutedEventArgs e)
        {
            var f = _flexGroup;
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

        // show groups
        void _chkShowGroups_Click(object sender, RoutedEventArgs e)
        {
            _flexGroup.GroupRowPosition = ((CheckBox)sender).IsChecked.Value
                ? GroupRowPosition.AboveData
                : GroupRowPosition.None;
        }

        // keep current item visible 
        // (e.g. is the user selects an object in the MS grid, the flex will
        // automatically scroll to keep the object in view)
        void _chkKeepCurrentVisible_Click(object sender, RoutedEventArgs e)
        {
            _flexGroup.KeepCurrentVisible = ((CheckBox)sender).IsChecked.Value;
        }

        // freeze/unfreeze panes
        void _chkFreezePanes_Click(object sender, RoutedEventArgs e)
        {
            if (_chkFreezePanes.IsChecked.Value)
            {
                _flexGroup.Rows.Frozen = Math.Min(_flexGroup.Selection.Row, 5);
                _flexGroup.Columns.Frozen = Math.Min(_flexGroup.Selection.Column, 3);
            }
            else
            {
                _flexGroup.Rows.Frozen = 0;
                _flexGroup.Columns.Frozen = 0;
            }
        }
    }
}
