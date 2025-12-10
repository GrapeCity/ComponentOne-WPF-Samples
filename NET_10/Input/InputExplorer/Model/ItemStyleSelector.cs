using C1.WPF.Input;
using C1.WPF.ListView;
using System.Windows;
using System.Windows.Controls;

namespace InputExplorer
{
    public class ItemStyleSelector : StyleSelector
    {
        Style _common;
        Style _alternate;

        private Style Common => _common ?? (Style)Resources["Common"];
        private Style Alternate => _alternate ?? (Style)Resources["Alternate"];

        public ResourceDictionary Resources { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if(item is C1ComboBoxItem comboBoxItem && container is ListViewPanel panel)
            {
                for (int i = 0; i < panel.Children.Count; i++)
                {
                    ListViewItemView child = (ListViewItemView)panel.Children[i];
                    if (child.Content == comboBoxItem.Content)
                    {
                        return panel.Children.IndexOf(child) % 2 == 0 ? Common : Alternate;
                    }
                }
            }
            return null;
        }
    }
}