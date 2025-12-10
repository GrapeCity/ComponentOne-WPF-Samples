using System;
using System.Collections.ObjectModel;

namespace MapsExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem(Properties.Resources.DemoTitle,
                Properties.Resources.DemoTitle,
                new Lazy<System.Windows.Controls.Control>(() => new DemoMaps())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
            
            _allItems.Add(new SampleItem(Properties.Resources.EarthquakesTitle,
                Properties.Resources.EarthquakesTitle,
                new Lazy<System.Windows.Controls.Control>(() => new EarthQuakes())));

            _allItems.Add(new SampleItem(Properties.Resources.FactoriesTitle,
                Properties.Resources.FactoriesTitle,
                new Lazy<System.Windows.Controls.Control>(() => new Factories())));

            _allItems.Add(new SampleItem(Properties.Resources.FlickerTitle,
                Properties.Resources.FlickerTitle,
                new Lazy<System.Windows.Controls.Control>(() => new Flicker())));

            _allItems.Add(new SampleItem(Properties.Resources.MarksTitle,
                Properties.Resources.MarksTitle,
                new Lazy<System.Windows.Controls.Control>(() => new Marks())));

            _allItems.Add(new SampleItem(Properties.Resources.MapChartTitle,
                Properties.Resources.MapChartTitle,
                new Lazy<System.Windows.Controls.Control>(() => new MapChart())));

            _allItems.Add(new SampleItem(Properties.Resources.MapShapeTitle,
                Properties.Resources.MapShapeTitle,
                new Lazy<System.Windows.Controls.Control>(() => new MapShape())));

            _allItems.Add(new SampleItem(Properties.Resources.GridTitle,
                Properties.Resources.GridTitle,
                new Lazy<System.Windows.Controls.Control>(() => new Grid())));

            _allItems.Add(new SampleItem(Properties.Resources.AirportsTitle,
                Properties.Resources.AirportsTitle,
                new Lazy<System.Windows.Controls.Control>(() => new Airports())));

        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
