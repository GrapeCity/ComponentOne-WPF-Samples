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
                new SampleItem("Overview",
                                "Overview",
                                new DockControlBasic()),

                new SampleItem("Visual Studio",
                                "Visual Studio",
                                new VisualStudioLookPage())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
