using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public abstract class ADXSeriesBase: C1.WPF.Chart.Finance.FinancialSeries
    {
        public int Period
        {
            get { return (int)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register("Period", typeof(int), typeof(ADXSeriesBase),
            new PropertyMetadata(14, new PropertyChangedCallback(
                (o, e) =>
                {
                    ADXSeriesBase adx = o as ADXSeriesBase;
                    if (adx != null && adx.Calculator != null)
                    {
                        adx.Calculator.Period = (int)e.NewValue;
                    }
                }
                )));

        public ADXCalculator Calculator
        {
            get;
            set;
        }

        public override double[] GetValues(int dim)
        {
            return GetADXValues(dim);
        }

        public abstract double[] GetADXValues(int dim);
    }
    public class DIPSeries : ADXSeriesBase
    {
        public override double[] GetADXValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints.Skip(this.Period);
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.DIP).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => p.X).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }


    public class DINSeries : ADXSeriesBase
    {
        public override double[] GetADXValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints.Skip(this.Period);
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.DIN).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => p.X).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
    public class ADXSeries : ADXSeriesBase
    {
        public override double[] GetADXValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints.Skip(this.Period * 2 - 1);
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.ADX).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => p.X).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }



}
