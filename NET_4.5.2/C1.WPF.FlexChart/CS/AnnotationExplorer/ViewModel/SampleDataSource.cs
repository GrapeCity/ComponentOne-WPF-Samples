using System;
using System.Collections.ObjectModel;

namespace AnnotationExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Basic",
                "Basic",
                "The sample below shows basic usage of AnnotationLayer and various built-in types of Annotations, including: Circle, Ellipse, Image, Line, Polygon, Rectangle, Square and Text.\n" +
                "You can use attachment property to set different attach types of annotation.",
                new Basic()));
            _allItems.Add(new SampleItem("Advanced",
                "Advanced",
                "The sample below shows how to display annotations easily on a FlexChart to show comments and useful information about data points on the plot area itself.",
                new Advanced()));
            _allItems.Add(new SampleItem("Callouts",
                "Callouts",
                "The sample below shows how to use Polygon type annotation to display different shapes of callouts.",
                new Callouts()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
