using System;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexChartCustomization
{
    /// <summary>
    /// Interaction logic for LineStyles.xaml
    /// </summary>
    public partial class LineStyles : UserControl
    {
        public LineStyles()
        {
            InitializeComponent();
            flexChart.Series[0].Style = new C1.WPF.Chart.ChartStyle { Stroke = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)) };
        }

        double previousX, previousY;
        int sequentialIncrement, sequentialDecrement;
        private void Series_SymbolRendered(object sender, C1.WPF.Chart.RenderSymbolEventArgs e)
        {
            if (e.Index > 0)
            {
                Color lineColor;
                if (e.Point.Y == previousY)
                {
                    lineColor = Color.FromRgb(255, 255, 0);
                    e.Engine.SetStrokePattern(new double[] { 2, 1 });
                }
                else if (e.Point.Y > previousY)
                {
                    lineColor = Color.FromArgb((byte)Math.Min(100 + 50 * sequentialIncrement, 255), 0, 255, 0);
                    sequentialIncrement++;
                    sequentialDecrement = 0;
                    e.Engine.SetStrokePattern(null);
                }
                else
                {
                    lineColor = Color.FromArgb((byte)Math.Min(100 + 50 * sequentialDecrement, 255), 255, 0, 0);
                    sequentialDecrement++;
                    sequentialIncrement = 0;
                    e.Engine.SetStrokePattern(null);
                }
                e.Engine.SetStroke(new SolidColorBrush(lineColor));
                e.Engine.SetStrokeThickness(3);
                e.Engine.DrawLine(previousX, previousY, e.Point.X, e.Point.Y);
            }
            previousX = e.Point.X;
            previousY = e.Point.Y;
        }
    }
}
