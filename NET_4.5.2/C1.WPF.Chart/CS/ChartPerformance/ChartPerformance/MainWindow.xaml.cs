using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;

namespace ChartPerformance
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            _chart.Data.Children.Clear();
            var chrtdt = PopulateProducts();
            var xvals = chrtdt.ConvertAll(e => e.Month).Distinct().ToArray();

            #region SeriesAdd
            //Adding the sale series
            DataSeries slsers = new DataSeries();
            slsers.ValuesSource = chrtdt.ConvertAll(e => e.Sales).ToArray();
            slsers.RenderMode = RenderMode.Bitmap;
            _chart.Data.Children.Add(slsers);

            //Adding the download series
            DataSeries dnldsrs = new DataSeries();
            dnldsrs.ValuesSource = chrtdt.ConvertAll(e => e.Downloads).ToArray();
            dnldsrs.RenderMode = RenderMode.Bitmap;
            _chart.Data.Children.Add(dnldsrs);

            //Adding the revision series
            DataSeries rvsrs = new DataSeries();
            rvsrs.ValuesSource = chrtdt.ConvertAll(e => e.Revisions).ToArray();
            rvsrs.RenderMode = RenderMode.Bitmap;
            _chart.Data.Children.Add(rvsrs); 
            #endregion

            //_chart.Data.ItemNames = xvals;

            //var ax = _chart.View.AxisX;
            //ax.IsTime = true; // << ** PERF NOTE: if values are DateTime, set this to true!
            //ax.ItemsValueBinding = new Binding("Month");
            //ax.AnnoFormat = "MMM-yy";
            //ax.AnnoAngle = 45;
            //_chart.View.AxisX.MajorGridStrokeThickness = 0;

            _chart.ChartType = ChartType.Line;
           _chart.MouseLeftButtonDown+=new MouseButtonEventHandler(_chart_MouseLeftButtonDown);
        }
        /// <summary>
        /// Handles the left mouse down event of the chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _chart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            C1.WPF.C1Chart.DataSeries ds;
            int pointIndex;
            if (TryGetClosestPoint(_chart, e, 200, out ds, out pointIndex))
            {
                string msg = string.Format("Thanks for clicking point {0} on data series {1}", pointIndex, ds.Label);
                MessageBox.Show(msg);
            }
        }
        // find the closest series to the mouse, including threshold
        bool TryGetClosestPoint(C1.WPF.C1Chart.C1Chart chart, MouseEventArgs e, double threshold, out C1.WPF.C1Chart.DataSeries ds, out int pointIndex)
        {
            // initialize variables
            ds = null;
            pointIndex = -1;
            int dsIndex = -1;
            int ptIndex = -1;
            double distMin = double.MaxValue;
            double dist = 0;

            // get position in chart coordinates
            var pt = e.GetPosition(chart);

            // look for the closest series using the DataIndexFromPoint method
            for (int i = 0; i < chart.Data.Children.Count; i++)
            {
                ptIndex = chart.View.DataIndexFromPoint(pt, i, C1.WPF.C1Chart.MeasureOption.XY, out dist);
                if (dist < distMin)
                {
                    distMin = dist;
                    dsIndex = i;
                }
            }

            // return result
            if (distMin <= threshold)
            {
                ds = chart.Data.Children[dsIndex];
                pointIndex = ptIndex;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Populates a list of productdata 
        /// This is to be used as a datasource for the chart
        /// </summary>
        /// <returns></returns>
        List<ProductData> PopulateProducts()
        {
            // create some random data
            var list = new List<ProductData>();

            var sales = 10;
            var downloads = 100;
            var revisions = 0;
            var rnd = new Random();
            for (var month = DateTime.Today.AddYears(-150); month <= DateTime.Today; month = month.AddMonths(1))
            {
                var sd = new ProductData();
                sd.ProductName = "Widgets";
                sd.Month = month;
                sd.Sales = sales;
                sd.Downloads = downloads;
                sd.Revisions = revisions;
                list.Add(sd);

                sales = Math.Max(0, sales + rnd.Next(-2, +2));
                downloads = Math.Max(0, downloads + rnd.Next(-8, +8));
                revisions = Math.Max(0, revisions + rnd.Next(-1, +1));

                if (rnd.NextDouble() > .90)
                {
                    sales = Math.Max(0, sales + rnd.Next(-100, +90));
                    downloads = Math.Max(0, downloads + rnd.Next(-500, +450));
                    revisions = Math.Max(0, revisions + rnd.Next(-50, +45));
                }
            }

            return list;
        }
    }
}
