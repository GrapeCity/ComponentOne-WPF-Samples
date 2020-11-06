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
    [System.ComponentModel.Description("This sample shows how to group items by dates to show summarized plots for years, months, weeks and so on.")]
    public partial class AggregateDateAxis : UserControl
    {
        public AggregateDateAxis()
        {
            InitializeComponent();
            CreateSampleChart();

            // initialize aggregation combobox
            spAggregation.Visibility = System.Windows.Visibility.Hidden;
            cbAggregation.Items.Add(Aggregate.Sum);
            cbAggregation.Items.Add(Aggregate.Average);
            cbAggregation.Items.Add(Aggregate.Minimum);
            cbAggregation.Items.Add(Aggregate.Maximum);
            cbAggregation.SelectedIndex = 0;
            cbAggregation.SelectionChanged += (s, e) =>
            {
                if (ds.AggregateGroupSelector != null)
                    ds.Aggregate = (Aggregate)cbAggregation.SelectedItem;
            };
        }

        XYDataSeries ds;

        void CreateSampleChart()
        {
            chart.BeginUpdate();

            // random time data
            var npts = 5000;
            var xs = new DateTime[npts];
            var ys = new double[npts];

            var x0 = DateTime.Now.AddDays(-npts);
            var rnd = new Random();
            for (int i = 0; i < npts; i++)
            {
                xs[i] = x0.AddDays(i);
                ys[i] = rnd.NextDouble() * 100;
            }

            ds = new XYDataSeries() { XValuesSource = xs, ValuesSource = ys, RenderMode = RenderMode.Fast };

            chart.Data.Children.Add(ds);
            chart.ChartType = ChartType.Line;
            chart.View.AxisX.IsTime = true;
            chart.Palette = ColorGeneration.Urban;
            chart.EndUpdate();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (ds == null)
                return;

            var btn = (RadioButton)sender;
            var cont = (string)btn.Content;

            // setup aggregation parameters 
            if (cont == "None")
            {
                ds.AggregateGroupSelector = null;
                ds.Aggregate = Aggregate.None;
                spAggregation.Visibility = System.Windows.Visibility.Hidden;
            }
            else
            {
                spAggregation.Visibility = System.Windows.Visibility.Visible;
                ds.Aggregate = (Aggregate)cbAggregation.SelectedItem;

                if (cont == "Year")
                {
                    ds.AggregateGroupSelector = (x, y, o) =>
                    {
                        var dt = DateTime.FromOADate(x);
                        var dt1 = new DateTime(dt.Year, 1, 1);
                        var dt2 = dt1.AddYears(1);
                        return GetMiddleDate(dt1, dt2).ToOADate();
                    };
                }
                else if (cont == "Month")
                {
                    ds.AggregateGroupSelector = (x, y, o) =>
                    {
                        var dt = DateTime.FromOADate(x);
                        var dt1 = new DateTime(dt.Year, dt.Month, 1);
                        var dt2 = dt1.AddMonths(1);
                        return GetMiddleDate(dt1, dt2).ToOADate();
                    };
                }
                else if (cont == "Quarter")
                {
                    ds.AggregateGroupSelector = (x, y, o) =>
                    {
                        var dt = DateTime.FromOADate(x);
                        var dt1 = new DateTime(dt.Year, 1 + ((dt.Month - 1) / 3) * 3, 1);
                        var dt2 = dt1.AddMonths(3);
                        return GetMiddleDate(dt1, dt2).ToOADate();
                    };
                }
                else if (cont == "Week")
                {
                    ds.AggregateGroupSelector = (x, y, o) =>
                    {
                        var dt = DateTime.FromOADate(x);
                        var dt1 = dt.AddDays(-(int)dt.DayOfWeek);
                        var dt2 = dt1.AddDays(7);
                        return GetMiddleDate(dt1, dt2).ToOADate();
                    };
                }
            }
        }

        // center of time range
        DateTime GetMiddleDate(DateTime dt1, DateTime dt2)
        {
            return dt1 + TimeSpan.FromTicks((dt2 - dt1).Ticks / 2);
        }
    }
}
