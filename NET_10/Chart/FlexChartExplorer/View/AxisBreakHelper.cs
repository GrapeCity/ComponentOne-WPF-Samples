using C1.Chart;
using C1.WPF.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using C1.WPF.Core;
using GridLength = System.Windows.GridLength;
using GridUnitType = System.Windows.GridUnitType;

namespace FlexChartExplorer.View
{
    /// <summary>
    /// Helper class that allows to create axis break on FlexChart. 
    /// </summary>
    public class AxisBreakHelper
    {
        static Dictionary<FlexChart, AxisBreak> dict = new Dictionary<FlexChart, AxisBreak>();

        class AxisBreak
        {
            FlexChart chart;
            Axis axis; // main
            Axis? axis2;// aux
            Series? ser2;
            Brush seriesStroke;

            public AxisBreak(Axis axis, double min1, double max1, double min2, double max2, double size, double gapPixels)
            {
                this.axis = axis;
                chart = (FlexChart)axis.Chart;

                chart.BeginUpdate();

                AxisBreakHelper.Remove(chart);
                dict.Add(chart, this);

                CreatePlotAreas(min1, max1, min2, max2, size, gapPixels);
                CreateSeries();
                chart.Rendered += ChartRendered;
                chart.EndUpdate();
            }

            void CreatePlotAreas(double min1, double max1, double min2, double max2, double size, double gapPixels)
            {
                if (axis.AxisType == AxisType.X)
                {
                    var pa1 = new PlotArea() { Column = 0, PlotAreaName = "pa1", Width = new GridLength(size, GridUnitType.Star) };
                    var paBreak = new PlotArea() { Column = 1, PlotAreaName = "pabreak", Width = new GridLength(gapPixels) };
                    var pa2 = new PlotArea() { Column = 2, PlotAreaName = "pa2" };
                    chart.PlotAreas.Add(pa1);
                    chart.PlotAreas.Add(paBreak);
                    chart.PlotAreas.Add(pa2);
                }
                else
                {
                    var pa1 = new PlotArea() { Row = 2, PlotAreaName = "pa1", Height = new GridLength(size, GridUnitType.Star) };
                    var paBreak = new PlotArea() { Row = 1, PlotAreaName = "pabreak", Height = new GridLength(gapPixels) };
                    var pa2 = new PlotArea() { Row = 0, PlotAreaName = "pa2" };
                    chart.PlotAreas.Add(pa1);
                    chart.PlotAreas.Add(paBreak);
                    chart.PlotAreas.Add(pa2);
                }

                axis.Min = min1;
                axis.Max = max1;
                axis.PlotAreaName = "pa1";

                axis2 = CreateAuxAxis(min2, max2);
            }

            Axis CreateAuxAxis(double min, double max)
            {
                return new Axis() // clone main settings from main axis
                {
                    Min = min,
                    Max = max,
                    Position = axis.Position,
                    MajorGrid = axis.MajorGrid,
                    MajorTickMarks = axis.MajorTickMarks,
                    AxisLine = false,
                    PlotAreaName = "pa2"
                };
            }

            void CreateSeries()
            {
                // 2nd series - aux axis
                ser2 = new Series();
                if (axis.AxisType == AxisType.X)
                    ser2.AxisX = axis2;
                else
                    ser2.AxisY = axis2;

                if (chart.Series.Count > 0)
                {
                    ser2.Style = chart.Series[0].Style; // same style for both seriea
                    seriesStroke = ser2.Style?.Stroke;
                }
                chart.Series.Add(ser2);
            }

            private void ChartRendered(object sender, RenderEventArgs e)
            {
                if (axis2 != null)
                    DrawAxisBreak(e.Engine, axis, axis2, seriesStroke);
            }

