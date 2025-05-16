using ListViewExplorer.Resources;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class StyleSelectorSample : UserControl
    {
        public StyleSelectorSample()
        {
            InitializeComponent();

            Tag = AppResources.StyleSelectorDesc;
            var items = new List<ItemBase>();
            for (int i = 0; i < 1000; i++)
            {
                if (i % 2 == 0)
                    items.Add(new TeamA());
                else
                    items.Add(new TeamB());
            }
            C1ListView.ItemsSource = items;

            cbSelectionMode.SelectedIndex = 1;
            cbShowCheckBox.IsChecked = true;

            C1ListView.Orientation = Orientation.Vertical;
            cbOrientation.SelectedIndex = 1;

        }

        private void cbShowCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            C1ListView.ShowCheckBoxes = cbShowCheckBox.IsChecked == true ? true : false;
        }

        private void cbShowSelectAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            C1ListView.ShowSelectAll = cbShowSelectAll.IsChecked == true ? true : false;
        }

        private void cbSelectionMode_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (C1ListView == null) return;
            if (e.NewValue == 0)
                C1ListView.SelectionMode = C1.WPF.Core.C1SelectionMode.Single;
            
            if (e.NewValue == 1)
                C1ListView.SelectionMode = C1.WPF.Core.C1SelectionMode.Extended;
            
            if (e.NewValue == 2)
                C1ListView.SelectionMode = C1.WPF.Core.C1SelectionMode.Multiple;
        }

        private void cbOrientation_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (C1ListView == null) return;
            if (e.NewValue == 0)
                C1ListView.Orientation = Orientation.Horizontal;

            if (e.NewValue == 1)
                C1ListView.Orientation = Orientation.Vertical;
        }
    }

    public class MyStyleSelector : StyleSelector
    {
        Style _teamA;
        Style _teamB;
        private Style TeamA
        {
            get
            {
                return _teamA ?? (_teamA = (Style)Resources["TeamA"]);
            }
        }
        private Style TeamB
        {
            get
            {
                return _teamB ?? (_teamB = (Style)Resources["TeamB"]);
            }
        }

        public ResourceDictionary Resources { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            var lvItem = item as C1.WPF.ListView.ListViewItem;
            if (lvItem.Content is TeamA)
                return TeamA;
            return TeamB;
        }
    }
}
