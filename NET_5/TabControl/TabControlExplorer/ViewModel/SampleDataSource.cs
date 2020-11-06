using System;
using System.Collections.ObjectModel;

namespace TabControlExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(
                    "Overview",
                    "Overview",
                    new Overview()),
                new SampleItem(
                    "Tab Positions",
                    "Tab Positions",
                    new TabPositions()),
                new SampleItem(
                    "Simplified Indicator",
                    "Simplified Indicator",
                    new SimplifiedIndicator()),
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
