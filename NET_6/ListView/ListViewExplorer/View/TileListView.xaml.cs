using ListViewExplorer.Resources;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class TileListView : UserControl
    {
        public TileListView()
        {
            InitializeComponent();

            Tag = AppResources.TileListViewTag;

            _list1.ItemsSource = Person.Generate(1000000);

            //for (int i = 1; i < 1000; i++)
            //{
            //    _list1.Items.Add
            //    (
            //        new C1.WPF.ListView.ListViewItem
            //        {
            //            Content = "Item " + i
            //        }
            //    );
            //}


            cbShowCheckBox.IsChecked = true;
            cbShowSelectAll.IsChecked = false;
            cbSelectionMode.SelectedIndex = 1;
            cb.SelectedIndex = 0;
            _list1.ShowCheckBoxes = true;
            _list1.ShowSelectAll = false;
        }

        private void cb_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (_list1 != null)
                _list1.Orientation = e.NewValue == 0 ? Orientation.Horizontal : Orientation.Vertical;
        }

        private void cbShowCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _list1.ShowCheckBoxes = cbShowCheckBox.IsChecked == true ? true : false;
        }

        private void cbShowSelectAll_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _list1.ShowSelectAll = cbShowSelectAll.IsChecked == true ? true : false;
        }
        private void cbSelectionMode_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (_list1 == null) return;
            if (e.NewValue == 0)
                _list1.SelectionMode = C1.WPF.Core.C1SelectionMode.Single;

            if (e.NewValue == 1)
                _list1.SelectionMode = C1.WPF.Core.C1SelectionMode.Extended;

            if (e.NewValue == 2)
                _list1.SelectionMode = C1.WPF.Core.C1SelectionMode.Multiple;
        }
    }
}
