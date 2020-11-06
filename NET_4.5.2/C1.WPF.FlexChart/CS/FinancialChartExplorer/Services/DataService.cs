using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace FinancialChartExplorer
{
    public class DataService
    {
        List<Company> _companies = new List<Company>();
        Dictionary<string, List<Quote>> _cache = new Dictionary<string, List<Quote>>();

        private DataService()
        {
            _companies.Add(new Company() { Symbol = "box", Name = "Box Inc" });
            _companies.Add(new Company() { Symbol = "fb", Name = "Facebook" });
        }

        public List<Company> GetCompanies()
        {
            return _companies;
        }

        public List<T> GetData<T>(string symbol)
        {
            string path = string.Format("FinancialChartExplorer.Resources.{0}.json", symbol);
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            var ser = new DataContractJsonSerializer(typeof(T[]));
            var data = (T[])ser.ReadObject(stream);

            return data.ToList();
        }

        public List<Quote> GetSymbolData(string symbol)
        {
            if (!_cache.Keys.Contains(symbol))
            {
                var data = GetData<Quote>(symbol);
                _cache.Add(symbol, data);
            }

            return _cache[symbol];
        }

        public Range FindRenderedYRange(List<Quote> data, double xmin, double xmax)
        {
            Quote item;
            double ymin = double.NaN;
            double ymax = double.NaN;

            for (int i = 0; i < data.Count; i ++)
            {
                item = data[i];
                if (xmin > i || i > xmax)
                {
                    continue;
                }
                if (double.IsNaN(ymax) || item.High > ymax)
                {
                    ymax = item.High;
                }
                if (double.IsNaN(ymin) || item.Low < ymin)
                {
                    ymin = item.Low;
                }
            }

            return new Range() { Min = ymin, Max = ymax };
        }

        public Range FindApproxRange(double min, double max, double percent)
        {
            var range = new Range();
            range.Max = max;
            range.Min = (max - min) * (1 - percent) + min;
            return range;
        }

        static DataService _ds;
        public static DataService GetService()
        {
            if (_ds == null)
                _ds = new DataService();
            return _ds;
        }
    }
}
