using C1.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public abstract class MassIndexSeriesBase : C1.WPF.Chart.Series
    {
        public int Period
        {
            get { return (int)GetValue(PeriodProperty); }
            set { SetValue(PeriodProperty, value); }
        }

        public static readonly DependencyProperty PeriodProperty =
            DependencyProperty.Register("Period", typeof(int), typeof(MassIndexSeriesBase),
            new PropertyMetadata(14, new PropertyChangedCallback(
                (o, e) =>
                {
                    MassIndexSeriesBase series = o as MassIndexSeriesBase;
                    if (series != null && series.Calculator != null)
                    {
                        series.Calculator.Period = (int)e.NewValue;
                    }
                }
                )));

        public MassIndexCalculator Calculator
        {
            get;
            set;
        }

        public override double[] GetValues(int dim)
        {
            return GetMassIndexValues(dim);
        }

        public abstract double[] GetMassIndexValues(int dim);
    }

    public class MassIndexSeries : MassIndexSeriesBase
    {
        public override double[] GetMassIndexValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints.Skip(this.Period);
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.MassIndex).ToArray();
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
