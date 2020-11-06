using System;
using System.Collections.ObjectModel;

namespace InputExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem("Overview",
                                "Overview",
                                new InputView()),
                new SampleItem("Range Slider",
                                "Range Slider",
                                new RangeSlider()),
                new SampleItem("Expander",
                                "Expander",
                                new Expander())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
