using C1.WPF.Chart;

namespace DataManipulation.Business.FunctionSeries
{
    public class FunctionSeries : Series
    {
        private double[] _xVals;
        private double[] _yVals;
        private double _min;
        private double _max;
        private int _sampleCount;
        /// <summary>
        /// Initializes a new instance of the FunctionSeries class.
        /// </summary>
        public FunctionSeries()
        {
            this._min = 0;
            this._max = 1;
            this.ChartType = C1.Chart.ChartType.Line;
        }
        /// <summary>
        /// Gets or sets the minimum value of the parameter for calculating a function. 
        /// </summary>
        public double Min
        {
            get
            {
                return this._min;
            }
            set
            {
                this._min = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the maximum value of the parameter for calculating a function.
        /// </summary>
        public double Max
        {
            get
            {
                return this._max;
            }
            set
            {
                this._max = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets the sample count for function calculation.
        /// </summary>
        public int SampleCount
        {
            get { return _sampleCount; }
            set { _sampleCount = value; }
        }

        /// <summary>
        /// Gets the data values.
        /// </summary>
        /// <param name="dim"></param>
        /// <returns></returns>
        public override double[] GetValues(int dim)
        {

            if (_xVals == null || _yVals == null)
            {
                this.CalculateValues();
            }
            if (dim == 0)
            {
                return _yVals;
            }
            if (dim == 1)
            {
                return _xVals;
            }
            return null;
        }
        private void CalculateValues()
        {
            double[] x = new double[_sampleCount];
            double[] y = new double[_sampleCount];
            double delta = (this.Max - this.Min) / (_sampleCount - 1);
            double t;

            for (int i = 0; i < _sampleCount; i++)
            {
                t = i == _sampleCount ? this.Max : this.Min + delta * i;
                x[i] = this.CalculateX(t);
                y[i] = this.CalculateY(t);
            }
            _xVals = x;
            _yVals = y;
        }

        internal virtual double CalculateY(double t)
        {
            return 0;
        }

        internal virtual double CalculateX(double t)
        {
            return 0;
        }
    }
    /// <summary>
    /// Represents a Y function series 
    /// The YFunctionSeries plots a function defined by formulas of type y=f(x), specified using the function property.
    /// </summary>
    public class YFunctionSeries : FunctionSeries
    {
        private System.Func<double, double> _function;
        /// <summary>
        /// Gets or sets the function used to calculate Y value. 
        /// </summary>
        public System.Func<double, double> Function
        {
            get
            {
                return this._function;
            }
            set
            {
                this._function = value;
                this.Invalidate();
            }
        }
        internal override double CalculateX(double t)
        {
            return t;
        }
        internal override double CalculateY(double t)
        {
            if (_function != null)
            {
                return this._function.Invoke(t);
            }
            return base.CalculateY(t);
        }
    }
    public class ParametricFunctionSeries : FunctionSeries
    {
        private System.Func<double, double> _xFunction;
        private System.Func<double, double> _yFunction;
        /// <summary>
        /// Gets or sets the function used to calculate X value. 
        /// </summary>
        public System.Func<double, double> XFunction
        {
            get
            {
                return this._xFunction;
            }
            set
            {
                this._xFunction = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// Gets or sets the function used to calculate Y value. 
        /// </summary>
        public System.Func<double, double> YFunction
        {
            get
            {
                return this._yFunction;
            }
            set
            {
                this._yFunction = value;
                this.Invalidate();
            }
        }
        internal override double CalculateX(double t)
        {
            return _xFunction.Invoke(t);
        }
        internal override double CalculateY(double t)
        {
            return _yFunction.Invoke(t);
        }
    }
}
