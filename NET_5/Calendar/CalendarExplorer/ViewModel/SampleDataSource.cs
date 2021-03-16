using System.Collections.ObjectModel;
using CalendarExplorer.Resources;

namespace CalendarExplorer
{
    internal class SampleDataSource
    {
        internal SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(
                    AppResources.OverviewTitle,
                    AppResources.OverviewTitle,
                    AppResources.OverviewDescription,
                    new Overview()),

                new SampleItem(
                    AppResources.CustomUITitle,
                    AppResources.CustomUITitle,
                    AppResources.CustomUIDescription,
                    new CustomUI()),

                new SampleItem(
                    AppResources.AdvancedCustomUITitle,
                    AppResources.AdvancedCustomUITitle,
                    AppResources.AdvancedCustomUIDescription,
                    new AdvancedUI())
            };
        }

        public ObservableCollection<SampleItem> AllItems { get; }
    }
}