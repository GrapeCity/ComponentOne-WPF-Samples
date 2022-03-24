using System.Collections.ObjectModel;
using SchedulerExplorer.Resources;

namespace SchedulerExplorer
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
                    AppResources.GroupingTitle,
                    AppResources.GroupingTitle,
                    AppResources.GroupingDescription,
                    new Grouping()),

                new SampleItem(
                    AppResources.CustomDialogsTitle,
                    AppResources.CustomDialogsTitle,
                    AppResources.CustomDialogsDescription,
                    new CustomDialogs()),
                new SampleItem(
                    AppResources.DataBindingTitle,
                    AppResources.DataBindingTitle,
                    AppResources.DataBindingDescription,
                    new DataBinding()),
                new SampleItem(
                    AppResources.DatabaseBindingTitle,
                    AppResources.DatabaseBindingTitle,
                    AppResources.DatabaseBindingDescription,
                    new DatabaseBinding())
            };
        }

        public ObservableCollection<SampleItem> AllItems { get; }
    }
}