            private void DrawAxisBreak(IRenderEngine e, Axis axis1, Axis axis2, Brush stroke)
            {
                var rect1 = ((IAxis)axis1).Rect;
                var rect2 = ((IAxis)axis2).Rect;

                if (axis1.AxisType == AxisType.X)
                {
                    var left = Math.Min(rect1.Left, rect2.Left);
                    var right = Math.Max(rect1.Right, rect2.Right);

                    var x = 0.5 * (Math.Min(rect1.Right, rect2.Right) + Math.Max(rect1.Left, rect2.Left));
                    var y = rect1.Top;

                    DrawBreakArea(e, chart.PlotRect.Top, x - 15, chart.PlotRect.Bottom, x + 5, true);

                    e.SetStrokeThickness(3);
                    e.SetStroke(stroke);

                    e.DrawLine(left, y, x - 10, y);
                    e.DrawLine(x - 10, y, x - 5, y + 10);
                    e.DrawLine(x - 5, y + 10, x + 5, y - 10);
                    e.DrawLine(x + 10, y, x + 5, y - 10);
                    e.DrawLine(x + 10, y, right, y);
                }
                else
                {
                    var x = rect1.Right;
                    var y = 0.5 * (Math.Min(rect1.Bottom, rect2.Bottom) + Math.Max(rect1.Top, rect2.Top));
                    var top = Math.Min(rect1.Top, rect2.Top);
                    var bottom = Math.Max(rect1.Bottom, rect2.Bottom);

                    DrawBreakArea(e, chart.PlotRect.Left, y - 15, chart.PlotRect.Right, y + 5);

                    e.SetStrokeThickness(3);
                    e.SetStroke(stroke);

                    e.DrawLine(x, top, x, y - 10);
                    e.DrawLine(x, y - 10, x + 10, y - 5);
                    e.DrawLine(x - 10, y + 5, x + 10, y - 5);
                    e.DrawLine(x, y + 10, x - 10, y + 5);
                    e.DrawLine(x, bottom, x, y + 10);
                }

                e.SetStroke(null);
            }

            static Random rnd = new Random(0);

            void DrawBreakArea(IRenderEngine engine, double x1, double y1, double x2, double y2, bool rotated = false)
            {
                var xs = new List<double>();
                var ys = new List<double>();

                var currentX = x2;

                xs.Add(currentX);
                ys.Add(y1);
                while (currentX > x1)
                {
                    var step = 10 + rnd.NextDouble() * 10;
                    currentX = Math.Max(0, currentX - step);
                    var offset = rnd.NextDouble() * 10;
                    xs.Add(Math.Max(currentX, x1));
                    ys.Add(y1 + offset);
                }

                var xa1 = xs.ToArray();
                var ya1 = ys.ToArray();

                var xpoly = new List<double>();
                var ypoly = new List<double>();
                xpoly.AddRange(xs);
                ypoly.AddRange(ys);

                for (int i = 0; i < ys.Count; i++)
                    ys[i] += y2 - y1;

                xs.Reverse();
                ys.Reverse();
                xpoly.AddRange(xs);
                ypoly.AddRange(ys);

                engine.SetStroke(null);
                engine.SetFill(Application.Current.MainWindow.Background);

                if (rotated)
                    engine.DrawPolygon(ypoly.ToArray(), xpoly.ToArray());
                else
                    engine.DrawPolygon(xpoly.ToArray(), ypoly.ToArray());

                engine.SetStrokeThickness(1);
                engine.SetStroke(chart.Foreground);

                if (rotated)
                {
                    engine.DrawLines(ya1.ToArray(), xa1.ToArray());
                    engine.DrawLines(ys.ToArray(), xs.ToArray());
                }
                else
                {
                    engine.DrawLines(xa1.ToArray(), ya1.ToArray());
                    engine.DrawLines(xs.ToArray(), ys.ToArray());
                }
            }

            public void Remove()
            {
                chart.Rendered -= ChartRendered;
                axis.Min = axis.Max = double.NaN;
                axis.PlotAreaName = null;
                if (ser2 != null)
                    chart.Series.Remove(ser2);
                chart.PlotAreas.Clear();
                dict.Remove(chart);
            }
        }

        /// <summary>
        /// Create axis break on the spefiied axis.
        /// </summary>
        /// <param name="axis">Main axis to break. Can be x-axis or y-axis.</param>
        /// <param name="min1">Mininmum (1st axis part)</param>
        /// <param name="max1">Maximum (1st axis part)</param>
        /// <param name="min2">Mininmum (2nd axis part)</param>
        /// <param name="max2">Maximum (2nd axis part)</param>
        /// <param name="size">Relative size of 1st part. Default value is 1, so 1st and 2nd part have the same size
        /// (height for y-axis and width for x-axis). For example, when size = 3 the 1st part is 75% and 2nd part is 25% of available space.
        /// </param>
        /// <param name="gapPixels">Gap(space) between two axis parts in pixels.</param>
        /// <remarks>
        /// Only single axis break on the chart supported.
        /// </remarks>
        public static void CreateAxisBreak(Axis axis, double min1, double max1, double min2, double max2, double size = 1, double gapPixels = 16)
        {
            new AxisBreak(axis, min1, max1, min2, max2, size, gapPixels);
        }

        /// <summary>
        /// Remove axis break from the chart.
        /// </summary>
        /// <param name="chart">Chart instance</param>
        public static void Remove(FlexChart chart)
        {
            if (dict.TryGetValue(chart, out AxisBreak? axisBreak))
                axisBreak?.Remove();
        }
    }

}
