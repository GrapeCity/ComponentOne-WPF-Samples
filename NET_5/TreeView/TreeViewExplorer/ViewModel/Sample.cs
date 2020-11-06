using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace TreeViewExplorer
{
    public class SampleItem
    {
        public SampleItem(string name, string title, Control sample)
        {
            Name = name;
            Title = title;
            Sample = sample;
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description => Sample.Tag?.ToString();
        public Control Sample { get; set; }
    }

    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Overview",
                "Overview",
                new DemoTreeView()));
            _allItems.Add(new SampleItem("Binding",
                "Binding",
                new DemoSimpleC1TreeView()));
            _allItems.Add(new SampleItem("Drag & Drop",
                "Drag & Drop",
                new DemoTreeViewDragDrop()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
