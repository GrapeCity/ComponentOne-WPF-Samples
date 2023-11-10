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
                new SampleItem(AppResources.ColorPickerTitle, AppResources.ColorPickerTitle, new DemoColorPicker()),
                new SampleItem(AppResources.BrushPickerTitle, AppResources.BrushPickerTitle, new Brushes())
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
