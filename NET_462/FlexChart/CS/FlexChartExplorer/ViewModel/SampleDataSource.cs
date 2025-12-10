using System;
using System.Collections.ObjectModel;

namespace FlexChartExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem("Introduction",
                "Introduction",
                "This view shows the FlexChart's basic features. It binds the chart to a data source and allows you to select the chart type, series stacking, and rotation.\r"+
                "If you move the mouse over a chart element, a tooltip will appear showing details about the data point.\r" + Environment.NewLine +
                "The simplest way to use the FlexChart is to:\r" + Environment.NewLine+
                "1.Set the chart's DataSource property to an array of data objects.\r" +
                "2.Set the chart's bindingX property to the name of the property that contains the X values (in this example fruit names).\r" +
                "3.Add one or more Series objects to the chart's series array and set their binding property to the name of the property that contains the Y values (in this example the months of March, April, and May).",
                new Introduction()));
            _allItems.Add(new SampleItem("Binding",
                "Binding",
                "This view shows how you can use the FlexChart to show two sets of values from a single array of data. This is the most common usage scenario for the FlexChart.\r" +
                "The sample does the following:\r" + Environment.NewLine +
                "1.Set the chart's DataSource property to an array of data objects. Each data object has values for 'date', 'sales', and 'downloads'.\r" +
                "2.Set the chart's bindingX property to 'date'.\r" +
                "3.Add a Series object to the chart's series array and set its binding property to 'sales'.\r" +
                "4.Add a second Series object to the chart's series array and set its binding property to 'downloads'.\r" + Environment.NewLine+
                "In addition to binding, this sample shows the effect of the interpolateNulls and legendToggle properties.When you set interpolateNulls to true, the chart fills in gaps created by null values in the data.When you set legendToggle to true, the chart toggles the visibility of the series when you click its name in the legend.",
                new Binding()));
            _allItems.Add(new SampleItem("Series Binding",
                "Series Binding",
                "This view shows how you can use the FlexChart to show data from multiple data sources, one per series.\r" +
                "The sample does the following:\r" + Environment.NewLine +
                "1.Set the chart's bindingX and binding properties to 'x' and 'y'.\r" +
                "2.Add a Series object to the chart's series array and set its DataSource property to an array of objects that have 'x' and 'y' properties.\r" +
                "3.Add a second Series object to the chart's series array and set its DataSource property to a different array of objects that have 'x' and 'y' properties.\r" + Environment.NewLine +
                "Alternatively, we could have set the bindingX and binding properties on the Series objects instead of setting then on the chart.In this case this was not necessary because the binding names are the same for all series.",
                new SeriesBinding()));
            _allItems.Add(new SampleItem("Header And Footer",
                "Chart header and footer",
                "This view shows how you can add a header and a footer to the chart.\r" +
                "The header and footer properties determine the content, and the headerStyle and footerStyle properties determine the appearance of the header and footer.",
                new HeaderAndFooter()));
            _allItems.Add(new SampleItem("Selection",
                "Selection",
                "This view demonstrates the FlexChart's selection feature.\r" +
                "The selectionMode property determines whether the chart should select series or points when the user clicks on the chart.",
                new Selection()));
            _allItems.Add(new SampleItem("Labels",
                "Labels",
                "This view demonstrates how you can use the FlexChart's dataLabel property to add labels to each data point.",
                new Labels()));
            _allItems.Add(new SampleItem("AutoLabels",
                "AutoLabels",
                "The demo shows automatic label positioning. When DataLabel.Position is Auto the chart performs automatic label layout trying to avoid their overlapping.",
                new AutoLabels()));
            _allItems.Add(new SampleItem("Hit Test",
                "Hit Test",
                "This view demonstrates the use of the FlexChart's hitTest method.\r" +
                "The hitTest method takes a point and returns the nearest chart element.It can be used to provide interactive features such as clickable regions, drill - downs, etc.\r" +
                "Move mouse over chart to see information about the chart element that is closest to the mouse.",
                new HitTest()));
            _allItems.Add(new SampleItem("Zoom",
               "Zoom",
               "This view shows how to implement a custom zoom for the FlexChart control.\r" +
                "Use the mouse or touch to select a rectangular area on the plot area.The chart will zoom in on the selected area.When you are done, click the 'Reset Zoom' button below the chart to return to the original view.",
               new Zoom()));
            _allItems.Add(new SampleItem("Bubble",
                "Bubble",
                "This view shows how to create bubble charts using the FlexChart control.\r" +
                "Bubble charts are similar to other chart types, except in addition to X and Y you must specify a binding for the bubble size.This is done by setting the binding property to a comma - delimited string that specifies the name of the properties to be used for the Y and size values for each bubble.\r" +
                "In this example, the chart is bound to a list containing objects with 'x', 'y', and 'size' properties.The chart contains a single series and its binding property is set to the string 'y,size'.",
                new Bubble()));
            _allItems.Add(new SampleItem("Financial Chart",
                "Financial Chart",
                "This view shows how to create financial charts with the FlexChart control.\r" +
                "The FlexChart supports two types of financial chart: Candlestick and HighLowOpenClose.To use them, set the chartType property to the type you want, and set the series binding property to a string that specifies the fields that contain the high, low, open, and close values in the data source.",
                new Financial()));
            _allItems.Add(new SampleItem("Axes",
                "Multiple axes",
                "This view shows the FlexChart with two y-axes.",
                new Axes()));
            _allItems.Add(new SampleItem("Axis Labels",
                "Axis Labels",
                "This view shows various Axis options to avoid label overlapping.",
                new AxisLabels()));
            _allItems.Add(new SampleItem("Axis Grouping",
    "Axis Grouping",
    "This view shows grouping of axis labels.",
    new AxisGrouping()));
            _allItems.Add(new SampleItem("Numeric Axis Grouping",
    "Numeric Axis Grouping",
    "This view shows grouping of numeric axis labels.",
    new NumericAxisGrouping()));
            _allItems.Add(new SampleItem("DateTime Axis Grouping",
    "DateTime Axis Grouping",
    "This view shows grouping of date-time axis labels.",
    new DateTimeAxisGrouping()));
            _allItems.Add(new SampleItem("Plot Areas",
                "Multiple plot areas",
                "This view shows the FlexChart with several plot areas.",
                new PlotAreas()));
            _allItems.Add(new SampleItem("Axis Binding",
                "Axis binding",
                "This view shows the FlexChart's axis binding feature.",
                new AxisBinding()));
            _allItems.Add(new SampleItem("Image Export",
                "Image Export",
                "Save FlexChart as image.",
                new ImageExport()));
            _allItems.Add(new SampleItem("Zones",
                "Zones",
                "The view creates a scatter chart of student grades, highlighting each grade range using zones.\r" +
                "The zones are drawn in rendering event of FlexChart. Statistic values are calculated and plotted.",
                new Zones()));
            _allItems.Add(new SampleItem("Trend Line",
                "Trend line",
                "This view shows TrendLine in FlexChart.\r" +
                "You can use different trendline types by setting the FitType property of TrendLine. The points on the plot can be dragged by mouse which updates the trend line and its equation automatically.",
                new TrendLine()));
            _allItems.Add(new SampleItem("Waterfall",
                "FlexChart Waterfall",
                "This view shows Waterfall Series in FlexChart.\r" +
                "The Waterfall series is normally used to demonstrate how the starting position either increases or decreases through a series of changes.",
                new Waterfall()));
            _allItems.Add(new SampleItem("PieChart",
                "Introduction",
                "This view shows the FlexPie's basic features. It binds the chart to a data source.\r" +
                "If you move the mouse over a chart element, a tooltip will appear showing details about the data point.",
                new PieIntroduction()));
            _allItems.Add(new SampleItem("PieChart Selection",
                "Selection",
                "This view shows the FlexPie's selection feature.",
                new PieSelection()));
            _allItems.Add(new SampleItem("PieChart Slice Color",
                "Slice Color",
                "This view shows how to customize FlexPie silce colors. The color intensity depends on the corresponding data value. It also demonstrates animation when loading new or updating existing data.",
                new PieSliceColor()));
            _allItems.Add(new SampleItem("Box & Whisker",
                "Box & Whisker",
                "The Box & Whisker series is normally used to compare distributions between different sets of numerical data",
                new BoxWhisker()));
            _allItems.Add(new SampleItem("Error Bar",
               "Error Bar",
               "The Error bar helps you see margins of error and standard deviations at a glance.",
               new ErrorBar()));
            _allItems.Add(new SampleItem("Legend",
                "Legend",
                "This view shows how you can use the FlexChart legend properties." + Environment.NewLine + Environment.NewLine +
                "The sample does the following: " + Environment.NewLine + Environment.NewLine +
                "1.Set legend position property to decide how you locate the legend.such as \"Auto\", \"Left\", \"Top\", \"Right\" and \"Bottom\"." + Environment.NewLine +
                "2.Set legend orientations." + Environment.NewLine +
                "3.Set the mode of legend text wrapping.such as \"None\", \"Wrap\" and \"Truncate\"." + Environment.NewLine +
                "4.Set a value more than 0 to legend item maxWidth property, set 0 to disable legend textWrapping." + Environment.NewLine +
                "5.Set the LegendGroup property of each series to group items within the legend.  Legend items within each group are arranged by the legend orientation property while the legend groups are arranged based on the legend position.",
                new Legend()));
            _allItems.Add(new SampleItem("TreeMap",
               "TreeMap",
               "This view shows main functionality of TreeMap control." + Environment.NewLine + Environment.NewLine +
               "TreeMap charts are compact way of visualizing hierarchical data in form of nested rectangles with area of each rectangle depicting the quantity of each category.",
               new TreeMap()));
            _allItems.Add(new SampleItem("TreeMap Node Color",
               "Node Color",
               "TreeMaps can be used to display two quantitive variables simultaneously, one represented by each rectangle's size and the other represented by its colour." + Environment.NewLine + Environment.NewLine +
               "This view demonstrates how you can use TreeMap.NodeRendering event to specify color of each node.",
               new TreeMapNodeColor()));
            _allItems.Add(new SampleItem("Histogram",
                "Histogram",
                "A Histogram chart is used to study the distribution of non-categorical data by dividing it into bins of specified width. It plots the frequency of data items that fall in each of these bins. This sample shows:" + Environment.NewLine + Environment.NewLine +
                "1. Histogram chart with columns" + Environment.NewLine +
                "2. Frequency Polygon: A variation of Histogram charts where the columns are replaced by a curve.These are helpful for comparing multiple sets of data." + Environment.NewLine +
                "3. Normal Curve: A bell - shaped curve, also known as Gaussian curve, which shows the probability distribution of a continuous random variable." + Environment.NewLine +
                "4. Cumulative Frequency Polygons: Popularly known as ogives, shows the running total of frequencies on the chart.",
               new Histogram()));
            _allItems.Add(new SampleItem("Ranged Histogram",
               "Ranged Histogram",
               "This view shows main functionality of Ranged Histogram." + Environment.NewLine + Environment.NewLine +
               "An unique Histogram chart that can be used to study categorical as well as non-categorical data. Provides multiple binning options as well as support for Overflow and Underflow bins. ",
               new RangedHistogram()));            
            _allItems.Add(new SampleItem("Pareto",
               "Pareto",
               "A Pareto is a special histogram chart with columns sorted in descending order and a rising line that represents the cumulative total percentage. Pareto charts makes it easier to analyze most significant issues and prioritise corrective actions.",
               new Pareto()));
            _allItems.Add(new SampleItem("BreakEven",
               "Break-Even",
                "Break-Even chart is a chart that shows the sales volume level at which total costs equal sales.",
                new BreakEven()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
