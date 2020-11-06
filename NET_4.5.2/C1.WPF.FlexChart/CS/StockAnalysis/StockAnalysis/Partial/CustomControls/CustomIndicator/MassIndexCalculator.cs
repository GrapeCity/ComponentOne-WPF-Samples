using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public class MassIndexCalculator
    {
        private static MassIndexCalculator _instance;
        public static MassIndexCalculator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MassIndexCalculator();
                }
                return _instance;
            }
        }

        public IEnumerable<MassIndexDataPoint> DataPoints
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

        public void Create()
        {
            Object.QuoteData[] quoteData = this.Source.ToArray();
            int len = quoteData.Length;


            MassIndexDataPoint[] massIndexDataPts = new MassIndexDataPoint[len];

            double ema = 0.0, ema2 = 0.0;
            for (int i = 0; i < 9; i++)
            {
                massIndexDataPts[i] = new MassIndexDataPoint(quoteData[i].LocalDate);
                ema += quoteData[i].High - quoteData[i].Low;
            }

            ema /= 9;
            ema2 = ema;

            for (int i = 9; i < 9 + 9 - 1; i++)
            {
                massIndexDataPts[i] = new MassIndexDataPoint(quoteData[i].LocalDate);
                ema = ema + 2.0 / (9 + 1) * (quoteData[i].High - quoteData[i].Low - ema);
                ema2 += ema;
            }

            ema2 /= 9;

            double emaRatio = 0.0;
            double[] emaRatios = new double[period];

            for (int i = 9 + 9 - 1; i < 9 + 9 - 1 + period; i++)
            {
                massIndexDataPts[i] = new MassIndexDataPoint(quoteData[i].LocalDate);
                ema = ema + 2.0 / (9 + 1) * (quoteData[i].High - quoteData[i].Low - ema);
                ema2 = ema2 + 2.0 / (9 + 1) * (ema - ema2);
                emaRatio += ema / ema2;
                emaRatios[i % period] = ema / ema2;
            }
            massIndexDataPts[9 + 9 - 2 + period] =
                new MassIndexDataPoint(quoteData[9 + 9 - 2 + period].LocalDate, emaRatio);

            for (int i = 9 + 9 + -1 + period; i < len; i++)
            {
                ema = ema + 2.0 / (9 + 1) * (quoteData[i].High - quoteData[i].Low - ema);
                ema2 = ema2 + 2.0 / (9 + 1) * (ema - ema2);
                emaRatio = ema / ema2;
                massIndexDataPts[i] = new MassIndexDataPoint(quoteData[i].LocalDate,
                    massIndexDataPts[i - 1].MassIndex - emaRatios[i % period] + emaRatio);
                emaRatios[i % period] = emaRatio;
            }

            for (int i = 0; i < massIndexDataPts.Length; i++)
            {
                massIndexDataPts[i].X = i;
            }

            this.DataPoints = massIndexDataPts;
        }
    }

    public class MassIndexDataPoint
    {
        public double X { get; set; }
        public DateTime LocalDate { get; set; }
        public double MassIndex { get; set; }

        public MassIndexDataPoint()
        {
            LocalDate = DateTime.MinValue;
            MassIndex = double.NaN;     // default to hole value
        }

        public MassIndexDataPoint(DateTime ldate)
        {
            LocalDate = ldate;
            MassIndex = double.NaN;     // default to hole value
        }

        public MassIndexDataPoint(DateTime ldate, double massIndex)
        {
            LocalDate = ldate;
            MassIndex = massIndex;
        }
    }
}
