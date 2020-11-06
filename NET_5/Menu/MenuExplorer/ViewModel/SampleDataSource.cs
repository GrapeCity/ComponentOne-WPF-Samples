using System;
using System.Collections.ObjectModel;

namespace MenuExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem("Overview",
                                "Overview",
                                new DemoMenu()),
                new SampleItem("Custom Appearance",
                                "Custom Appearance",
                                new CustomAppearance()),
                new SampleItem("Drop Down Menu",
                                "Drop Down Menu",
                                new DropDownMenu())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
