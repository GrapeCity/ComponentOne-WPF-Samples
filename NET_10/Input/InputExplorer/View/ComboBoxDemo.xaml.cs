using C1.DataCollection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for ComboBoxDemo.xaml
    /// </summary>
    public partial class ComboBoxDemo : UserControl
    {
        public ComboBoxDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.C1ComboBoxDes;

            ComboBox1.ItemsSource = Country.GetCountries();
            ComboBox2.ItemsSource = Employee.GenerateData(7);

            var items = new ObservableCollection<Item>
            {
                new Item { Name = "Keyboard", Category = "Electronics" },
                new Item { Name = "Monitor", Category = "Electronics" },
                new Item { Name = "Apple", Category = "Food" },
                new Item { Name = "Bread", Category = "Food" },
                new Item { Name = "Jacket", Category = "Clothing" }
            };
            var dataCollection = new C1GroupDataCollection<Item>(items, false) { RunSynchronously = true };
            dataCollection.GroupAsync(nameof(Item.Category)).GetAwaiter().GetResult();
            GroupedComboBox.DisplayMemberPath = nameof(Item.Name);
            GroupedComboBox.ItemsSource = dataCollection;
        }

        public class Item
        {
            public string Name { get; set; }
            public string Category { get; set; }
        }
    }
}
