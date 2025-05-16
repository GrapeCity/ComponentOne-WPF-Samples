using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for AutoFilterPage.xaml
    /// </summary>
    public partial class AutoFilter : UserControl
    {
        public AutoFilter()
        {
            InitializeComponent();

            dataGrid.ItemsSource = PersonInfo.All;
        }

        private void dataGrid_FilterLoading(object sender, DataGridColumnEditableValueEventArgs<IDataGridFilter> e)
        {
            var column = e.Column;

            var multiValueFilter = new DataGridMultiValueFilter();
            multiValueFilter.Style = Resources["VirtualizingMultiValueFilter"] as Style;
            multiValueFilter.SetBinding(DataGridMultiValueFilter.BackgroundProperty, dataGrid, dg => dg.HeaderBackground);
            multiValueFilter.SetBinding(DataGridMultiValueFilter.ForegroundProperty, dataGrid, dg => dg.HeaderForeground);
            multiValueFilter.SetBinding(DataGridMultiValueFilter.BorderBrushProperty, dataGrid, dg => dg.BorderBrush);
            multiValueFilter.SetBinding(DataGridMultiValueFilter.MouseOverBrushProperty, dataGrid, dg => dg.MouseOverBrush);
            multiValueFilter.SetBinding(DataGridMultiValueFilter.PressedBrushProperty, dataGrid, dg => dg.PressedBrush);
            multiValueFilter.SetBinding(DataGridMultiValueFilter.InputBackgroundProperty, dataGrid, dg => dg.RowBackground);
            multiValueFilter.SetBinding(DataGridMultiValueFilter.InputForegroundProperty, dataGrid, dg => dg.RowForeground);
            (e.Value as DataGridFilter).InnerControl = multiValueFilter;
        }

        /*
         * --------------------------------------------------------------------------------------------------------
         *  Excel AutoFilter works as follows:
         *  For a specific column, it shows all the possible values in that column, 
         *  given the rest of the column filters as set (but not the current column one)
         * --------------------------------------------------------------------------------------------------------
         */
        void dataGrid_FilterOpened(object sender, DataGridColumnValueEventArgs<IDataGridFilter> e)
        {
            var boundColumn = e.Column as C1.WPF.DataGrid.DataGridBoundColumn;
            if (boundColumn == null)
                return;

            // get source values that satisfies all the rest of the filters
            var items = AutoFilterHelper.GetAutoFilterValues(PersonInfo.All, boundColumn);

            // get the distinct values for THIS column
            var currentColumnPath = boundColumn.Binding.Path.Path;
            var selector = C1PropertyPathHelper.CreateSelector<PersonInfo, object>(currentColumnPath);

            // create and set multi-value filter control into the column
            var filterControl = (DataGridMultiValueFilter)((DataGridFilter)e.Value).InnerControl;
            filterControl.ItemsSource = items.Select(selector).Distinct().OrderBy(o => o);
        }
    }
}
