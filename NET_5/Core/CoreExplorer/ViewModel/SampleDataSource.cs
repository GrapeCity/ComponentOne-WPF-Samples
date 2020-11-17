using CoreExplorer.Resources;
using System;
using System.Collections.ObjectModel;

namespace CoreExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem(
                AppResources.InputHandlingTitle,
                AppResources.InputHandlingDescription,
                new InputHandling()));

            AllItems.Add(new SampleItem(
                AppResources.DragDropManagerTitle,
                AppResources.DragDropManagerDescription,
                new DemoDragDropManager()));

            AllItems.Add(new SampleItem(
                AppResources.DragDropListBoxTitle,
                AppResources.DragDropListBoxDescription,
                new DemoListBox()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
