using C1.WPF;
using C1.WPF.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InputSamples
{
    /// <summary>
    /// Interaction logic for C1MultiSelectPage.xaml
    /// </summary>
    public partial class C1MultiSelectPage : UserControl
    {
        string[] PropertyNames = new string[] {
            "IsTagEditable", "Foreground", "MouseOverBrush", "SelectionBackground", "ShowCheckBoxes", "ShowSelectAll",
            "SelectAllCaption", "UnSelectAllCaption", "DisplayMemberPath", "SelectionMode",
            "ShowDropDownButton", "SelectedIndex", "IsTagEditable", "IsEditable", "PlaceHolder", "Separator", "HeaderFormat", "Text",
            "MaxHeaderItems", "MaxSelectedItems", "DisplayMode", "AutoSuggestMode","AutoCompleteMode"};

        public C1MultiSelectPage()
        {
            InitializeComponent();
        }

        ObservableCollection<Customer> _customers = DataProvider.GetCustomers();
        public ObservableCollection<Customer> Customers { get { return _customers; } }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsViewer.ItemsSource = MultiSelect.SelectedItems;
            List<object> addItems = new List<object>();
            foreach(object item in e.AddedItems)
                addItems.Add(item);
            AddItemsViewer.ItemsSource = addItems;
            List<object> removeItems = new List<object>();
            foreach (object item in e.RemovedItems)
                removeItems.Add(item);
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

        private void C1PropertyGrid_AddingPropertyBox(object sender, C1.WPF.Extended.ChangingPropertyBoxEventArgs e)
        {
            if (!PropertyNames.Contains(e.Property.Name))
                e.Cancel = true;
        }
    }
}
