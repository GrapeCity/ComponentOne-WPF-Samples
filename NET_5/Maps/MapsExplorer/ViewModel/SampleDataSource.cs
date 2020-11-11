using System;
using System.Collections.ObjectModel;

namespace MapsExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Demo",
                "Demo",
                new Lazy<System.Windows.Controls.Control>(() => new DemoMaps())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
            //_allItems.Add(new SampleItem("Cities",
            //    "Cities",
            //    new Lazy<System.Windows.Controls.Control>(() => new Cities())));
            _allItems.Add(new SampleItem("Earthquakes",
                "Earthquakes",
                new Lazy<System.Windows.Controls.Control>(() => new EarthQuakes())));
            _allItems.Add(new SampleItem("Factories",
                "Factories",
                new Lazy<System.Windows.Controls.Control>(() => new Factories())));
            _allItems.Add(new SampleItem("GeoRSS",
                "GeoRSS",
                new Lazy<System.Windows.Controls.Control>(() => new Flicker())));
            _allItems.Add(new SampleItem("Marks",
                "Marks",
                new Lazy<System.Windows.Controls.Control>(() => new Marks())));
            _allItems.Add(new SampleItem("KML",
                "KML",
                new Lazy<System.Windows.Controls.Control>(() => new MapChart())));
            _allItems.Add(new SampleItem("ShapeFile",
                "ShapeFile",
                new Lazy<System.Windows.Controls.Control>(() => new MapShape())));
            _allItems.Add(new SampleItem("Grid",
                "Grid",
                new Lazy<System.Windows.Controls.Control>(() => new Grid())));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
