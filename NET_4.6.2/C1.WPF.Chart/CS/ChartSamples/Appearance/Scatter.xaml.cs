using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Animation;
using C1.WPF.C1Chart;

namespace ChartSamples
{
    /// <summary>
    /// Interaction logic for Scatter.xaml
    /// </summary>
    [System.ComponentModel.Description("This sample shows how to provide a custom loading animation using StoryBoards.")]
    public partial class Scatter : System.Windows.Controls.UserControl
    {
        int cnt = 80;
        Random rnd = new Random();

        public Scatter()
        {
            InitializeComponent();

            c1Chart1.View.AxisX.Visible = c1Chart1.View.AxisY.Visible = false;
            NewData();
        }

        void CreateTrigonometric()
        {
            double[] y1 = new double[100];
            double[] y2 = new double[100];

            double kx = rnd.Next(1, 5);
            double ky = rnd.Next(1, 5);

            for (int i = 0; i < y1.Length; i++)
            {
                y1[i] = Math.Sin(0.05 * kx * i);
                y2[i] = Math.Cos(0.05 * ky * i);
            }

            c1Chart1.BeginUpdate();

            DataSeries ds1 = new DataSeries();
            ds1.ValuesSource = y1;
            DataSeries ds2 = new DataSeries();
            ds2.ValuesSource = y2;

            ds1.SymbolStyle = ds2.SymbolStyle = (Style)FindResource("sstyle");
            ds1.ConnectionStyle = ds2.ConnectionStyle = (Style)FindResource("cstyle");

            c1Chart1.Data.Children.Clear();
            c1Chart1.Data.Children.Add(ds1);
            c1Chart1.Data.Children.Add(ds2);
            c1Chart1.EndUpdate();
        }

        void CreateParametric()
        {
            double[] x = new double[cnt];
            double[] y = new double[cnt];

            double kx = rnd.Next(1, 5);
            double ky = rnd.Next(1, 5);
            for (int i = 0; i < cnt; i++)
            {
                x[i] = (cnt - i - 1) * Math.Sin(0.1 * kx * i);
                y[i] = (cnt - i - 1) * Math.Cos(0.1 * ky * i);
            }

            c1Chart1.BeginUpdate();
            c1Chart1.Data.Children.Clear();
            XYDataSeries ds = new XYDataSeries();
            ds.ValuesSource = x;
            ds.XValuesSource = y;
            ds.SymbolStyle = (Style)FindResource("sstyle");
            ds.ConnectionStyle = (Style)FindResource("cstyle");
            c1Chart1.Data.Children.Add(ds);
            c1Chart1.EndUpdate();
        }

        private void Symbol_Loaded(object sender, RoutedEventArgs e)
        {
            PlotElement pe = (PlotElement)sender;

            Storyboard stb = new Storyboard();

            // time offset depending on the point index
            TimeSpan begin = pe.DataPoint != null ?
                TimeSpan.FromSeconds(0.04 * (pe.DataPoint.PointIndex)) : TimeSpan.Zero;
            Duration duration = new Duration(TimeSpan.FromSeconds(4));

            DoubleAnimation da = new DoubleAnimation();
            da.To = 0;
            da.Duration = new Duration(TimeSpan.Zero);
            Storyboard.SetTargetProperty(da, new PropertyPath("Opacity"));
            stb.Children.Add(da);

            da = new DoubleAnimation();
            da.From = 0; da.To = 1;
            da.DecelerationRatio = 1;
            da.BeginTime = begin;
            da.Duration = new Duration(TimeSpan.FromSeconds(2));
            Storyboard.SetTargetProperty(da, new PropertyPath("Opacity"));
            stb.Children.Add(da);

            Canvas cnv = (Canvas)pe.Parent;

            double cx = 0.5 * cnv.ActualWidth;
            double cy = 0.5 * cnv.ActualHeight;

            // set initial position
            da = new DoubleAnimation();
            da.To = -Canvas.GetTop(pe) + cy;
            da.Duration = new Duration(TimeSpan.Zero);
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Children[1].Y"));
            stb.Children.Add(da);

            da = new DoubleAnimation();
            da.To = -Canvas.GetLeft(pe) + cx;
            da.Duration = new Duration(TimeSpan.Zero);
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Children[1].X"));
            stb.Children.Add(da);

            // animate position
            da = new DoubleAnimation();
            da.From = -Canvas.GetTop(pe) + cy; da.To = 0;
            da.BeginTime = begin; da.Duration = duration;
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Children[1].Y"));
            da.AccelerationRatio = 1;
            stb.Children.Add(da);

            da = new DoubleAnimation();
            da.From = -Canvas.GetLeft(pe) + cx; da.To = 0;
            da.BeginTime = begin; da.Duration = duration;
            da.AccelerationRatio = 1;
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Children[1].X"));
            stb.Children.Add(da);

            // animate rotation
            da = new DoubleAnimation();
            da.From = 0; da.To = 720;
            da.BeginTime = begin; da.Duration = duration;
            Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.Children[2].Angle"));
            stb.Children.Add(da);

            stb.Begin(pe);
        }

        private void NewData()
        {
            switch (set.SelectedIndex)
            {
                case 0:
                    CreateTrigonometric();
                    break;
                case 1:
                    CreateParametric();
                    break;
            }
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewData();
        }

        private void btnReplay_Click(object sender, RoutedEventArgs e)
        {
            c1Chart1.BeginUpdate();
            c1Chart1.EndUpdate();
        }

        private void cbAxes_Click(object sender, RoutedEventArgs e)
        {
            c1Chart1.View.AxisX.Visible = c1Chart1.View.AxisY.Visible = (bool)cbAxes.IsChecked;
        }

        private void set_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (c1Chart1 != null && c1Chart1.IsLoaded)
                NewData();
        }
    }
}