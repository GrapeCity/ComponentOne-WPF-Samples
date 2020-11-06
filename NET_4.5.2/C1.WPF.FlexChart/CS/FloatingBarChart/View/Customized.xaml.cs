using C1.Chart;
using C1.WPF.Chart;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace FloatingBarChart
{
    /// <summary>
    /// Interaction logic for Customized.xaml
    /// </summary>
    public partial class Customized : UserControl
    {
        Series series;
        public Customized()
        {
            InitializeComponent();

            series = new Series();
            series.Binding = "Start,End";
            series.SymbolRendered += Series_SymbolRendered;
            flexChart.Rendered += FlexChart_Rendered;

            flexChart.AxisY.Reversed = true;
            flexChart.AxisY.MajorGrid = false;
            flexChart.AxisY.MinorGrid = true;
            flexChart.AxisY.MinorGridStyle.Stroke = new SolidColorBrush(Color.FromRgb(211,211,211));
            flexChart.AxisX.Min = ViewModel.ChartData.Min(t => t.Start).ToOADate();
            flexChart.AxisX.Format = "MMM yyyy";

            flexChart.Series.Add(series);
        }

        private void FlexChart_Rendered(object sender, RenderEventArgs e)
        {
            ViewModel.ChartData.ForEach(task => {
                var parents = GetTaskParents(task);
                parents.ForEach(parent => {
                    DrawConnectingLine(e.Engine, task, parent);
                });
            });
        }

        private void Series_SymbolRendered(object sender, RenderSymbolEventArgs e)
        {
            var task = (Task)e.Item;
            var rect = GetTaskRect(task);
            rect.Inflate(-8, -8);
            var pct = task.Percent;
            var clap = (pct < 0 ? 0 : (pct > 100 ? 100 : pct)) / 100;
            e.Engine.SetFill(new SolidColorBrush(clap == 1 ? Color.FromRgb(0, 128, 0) : Color.FromRgb(255, 215, 00)));
            e.Engine.DrawRect(rect.Left, rect.Top, rect.Width * pct / 100, rect.Height);
        }

        _Rect GetTaskRect(Task task)
        {
            float left = (float)flexChart.AxisX.Convert(task.Start.ToOADate());
            float right = (float)flexChart.AxisX.Convert(task.End.ToOADate());
            var index = ViewModel.ChartData.IndexOf(task);
            float top = (float)flexChart.AxisY.Convert(index - 0.35);
            float bottom = (float)flexChart.AxisY.Convert(index + 0.35);
            return new _Rect(left, top, right - left, bottom - top);
        }

        private List<Task> GetTaskParents(Task task)
        {
            var parents = new List<Task>();
            if (task.Parent != null)
            {
                string[] names = task.Parent.Split(',');
                foreach (string name in names)
                {
                    var parent = ViewModel.ChartData.Find(t => t.Name == name);
                    if (parent != null)
                    {
                        parents.Add(parent);
                    }
                }
            }
            return parents;
        }

        void DrawConnectingLine(IRenderEngine engine, Task task, Task parent)
        {
            var rc1 = GetTaskRect(parent);      // parent rect
            var rc2 = GetTaskRect(task);        // task rect
            var x1 = rc1.Left + rc1.Width / 2;  // parent x center
            var x2 = rc2.Left;                  // task left
            var y1 = rc1.Bottom;                // parent bottom
            var y2 = rc2.Top + rc2.Height / 2;  // task y center

            // draw connecting line
            var xs = new double[] { x1, x1, x2 };
            var ys = new double[] { y1, y2, y2 };
            engine.SetStroke(new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            engine.DrawLines(xs, ys);

            // draw arrow at the end
            var sz = 5;
            xs = new double[] { x2 - 2 * sz, x2, x2 - 2 * sz };
            ys = new double[] { y2 - sz, y2, y2 + sz };
            engine.SetFill(new SolidColorBrush(Color.FromRgb(0, 0, 0)));
            engine.DrawPolygon(xs, ys);
        }
    }
}
