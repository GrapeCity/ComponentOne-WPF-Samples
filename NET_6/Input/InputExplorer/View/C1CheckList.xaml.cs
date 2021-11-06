using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InputExplorer
{
    public partial class C1CheckList : UserControl
    {
        string[] PropertyNames = new string[] {
            "Background", "Foreground", "MouseOverBrush", "SelectionBackground", "ShowCheckBoxes", "ShowSelectAll",
            "SelectAllCaption", "UnSelectAllCaption", "DisplayMemberPath", "SelectionMode", "HorizontalContentAlignment",
            "VerticalContentAlignment"};

        public C1CheckList()
        {
            InitializeComponent();
            Tag = Properties.Resources.CheckListDes;
        }

        ObservableCollection<Customer> _customers = DataProvider.GetCustomers();
        public ObservableCollection<Customer> Customers { get { return _customers; } }

        private void OnSelectAllClick(object sender, RoutedEventArgs e)
        {
            CheckList.SelectAll();
        }

        private void OnUnselectAllClick(object sender, RoutedEventArgs e)
        {
            CheckList.UnselectAll();
        }

        private void OnSelectionChanged(object sender, C1.WPF.Core.SelectionChangedEventArgs<int> e)
        {
            List<object> addItems = new List<object>();
            foreach (int index in e.AddedItems)
            {
                addItems.Add(CheckList.Items[index].Content);
            }
            AddItemsViewer.ItemsSource = addItems;
            List<object> removeItems = new List<object>();
            foreach (int index in e.RemovedItems)
            {
                removeItems.Add(CheckList.Items[index].Content);
            }
            RemoveItemsViewer.ItemsSource = removeItems;
        }

        private void C1PropertyGrid_AddingPropertyBox(object sender, C1.WPF.PropertyGrid.ChangingPropertyBoxEventArgs e)
        {
            if (!PropertyNames.Contains(e.Property.Name))
                e.Cancel = true;
        }
    }
}
