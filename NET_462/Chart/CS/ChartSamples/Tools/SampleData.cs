using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#if (SILVERLIGHT)
using C1.Silverlight.Chart;
#else
using C1.WPF.C1Chart;
#endif

namespace ChartSamples
{
  /// <summary>
  /// Sample data for different chart types
  /// </summary>
  class SampleData
  {
    EventHandler _dataSeriesLoaded;
    ChartData _defaultData, _finData, _bubbleData, _ganttData, _polarData, _polygonData;

    public ChartData GetData(ChartType ct)
    {
      if (ct == ChartType.Bubble)
        return BubbleData;
      else if (ct == ChartType.Candle || ct == ChartType.HighLowOpenClose)
        return FinancialData;
      else if (ct == ChartType.Gantt)
        return GanttData;
      else if (ct.ToString().StartsWith("Polar"))
        return PolarData;
      else if (ct.ToString().StartsWith("Polygon"))
        return PolygonData;
      else
        return DefaultData;
    }

    public SampleData()
    {
    }

    public SampleData(EventHandler dataSeriesLoaded)
    {
      _dataSeriesLoaded = dataSeriesLoaded;
    }

    /// <summary>
    /// Default data is appropriate for the most chart types.
    /// </summary>
    public ChartData DefaultData
    {
      get
      {
        if (_defaultData == null)
        {
          _defaultData = new ChartData();

          DataSeries ds1 = new DataSeries() { Label = "s1" };
          ds1.ValuesSource = new double[] { 3, 2, 7, 4, 8 };
          _defaultData.Children.Add(ds1);

          DataSeries ds2 = new DataSeries() { Label = "s2" };
          ds2.ValuesSource = new double[] { 1, 2, 3, 4, 6 };
          _defaultData.Children.Add(ds2);

          DataSeries ds3 = new DataSeries() { Label = "s3" };
          ds3.ValuesSource = new double[] { 0, 1, 6, 2, 3 };
          _defaultData.Children.Add(ds3);

          if (_dataSeriesLoaded != null)
          {
            foreach (DataSeries ds in _defaultData.Children)
              ds.PlotElementLoaded += new EventHandler(_dataSeriesLoaded);
          }
        }

        return _defaultData;
      }
    }

    /// <summary>
    /// Data for financial charts(HighLowOpenClose and Candle).
    /// </summary>
    public ChartData FinancialData
    {
      get
      {
        if (_finData == null)
        {
          _finData = new ChartData();

          HighLowOpenCloseSeries ds = new HighLowOpenCloseSeries() { Label = "s1" };
          ds.SymbolStrokeThickness = 3; ds.SymbolSize = new Size(20, 20);
          ds.XValuesSource = new DateTime[] { 
            new DateTime(2008,10,1), new DateTime(2008,10,2), new DateTime(2008,10,3),
            new DateTime(2008,10,6), new DateTime(2008,10,7), new DateTime(2008,10,8) 
          };
          ds.OpenValuesSource = new double[] { 100, 102, 104, 100, 107, 102 };
          ds.CloseValuesSource = new double[] { 102, 104, 100, 107, 102, 100 };
          ds.HighValuesSource = new double[] { 102, 105, 105, 108, 109, 105 };
          ds.LowValuesSource = new double[] { 99, 95, 95, 100, 96, 99, 98 };
          if (_dataSeriesLoaded != null)
            ds.PlotElementLoaded += new EventHandler(_dataSeriesLoaded);
          _finData.Children.Add(ds);
        }
        return _finData;
      }
    }

    /// <summary>
    /// Data for Bubble chart.
    /// </summary>
    public ChartData BubbleData
    {
      get
      {
        if (_bubbleData == null)
        {
          _bubbleData = new ChartData();

          BubbleSeries ds1 = new BubbleSeries() { Label = "s1" };
          ds1.ValuesSource = new double[] { 3, 4, 7, 5, 8 };
          ds1.SizeValuesSource = new double[] { 1, 5, 7, 4, 9 };
          _bubbleData.Children.Add(ds1);

          BubbleSeries ds2 = new BubbleSeries() { Label = "s2" };
          ds2.ValuesSource = new double[] { 1, 2, 3, 4, 6 };
          ds2.SizeValuesSource = new double[] { 10, 3, 6, 8, 9 };
          _bubbleData.Children.Add(ds2);

          if (_dataSeriesLoaded != null)
          {
            foreach (DataSeries ds in _bubbleData.Children)
              ds.PlotElementLoaded += new EventHandler(_dataSeriesLoaded);
          }
        }

        return _bubbleData;
      }
    }

