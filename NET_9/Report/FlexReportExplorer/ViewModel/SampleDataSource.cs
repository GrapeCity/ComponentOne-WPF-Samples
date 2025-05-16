using FlexReportExplorer.Resources;
using System.Collections.ObjectModel;

namespace FlexReportExplorer
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
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
