using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;

namespace ChartSamples
{
    [System.ComponentModel.Description("This demo shows multiple plot areas stacked vertically.")]
    public partial class PlotAreasVertical : UserControl
    {
        public PlotAreasVertical()
        {
            InitializeComponent();

            NewData();
        }

        void NewData()
        {
            chart.BeginUpdate();

            chart.View.AxisX.ScrollBar = new AxisScrollBar();
            chart.View.AxisX.Scale = 0.125;

            chart.View.AxisX.Title = "Time";

            chart.View.AxisY.PlotAreaIndex = 1;
            chart.View.AxisY.Title = "Price";

            chart.View.PlotAreas.Add(new PlotArea()
            {
                Row = 1,
                Stroke = new SolidColorBrush(Colors.DarkGray),
                StrokeThickness = 0.5,
                Background = new SolidColorBrush(Colors.Blue) { Opacity = 0.1 }
            });
            chart.View.PlotAreas.Add(new PlotArea()
            {
                Row = 0,
                Stroke = new SolidColorBrush(Colors.DarkGray),
                StrokeThickness = 0.5,
                Background = new SolidColorBrush(Colors.Green) { Opacity = 0.1 }
            });

            chart.View.PlotAreas.RowDefinitions.Add(
              new PlotAreaRowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
            chart.View.PlotAreas.RowDefinitions.Add(
              new PlotAreaRowDefinition() { Height = new GridLength(4, GridUnitType.Star) });

            chart.Data.Children.Add(
              new HighLowOpenCloseSeries()
              {
                  Label = "Price",
                  XValueBinding = new Binding("Time"),
                  HighValueBinding = new Binding("High"),
                  LowValueBinding = new Binding("Low"),
                  OpenValueBinding = new Binding("Open"),
                  CloseValueBinding = new Binding("Close"),
                  SymbolFill = new SolidColorBrush(Colors.Blue),
                  SymbolStroke = new SolidColorBrush(Colors.Blue)
              });

            chart.Data.Children.Add(
              new XYDataSeries()
              {
                  Label = "Volume",
                  AxisY = "Volume",
                  XValueBinding = new Binding("Time"),
                  ValueBinding = new Binding("Volume"),
                  ChartType = ChartType.Column,
                  SymbolFill = new SolidColorBrush(Colors.Green)
              });

            chart.View.Axes.Add(new Axis()
            {
                AxisType = AxisType.Y,
                Name = "Volume",
                Title = "Volume",
                PlotAreaIndex = 0,
                MajorGridStroke = chart.View.AxisY.MajorGridStroke
            });

            chart.View.PlotAreas.Add(new PlotArea());

            chart.Data.ItemsSource = SampleFinancialData.Create(364);

            chart.ChartType = ChartType.Candle;
            chart.EndUpdate();
        }
    }
}
