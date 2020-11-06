using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.Silverlight.DataGrid;
using System.Windows.Controls.Primitives;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    public class DataGridToggleValuesFilter : DataGridMultiValueFilter
    {
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToggleButton();
        }
    }
}
