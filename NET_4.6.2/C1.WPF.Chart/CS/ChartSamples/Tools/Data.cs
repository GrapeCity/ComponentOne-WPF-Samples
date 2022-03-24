using System;
using System.Collections;

using C1.WPF.C1Chart;

namespace ChartSamples
{
  interface IDataSeries
  {
    DataSeries Create(int npts);
    void Update(DataSeries ds, int ntps);
  }

  interface IChart
  {
    void Create(C1Chart chart, int nser, int npts);
  }

  /// <summary>
  /// Creates simple data series
  /// </summary>
  class DS : IDataSeries
  {
    double _min = 0;
    double _max = 10000;

    protected static Random rnd = new Random();

    protected double[] CreatDoubleArray(int npts, double min, double max)
    {
      double[] vals = new double[npts];

      for (int i = 0; i < npts; i++)
        vals[i] = min + (max - min) * rnd.NextDouble();

      return vals;
    }

    public virtual DataSeries Create(int n)
    {
      DataSeries ds = new DataSeries();
      Update(ds, n);

      return ds;
    }

    public virtual void Update(DataSeries ds, int n)
    {
      double min = _min + (_max - _min) * rnd.NextDouble();
      double max = _min + (_max - _min) * rnd.NextDouble();

      ds.ValuesSource = CreatDoubleArray(n, Math.Min(_min, _max), Math.Max(_min, _max));
    }
  }

  // creates x-time/y-value data series
  class DSXYTime : DS
  {
    public override DataSeries Create(int n)
    {
      XYDataSeries ds = new XYDataSeries();

      Update(ds, n);

      return ds;
    }

    public override void Update(DataSeries ads, int n)
    {
      XYDataSeries ds = ads as XYDataSeries;

      double[] vals = new double[n];
      DateTime[] x = new DateTime[n];
      DateTime dt = DateTime.Today;

      for (int i = 0; i < n; i++)
      {
        x[i] = dt.AddDays(i);
        vals[i] = rnd.Next(200);
      }

      ds.XValuesSource = x;
      ds.ValuesSource = vals;
    }
  }

  /// <summary>
  /// creates financial data series
  /// </summary>
  class DSHLOC : DS
  {
    public override DataSeries Create(int n)
    {
      HighLowOpenCloseSeries ds = new HighLowOpenCloseSeries();
      Update(ds, n);

      return ds;
    }

    public override void Update(DataSeries ads, int n)
    {
      HighLowOpenCloseSeries ds = (HighLowOpenCloseSeries)ads;

      DateTime[] x = new DateTime[n];
      double[] hi = new double[n];
      double[] lo = new double[n];
      double[] op = new double[n];
      double[] cl = new double[n];

      DateTime dt = DateTime.Today;

      for (int i = 0; i < n; i++)
      {
        x[i] = dt.AddDays(i);

        if (i > 0)
          op[i] = cl[i - 1];
        else
          op[i] = 1000;

        hi[i] = op[i] + rnd.Next(50);
        lo[i] = op[i] - rnd.Next(50);

        cl[i] = rnd.Next((int)lo[i], (int)hi[i]);
      }

      ds.XValuesSource = x;
      ds.ValuesSource = hi;
      ds.HighValuesSource = hi;
      ds.LowValuesSource = lo;
      ds.OpenValuesSource = op;
      ds.CloseValuesSource = cl;
    }
  }

  // generates chart with sample data
  class ChartGenerator //: IChart
  {
    protected void Reset(C1Chart chart)
    {
      chart.Data.Renderer = new Renderer2D();
    }

    public void Create(C1Chart chart, IDataSeries ds, int nser, int npts)
    {
      IDataSeries[] dss = new IDataSeries[nser];
      for (int i = 0; i < nser; i++)
        dss[i] = ds;

      Create(chart, dss, npts);
    }

    public virtual void Create(C1Chart chart, IDataSeries[] dss, int npts)
    {
      Reset(chart);

      IEnumerable x = null;

      for (int i = 0; i < dss.Length; i++)
      {
        DataSeries ds = null;
        if (i < chart.Data.Children.Count)
        {
          ds = (DataSeries)chart.Data.Children[i];
          dss[i].Update(ds, npts);
        }
        else
          ds = dss[i].Create(npts);

        XYDataSeries xds = ds as XYDataSeries;
        if (xds != null)
        {
          if (x == null)
            x = xds.XValuesSource;
          else
            xds.XValuesSource = x;
        }

        if( !chart.Data.Children.Contains(ds))
          chart.Data.Children.Add(ds);
      }
    }
  }
}
