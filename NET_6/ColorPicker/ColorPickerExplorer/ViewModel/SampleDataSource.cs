using ColorPickerExplorer.Resources;
using System.Collections.ObjectModel;

namespace ColorPickerExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem(
                AppResources.OverviewTitle,
                AppResources.OverviewDescription,
                new DemoColorPicker()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
