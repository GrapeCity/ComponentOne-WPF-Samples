using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockAnalysis.Object;

namespace StockAnalysis.Partial.CustomControls.CustomIndicator
{
    public abstract class PivotPointSeriesBase : C1.WPF.Chart.Series
    {
        public PivotPointCalculator Calculator
        {
            get;
            set;
        }

        public override double[] GetValues(int dim)
        {
            return GetPivotPointValues(dim);
        }

        public abstract double[] GetPivotPointValues(int dim);

    }

    public class PivotPointSeries : PivotPointSeriesBase
    {
        public override double[] GetPivotPointValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints;
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.Pivot).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => (double)(p.StartingIndex)).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
        
    }
    public class R1Series : PivotPointSeriesBase
    {
        public override double[] GetPivotPointValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints;
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.R1).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => (double)(p.StartingIndex)).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
    public class R2Series : PivotPointSeriesBase
    {
        public override double[] GetPivotPointValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints;
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.R2).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => (double)(p.StartingIndex)).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
    public class R3Series : PivotPointSeriesBase
    {
        public override double[] GetPivotPointValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints;
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.R3).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => (double)(p.StartingIndex)).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }

    public class S1Series : PivotPointSeriesBase
    {
        public override double[] GetPivotPointValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints;
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.S1).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => (double)(p.StartingIndex)).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
    public class S2Series : PivotPointSeriesBase
    {
        public override double[] GetPivotPointValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints;
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.S2).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => (double)(p.StartingIndex)).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
    public class S3Series : PivotPointSeriesBase
    {
        public override double[] GetPivotPointValues(int dim)
        {
            double[] result = null;
            if (this.Calculator != null && this.Calculator != null)
            {
                var source = Calculator.DataPoints;
                if (source == null)
                {
                    return null;
                }
                switch (dim)
                {
                    case 0:
                        result = source.Select(p => p.S3).ToArray();
                        break;
                    case 1:
                        result = source.Select(p => (double)(p.StartingIndex)).ToArray();
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
