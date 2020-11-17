using DockingExplorer.Resources;
using System;
using System.Collections.ObjectModel;

namespace DockingExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(AppResources.OverviewTitle,
                               AppResources.OverviewDescription,
                               new DockControlBasic()),

                new SampleItem(AppResources.VisualStudioTitle,
                               AppResources.VisualStudioDescription,
                                new VisualStudioLookPage())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
