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
                AppResources.CarListDescription,
                new Lazy<System.Windows.Controls.Control>(() => new CarsListControl())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
            _allItems.Add(new SampleItem(
                AppResources.FilterSummaryTitle,
                AppResources.FilterSummaryDescription,
                new Lazy<System.Windows.Controls.Control>(() => new FilterSummarySample())));
            _allItems.Add(new SampleItem(
                AppResources.FilterEditorTitle,
                AppResources.FilterEditorDescription,
                new Lazy<System.Windows.Controls.Control>(() => new FilterEditorSample())));
            _allItems.Add(
                new SampleItem(
                    AppResources.CustomFilterTitle,
                    AppResources.CustomFilterDescription,
                    new Lazy<System.Windows.Controls.Control>(() => new CustomFiltersSample())
                )
            );
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
