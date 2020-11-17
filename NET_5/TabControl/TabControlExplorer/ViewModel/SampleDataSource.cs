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
                    Properties.Resources.OverviewTitle,
                    Properties.Resources.OverviewTitle,
                    new Overview()),
                new SampleItem(
                    Properties.Resources.TabPositionTitle,
                    Properties.Resources.TabPositionTitle,
                    new TabPositions()),
                new SampleItem(
                    Properties.Resources.IndicatorTitle,
                    Properties.Resources.IndicatorTitle,
                    new SimplifiedIndicator()),
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
