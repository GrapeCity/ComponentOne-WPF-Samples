using System;
using System.Collections.ObjectModel;

namespace ColorPickerExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem("Overview",
                "Overview",
                new DemoColorPicker()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
