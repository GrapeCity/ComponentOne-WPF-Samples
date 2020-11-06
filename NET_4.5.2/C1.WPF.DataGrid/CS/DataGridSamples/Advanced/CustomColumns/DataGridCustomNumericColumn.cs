using System.Reflection;
using System.Windows.Controls;
using C1.WPF;
using C1.WPF.DataGrid;
using System.Windows;

namespace DataGridSamples
{
    public class DataGridCustomNumericColumn : DataGridNumericColumn
    {
        public DataGridCustomNumericColumn()
            : base()
        {
        }

        public DataGridCustomNumericColumn(PropertyInfo property)
            : base(property)
        {
        }

        public override FrameworkElement GetCellEditingContent(C1.WPF.DataGrid.DataGridRow row)
        {
            var numeric = (C1NumericBox)base.GetCellEditingContent(row);
            numeric.HandleUpDownKeys = false;
            return numeric;
        }
    }
}
