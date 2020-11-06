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
using System.Windows.Controls.Primitives;

namespace SeriesAxis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Marker[] marks = new Marker[] { Marker.Dot, Marker.Box, Marker.Diamond, Marker.Star4 };
        Random rnd = new Random();
        public MainWindow()
        {
            InitializeComponent();
            MsgTxtBlk.Text = "* Click New Data Button"+Environment.NewLine +
                            "To Show New Data In Chart"+ Environment.NewLine +Environment.NewLine+
                            "* Hold The Left Mouse Button " + Environment.NewLine +
                            "And Click To Zoom" + Environment.NewLine +Environment.NewLine+
                            "* Click The Reset Scale Button" + Environment.NewLine +
                            "To Reset Zoom";
            NewData();
            chart.ActionLeave+=new EventHandler(chart_ActionLeave);
        }
        void chart_ActionLeave(object sender, EventArgs e)
        {
            chart.BeginUpdate();
            var ay = chart.View.AxisY;
            foreach (var ax in chart.View.Axes)
            {
                // for all auxiliary y-axes synch state with the main axis
                if (ax != ay && ax.AxisType == AxisType.Y)
                {
                    ax.Scale = ay.Scale; ax.Value = ay.Value;
                }
            }

            chart.EndUpdate();
        }
        void NewData()
        {
            chart.BeginUpdate();
            chart.Reset(true);
            chart.ChartType = ChartType.LineSymbols;
            chart.View.AxisY.Visible = false;

            int npts = 10;

            // axes/series at left
            for (int i = 0; i < 3; i++)
            {
                var max = Math.Pow(10, i + 1);
                var ds = SampleData.CreateSeries("series " + i, npts, 0, max);

                ds.AxisY = "ay" + i;

                ds.SymbolMarker = marks[rnd.Next(marks.Length)];
                chart.Data.Children.Add(ds);

                var ax = new YSeriesAxis(chart, ds, AxisPosition.Near);
                ax.Min = 0; ax.Max = max;

                chart.View.Axes.Add(ax);

                // set limits for the main y-axis
                if (i > 0)
                {
                    chart.View.AxisY.Min = 0; chart.View.AxisY.Max = max;
                    ax.MajorGridStroke = new SolidColorBrush(Colors.LightGray);
                }
            }

            // axes/series at right
            for (int i = 3; i < 6; i++)
            {
                var max = Math.Pow(10, i + 1);
                var ds = SampleData.CreateSeries("series " + i, npts, 0, max);
                ds.AxisY = "ay" + i;
                ds.SymbolMarker = marks[rnd.Next(marks.Length)];
                chart.Data.Children.Add(ds);

                var ax = new YSeriesAxis(chart, ds, AxisPosition.Far);
                
                ax.Min = 0;
                ax.Max = max;
                chart.View.Axes.Add(ax);
            }
           
            chart.Actions.Add(new C1.WPF.C1Chart.ZoomAction());
            chart.EndUpdate();
        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewData();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
           
            //  reset zoom for all axes
            chart.BeginUpdate();

            chart.View.AxisX.Scale = chart.View.AxisY.Scale = 0.5;
            chart.View.AxisX.Value = chart.View.AxisY.Value = 0.5;

            foreach (var ax in chart.View.Axes)
            {
                ax.Value = 0.5; ax.Scale = 1;
            }
            chart.EndUpdate();
        }

        private void btnhelp_Click(object sender, RoutedEventArgs e)
        {
            
            msgpop.IsOpen = true;
           
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            msgpop.IsOpen = false;
        }

        
    }
}
