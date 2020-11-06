using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DataManipulation
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Top-N",
                "Top-N",
                "The sample below shows how to exclude the points with small values from the chart except the Top-N points. You can also aggregate them into a single cumulative point by selecting the Show Others checkbox.",
                new TopNView()));
            _allItems.Add(new SampleItem("Sorting",
               "Sorting",
               "The sample below shows how to sort series data in an ascending or descending order. You can also sort the data according to your customizable order, such as the match of two series in this example.",
               new SortingView()));
            _allItems.Add(new SampleItem("Aggregate",
                "Aggregate",
                "The sample below shows how to show an aggregated view of a large amount of data. You can also specify on what function you would like to use to aggregate the data.",
                new AggregateView()));
            _allItems.Add(new SampleItem("Y-Function Series",
                "Y-Function Series",
                "This view shows how to implement a Y-Function Series in FlexChart. The Y - Function Series allows to plot a function that is defined with formula y = y(x).Use the 'Function' property to specify the function. ",
                new YFunctionSeriesView()));
            _allItems.Add(new SampleItem("Parametric Function Series",
                "Parametric Function Series",
                "This view shows how to implement a Parametric Function Series in FlexChart. The Parametric Function Series allows to plot a function that is defined parametrically with formulas x = x(t) and y = y(t).Use the 'XFunction' and 'YFunction' properties to specify the functions for x and y coordinates. ",
                new ParametricFunctionSeriesView()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
