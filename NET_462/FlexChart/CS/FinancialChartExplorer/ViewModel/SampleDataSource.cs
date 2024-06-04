using System.Collections.ObjectModel;

namespace FinancialChartExplorer
{
    public class SampleDataSource
    {
        ObservableCollection<SampleGroup> _groups = new ObservableCollection<SampleGroup>();

       public SampleDataSource()
        {
            var chartTypeGroup = new SampleGroup("Chart Types");
            chartTypeGroup.Samples.Add(new Sample("Heikin-Ashi",
                "Heikin-Ashi charts are a variation of Japanese candlestick charts that were designed to remove noise from candlesticks and behave much like a moving average. These charts can be used to identify trends, potential reversal points, and other technical analysis patterns.",
                new HeikinAshi()){ IsSelected = true });
            chartTypeGroup.Samples.Add(new Sample("Line Break",
                "A Line Break or Three Line Break chart uses vertical boxes or lines to illustrate the price changes of an asset or market. Movements are depicted with box colors and styles; movements that continue the trend of the previous box are colored similarly while movements that trend oppositely are indicated with a different color and/or style. The opposite trend is only drawn if its value exceeds the extreme value of the previous n number of boxes or lines, which is determined by the newLineBreaks option.",
                new LineBreak()));
            chartTypeGroup.Samples.Add(new Sample("Renko",
                "The Renko chart uses bricks of uniform size to chart the price movement. When a price moves to a greater or lesser value than the preset boxSize option required to draw a new brick, a new brick is drawn in the succeeding column. The change in box color and direction signifies a trend reversal.",
                new Renko()));
            chartTypeGroup.Samples.Add(new Sample("Kagi",
                "A Kagi chart displays supply and demand trends using a sequence of linked vertical lines. The thickness and direction of the lines vary depending on the price movement. If closing prices go in the direction of the previous Kagi line, then that Kagi line is extended. However, if the closing price reverses by the preset reversal amount, a new Kagi line is charted in the next column in the opposite direction. Thin lines indicate that the price breaks the previous low (supply) while thick lines indicate that the price breaks the previous high (demand).",
                new Kagi()));
            chartTypeGroup.Samples.Add(new Sample("ColumnVolume",
                "ColumnVolume charts are similar to Column charts, except that they accept a second value, volume, which dictates the width of each bar.",
                new ColumnVolume()));
            chartTypeGroup.Samples.Add(new Sample("EquiVolume",
                "EquiVolume charts are similar to Candlestick charts, but they only show the high and low values. In addition, the width of each bar is determined by a fifth value, volume.",
                new EquiVolume()));
            chartTypeGroup.Samples.Add(new Sample("CandleVolume",
                "CandleVolume charts are identical to standard Candlestick charts, except that the width of each bar is determined by a fifth value, volume.",
                new CandleVolume()));
            chartTypeGroup.Samples.Add(new Sample("Arms CandleVolume",
                "Created by Richard Arms, Arms CandleVolume charts are a combination of EquiVolume and CandleVolume charts.",
                new ArmsCandleVolume()));
            chartTypeGroup.Samples.Add(new Sample("Point&Figure",
                "Point and Figure chart consists of columns of X's and O's that represent filtered price movements. X-Columns represent rising prices and O-Columns represent falling prices.",
                new PointAndFigure()));
            Groups.Add(chartTypeGroup);

            var interactionGroup = new SampleGroup("Interaction");
            interactionGroup.Samples.Add(new Sample("Markers",
                "Markers display a text area on the FinancialChart that displays the data values based on the mouse cursor's position on the chart. Markers also support optional vertical and horizontal lines to enable a cross-hair effect.",
                new Markers()));
            interactionGroup.Samples.Add(new Sample("Range Selector",
                "The RangeSelector allows end users to adjust the FinancialChart's visible range of data at runtime. The example below demonstrates the typical use case for the RangeSelector and how to apply a custom style.",
                new RangeSelector()));
            Groups.Add(interactionGroup);

            var analyticsGroup = new SampleGroup("Analytics");
            analyticsGroup.Samples.Add(new Sample("Trend Lines",
                "Trend lines are used to visualize trends in data and to help analyze the problems of prediction.",
                new TrendLines()
                ));
            analyticsGroup.Samples.Add(new Sample("Moving Average",
               "Moving average trend lines are used to analyze data by creating a series of averages of the original data set.",
               new MovingAverages()
               ));
            analyticsGroup.Samples.Add(new Sample("Overlays",
               "Overlays, like technical indicators, are a set of derived data that is calculated by applying one or more formulas to the original set of data. Overlays are generally used to forecast an asset's market direction and generally plotted with the original data set since the the Y-axis scales are the same.",
               new Overlays()
               ));
            analyticsGroup.Samples.Add(new Sample("Indicators",
               "A technical indicator is a set of derived data that is calculated by applying one or more formulas to the original set of data. Technical indicators are generally used to forecast the asset's market direction and generally plotted separately from the original data since the Y-axis scales differ.",
               new Indicators()
               ));
            analyticsGroup.Samples.Add(new Sample("Event Annotations",
             "Event annotations are used to mark important events that can be attached to a specific data point on the FinancialChart. Hovering over the event annotation will reveal the full details of the event.",
             new EventAnnotations()
             ));
            analyticsGroup.Samples.Add(new Sample("Fibonacci Tool",
              "Fibonacci tool is used for trend analysis in financial charts. With the help of range selector, you can choose data range for calculation.",
              new FibonacciTool()
              ));
            Groups.Add(analyticsGroup);
        }

        public ObservableCollection<SampleGroup> Groups
        {
            get
            {
                return _groups;
            }
        }
    }
}
