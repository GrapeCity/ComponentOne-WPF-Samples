using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace InputExplorer
{
    public partial class C1MultiSelect : UserControl
    {
        string[] PropertyNames = new string[] {
            "IsTagEditable", "Foreground", "MouseOverBrush", "SelectionBackground", "ShowCheckBoxes", "ShowSelectAll",
            "SelectAllCaption", "UnSelectAllCaption", "DisplayMemberPath", "SelectionMode",
            "ShowDropDownButton", "SelectedIndex", "IsTagEditable", "IsEditable", "PlaceHolder", "Separator", "HeaderFormat", "Text",
            "MaxHeaderItems", "MaxSelectedItems", "DisplayMode", "AutoSuggestMode","AutoCompleteMode", "HorizontalContentAlignment",
            "VerticalContentAlignment"};

        public C1MultiSelect()
        {
            InitializeComponent();
            Tag = Properties.Resources.MultiSelectDes;
        }

        ObservableCollection<Customer> _customers = DataProvider.GetCustomers();
        public ObservableCollection<Customer> Customers { get { return _customers; } }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsViewer.ItemsSource = MultiSelect.SelectedItems.ToList();
            
            var addItems = new List<object>();
            foreach (var item in e.AddedItems)
            {
                addItems.Add(item);
            }
            AddItemsViewer.ItemsSource = addItems;
            
            var removeItems = new List<object>();
            foreach (var item in e.RemovedItems)
            {
                removeItems.Add(item);
            }
            RemoveItemsViewer.ItemsSource = removeItems;
        }

        private void OnSelectAllClick(object sender, RoutedEventArgs e)
        {
            MultiSelect.SelectAll();
        }

        private void OnUnselectAllClick(object sender, RoutedEventArgs e)
        {
            MultiSelect.UnselectAll();
        }

        private void C1PropertyGrid_AddingPropertyBox(object sender, C1.WPF.PropertyGrid.ChangingPropertyBoxEventArgs e)
        {
            if (!PropertyNames.Contains(e.Property.Name))
                e.Cancel = true;
        }
    }
}
