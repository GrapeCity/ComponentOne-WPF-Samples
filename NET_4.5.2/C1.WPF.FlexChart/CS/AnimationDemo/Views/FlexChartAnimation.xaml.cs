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
    /// Interaction logic for FlexChartAnimation.xaml
    /// </summary>
    public partial class FlexChartAnimation : UserControl
    {
        DataSource dataSource;

        public FlexChartAnimation()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            dataSource.NewData();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            dataSource.UpdateData();
        }

        private void AddSer_Click(object sender, RoutedEventArgs e)
        {
            dataSource.AddSeries();
        }

        private void DelSer_Click(object sender, RoutedEventArgs e)
        {
            dataSource.RemoveSeries();
        }

        private void AddPoint_Click(object sender, RoutedEventArgs e)
        {
            dataSource.AddPoint();
        }

        private void DelPoint_Click(object sender, RoutedEventArgs e)
        {
            dataSource.RemovePoint();
        }

        private void chart_Loaded(object sender, RoutedEventArgs e)
        {
            cbChartType.ItemsSource = new ChartType[] { ChartType.Column, ChartType.Bar, ChartType.Line, ChartType.LineSymbols, ChartType.Scatter, ChartType.Area };
            cbStacking.ItemsSource = new Stacking[] { Stacking.None, Stacking.Stacked, Stacking.Stacked100pc };
            cbRenderMode.ItemsSource = new RenderMode[] { RenderMode.Default, RenderMode.Direct2D };
            cbAnimation.ItemsSource = new AnimationSettings[]
                    { AnimationSettings.None, AnimationSettings.Load, AnimationSettings.Update,
                        AnimationSettings.AxesLoad, AnimationSettings.AxesUpdate, AnimationSettings.Axes,
                        AnimationSettings.All };
            cbAnimation.SelectedValue = AnimationSettings.All;

            dataSource = new DataSource(chart);
            dataSource.NewData();
        }

        private void cbAnimation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pg1.IsEnabled = (chart.AnimationSettings & (AnimationSettings.Load | AnimationSettings.AxesLoad)) != 0;
            pg2.IsEnabled = (chart.AnimationSettings & (AnimationSettings.Update | AnimationSettings.AxesUpdate)) != 0;
        }
    }

    public enum DataShape
    {
        Line,
        Sin,
        Random,
        Ellipse,
        Spiral,
        Grid
    }

    class DataSource
    {
        C1FlexChart chart;
        int npts = 10;
        int nser = 3;
        static Random rnd = new Random();

        public DataSource(C1FlexChart chart)
        {
            this.chart = chart;
        }

        public void NewData()
        {
            var max = (1 + (int)(rnd.NextDouble() * 5)) * 100;

            chart.Series.Clear();

            for (var i = 0; i < nser; i++)
                chart.Series.Add(new Series()
                {
                    Binding = "Y",
                    BindingX = "X",
                    ItemsSource = DataHelper.Create(DataShape.Random, npts, max),
                    SeriesName = i.ToString()
                });
        }

        public void UpdateData()
        {
            var max = (1 + (int)(rnd.NextDouble() * 5)) * 100;
            var nser = chart.Series.Count;

            for (var i = 0; i < nser; i++)
                chart.Series[i].ItemsSource = DataHelper.Create(DataShape.Random, npts, max);
        }

        public void AddSeries()
        {
            var max = (1 + (int)(rnd.NextDouble() * 5)) * 100;

            chart.Series.Add(new Series()
            {
                Binding = "Y",
                BindingX = "X",
                ItemsSource = DataHelper.Create(DataShape.Random, npts, max),
                SeriesName = chart.Series.Count.ToString()
            });
        }

        public void RemoveSeries()
        {
            var cnt = chart.Series.Count;
            if (cnt > 0)
                chart.Series.RemoveAt(cnt - 1);
        }

        public void AddPoint()
        {
            var max = (1 + (int)(rnd.NextDouble() * 5)) * 100;

            foreach (var s in chart.Series)
            {
                var col = (ObservableCollection<Point>)s.ItemsSource;
                col.Add(new Point(col.Count, rnd.NextDouble() * max));
            }
        }

        public void RemovePoint()
        {
            foreach (var s in chart.Series)
            {
                var col = (ObservableCollection<Point>)s.ItemsSource;
                if (col.Count > 0)
                    col.RemoveAt(col.Count - 1);
            }
        }
    }

    class DataHelper
    {
        static Random rnd = new Random();

        public static ObservableCollection<Point> Create(DataShape shape, int npts, float max = 100)
        {
            var pts = new Point[npts];

            Func<int, Point> f = null;

            switch (shape)
            {
                case DataShape.Line:
                    f = (i) => new Point(i, i);
                    break;
                case DataShape.Sin:
                    f = (i) => new Point(i, (float)Math.Sin(0.1 * i));
                    break;
                case DataShape.Random:
                    f = (i) => new Point(i, (int)(rnd.NextDouble() * max));
                    break;
                case DataShape.Ellipse:
                    f = (i) => new Point(Math.Sin((2 * Math.PI * i) / npts), Math.Cos((2 * Math.PI * i) / npts));
                    break;
                case DataShape.Spiral:
                    f = (i) => new Point(i * (float)Math.Sin((4 * Math.PI * i) / npts), i * (float)Math.Cos((4 * Math.PI * i) / npts));
                    break;
                case DataShape.Grid:
                    var l = (int)Math.Sqrt(npts);
                    f = (i) => new Point(i % l, (int)(i / l));
                    break;
            }

            for (var i = 0; i < npts; i++)
                pts[i] = f(i);

            return new ObservableCollection<Point>(pts);
        }
    }
}
