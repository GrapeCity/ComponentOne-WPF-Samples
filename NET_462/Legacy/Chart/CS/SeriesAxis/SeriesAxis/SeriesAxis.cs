using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.C1Chart;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace SeriesAxis
{
    /// <summary>
    /// Represent series axis.
    /// </summary>
    public class SeriesAxis1 : Axis
    {
        #region fields

        C1Chart _chart;
        DataSeries _series;

        #endregion

        #region .ctor

        /// <summary>
        /// Creates an instance of SeriesAxis.
        /// </summary>
        /// <param name="chart"></param>
        /// <param name="series"></param>
        public SeriesAxis1(C1Chart chart, DataSeries series)
        {
            
            _chart = chart;
            _series = series;

            if (_series != null)
                Name = _series.AxisY;

            AnnoCreated += new AnnoCreatedEventHandler(SeriesAxis_AnnoCreated);
            MajorGridStroke = null;
        }

        #endregion

        #region implementation

        void SeriesAxis_AnnoCreated(object sender, AnnoCreatedEventArgs e)
        {

            var ax = (Axis)sender;
            LegendItem li = null;
            var isFar = (Position & AxisPosition.Far) != 0;

            // find legend item
            if (_chart != null)
            {
                foreach (var item in _chart.LegendItems)
                {
                    if (item.Item == _series)
                    {
                        li = item;
                        break;
                    }
                }
            }

            // label color
            if (li != null)
            {
                var tb = e.Label as TextBlock;
                if (tb != null)
                    tb.Foreground = li.Symbol.Fill;
            }

            if (e.Index == 0 && _series != null)
            {
                var slbl = new TextBlock() { Text = _series.Label, VerticalAlignment = VerticalAlignment.Center };

                // display symbol
                if (li != null)
                {
                    slbl.Foreground = li.Symbol.Fill;
                    Canvas.SetLeft(li.Symbol, -12);
                    Canvas.SetTop(li.Symbol, 6);
                    e.Canvas.Children.Add(li.Symbol);
                }

                // show label
                slbl.Measure(new Size(10000, 10000));
                Canvas.SetLeft(slbl, 0);
                Canvas.SetTop(slbl, 0);
                e.Canvas.Children.Add(slbl);
                e.Canvas.Tag = GetPosition(slbl);

                if (!isFar)
                {
                    slbl.RenderTransform = new RotateTransform()
                    {
                        Angle = 180,
                        CenterX = 0.5 * slbl.ActualWidth,
                        CenterY = 0.5 * slbl.ActualHeight + 2
                    };
                }

                slbl.MouseLeftButtonDown += (s, args) =>
                {
                    args.Handled = true;
                    MessageBox.Show(string.Format("Clicked on axis label = {0}", _series.Label));
                };
            }

            // hide label if it is overlapped with series label
            if (e.Canvas.Tag is Rect)
            {
                var tb = e.Label as FrameworkElement;
                var rect = (Rect)e.Canvas.Tag;
                rect.Intersect(GetPosition(tb));
                e.Cancel = !rect.IsEmpty;
            }

            // e.Canvas.ClipToBounds = false;

            // handle clicking of label
            if (!e.Cancel)
            {
                e.Label.MouseLeftButtonDown += (s, args) =>
                {
                    if (e.Label is TextBlock)
                        MessageBox.Show(string.Format("Clicked on axis = {0} at label = {1}", _series.Label, ((TextBlock)e.Label).Text));
                    args.Handled = true;
                };
            }
        }

        Rect GetPosition(FrameworkElement element)
        {

            return new Rect(Canvas.GetLeft(element), Canvas.GetTop(element), element.DesiredSize.Width, element.DesiredSize.Height);
        }

        // first axis starting from plot area
        bool IsFirstAxis()
        {
            if (_chart == null)
                return false;

            return this == GetFirstAxis();
        }

        // find first axis from left or right side of plot area
        SeriesAxis1 GetFirstAxis()
        {
            if (_chart == null)
                return null;

            var pos = Position;
            var axtype = AxisType;

            foreach (var ax in _chart.View.Axes)
            {
                if (ax.Visible)
                {
                    if (ax.Position == pos && axtype == ax.AxisType)
                        return ax as SeriesAxis1;
                }
            }

            return null;
        }

        #endregion
    }
}
