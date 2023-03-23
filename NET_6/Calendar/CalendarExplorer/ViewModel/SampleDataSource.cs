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
                    AppResources.MultiMonthsTitle,
                    AppResources.MultiMonthsTitle,
                    AppResources.MultiMonthsDes,
                    new MultiMonths()),

                new SampleItem(
                    AppResources.CustomUITitle,
                    AppResources.CustomUITitle,
                    AppResources.CustomUIDescription,
                    new CustomUI()),

                new SampleItem(
                    AppResources.AdvancedCustomUITitle,
                    AppResources.AdvancedCustomUITitle,
                    AppResources.AdvancedCustomUIDescription,
                    new AdvancedUI()),

                new SampleItem(
                    AppResources.WeekdaysTitle,
                    AppResources.WeekdaysTitle,
                    AppResources.WeekdaysDescription,
                    new Weekdays()),

                new SampleItem(
                    AppResources.WeekSelectionTitle,
                    AppResources.WeekSelectionTitle,
                    AppResources.WeekSelectionDescription,
                    new WeekSelection()),

                new SampleItem(
                    AppResources.CustomSlotsTitle,
                    AppResources.CustomSlotsTitle,
                    AppResources.CustomSlotsDescription,
                    new CustomSlots()),
            };
        }

        public ObservableCollection<SampleItem> AllItems { get; }
    }
}