using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;

namespace ChartSamples
{
    [Description("This sample dhows a financial chart with Two Y-axes.")]
    public partial class FinancialChart : UserControl
    {
        public FinancialChart()
        {
            InitializeComponent();
            chart.View.AxisX.Scale = 0.25;
            NewData();
        }

        void NewData()
        {
            long ticks = DateTime.Now.Ticks;
            int len = 100;
            List<Quotation> data = SampleFinancialData.Create(len);
            chart.BeginUpdate();
            chart.Data.ItemsSource = data;
            chart.View.AxisX.Min = data[0].Time.ToOADate() - 0.5;
            chart.View.AxisX.Max = data[len - 1].Time.ToOADate() + 0.5;
            chart.EndUpdate();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            NewData();
        }

        private void Series_Loaded(object sender, EventArgs e)
        {
            PlotElement pe = (PlotElement)sender;
            if (pe is HLOC)
            {
                TextBlock tb = (TextBlock)FindName("txtPrice");
                tb.Foreground = pe.Stroke;
            }
            else
            {
                TextBlock tb = (TextBlock)FindName("txtVol");
                tb.Foreground = pe.Stroke;
            }
        }

        private void btnPrice_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            if ((string)btn.Content == "Standard")
            {
                btn.Content = "Candle";
                chart.ChartType = ChartType.HighLowOpenClose;
            }
            else
            {
                btn.Content = "Standard";
                chart.ChartType = ChartType.Candle;
            }
        }

        private void btnVol_Click(object sender, RoutedEventArgs e)
        {
            chart.BeginUpdate();
            Button btn = (Button)sender;
            if ((string)btn.Content == "Bar")
            {
                btn.Content = "Area";
                chart.Data.Children[1].ChartType = ChartType.Bar;
            }
            else
            {
                btn.Content = "Bar";
                chart.Data.Children[1].ChartType = ChartType.Area;
            }
            chart.EndUpdate();
        }
    }

    public class Quotation
    {
        public DateTime Time
        {
            get;
            set;
        }

        public double Open
        {
            get;
            set;
        }

        public double Close
        {
            get;
            set;
        }

        public double High
        {
            get;
            set;
        }

        public double Low
        {
            get;
            set;
        }

        public double Volume
        {
            get;
            set;
        }
    }

    public class SampleFinancialData
    {
        static Random rnd = new Random();
        public static List<Quotation> Create(int npts)
        {
            List<Quotation> data = new List<Quotation>();
            DateTime dt = DateTime.Today.AddDays(0);

            for (int i = 0; i < npts; i++)
            {
                Quotation q = new Quotation();

                q.Time = dt.AddDays(i);

                if (i > 0)
                    q.Open = data[i - 1].Close;
                else
                    q.Open = 1000;

                q.High = q.Open + rnd.Next(50);
                q.Low = q.Open - rnd.Next(50);

                q.Close = rnd.Next((int)q.Low, (int)q.High);

                q.Volume = rnd.Next(0, 100);

                data.Add(q);
            }

            return data;
        }
    }
}
