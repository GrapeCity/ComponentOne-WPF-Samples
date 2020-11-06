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
using System.ComponentModel;
using C1.WPF;

namespace ControlExplorer
{
    public class HierarchicalStyleSelector : C1StyleSelector
    {
        public Style FirstLevelStyle { get; set; }
        public Style SecondLevelStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var treeViewItem = container as C1TreeViewItem;
            var level = GetItemLevel(item);
            if (level == 0)
            {
                return FirstLevelStyle;
            }
            else if (level == 1)
            {
                return SecondLevelStyle;
            }
            else
            {
                return base.SelectStyle(item, container);
            }
        }

        public static int GetItemLevel(object item)
        {
            if (item is SearchResult)
                return 0;
            else
                return 1;
        }
    }

    public class HierarchicalTemplateSelector : C1DataTemplateSelector
    {
        public DataTemplate FirstLevelTemplate { get; set; }
        public DataTemplate SecondLevelTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var treeViewItem = container as C1TreeViewItem;
            var level = HierarchicalStyleSelector.GetItemLevel(item);
            if (level == 0)
            {
                return FirstLevelTemplate;
            }
            else if (level == 1)
            {
                return SecondLevelTemplate;
            }
            else
            {
                return base.SelectTemplate(item, container);
            }
        }
    }
}
