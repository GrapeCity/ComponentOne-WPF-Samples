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

namespace MapsExplorer
{
    public partial class EarthQuakes : UserControl
    {
        public EarthQuakes()
        {
            InitializeComponent();
            Tag = "Earthquakes";

            this.Loaded += EarthQuakes_Loaded;
        }

        private void EarthQuakes_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.maps.Layers.Count == 0)
            {
                var layer = new VectorLayer()
                {
                    ItemStyle = (Style)this.Resources["style"],
                };
                Utils.LoadKMLFromResources(layer, "MapsExplorer.Resources.2.5_day_depth.kml", true, null);
                this.maps.Layers.Add(layer);
            }
            else
            {
                var layer = maps.Layers[0] as VectorLayer;
                Utils.LoadKMLFromResources(layer, "MapsExplorer.Resources.2.5_day_depth.kml", true, null);
            }
        }
    }
}
