using System.Windows;
using C1.WPF;

namespace C1TreeViewTemplateSample2010
{
    /// <summary>
    /// Selects a style for the nodes that are ancestors of a C1.Silverlight assembly class
    /// so they start with the IsExpanded property set to True
    /// </summary>
    public class CustomStyleSelector : C1StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            var group = ((C1TreeViewItem)container).Header as WorldCupGroup;

            if (group != null)
                return (Style)Resources["ExpandedStyle"];

            return null;
        }
    }
}
