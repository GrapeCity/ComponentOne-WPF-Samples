using C1.DataCollection;
using C1.WPF.Core;
using C1.WPF.Menu;
using ListViewExplorer.Resources;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class CustomTemplate : UserControl
    {
        public CustomTemplate()
        {
            InitializeComponent();

            Tag = AppResources.CustomTemplateDescription;
            C1ListView.ItemsSource = Person.Generate(1000000);

            cbSelectionMode.SelectedIndex = 0;
            cbShowCheckBox.IsChecked = true;

            C1ListView.Orientation = Orientation.Vertical;
            C1ListView.ItemHeight = 72;
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

        private void OnItemTapped(object sender, C1.WPF.Core.C1TappedEventArgs e)
        {
            if (e.IsRightTapped)
            {
                var menu = new C1ContextMenu();
                var removeMenuItem = new C1MenuItem() { IconTemplate = C1IconTemplate.Delete, Header = "Remove item" };
                removeMenuItem.Click += OnRemoveMenuItem;
                menu.Items.Add(removeMenuItem);
                menu.Show(sender as FrameworkElement);
            }
        }

        private async void OnRemoveMenuItem(object sender, SourcedEventArgs e)
        {
            if (C1ListView.SelectedIndex >= 0)
                await C1ListView.DataCollection.RemoveAsync(C1ListView.SelectedIndex);
        }
    }
}
