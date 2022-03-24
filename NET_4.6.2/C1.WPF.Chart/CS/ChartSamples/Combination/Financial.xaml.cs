using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

using C1.WPF.C1Chart;

namespace ChartSamples
{
    /// <summary>
    /// Interaction logic for Financial.xaml
    /// </summary>

    public partial class Financial : System.Windows.Controls.UserControl
    {
        Random rnd = new Random();

        public Financial()
        {
            InitializeComponent();

            NewData(this, new RoutedEventArgs());
        }

        void NewData(object sender, RoutedEventArgs args)
        {
            int n = 20;

            // price series
            HighLowOpenCloseSeries hloc = (HighLowOpenCloseSeries)chart.Data.Children[0];

            // volume series
            XYDataSeries xyds = (XYDataSeries)chart.Data.Children[1];

            DateTime[] x = new DateTime[n];
            double[] hi = new double[n];
            double[] lo = new double[n];
            double[] op = new double[n];
            double[] cl = new double[n];
            double[] vals = new double[n];

            DateTime dt = DateTime.Today;

            for (int i = 0; i < n; i++)
            {
                x[i] = dt.AddDays(i);

                if (i > 0)
                    op[i] = cl[i - 1];
                else
                    op[i] = 1000;

                hi[i] = op[i] + rnd.Next(50);
                lo[i] = op[i] - rnd.Next(50);

                cl[i] = rnd.Next((int)lo[i], (int)hi[i]);
                vals[i] = rnd.Next(200);
            }

            hloc.ChartType = Price.SelectedIndex == 0 ? ChartType.HighLowOpenClose : ChartType.Candle;
            hloc.XValuesSource = x;
            hloc.ValuesSource = hi;
            hloc.HighValuesSource = hi;
            hloc.LowValuesSource = lo;
            hloc.OpenValuesSource = op;
            hloc.CloseValuesSource = cl;

            xyds.ChartType = Vol.SelectedIndex == 0 ? ChartType.Column : ChartType.Area;
            xyds.XValuesSource = x;
            xyds.ValuesSource = vals;
           
            chart.View.AxisX.Min = DateTime.Today.AddDays(-1).ToOADate();
            chart.View.AxisX.Max = DateTime.Today.AddDays(20).ToOADate();
        }

        private void Price_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (chart != null)
            {
                HighLowOpenCloseSeries ds = (HighLowOpenCloseSeries)chart.Data.Children[0];
                if (Price.SelectedIndex == 0)
                    ds.ChartType = ChartType.HighLowOpenClose;
                else
                    ds.ChartType = ChartType.Candle;
            }
        }

        private void Vol_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (chart != null)
            {
                XYDataSeries ds = (XYDataSeries)chart.Data.Children[1];
                if (Vol.SelectedIndex == 0)
                    ds.ChartType = ChartType.Column;
                else
                    ds.ChartType = ChartType.Area;
            }
        }

        private void Bar_Loaded(object sender, RoutedEventArgs e)
        {
            //PlotElement pe = (PlotElement)sender;

            //Storyboard sb = new Storyboard();

            //DoubleAnimation da = new DoubleAnimation();
            //if( pe.DataPoint!=null)
            //    da.BeginTime = TimeSpan.FromSeconds(0.03 * pe.DataPoint.PointIndex);
            //da.From = 0; da.To = 1; da.Duration = new Duration(TimeSpan.FromSeconds(0.4));
            //Storyboard.SetTargetProperty(da, new PropertyPath("RenderTransform.ScaleY"));
            //sb.Children.Add(da);

            //sb.Begin(pe);
        }
    }

    public class TimeConverter : IValueConverter
    {
        public static readonly TimeConverter Default =
           new TimeConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double)
                return DateTime.FromOADate((double)value).ToShortDateString();
            if (value is DateTime)
                return ((DateTime)value).ToLongDateString();
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}