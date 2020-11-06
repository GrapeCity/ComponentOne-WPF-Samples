using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.WPF.C1Chart;
using System.Windows;


namespace SeriesAxis
{
    class SampleData
    {
        static Random rnd = new Random();
        public static XYDataSeries CreateSeries(string label, int npts)
        {
            return CreateSeries(label, npts, 0, 1);
        }
        public static XYDataSeries CreateSeries(string label, int npts, double min, double max, bool random = true)
        {
            var x = new double[npts];
            var y = new double[npts];

            var d = max - min;
            var phase = Math.PI * 2 * rnd.NextDouble();
            var amp = rnd.NextDouble();

            for (int i = 0; i < npts; i++)
            {
                x[i] = i;

                if (random)
                    y[i] = min + d * rnd.NextDouble();
                else
                    y[i] = min + amp * d * Math.Sin(0.1 * i + phase);
            }

            return new XYDataSeries() { SymbolSize = new Size(6, 6), Label = label, XValuesSource = x, ValuesSource = y };
        }
    }
}
