using C1.WPF.Maps;
using System;
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

namespace MapsSamples
{
  public partial class EarthQuakes : UserControl
  {
    public EarthQuakes()
    {
      InitializeComponent();

      this.Loaded += EarthQuakes_Loaded;
    }

    void EarthQuakes_Loaded(object sender, RoutedEventArgs e)
    {
        if (this.maps.Layers.Count == 0)
        {
            var layer = new C1VectorLayer()
            {
                ItemStyle = (Style)this.Resources["style"],
            };
            Utils.LoadKMLFromResources(layer, "MapsSamples.Resources.2.5_day_depth.kml", true, null);
            this.maps.Layers.Add(layer);
        }
        else
        {
            var layer = maps.Layers[0] as C1VectorLayer;
            Utils.LoadKMLFromResources(layer, "MapsSamples.Resources.2.5_day_depth.kml", true, null);
        }
    }
  }
}
