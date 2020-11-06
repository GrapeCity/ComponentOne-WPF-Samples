using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

#if SILVERLIGHT
using C1.Silverlight.Extended;
using C1.Silverlight.Extended.PropertyGrid;
using C1.Silverlight.Chart3D;
#else
using C1.WPF.Extended;
using C1.WPF.Extended.PropertyGrid;
using C1.WPF.C1Chart3D;
#endif

namespace ChartSamples
{
  public partial class Function3D : UserControl
  {
    public Function3D()
    {
      InitializeComponent();

      var fg = new SolidColorBrush(Colors.White);
      chart3D.AxisX.Title = new TextBlock { Text = "x", Foreground = fg };
      chart3D.AxisY.Title = new TextBlock { Text = "y", Foreground = fg };
      chart3D.AxisZ.Title = new TextBlock { Text = "z", Foreground = fg };

      pgChart.AutoGenerateProperties = false;
      pgChart.PropertyAttributes.Add(new PropertyAttribute() { MemberName = "ChartType" });
      pgChart.PropertyAttributes.Add(new PropertyAttribute() { MemberName = "CeilAppearance" });
      pgChart.PropertyAttributes.Add(new PropertyAttribute() { MemberName = "FloorAppearance" });
      pgChart.PropertyAttributes.Add(new PropertyAttribute()
      {
        MemberName = "Elevation",
        Editor = new MyNumericEditor() { Increment = 6 }
      });
      pgChart.PropertyAttributes.Add(new PropertyAttribute()
      {
        MemberName = "Azimuth",
        Editor = new MyNumericEditor() { Increment = 6 }
      });

      pgChart.SelectedObject = chart3D;

      cbFuncs.ItemsSource = new Func3D[]
      {
        new Func3D( -0.5, -0.5, .5, .5, "x*x*x*y - x*y*y*y", (x,y) => x*x*x*y - x*y*y*y),
        new Func3D( -1, -1, 1, 1, "cos(abs(x)+abs(y))*(abs(x)+abs(y))", 
          (x,y) => Math.Cos(Math.Abs(x) + Math.Abs(y)) * (Math.Abs(x) + Math.Abs(y))),
        new Func3D( -1, -1, 1, 1, "-1 / (x*x + y*y)", (x,y) => -1 / (x*x + y*y)),
        new Func3D( 0, 0, 10, 10, "sin(x)*cos(y)/(sqrt(sqrt(x*y+x*y))+1)*10", 
          (x,y) => Math.Sin(x)*Math.Cos(y)/(Math.Sqrt(Math.Sqrt(x*y+x*y))+1)*10),
      };

      cbFuncs.SelectedIndex = 0;

      chart3D.ColorPalette = new Color[] { Color.FromArgb(255, 50, 50, 255),
        Color.FromArgb(255, 255,255,255), Color.FromArgb(255,255,50,50) };

      cbRenderMode.ItemsSource = Enum.GetValues(typeof(RenderMode));
      cbRenderMode.SelectedIndex = 0;
      cbRenderMode.SelectionChanged += (s, e) => chart3D.RenderMode = (RenderMode)cbRenderMode.SelectedItem;

      chart3D.AzimuthAction = ActionType.LeftMouseDrag;
      chart3D.ElevationAction = ActionType.LeftMouseDrag;
    }

    private void cbFuncs_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (e.RemovedItems != null)
      {
        foreach(Func3D item in e.RemovedItems)
          item.PropertyChanged -= new PropertyChangedEventHandler(item_PropertyChanged);
      }

      if (e.AddedItems != null)
      {
        foreach (Func3D item in e.AddedItems)
          item.PropertyChanged += new PropertyChangedEventHandler(item_PropertyChanged);
      }

      var func = cbFuncs.SelectedItem as Func3D;
      if (func != null)
      {
        pg.SelectedObject = func;
        chart3D.Children.Clear();
        chart3D.Children.Add(func.CreateGridSeries());
      }
    }

    void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      chart3D.Children.Clear();
      chart3D.Children.Add(((Func3D)sender).CreateGridSeries());
    }

    private void btn3D_Def_Click(object sender, RoutedEventArgs e)
    {
      chart3D.Elevation = 150;
      chart3D.Azimuth = 30;
    }

    private void btn2D_XY_Click(object sender, RoutedEventArgs e)
    {
      chart3D.Elevation = 90;
      chart3D.Azimuth = 0;
    }

    private void btn2D_XZ_Click(object sender, RoutedEventArgs e)
    {
      chart3D.Elevation = 180;
      chart3D.Azimuth = 0;
    }

    private void btn2D_YZ_Click(object sender, RoutedEventArgs e)
    {
      chart3D.Elevation = 180;
      chart3D.Azimuth = 90;
    }
  }

  public delegate double zfunc(double x, double y);

  public class Func3D : INotifyPropertyChanged
  {
    private double _xmin, _xmax, _ymin, _ymax;
    private zfunc _zfunc;
    private string _label;

    public Func3D(double xmin, double ymin, double xmax, double ymax, string label, zfunc zfunc)
    {
      _xmin = xmin; _ymin = ymin; _xmax = xmax; _ymax = ymax; _label = label;
      _zfunc = zfunc;
    }

    public double XMin
    {
      get { return _xmin; }
      set 
      {
        if (_xmin != value)
        {
          _xmin = value;
          OnPropertyChanged("XMin");
        }
      }
    }

    public double YMin
    {
      get { return _ymin; }
      set
      {
        if (_ymin != value)
        {
          _ymin = value;
          OnPropertyChanged("YMin");
        }
      }
    }

    public double XMax
    {
      get { return _xmax; }
      set
      {
        if (_xmax != value)
        {
          _xmax = value;
          OnPropertyChanged("XMax");
        }
      }
    }

    public double YMax
    {
      get { return _ymax; }
      set
      {
        if (_ymax != value)
        {
          _ymax = value;
          OnPropertyChanged("YMax");
        }
      }
    }

    void OnPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public GridDataSeries CreateGridSeries()
    {
      int n = 21;
      var zdata = new double[n, n];
      var dx = (_xmax - _xmin) / (n - 1);
      var dy = (_ymax - _ymin) / (n - 1);

      for (int i = 0; i < n; i++)
      {
        for (int j = 0; j < n; j++)
        {
          zdata[i, j] = _zfunc(_xmin + i * dx, _ymin + j * dy);
        }
      }

      return new GridDataSeries()
      {
        Start = new Point(_xmin, _ymin),
        Step = new Point(dx, dy),
        ZData = zdata
      };
    }

    public override string ToString()
    {
      return _label;
    }
  }

  class MyNumericEditor : NumericEditor
  {
    public override ITypeEditorControl Create()
    {
      return new NumericEditor() { Increment = Increment };
    }

    private void properties_AddingPropertyBox(object sender, ChangingPropertyBoxEventArgs e)
    {
        e.PropertyBox.Style = Resources["PropertiesPropertyBox"] as Style;
    }
  }
}
