using ColorPickerExplorer.Resources;
using System.Collections.ObjectModel;

namespace ColorPickerExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(AppResources.OverviewTitle, AppResources.OverviewDescription, new DemoColorPicker()),
                new SampleItem(AppResources.BrushesTitle, AppResources.BrushesTitle, new Brushes())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
