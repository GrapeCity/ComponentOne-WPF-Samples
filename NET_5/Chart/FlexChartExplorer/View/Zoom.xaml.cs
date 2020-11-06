using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using C1.WPF.Chart;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Zoom : UserControl
    {
        List<DataPoint> _function1Source;
        List<DataPoint> _function2Source;
        bool zooming = false;
        Point ptStart = new Point();

        public Zoom()
        {
            this.InitializeComponent();
            this.Loaded += Zoom_Loaded;
            Tag = "This view shows how to implement a custom zoom for the FlexChart control.\r" +
                "Use the mouse or touch to select a rectangular area on the plot area.The chart will zoom in on the selected area.When you are done, click the 'Reset Zoom' button below the chart to return to the original view.";
        }

        private void Zoom_Loaded(object sender, RoutedEventArgs e)
        {
            SetupChart();
        }

        void SetupChart()
        {
            flexChart.AddHandler(FlexChart.MouseLeftButtonDownEvent, new MouseButtonEventHandler(flexChart_MouseLeftButtonDown), true);
            flexChart.AddHandler(FlexChart.MouseMoveEvent, new MouseEventHandler(flexChart_MouseMove), true);
            flexChart.AddHandler(FlexChart.MouseLeftButtonUpEvent, new MouseButtonEventHandler(flexChart_MouseLeftButtonUp), true);
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

        private void PerformZoom(Point ptStart, Point ptLast)
        {
            var p1 = flexChart.PointToData(ptStart);
            var p2 = flexChart.PointToData(ptLast);
            flexChart.BeginUpdate();
            // Update axes with new limits
            flexChart.AxisX.Min = Math.Min(p1.X, p2.X);
            flexChart.AxisY.Min = Math.Min(p1.Y, p2.Y);
            flexChart.AxisX.Max = Math.Max(p1.X, p2.X);
            flexChart.AxisY.Max = Math.Max(p1.Y, p2.Y);
            flexChart.EndUpdate();
        }

        private void DrawReversibleRectangle(Point p1, Point p2)
        {
            // Normalize the rectangle
            var rc = new Rect(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y),
                Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y));

            // Draw the reversible frame
            reversibleFrame.Rect = rc;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            flexChart.BeginUpdate();
            // Restore default values for axis limits
            flexChart.AxisX.Min = double.NaN;
            flexChart.AxisY.Min = double.NaN;
            flexChart.AxisX.Max = double.NaN;
            flexChart.AxisY.Max = double.NaN;
            flexChart.EndUpdate();
        }

        private void flexChart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            reversibleFrameContainer.Visibility = Visibility.Visible;
            //    // Start zooming
            zooming = true;
            //    // Save starting point of selection rectangle
            ptStart = e.GetPosition(flexChart);
            flexChart.CaptureMouse();
        }

        private void flexChart_MouseMove(object sender, MouseEventArgs e)
        {
            // when zooming update selection range
            if (zooming)
            {
                var currentPosition = e.GetPosition(flexChart);
                Point ptCurrent = currentPosition;
                // Draw new frame
                DrawReversibleRectangle(ptStart, ptCurrent);
            }
        }

        private void flexChart_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //End zooming
            zooming = false;
            reversibleFrameContainer.Visibility = Visibility.Collapsed;
            reversibleFrame.Rect = new Rect();
            var currentPosition = e.GetPosition(flexChart);
            PerformZoom(ptStart, currentPosition);
            //Clean up
            ptStart = new Point();
            flexChart.ReleaseMouseCapture();
        }
    }
}
