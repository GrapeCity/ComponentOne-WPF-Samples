using System;
using System.Collections.ObjectModel;

namespace FlexPivotExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Overview",
                "Overview",
                new System.Lazy<System.Windows.Controls.UserControl>(() => new FlexPivotDemo())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
            _allItems.Add(new SampleItem("Custom Cell Factory",
                "Custom Cell Factory",
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomCellFactory())));
            _allItems.Add(new SampleItem("Custom Columns",
                "Custom Columns",
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomColumns())));
            _allItems.Add(new SampleItem("Custom Page",
                "Custom Page",
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomPage())));
            _allItems.Add(new SampleItem("Multi Values",
                "Multi Values",
                new System.Lazy<System.Windows.Controls.UserControl>(() => new MultiValues())));
            _allItems.Add(new SampleItem("Custom Template",
                "Custom Template",
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomTemplate())));
            _allItems.Add(new SampleItem("Data Engine",
                "Data Engine",
                new System.Lazy<System.Windows.Controls.UserControl>(() => new DataEngine())));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
