using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using C1.WPF.C1Chart3D;

namespace ChartSamples
{
  /// <summary>
  /// Interaction logic for Elevation.xaml
  /// </summary>
  public partial class Elevation : UserControl
  {
    public Elevation()
    {
      InitializeComponent();
    }

    private void chart3d_Loaded(object sender, RoutedEventArgs e)
    {
      chart3d.ColorPalette = new Color[] { Colors.Green, Colors.Red };
      
      string zdata = null;

      using (Stream stream = Assembly.GetExecutingAssembly().
        GetManifestResourceStream("ChartSamples.Resources.elevation.txt"))
      {
        using (var sr = new StreamReader(stream))
        {
          zdata = sr.ReadToEnd();
        }
      }

      if (zdata != null)
      {
        var strs = zdata.Split( new char[0], StringSplitOptions.RemoveEmptyEntries);

        var vals = new List<double>();
        int i = 0;
        for (i = 0; i < strs.Length; i++)
          vals.Add(double.Parse(strs[i]));

        int xlen = 30, ylen = 20;
        var zvals = new double[xlen, ylen];

        i = 0;
        for (int ix = 0; ix < xlen; ix++)
          for (int iy = 0; iy < ylen; iy++)
            zvals[ix, iy] = vals[i++];

        chart3d.Children.Add(new GridDataSeries() { ZData = zvals });
      }
    }

    private void CheckBox_Checked(object sender, RoutedEventArgs e)
    {
      if (cbD3D.IsChecked == true)
      {
        chart3d.RenderMode = RenderMode.Direct3D;
        chart3d.ChartType = Chart3DType.SurfaceZoneGradient;
      }
      else
      {
        chart3d.RenderMode = RenderMode.Default;
        chart3d.ChartType = Chart3DType.Surface;
      }
    }

    private void chart3d_MouseMove(object sender, MouseEventArgs e)
    {
      var pt = e.GetPosition(chart3d);

      var pt3 = chart3d.PointToData(pt);

      if (pt3 != null)
      {
        label.Text = string.Format("x={0:0.00}\ny={1:0.00}\nz={2:0.00}", pt3.Value.X, pt3.Value.Y, pt3.Value.Z);
        Canvas.SetLeft(bdr, pt.X);
        Canvas.SetTop(bdr, pt.Y - bdr.ActualHeight);
        bdr.Visibility = Visibility.Visible;
      }
      else
      {
        bdr.Visibility = Visibility.Collapsed;
      }
    }
  }
}
