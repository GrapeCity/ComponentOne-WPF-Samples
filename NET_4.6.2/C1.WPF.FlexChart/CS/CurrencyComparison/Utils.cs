using C1.Chart;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace CurrencyComparison
{
    class Utils
    {
        public static bool IsVisible(ChartSeries series)
        {
            return (series.Visibility == SeriesVisibility.Plot || series.Visibility == SeriesVisibility.Visible);
        }

        public static string Read(string fileName)
        {
            string text;
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/" + fileName, UriKind.Relative));
            var stream = resource.Stream;
            if (stream != null)
            {
                using (var reader = new StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }
            }
            else
            {
                var filePath = string.Format("../../{0}", fileName);
                text = File.ReadAllText(filePath);
            }
            return text;
        }
    }
}
