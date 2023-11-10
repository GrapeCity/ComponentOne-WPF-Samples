using DocumentExplorer.Resources;
using System.Collections.ObjectModel;

namespace DocumentExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem(
                AppResources.ExportTitle,
                AppResources.ExportTitle,
                new Export()));
            AllItems.Add(new SampleItem(
                AppResources.PrintTitle,
                AppResources.PrintTitle,
                new Print()));
            AllItems.Add(new SampleItem(
                AppResources.SsrsTitle,
                AppResources.SsrsTitle,
                new Ssrs()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
