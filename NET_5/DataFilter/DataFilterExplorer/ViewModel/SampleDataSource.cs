using System;
using System.Collections.ObjectModel;

namespace DataFilterExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Cars List",
                "Cars List",
                new Lazy<System.Windows.Controls.Control>(() => new CarsListControl())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
            _allItems.Add(new SampleItem("Filter Summary",
                "Filter Summary",
                new Lazy<System.Windows.Controls.Control>(() => new FilterSummarySample())));
            _allItems.Add(new SampleItem("Filter Editor",
                "Filter Editor",
                new Lazy<System.Windows.Controls.Control>(() => new FilterEditorSample())));
            _allItems.Add(new SampleItem("Custom Filters",
                "Custom Filters",
                new Lazy<System.Windows.Controls.Control>(() => new CustomFiltersSample())));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
