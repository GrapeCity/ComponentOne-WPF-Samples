using System.Collections.ObjectModel;

namespace RibbonExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem("Overview",
                                "Overview",
                                new Ribbon()),

                new SampleItem("Toolstrip",
                                "Toolstrip",
                                new Toolstrip())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
