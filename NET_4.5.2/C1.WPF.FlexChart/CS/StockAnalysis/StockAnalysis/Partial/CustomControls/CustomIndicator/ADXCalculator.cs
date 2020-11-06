using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public class ADXCalculator
    {
        private static ADXCalculator _instance;
        public static ADXCalculator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ADXCalculator();
                }
                return _instance;
            }
        }

        public IEnumerable<ADXDataPoint> DataPoints
        {
            get;
            private set;
        }

        private int period = 14;
        public int Period
        {
            get { return period; }
            set
            {
                if (period != value && value >= 1)
                {
                    period = value;
                    this.Create();
                }
            }
        }
        

        private IEnumerable<Object.QuoteData> source;
        public IEnumerable<Object.QuoteData> Source
        {
            get
            {
                return this.source;
            }
            set
            {
                if (this.source != value)
                {
                    this.source = value;

                    Create();
                }
            }
        }

        public void Create()
        {
            var dps = new List<ADXDataPoint>();


            Object.QuoteData[] quoteData = this.Source.ToArray();

            // calculate the DIP-Average, DIN-Average and ADX.
            int dataLen = quoteData.Length;

            double pfactor = (double)(period - 1) / period;

            double tr;
            double dmp;
            double dmn;
            double dx = 0.0;
            double dmpA = 0.0;
            double dmnA = 0.0;
            double trA = 0.0;

            double lastAdx = 0;
            for (int i = 0; i < dataLen; i++)
            {
                double dip = 0, din = 0, adx = 0;

                if (i >= period)
                {

                    double h = quoteData[i].High, l = quoteData[i].Low;
                    double ph = quoteData[i - 1].High, pl = quoteData[i - 1].Low, pc = quoteData[i - 1].Close;

                    tr = Math.Max(h - l, Math.Max(Math.Abs(h - pc), Math.Abs(l - pc)));

                    if ((h - ph) >= (pl - l))
                    {
                        dmp = ((h - ph) > 0.0) ? (h - ph) : 0.0;
                        dmn = 0.0;
                    }
                    else
                    {
                        dmp = 0.0;
                        dmn = ((pl - l) > 0.0) ? (pl - l) : 0.0;
                    }

                    if (i <= period)
                    {
                        trA += tr;
                        dmpA += dmp;
                        dmnA += dmn;
                    }
                    else
                    {
                        trA = trA * pfactor + tr;
                        dmpA = dmpA * pfactor + dmp;
                        dmnA = dmnA * pfactor + dmn;
                    }
                    

                    dip = dmpA / trA * 100;
                    din = dmnA / trA * 100;

                    dx = Math.Abs(dip - din) / (dip + din) * 100;

                    //if (i <= period + period - 1)     // (i <= period + period - 1)
                    //    adx[0] += dx / period;

                    if (i > period + period - 1)          // (i > period + period - 1)
                    {
                        adx = (lastAdx * (period - 1) + dx) / period;
                    }
                }
                
                dps.Add(new ADXDataPoint()
                {
                    X = i,
                    DIP = dip,
                    DIN = din,
                    ADX = adx,
                });

                lastAdx = adx;
            }

            this.DataPoints = dps;
        }
    }

    public class ADXDataPoint
    {
        public double X { get; set; }
        public double DIP { get; set; }
        public double DIN { get; set; }
        public double ADX { get; set; }
    }

}
