using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using C1.WPF.C1Chart;


namespace ChartSamples
{
    [System.ComponentModel.Description("This sample shows how you can count items by value range. This chart shows the number of items that meet the criteria of each value range.")]
    public partial class AggregateRange : UserControl
    {
        public AggregateRange()
        {
            InitializeComponent();
            AggregateByRange();
        }

        void AggregateByRange()
        {
            var keys = new string[] { "<4", "4-8", ">8" };

            for (int i = 0; i < 3; i++)
            {
                var ds = new DataSeries()
                {
                    ItemsSource = SampleItem.CreateSampleData(100),
                    ValueBinding = new Binding("Value"),
                    Aggregate = Aggregate.Count,
                    Label = "Sample Group " + (i + 1).ToString(),
                    AggregateGroupSelector = RangeSelector
                };
                chart.Data.Children.Add(ds);
            }

            chart.Data.ItemNames = keys;

            // style chart
            chart.View.AxisY.MajorTickThickness = 0;
            chart.View.AxisY.MinorTickThickness = 0;
            chart.View.AxisY.AxisLine = new Line() { StrokeThickness = 0 };
            chart.View.AxisX.MajorGridStrokeThickness = 0;
            chart.View.AxisX.MinorGridStrokeThickness = 0;
            chart.View.AxisX.MajorTickThickness = 0;
            chart.View.AxisX.MinorTickThickness = 0;
        }

        double RangeSelector(double x, double y, object o)
        {
            var sample = (SampleItem)o;
            if (sample.Value < 4)
                return 0;
            else if (sample.Value <= 8)
                return 1;
            else
                return 2;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chart.Data.Children.Clear();
            AggregateByRange();
        }
    }
}
