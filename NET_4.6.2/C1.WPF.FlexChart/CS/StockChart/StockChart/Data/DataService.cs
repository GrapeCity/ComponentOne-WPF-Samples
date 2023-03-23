using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace StockChart
{

    class DataService
    {
        static Dictionary<string, string> _names = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        static Dictionary<string, QuoteData> _prices = new Dictionary<string, QuoteData>(StringComparer.OrdinalIgnoreCase);

        static DataService()
        {
        }

        private DataService()
        {

        }

        private static DataService _instance;
        public static DataService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataService();
                }
                return _instance;
            }
        }

        public Dictionary<string, string> SymbolNames
        {
            get
            {
                // load company names (once)
                if (_names.Count == 0)
                {
                    using (FileStream fs = new FileStream(System.Environment.CurrentDirectory + @"\Resources\symbolNames.txt", FileMode.Open, FileAccess.Read))
                    {
                        using (var sr = new StreamReader(fs))
                        {
                            for (var line = sr.ReadLine(); line != null; line = sr.ReadLine())
                            {
                                var parts = line.Split('\t');
                                if (parts.Length >= 2)
                                {
                                    var key = parts[0].Trim();
                                    var value = parts[1].Trim();
                                    if (key.Length > 0 && value.Length > 0)
                                    {
                                        _names[key] = value;
                                    }
                                }
                            }
                        }
                    }

                }
                return _names;
            }
        }

        public string GetSymbolName(string symbol)
        {
            string value = null;
            if (_names != null)
            {
                if (_names.TryGetValue(symbol, out value))
                {
                    return value + "(" + symbol.ToUpper() + ")";
                }
            }
            return value;
        }

        public QuoteData GetSymbolData(string symbol, Action<string> onWebError = null)
        {
            // try getting from cache
            QuoteData quoteCache;
            if (_prices.TryGetValue(symbol, out quoteCache))
            {
                return quoteCache;
            }
            QuoteData quoteData = new QuoteData();

            DateTime t = DateTime.Today;
            DateTime startDate = new DateTime(t.Year - 10, 01, 01);
            Stream dataStream = null;
            StreamWriter dataCacheStream = null;

            // check the file cache as well
            FileInfo fileInfo = new FileInfo(symbol + ".dataCache");
            if (fileInfo.Exists && fileInfo.LastWriteTime.Date == t)
            {
                try { dataStream = fileInfo.OpenRead(); }
                catch { dataStream = null; }
            }

            // not in cache, get now
            if (dataStream == null)
            {
                var fmt = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY_ADJUSTED&symbol={0}&apikey=IF6RVQ6S90CZZ7VJ&datatype=csv&outputsize=full";
                var url = string.Format(fmt, symbol);
                try
                {
                    var wc = new WebClient();
                    dataStream = wc.OpenRead(url);
                    dataCacheStream = fileInfo.CreateText();
                }
                catch { dataStream = null; }
            }

            if (dataStream == null)
            {
                string msg = "Data for \"" + symbol + "\" not available.";
                if (fileInfo.Exists)
                {
                    try
                    {
                        dataStream = fileInfo.OpenRead();
                        msg = "New Data for \"" + symbol + "\" not available.\r\n" +
                              "Using cached data from " +
                              fileInfo.LastWriteTime.Date.ToShortDateString();
                    }
                    catch
                    {
                        dataStream = null;
                    }
                }
                if (onWebError != null)
                    onWebError(msg);
                else if (dataStream == null)
                    throw new Exception(msg);
                if (dataStream == null) return quoteData;
            }

            try
            {
                using (var sr = new StreamReader(dataStream))
                {
                    // skip headers
                    if (dataCacheStream != null)
                    {
                        dataCacheStream.WriteLine(sr.ReadLine());
                        dataCacheStream.WriteLine(sr.ReadLine());
                    }
                    else
                    {
                        sr.ReadLine();
                        sr.ReadLine();
                    }

                    // read each line
                    for (var line = sr.ReadLine(); line != null; line = sr.ReadLine())
                    {
                        if (dataCacheStream != null) dataCacheStream.WriteLine(line);

                        // append date (field 0) and adjusted close price (field 6)
                        var items = line.Split(',');

                        {
                            Quote q = new Quote(quoteData.ReferenceValue)
                            {

                                date = DateTime.Parse(items[0], CultureInfo.InvariantCulture),
                                open = Convert.ToDouble(items[1], CultureInfo.InvariantCulture),
                                high = Convert.ToDouble(items[2], CultureInfo.InvariantCulture),
                                low = Convert.ToDouble(items[3], CultureInfo.InvariantCulture),
                                close = Convert.ToDouble(items[4], CultureInfo.InvariantCulture),
                                volume = Convert.ToDouble(items[5], CultureInfo.InvariantCulture),
                            };
                            if (q.date < startDate) break;
                            quoteData.Add(q);
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                if (onWebError != null)
                {
                    onWebError(ex.Message);
                }
                else
                {
                    throw ex;
                }
            }
            finally
            {
                if (dataCacheStream != null)
                    dataCacheStream.Close();
            }

            if (symbol != "SP")
                FillEvents(symbol, quoteData);

            quoteData.Reverse();
            _prices[symbol] = quoteData;

            return quoteData;
        }


        public void GetSymbolDataAsync(string symbol, Action<QuoteData> onCallback, Action<string> onWebError = null)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((oo) =>
            {
                var data = GetSymbolData(symbol, onWebError);
                if (onCallback != null)
                {
                    onCallback(data);
                }
            }));
        }

        private void FillEvents(string symbol, List<Quote> qs)
        {
            // not in cache, get now
            var fmt = "https://www.nasdaq.com/feed/rssoutbound?symbol={0}";
            var url = string.Format(fmt, symbol);

            // get content
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.Accept] = "application/xhtml+xml";
                wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64)";

                using (var stream = wc.OpenRead(url))
                {
                    var doc = new System.Xml.XmlDocument();
                    doc.Load(stream);

                    var items = doc.GetElementsByTagName("item");
                    int i = 0;
                    // read each line
                    foreach (System.Xml.XmlNode item in items)
                    {
                        var dt = DateTime.Parse(item["pubDate"].InnerText);
                        var text = item["title"].InnerText;

                        // var quote = qs.FirstOrDefault(q => (q.date - dt).Days == 0);
                        // note! the event day isn't correct
                        // we attach each new event to the previous day to spread them more evenly
                        var quote = qs[i++];

                        if (quote != null)
                        {
                            if (quote.events != null)
                            {
                                if (quote.events.Length > 0)
                                {
                                    quote.events += Environment.NewLine;
                                }
                                quote.events += text;
                            }
                            else
                            {
                                quote.events = text;
                            }
                        }
                    }

                }
            }
        }

        public QuoteRange GetSymbolYRange(IEnumerable<Quote> quoteData, double s, double e, string porperty = null)
        {
            QuoteRange qr = null;
            DateTime ds = DateTime.FromOADate(s);
            DateTime de = DateTime.FromOADate(e);
            IEnumerable<Quote> quoteCache = from p in quoteData where p.date >= ds && p.date <= de select p;

            if (quoteCache.Any())
            {
                qr = new QuoteRange();
                qr.PriceMin = quoteCache.Min((k) =>
                {
                    if (string.IsNullOrEmpty(porperty) || porperty.ToUpper() == "high,low,open,close".ToUpper())
                    {
                        return k.low;
                    }
                    else
                    {
                        return k.GetValue(porperty);
                    }
                });
                qr.PriceMax = quoteCache.Max((k) =>
                {
                    if (string.IsNullOrEmpty(porperty) || porperty.ToUpper() == "high,low,open,close".ToUpper())
                    {
                        return k.high;
                    }
                    else
                    {
                        return k.GetValue(porperty);
                    }
                });
                qr.VolumeMin = quoteCache.Min(k => k.volume);
                qr.VolumeMax = quoteCache.Max(k => k.volume);
            }
            return qr;
        }


    }
}
