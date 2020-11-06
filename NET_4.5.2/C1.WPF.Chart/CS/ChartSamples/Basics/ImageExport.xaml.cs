using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Win32;
using C1.WPF.C1Chart;
using C1.WPF.C1Chart.Extended;


namespace ChartSamples
{
    [System.ComponentModel.Description("Save chart as png/jpg image")]
    public partial class ImageExport : UserControl
    {
        string[] coef = new string[]
    {
      "AMTMNQQXUYGA",
      "CVQKGHQTPHTE",
      "FIRCDERRPVLD",
      "GIIETPIQRRUL",
      "GLXOESFTTPSV",
      "GXQSNSKEECTX",
    };
        Random rnd = new Random();

        public ImageExport()
        {
            InitializeComponent();

            CreateChart();
        }

        void CreateChart()
        {
            chart.BeginUpdate();
            chart.Data.Children.Clear();
            int cnt = 400;

            double[] x = new double[cnt];
            double[] y = new double[cnt];

            double[] c = StringToCoeff(coef[rnd.Next(coef.Length)]);

            for (int i = 1; i < cnt; i++)
            {
                Point pt = Iterate(x[i - 1], y[i - 1], c);
                x[i] = pt.X; y[i] = pt.Y;
            }

            chart.Data.Children.Add(
              new XYDataSeries()
              {
                  XValuesSource = x,
                  ValuesSource = y,
                  SymbolSize = new Size(5, 5)
              });
            chart.ChartType = ChartType.XYPlot;
            chart.EndUpdate();
        }

        Point Iterate(double x, double y, double[] c)
        {
            double x1 = c[0] + c[1] * x + c[2] * x * x + c[3] * x * y + c[4] * y + c[5] * y * y;
            double y1 = c[6] + c[7] * x + c[8] * x * x + c[9] * x * y + c[10] * y + c[11] * y * y;

            return new Point(x1, y1);
        }

        double[] StringToCoeff(string s)
        {
            int len = s.Length;
            double[] c = new double[len];

            for (int i = 0; i < len; i++)
            {
                c[i] = -1.2 + 0.1 * (s[i] - 'A');
            }

            return c;
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            CreateChart();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Jpeg files (*.jpg)|*.jpg|Png files (*.png)|*.png";

            if (sfd.ShowDialog() == true)
            {
                using (var stream = sfd.OpenFile())
                {
                    if (sfd.SafeFileName.EndsWith(".jpg"))
                        chart.SaveImage(stream, ImageFormat.Jpeg);
                    else
                        chart.SaveImage(stream, ImageFormat.Png);
                }
            }
        }
    }
}
