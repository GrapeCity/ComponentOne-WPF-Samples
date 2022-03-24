using DataFilterExplorer.Resources;
using System;
using System.Collections.ObjectModel;

namespace DataFilterExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem(
                AppResources.CarListTitle,
                AppResources.CarListTitle,
                new Lazy<System.Windows.Controls.Control>(() => new CarsListControl())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
            _allItems.Add(new SampleItem(
                AppResources.FilterSummaryTitle,
                AppResources.FilterSummaryTitle,
                new Lazy<System.Windows.Controls.Control>(() => new FilterSummarySample())));
            _allItems.Add(new SampleItem(
                AppResources.ConditionalFiltersTitle,
                AppResources.ConditionalFiltersTitle,
                new Lazy<System.Windows.Controls.Control>(() => new ConditionalFiltersSample())));
            _allItems.Add(new SampleItem(
                AppResources.FilterEditorTitle,
                AppResources.FilterEditorTitle,
                new Lazy<System.Windows.Controls.Control>(() => new FilterEditorSample())));
            _allItems.Add(new SampleItem(
                AppResources.CustomFilterTitle,
                AppResources.CustomFilterTitle,
                new Lazy<System.Windows.Controls.Control>(() => new CustomFiltersSample())));
            _allItems.Add(new SampleItem(
                AppResources.VirtualSourceTitle,
                AppResources.VirtualSourceTitle,
                new Lazy<System.Windows.Controls.Control>(() => new VirtualSource())));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
