using C1.C1Zip;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace FlexChart101
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Getting Started",
                "Getting Started",
                "Steps for getting started with FlexChart in WPF applications:\r" + Environment.NewLine+
                "1. Drop FlexChart from the Toolbox to a window and add needed series.\r"+
                "2. Bind the data to FlexChart.",
                new GettingStarted()));
            _allItems.Add(new SampleItem("Chart Types",
                "Chart Types",
                "FlexChart control has three properties that allow you to customize the chart type:\r" + Environment.NewLine +
                "1. chartType: Selects the default chart type to be used for all series objects.Individual series objects can override this.\r"+
                "2. stacking: Determines whether series objects are plotted independently, stacked, or stacked so their sum is 100%.\r"+
                "3. rotated: Flips the X and Y axes so X becomes vertical and Y horizontal.\r",
                new ChartTypes()));
            _allItems.Add(new SampleItem("Funnel Chart",
                "Funnel Chart",
                "The example below shows you how to create and customize Funnel Chart:",
                new FunnelChart()));
            _allItems.Add(new SampleItem("Mixed Chart Types",
                "Mixed Chart Types",
                "You can use different chart types for each chart series by setting the chartType property on the series itself. This overrides the chart's default chart type.\r"+
                "In the example below, the chart's chartType property is set to Column, but the Sales series overrides that to use the LineSymbols chart type.",
                new MixedChartTypes()));
            _allItems.Add(new SampleItem("Legend And Titles",
                "Legend And Titles",
                "Customize the location of the chart legend with the Legend properties. Add titles to your charts with the Header, Footer, and axis Title properties.",
                new LegendAndTitles()));
            _allItems.Add(new SampleItem("Tooltips",
                "Tooltips",
                "FlexChart has built-in support for tooltips. By default, the control displays tooltips when the user touches or hovers the mouse on a data point."+ Environment.NewLine +
                "The tooltip content is generated using a template that may contain the following parameters:\r" + Environment.NewLine +
                "1. seriesName: Name of the series that contains the chart element\r"+
                "2. pointIndex: Index of the chart element within the series\r"+
                "3. x: X - value of the chart element\r"+
                "4. y: Y - value of the chart element",
                new Tooltips()));
            _allItems.Add(new SampleItem("Styling Series",
                "Styling Series",
                "FlexChart automatically picks colors for each series based on a default palette, which you can override by setting the Palette property. But you can also override the default settings by setting the Style property of any series to an object that specifies styling attributes, including Fill, Stroke, StrokeThickness, and so on.",
                new StylingSeries()));
            _allItems.Add(new SampleItem("Customizing Axes",
                "Customizing Axes",
                "Use axis properties to customize the chart's axes, including ranges (minimum and maximum), label format, tickmark spacing, and gridlines.\r"+
                "The AxisX and AxisY class have boolean properties that allow you to turn features on or off(AxisLine, Labels, MajorTickMarks, and MajorGrid)",
                new CustomizingAxes()));
            _allItems.Add(new SampleItem("Selection Modes",
               "Selection Modes",
               "FlexChart allows you to select series or data points by clicking or touching them. Use the SelectionMode property to specify whether you want to allow selection by series, by data point, or no selection at all (selection is off by default).",
               new SelectionModes()));
            _allItems.Add(new SampleItem("Toggle",
                "Toggle",
                "This view shows how to create bubble charts using the FlexChart control.\r" + Environment.NewLine +
                "The Series class has a Visibility property that allows you to determine whether a series should be shown in the chart and in the legend, only in the legend, or completely hidden.\r"+ Environment.NewLine +
                "This sample shows how you can use the Visibility property to toggle the visibility of a series using two methods:\r"+
                "1. Clicking on legend entries: The chart directive sets the chart's LegendToggle property to true, which toggles the Visibility property of a series when its legend entry is clicked.\r"+
                "2. Using checkboxes: The form binds input controls directly to the Visibility property of each series.",
                new Toggle()));
            _allItems.Add(new SampleItem("Dynamic",
                "Dynamic",
                "You can use INotifyCollectionChanged data collections, so any changes you make to the data source are automatically reflected in the chart.\r"+
                "In this sample, we use a timer to add items to the data source, discarding old items to keep the total count at 20.The result is a dynamic chart that scrolls as new data arrives.",
                new Dynamic()));
            LoadSourceCodes();
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }

        void LoadSourceCodes()
        {
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/SamplesCodes.zip", UriKind.Relative));
            if (resource != null)
            {
                using (C1ZipFile zip = new C1ZipFile(resource.Stream))
                {
                    AllItems.ToList<SampleItem>().ForEach(sampleItem =>
                    {
                        var ets = zip.Entries.Where(entry => entry.FileName.Equals(sampleItem.SourceCodes.XamlFileName)
                        || entry.FileName.Equals(sampleItem.SourceCodes.CodeFileName));
                        ets.ToList<C1ZipEntry>().ForEach(entry =>
                        {
                            if (entry.FileName.Contains("xaml.cs"))
                                sampleItem.SourceCodes.Code = GetContent(entry);
                            else
                                sampleItem.SourceCodes.Xaml = GetContent(entry);
                        });
                    });
                }
            }
        }

        string GetContent(C1ZipEntry entry)
        {
            using (Stream entryStream = entry.OpenReader())
            {
                using (StreamReader streamReader = new StreamReader(entryStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
