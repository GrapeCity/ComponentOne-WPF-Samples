using System;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using FlexChartExplorer.Resources;

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
            Tag = AppResources.SeriesBindingTag;
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
