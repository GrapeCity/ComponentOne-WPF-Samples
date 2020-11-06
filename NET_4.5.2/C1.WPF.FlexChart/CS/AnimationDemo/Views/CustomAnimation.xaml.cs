using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using C1.Chart;
using C1.WPF.Chart;

namespace AnimationDemo.Views
{
    /// <summary>
    /// Interaction logic for CustomAnimation.xaml
    /// </summary>
    public partial class CustomAnimation : UserControl
    {
        int npts = 100;

        public CustomAnimation()
        {
            InitializeComponent();
        }

        private void AddSer_Click(object sender, RoutedEventArgs e)
        {
            AddsSeries();
        }

        private void DelSer_Click(object sender, RoutedEventArgs e)
        {
            if (chart.Series.Count > 0)
                chart.Series.RemoveAt(chart.Series.Count - 1);
        }

        private void chart_Loaded(object sender, RoutedEventArgs e)
        {
            chart.AnimationLoad.Direction = AnimationDirection.XY;
            chart.AnimationLoad.Easing = Easing.Linear;

            AddsSeries();
        }

        void AddsSeries()
        {
            var pts = new Point[npts];

            var rnd = new Random();
            for (var i = 0; i < npts; i++)
                pts[i] = new Point(2 * (rnd.NextDouble() - 0.5f), 2 * (rnd.NextDouble() - 0.5f));

            var ser = new Series() { BindingX = "X", Binding = "Y", ItemsSource = pts, SymbolSize = 4 };
            chart.Series.Add(ser);
        }

        private void AnimationTransform(object sender, AnimationTransformEventArgs e)
        {
            var center = ((Point[])e.Series.DataSource)[0];
            e.Start = e.AxisType == AxisType.X ? center.X : center.Y;

            //var pt = ((Point[])e.Series.DataSource)[e.PointIndex];

            //var r = Math.Sqrt(pt.X * pt.X + pt.Y * pt.Y);
            //var angle = Math.Atan2(pt.Y, pt.X) + 2 * Math.PI * (1 - e.Position);

            //if (e.AxisType == AxisType.X)
            //    e.Value = r * e.Position * Math.Cos(angle);
            //else
            //    e.Value = r * e.Position * Math.Sin(angle);
            //e.Start = 0;

            //e.Cancel = true;
        }

        private void chart_Unloaded(object sender, RoutedEventArgs e)
        {
            chart.Series.Clear();
        }
    }

}
