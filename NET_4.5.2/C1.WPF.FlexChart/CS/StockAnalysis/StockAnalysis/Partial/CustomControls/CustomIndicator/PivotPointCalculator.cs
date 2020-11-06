using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public class PivotPointCalculator
    {
        private static PivotPointCalculator _instance;
        public static PivotPointCalculator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PivotPointCalculator();
                }
                return _instance;
            }
        }

        public IEnumerable<PivotPointDataPoint> DataPoints
        {
            get;
            private set;
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

        public PivotPointType PivotType
        {
            get;
            set;
        }
        public void Create()
        {
            var isStandard = this.PivotType == PivotPointType.Standard;
            Object.QuoteData[] quoteData = this.Source.ToArray();
            int len = quoteData.Length;
            

            List<PivotPointDataPoint> pdatas = new List<PivotPointDataPoint>();
            

            DateTime dataDate = DateTime.MinValue;
            double high = double.MinValue, low = double.MaxValue, close = -1, pivot = -1;

            int p = 0;
            while (quoteData[p].LocalDate.Day > 7 && high == double.MinValue)
                p++; // skip to first complete month

            int month = quoteData[p].LocalDate.Month;
            high = quoteData[p].High;
            low = quoteData[p].Low;
            double delta = high - low;
            p++;

            while (p < quoteData.Length)
            {
                DateTime qdate = quoteData[p].LocalDate;
                if (qdate.Month == month)
                {
                    if (quoteData[p].High > high) high = quoteData[p].High;
                    if (quoteData[p].Low < low) low = quoteData[p].Low;
                }
                else
                {
                    month = qdate.Month;
                    close = quoteData[p - 1].Close;
                    pivot = (high + low + close) / 3;
                    delta = high - low;
                    pdatas.Add(new PivotPointDataPoint()
                    {
                        EffectiveDate = new DateTime(qdate.Year, qdate.Month, 1),
                        StartingIndex = p,
                        Pivot = pivot,
                        S1 = isStandard ? pivot * 2 - high : pivot - (0.382 * delta),
                        S2 = isStandard ? pivot - delta : pivot - (0.618 * delta),
                        S3 = isStandard ? low - 2 * (high - pivot) : pivot - (1.0 * delta),
                        R1 = isStandard ? pivot * 2 - low : pivot + (0.382 * delta),
                        R2 = isStandard ? pivot + delta : pivot + (0.618 * delta),
                        R3 = isStandard ? high + 2 * (pivot - low) : pivot + (1.0 * delta),
                    });

                    high = quoteData[p].High;
                    low = quoteData[p].Low;
                }
                p++;
            }

            if (quoteData[--p].LocalDate.Month == month)
            {
                //create an extra PivotPoint element to match the last date
                PivotPointDataPoint pp = pdatas[pdatas.Count - 1];
                pdatas.Add(new PivotPointDataPoint()
                {
                    EffectiveDate = quoteData[p].LocalDate,
                    StartingIndex = p,
                    Pivot = pp.Pivot,
                    S1 = pp.S1,
                    S2 = pp.S2,
                    S3 = pp.S3,
                    R1 = pp.R1,
                    R2 = pp.R2,
                    R3 = pp.R3,
                });
            }
            this.DataPoints = pdatas;
        }
    }

    public class PivotPointDataPoint
    {
        public DateTime EffectiveDate { get; set; }
        public int StartingIndex { get; set; }
        public double Pivot { get; set; }
        public double R1 { get; set; }
        public double R2 { get; set; }
        public double R3 { get; set; }
        public double S1 { get; set; }
        public double S2 { get; set; }
        public double S3 { get; set; }
    }
}
