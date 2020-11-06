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
    [System.ComponentModel.Description("This sample shows how you can group on some property from your data object such as a name or category.")]
    public partial class AggregateCategoryAxis : UserControl
    {
        List<string> keys = new List<string> { "oranges", "grapes", "apples", "lemons" };

        public AggregateCategoryAxis()
        {
            InitializeComponent();

            CreateNewDataSeries();
            AggregateByCategory(true);

            // style chart
            chart.Palette = ColorGeneration.Oriel;
            chart.View.AxisY.MajorTickThickness = 0;
            chart.View.AxisY.MinorTickThickness = 0;
            chart.View.AxisY.AxisLine = new Line() { StrokeThickness = 0 };
            chart.View.AxisX.MajorGridStrokeThickness = 0;
            chart.View.AxisX.MinorGridStrokeThickness = 0;
            chart.View.AxisX.MajorTickThickness = 0;
            chart.View.AxisX.MinorTickThickness = 0;
        }

        void CreateNewDataSeries()
        {
            for (int i = 0; i < 2; i++)
            {
                var ds = new DataSeries()
                {
                    ItemsSource = SampleItem.CreateSampleData(40),
                    ValueBinding = new Binding("Value"),
                    PointTooltipTemplate = this.Resources["lbl"] as DataTemplate,
                    Label = "s" + i
                };
                chart.Data.Children.Add(ds);
            }
        }

        void AggregateByCategory(bool isGrouped)
        {
            foreach (DataSeries ds in chart.Data.Children)
            {
                if (isGrouped)
                {
                    ds.Aggregate = Aggregate.Sum;
                    ds.AggregateGroupSelector = (x, y, o) =>
                    {
                        // index from categories list 
                        return keys.IndexOf(((SampleItem)o).Name);
                    };
                    chart.Data.ItemNames = keys;
                }
                else
                {
                    ds.Aggregate = Aggregate.None;
                    ds.AggregateGroupSelector = null;
                    chart.Data.ItemNames = null;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            chart.Data.Children.Clear();
            CreateNewDataSeries();
            AggregateByCategory((bool)chkGrouping.IsChecked);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (chart != null)
            {
                AggregateByCategory((bool)chkGrouping.IsChecked);
            }
        }
    }

    public class SampleItem
    {
        public string Name { get; set; }
        public double Value { get; set; }

        static Random rnd = new Random();

        public static SampleItem[] CreateSampleData(int cnt)
        {
            var names = new string[] { "oranges", "apples", "lemons", "grapes" };
            var array = new SampleItem[cnt];

            for (int i = 0; i < cnt; i++)
            {
                array[i] = new SampleItem() { Value = rnd.Next(1, 10), Name = names[rnd.Next(names.Length)] };
            }

            return array;
        }

    }
}
