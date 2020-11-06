using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SeriesBinding : UserControl
    {
        List<DataPoint> _function1Source;
        List<DataPoint> _function2Source;

        public SeriesBinding()
        {
            this.InitializeComponent();
            this.Loaded += SeriesBinding_Loaded;
            Tag = "This view shows how you can use the FlexChart to show data from multiple data sources, one per series.\r" +
                "The sample does the following:\r" + Environment.NewLine +
                "1.Set the chart's bindingX and binding properties to 'x' and 'y'.\r" +
                "2.Add a Series object to the chart's series array and set its DataSource property to an array of objects that have 'x' and 'y' properties.\r" +
                "3.Add a second Series object to the chart's series array and set its DataSource property to a different array of objects that have 'x' and 'y' properties.\r" + Environment.NewLine +
                "Alternatively, we could have set the bindingX and binding properties on the Series objects instead of setting then on the chart.In this case this was not necessary because the binding names are the same for all series.";
        }

        private void SeriesBinding_Loaded(object sender, RoutedEventArgs e)
        {
            SetupChart();
        }

        void SetupChart()
        {
            flexChart.BeginUpdate();
            this.Function1.ItemsSource = Function1Source;
            this.Function2.ItemsSource = Function2Source;
            flexChart.EndUpdate();
        }

        public List<DataPoint> Function1Source
        {
            get
            {
                if (_function1Source == null)
                {
                    _function1Source = DataCreator.Create(x => 2 * Math.Sin(0.16 * x), y => 2 * Math.Cos(0.12 * y), 160);
                }

                return _function1Source;
            }
        }

        public List<DataPoint> Function2Source
        {
            get
            {
                if (_function2Source == null)
                {
                    _function2Source = DataCreator.Create(x => Math.Sin(0.1 * x), y => Math.Cos(0.15 * y), 160);
                }

                return _function2Source;
            }
        }
    }
}
