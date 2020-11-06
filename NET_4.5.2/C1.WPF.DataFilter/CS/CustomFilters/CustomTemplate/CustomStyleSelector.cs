using System.Windows;
using C1.WPF;
using CustomFilters.Model;

namespace CustomFilters
{
    /// <summary>
    /// Selects a style for the nodes that are ancestors of a C1.Silverlight assembly class
    /// so they start with the IsExpanded property set to True
    /// </summary>
    public class CustomStyleSelector : C1StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (((C1TreeViewItem)container).Header is CarStoreGroup)
            {
                return (Style)Resources["ExpandedStyle"];
            }

            return null;
        }
    }
}
