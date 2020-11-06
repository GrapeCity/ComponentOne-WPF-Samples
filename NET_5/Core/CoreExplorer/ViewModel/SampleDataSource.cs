using System;
using System.Collections.ObjectModel;

namespace CoreExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem("Input Handling",
                "Input Handling",
                new InputHandling()));

            AllItems.Add(new SampleItem("Drag Drop Manager",
                "Drag Drop Manager",
                new DemoDragDropManager()));

            AllItems.Add(new SampleItem("Drag Drop ListBox",
                "Drag Drop ListBox",
                new DemoListBox()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
