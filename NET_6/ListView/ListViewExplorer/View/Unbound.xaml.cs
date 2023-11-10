using ListViewExplorer.Resources;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class Unbound : UserControl
    {
        public Unbound()
        {
            InitializeComponent();

            Tag = AppResources.UnbounDesc;

            for (int i = 1; i < 100; i++)
            {
                listView.Items.Add("Item " + i);
            }

            listView.ShowCheckBoxes = true;

            cbSelectionMode.SelectedIndex = 1;
            cbShowCheckBox.IsChecked = true;

            listView.Orientation = Orientation.Vertical;
            cbOrientation.SelectedIndex = 1;
        }

        private void cbShowCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            listView.ShowCheckBoxes = cbShowCheckBox.IsChecked == true ? true : false;
        }

        private void cbShowSelectAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            listView.ShowSelectAll = cbShowSelectAll.IsChecked == true ? true : false;
        }

        private void cbSelectionMode_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (listView == null) return;
            if (e.NewValue == 0)
                listView.SelectionMode = C1.WPF.Core.C1SelectionMode.Single;

            if (e.NewValue == 1)
                listView.SelectionMode = C1.WPF.Core.C1SelectionMode.Extended;

            if (e.NewValue == 2)
                listView.SelectionMode = C1.WPF.Core.C1SelectionMode.Multiple;
        }

        private void cbOrientation_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (listView == null) return;
            if (e.NewValue == 0)
                listView.Orientation = Orientation.Horizontal;

            if (e.NewValue == 1)
                listView.Orientation = Orientation.Vertical;
        }
    }
}
