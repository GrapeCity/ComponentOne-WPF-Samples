using System.Collections.Generic;
using System.Linq;
using C1.WPF;
using C1.WPF.DataGrid;


namespace DataGridSamples
{
    public static class AutoFilterHelper
    {
        public static IEnumerable<T> GetAutoFilterValues<T>(IEnumerable<T> source, DataGridColumn column)
        {
            var grid = column.DataGrid;

            // get all filter states without taking the current column into account
            var filterStates = grid.FilteredColumns
                                    .Where(c => c != column)
                                    .Select(c => new DataGridColumnValue<DataGridFilterState>(c, c.FilterState));

            var filter = C1DataGridFilterHelper.BuildFilterPredicate<T>(filterStates.ToArray(), DataGridFilterCombination.And);

            if (filter == null)
                return source;

            var filteredElements = new List<T>();
            foreach (var elem in source)
            {
                if (filter(elem))
                {
                    filteredElements.Add(elem);
                }
            }
            return filteredElements;
        }
    }
}
