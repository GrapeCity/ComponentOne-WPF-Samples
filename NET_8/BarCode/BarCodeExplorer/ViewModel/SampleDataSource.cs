using System;
using System.Collections.ObjectModel;

namespace BarCodeExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(Properties.Resources.DemoTitle,
                                Properties.Resources.DemoTitle,
                                new Demo()),
                new SampleItem(Properties.Resources.NewBarcodesTitle,
                                Properties.Resources.NewBarcodesTitle,
                                new NewBarCodes()),
                           };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