    /// <summary>
    /// Data for Gantt chart.
    /// </summary>
    public ChartData GanttData
    {
      get
      {
        if (_ganttData == null)
        {
          _ganttData = new ChartData();
          _ganttData.ItemNames = new string[] { "Task1", "Task2", "Task3", "Task4" };

          HighLowSeries ds1 = new HighLowSeries() { Label = "Task1" };
          ds1.XValuesSource = new double[] { 0 };
          ds1.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 1) };
          ds1.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 5) };
          _ganttData.Children.Add(ds1);

          HighLowSeries ds2 = new HighLowSeries() { Label = "Task2" };
          ds2.XValuesSource = new double[] { 1 };
          ds2.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 3) };
          ds2.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 4) };
          _ganttData.Children.Add(ds2);

          HighLowSeries ds3 = new HighLowSeries() { Label = "Task3" };
          ds3.XValuesSource = new double[] { 2 };
          ds3.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 2) };
          ds3.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 8) };
          _ganttData.Children.Add(ds3);

          HighLowSeries ds4 = new HighLowSeries() { Label = "Task4" };
          ds4.XValuesSource = new double[] { 3 };
          ds4.LowValuesSource = new DateTime[] { new DateTime(2008, 10, 4) };
          ds4.HighValuesSource = new DateTime[] { new DateTime(2008, 10, 7) };
          _ganttData.Children.Add(ds4);

          foreach (DataSeries ds in _ganttData.Children)
          {
            ds.SymbolSize = new Size(30, 30);
            if (_dataSeriesLoaded != null)
              ds.PlotElementLoaded += new EventHandler(_dataSeriesLoaded);
          }
        }
        return _ganttData;
      }
    }

        /// <summary>
    /// Data for polar chart.
    /// </summary>
    public ChartData PolarData
    {
      get
      {
        if (_polarData == null)
        {
          int cnt = 61;
          double[] x = new double[cnt];
          double[] y = new double[cnt];
          for (int i = 0; i < cnt; i++)
          {
            x[i] = i*6;
            y[i] = 100 * Math.Abs( Math.Cos( 9 * i * Math.PI / 180));
          }
          XYDataSeries ds = new XYDataSeries() { Label="s1", XValuesSource = x, ValuesSource = y };
          _polarData = new ChartData();
          _polarData.Children.Add(ds);
        }

        return _polarData;
      }
    }

    /// <summary>
    /// Data for polygon chart.
    /// </summary>
    public ChartData PolygonData
    {
      get
      {
        if (_polygonData == null)
        {
          int cnt = 61;
          double[] x = new double[cnt];
          double[] y = new double[cnt];
          for (int i = 0; i < cnt; i++)
          {
            x[i] = i * 6;
            y[i] = 100 * Math.Abs(Math.Cos(9 * i * Math.PI / 180));
          }
          XYDataSeries ds = new XYDataSeries() { Label = "s1", XValuesSource = x, ValuesSource = y };
          _polygonData = new ChartData();
          _polygonData.Children.Add(ds);
        }

        return _polygonData;
      }
    }
 
    static Random rnd = new Random();

    public static void CreateData(C1Chart chart)
    {
      chart.BeginUpdate();

      chart.Data.Children.Clear();

      var ds = CreateDataSeries(100, false);
      chart.Data.Children.Add(ds);
      chart.EndUpdate();

    }

    public static DataSeries CreateDataSeries(int npts, bool invert)
    {
      var ds = new XYDataSeries();// { SymbolSize = new Size(5, 5), ConnectionStrokeThickness = 1 };

      int cnt = npts;
      var x = new double[cnt];
      var y = new double[cnt];

      for (int i = 0; i < cnt; i++)
      {
        x[i] = i; y[i] = rnd.NextDouble();
      }

      if (invert)
      {
        ds.XValuesSource = y; ds.ValuesSource = x;
      }
      else
      {
        ds.XValuesSource = x; ds.ValuesSource = y;
      }
      return ds;
    }

    public static DataSeries CreateDataSeries2(int npts, bool invert)
    {
      var ds = new XYDataSeries() { SymbolSize = new Size(5, 5), ConnectionStrokeThickness = 1 };

      int cnt = npts;
      var x = new double[2 * cnt];
      var y = new double[2 * cnt];

      for (int i = 0; i < cnt; i++)
      {
        x[i] = i; y[i] = rnd.NextDouble();
      }

      for (int i = 0; i < cnt; i++)
      {
        x[cnt + i] = cnt - i - 1; y[cnt + i] = rnd.NextDouble();
      }

      if (invert)
      {
        ds.XValuesSource = y; ds.ValuesSource = x;
      }
      else
      {
        ds.XValuesSource = x; ds.ValuesSource = y;
      }
      return ds;
    }
  }
}
