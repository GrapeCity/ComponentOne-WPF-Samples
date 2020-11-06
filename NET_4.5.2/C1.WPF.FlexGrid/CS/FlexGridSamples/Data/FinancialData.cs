using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace MainTestApplication
{
    /// <summary>
    /// Based on this: http://en.wikipedia.org/wiki/Market_data
    /// </summary>
    public class FinancialData : INotifyPropertyChanged
    {
        const int HISTORY_SIZE = 5;
        List<decimal> _askHistory = new List<decimal>();
        List<decimal> _bidHistory = new List<decimal>();
        List<decimal> _saleHistory = new List<decimal>();

        [Display(Name = "Symbol")]
        public string Symbol
        {
            get { return (string)GetProp("Symbol"); }
            set { SetProp("Symbol", value); }
        }

        [Display(Name = "Name")]
        public string Name
        {
            get { return (string)GetProp("Name"); }
            set { SetProp("Name", value); }
        }

        [Display(Name = "Bid")]
        public decimal Bid
        {
            get { return (decimal)GetProp("Bid"); }
            set 
            {
                AddHistory(_bidHistory, value);
                SetProp("Bid", value);
            }
        }

        [Display(Name = "Ask")]
        public decimal Ask
        {
            get { return (decimal)GetProp("Ask"); }
            set 
            {
                AddHistory(_askHistory, value);
                SetProp("Ask", value);
            }
        }

        [Display(Name = "LastSale")]
        public decimal LastSale
        {
            get { return (decimal)GetProp("LastSale"); }
            set 
            {
                AddHistory(_saleHistory, value);
                SetProp("LastSale", value);
            }
        }

        [Display(Name = "BidSize")]
        public int BidSize
        {
            get { return (int)GetProp("BidSize"); }
            set { SetProp("BidSize", value); }
        }

        [Display(Name = "AskSize")]
        public int AskSize
        {
            get { return (int)GetProp("AskSize"); }
            set { SetProp("AskSize", value); }
        }

        [Display(Name = "LastSize")]
        public int LastSize
        {
            get { return (int)GetProp("LastSize"); }
            set { SetProp("LastSize", value); }
        }

        [Display(Name = "Volume")]
        public int Volume
        {
            get { return (int)GetProp("Volume"); }
            set { SetProp("Volume", value); }
        }

        [Display(Name = "QuoteTime")]
        public DateTime QuoteTime
        {
            get { return (DateTime)GetProp("QuoteTime"); }
            set { SetProp("QuoteTime", value); }
        }

        [Display(Name = "TradeTime")]
        public DateTime TradeTime
        {
            get { return (DateTime)GetProp("TradeTime"); }
            set { SetProp("TradeTime", value); }
        }

        public List<decimal> GetHistory(string propName)
        {
            switch (propName)
            {
                case "Ask":
                    return _askHistory;
                case "Bid":
                    return _bidHistory;
                case "LastSale":
                    return _saleHistory;
            }
            return null;
        }
        void AddHistory(List<decimal> list, decimal value)
        {
            while (list.Count >= HISTORY_SIZE)
            {
                list.RemoveAt(0);
            }
            while (list.Count < HISTORY_SIZE)
            {
                list.Add(value);
            }
        }

        Dictionary<string, object> _propBag = new Dictionary<string, object>();
        object GetProp(string propName)
        {
            object value = null;
            _propBag.TryGetValue(propName, out value);
            return value;
        }
        void SetProp(string propName, object value)
        {
            if (!_propBag.ContainsKey(propName) || GetProp(propName) != value)
            {
                _propBag[propName] = value;
                OnPropertyChanged(new PropertyChangedEventArgs(propName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        // get a default list of financial items
        public static FinancialDataList GetFinancialData()
        {
            var list = new FinancialDataList();
            var rnd = new Random(0);
            var asm = Assembly.GetExecutingAssembly();
            foreach (string resName in asm.GetManifestResourceNames())
            {
                if (resName.EndsWith("data.zip"))
                {
                    var zip = new C1.C1Zip.C1ZipFile(asm.GetManifestResourceStream(resName));
                    using (var sr = new StreamReader(zip.Entries["StockSymbols.txt"].OpenReader()))
                    {
                        while (!sr.EndOfStream)
                        {
                            var sn = sr.ReadLine().Split('\t');
                            if (sn.Length > 1 && sn[0].Trim().Length > 0)
                            {
                                var data = new FinancialData();
                                data.Symbol = sn[0];
                                data.Name = sn[1];
                                data.Bid = rnd.Next(1, 1000);
                                data.Ask = data.Bid + (decimal)rnd.NextDouble();
                                data.LastSale = data.Bid;
                                data.BidSize = rnd.Next(10, 500);
                                data.AskSize = rnd.Next(10, 500);
                                data.LastSize = rnd.Next(10, 500);
                                data.Volume = rnd.Next(10000, 20000);
                                data.QuoteTime = DateTime.Now;
                                data.TradeTime = DateTime.Now;
                                list.Add(data);
                            }
                        }
                    }
                    list.AutoUpdate = true;
                    return list;
                }
            }
            throw new Exception("Can't find 'data.zip' embedded resource.");
        }
    }
    public class FinancialDataList : List<FinancialData>
    {
        // fields
        C1.Util.Timer _timer;
        Random _rnd = new Random(0);

        // object model
        public int UpdateInterval 
        { 
            get { return (int)_timer.Interval.TotalMilliseconds; }
            set { _timer.Interval = TimeSpan.FromMilliseconds(value); }
        }
        public int BatchSize { get; set; }
        public bool AutoUpdate
        {
            get { return _timer.IsEnabled; }
            set { _timer.IsEnabled = value; }
        }

        // ctor
        public FinancialDataList()
        {
            _timer = new C1.Util.Timer();
            _timer.Tick += _timer_Tick;
            UpdateInterval = 100;
            BatchSize = 100;
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if (this.Count > 0)
            {
                for (int i = 0; i < BatchSize; i++)
                {
                    int index = _rnd.Next() % this.Count;
                    var data = this[index];
                    data.Bid = data.Bid * (decimal)(1 + (_rnd.NextDouble() * .11 - .05));
                    data.Ask = data.Ask * (decimal)(1 + (_rnd.NextDouble() * .11 - .05));
                    data.BidSize = _rnd.Next(10, 1000);
                    data.AskSize = _rnd.Next(10, 1000);
                    var sale = (data.Ask + data.Bid) / 2;
                    data.LastSale = sale;
                    data.LastSize = (data.AskSize + data.BidSize) / 2;
                    data.QuoteTime = DateTime.Now;
                    data.TradeTime = DateTime.Now.AddSeconds(-_rnd.Next(0, 60));
                }
            }
        }
    }
}
