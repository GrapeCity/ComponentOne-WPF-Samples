using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace StockAnalysis.Data
{
    public class QuoteEventArg : EventArgs
    {
        public QuoteEventArg(Object.Quote quote)
        {
            Quote = quote;
        }

        public Object.Quote Quote
        {
            get;
            set;
        }
    }
    public class QuotesEventArg: EventArgs
    {
        public QuotesEventArg(IEnumerable<Object.Quote> quotes)
        {
            Quotes = quotes;
        }

        public IEnumerable<Object.Quote> Quotes
        {
            get;
            set;
        }
    }

    public class DataService
    {
        public static IEnumerable<Object.Quote> GetQuotes(Dictionary<string, string> symbols)
        {
            Stack<Object.Quote> quotes = new Stack<Object.Quote>();
            foreach (var symbol in symbols)
            {
                quotes.Push(GetQuote(symbol));
            }
            return quotes;
        }

        public static void GetQuotesAsync(Dictionary<string, string> symbols, EventHandler<QuotesEventArg> callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((o) => {
                Stack<Object.Quote> quotes = new Stack<Object.Quote>();
                foreach (var symbol in symbols)
                {
                    GetQuoteAsync(symbol,
                        (sender, e) =>
                        {
                            quotes.Push(e.Quote);

                            var ordered = quotes.OrderBy(p => p != null ? p.Name : null);
                            callback(sender, new QuotesEventArg(ordered));
                        }
                        );
                }
            });
        }

        public static Object.Quote GetQuote(KeyValuePair<string, string> symbol)
        {
            string url = string.Format("http://www.nasdaq.com/symbol/{0}/historical", symbol.Key);
            string reqStr = "1y|true|" + symbol.Key.ToUpper();

            return new Object.Quote()
            {
                Symbol = symbol.Key,
                Name = symbol.Value,
                //Data = Request(url, reqStr, Encoding.ASCII),
                Data = Request(symbol.Key.ToLower(), Encoding.ASCII),
            };
        }


        public static void GetQuoteAsync(KeyValuePair<string, string> symbol, EventHandler<QuoteEventArg> callback)
        {
            System.Threading.ThreadPool.QueueUserWorkItem((o) => {
                var quote = GetQuote(symbol);
                callback(o, new QuoteEventArg(quote));
            });
        }

        //internal static IEnumerable<Object.QuoteData> Request(string url, string data, Encoding encoding)
        //{
        //    int retryCounter = 3;
        //    System.Net.HttpWebRequest request = null;
        //    System.Net.HttpWebResponse response = null;
        //    var bs = encoding.GetBytes(data);
        //    url = url.ToLower();

        //    RETRY:
        //    try
        //    {
        //        // Uncomment the code below if your environment is using proxy.
        //        // WebRequest.DefaultWebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
        //        request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                
        //        request.Method = "POST";
        //        request.ContentType = "application/json";
        //        request.ContentLength = bs.Length;
        //        using (Stream stream = request.GetRequestStream())
        //        {
        //            stream.Write(bs, 0, bs.Length);
        //            stream.Flush();
        //        }

        //        response = (System.Net.HttpWebResponse)request.GetResponse();
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);

        //        if (retryCounter > 0)
        //        {
        //            request.Abort();
        //            retryCounter--;
        //            goto RETRY;
        //        }
        //    }

        //    if ((response == null || response.StatusCode != HttpStatusCode.OK) && retryCounter > 0)
        //    {
        //        request.Abort();
        //        retryCounter--;
        //        goto RETRY;
        //    }

        //    Stack<Object.QuoteData> datas = new Stack<Object.QuoteData>();
        //    try
        //    {
        //        using (System.IO.Stream stream = response.GetResponseStream())
        //        {
        //            using (StreamReader sr = new StreamReader(stream, encoding))
        //            {
        //                // skip headers
        //                sr.ReadLine();
        //                sr.ReadLine();

        //                for (var line = sr.ReadLine(); line != null; line = sr.ReadLine())
        //                {
        //                    datas.Push(ParseLine(line));
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine(ex.Message);
        //        throw ex;
        //    }

        //    return datas;
        //}


        internal static IEnumerable<Object.QuoteData> Request( string symbolName, Encoding encoding)
        {

            var dirData = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + @"\Data";

            var filePath = dirData + @"\" + symbolName + ".csv";
            if (!File.Exists(filePath))
            {
                return null;
            }

            Stack<Object.QuoteData> datas = new Stack<Object.QuoteData>();

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader sr = new StreamReader(fs, encoding))
                    {
                        // skip headers
                        sr.ReadLine();
                        sr.ReadLine();

                        for (var line = sr.ReadLine(); line != null; line = sr.ReadLine())
                        {
                            datas.Push(ParseLine(line));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            
            return datas;
        }

        private static Object.QuoteData ParseLine(string line)
        {
            line = line.Replace("\"", string.Empty);

            var items = line.Split(',');
            Object.QuoteData data = new Object.QuoteData()
            {
                //"\"date\",\"close\",\"volume\",\"open\",\"high\",\"low\""
                Date = items[0].ToString(),
                Open = Convert.ToDouble(items[3]),
                High = Convert.ToDouble(items[4]),
                Low = Convert.ToDouble(items[5]),
                Close = Convert.ToDouble(items[1]),
                Volume = Convert.ToDouble(items[2]),
                LocalDate = Convert.ToDateTime(items[0])
            };

            return data;
        }

    }
}
