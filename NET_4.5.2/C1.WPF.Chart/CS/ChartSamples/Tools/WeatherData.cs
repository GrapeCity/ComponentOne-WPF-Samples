using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace ChartSamples
{
  public class WeatherData : DependencyObject
  {
    public static DependencyProperty DateTimeProperty =
        DependencyProperty.Register("DateTime", typeof(DateTime), typeof(WeatherData), null);

    public DateTime DateTime
    {
      get { return (DateTime)GetValue(DateTimeProperty); }
      set { SetValue(DateTimeProperty, value); }
    }

    public static DependencyProperty TMinProperty =
      DependencyProperty.Register("TMin", typeof(double), typeof(WeatherData), null);

    public double TMin
    {
      get { return (double)GetValue(TMinProperty); }
      set { SetValue(TMinProperty, value); }
    }

    public static DependencyProperty TMaxProperty =
      DependencyProperty.Register("TMax", typeof(double), typeof(WeatherData), null);

    public double TMax
    {
      get { return (double)GetValue(TMaxProperty); }
      set { SetValue(TMaxProperty, value); }
    }

    public static DependencyProperty TAvgProperty =
      DependencyProperty.Register("TAvg", typeof(double), typeof(WeatherData), null);

    public double TAvg
    {
      get { return (double)GetValue(TAvgProperty); }
      set { SetValue(TAvgProperty, value); }
    }

    public WeatherData(DateTime dt, double tmax, double tavg, double tmin)
    {
      DateTime = dt;
      TMin = tmin;
      TMax = tmax;
      TAvg = tavg;
    }
  }

  public class CSVData
  {
    private List<string> _fields = new List<string>();
    private List<string[]> _records = new List<string[]>();

    List<string> Fields
    {
      get { return _fields; }
    }

    List<string[]> Records
    {
      get { return _records; }
    }

    public int Length
    {
      get { return _records.Count; }
    }

    public string this[int i, int j]
    {
      get
      {
        string[] ss = Records[i];
        if (j < 0 || j >= ss.Length)
          throw new ArgumentException("j");

        return ss[j].Trim('"');
      }
    }

    public string this[int i, string name]
    {
      get
      {
        string[] ss = Records[i];
        int j = Fields.IndexOf(name);
        if (j < 0 || j >= ss.Length)
          throw new ArgumentException("name");

        return ss[j].Trim('"');
      }
    }

    public void Read(Stream stream, bool readNames, bool filter)
    {
      if (stream == null)
        throw new ArgumentNullException("stream");

      Fields.Clear();
      Records.Clear();

      if (stream != null)
      {
        using (StreamReader sr = new StreamReader(stream))
        {
          int i = 0;
          while (sr.Peek() > -1)
          {
            string line = sr.ReadLine();

            if (line.StartsWith("#"))
              continue;

            string[] ss = line.Split(',');

            if (readNames && Fields.Count == 0)
              Fields.AddRange(ss);
            else
            {
              if( !filter || (i%3 == 0)) // skip some data if necessary
                Records.Add(ss);
              i++;
            }
          }
          sr.Close();
        }
      }
    }
  }
}
