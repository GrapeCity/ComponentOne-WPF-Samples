using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace FlexRadarIntro
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Radar Chart",
                "Radar Chart",
		"The demo shows how to create radar chart with FlexRadar control. Using FlexRadar is very similar to FlexChart:\n" +
		"1. Set the chart's ItemsSource property to a collection of data objects.\n" +
		"2. Set the chart's BindingX property to the name of the property that contains the values on circular axis.\n" +
		"3. Add one or more Series to the chart's Series collection and set their Binding property to the name of the property that contains the values on radial axis.\n",
                new RadarChart()));
            _allItems.Add(new SampleItem("Polar Chart",
                "Polar Chart",
                "The FlexRadar control represents a polar chart when x-values are numbers that specifies angular values in degrees.\n" +
                "The demo shows plot of function defined in polar coordinates.",
                new PolarChart()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
