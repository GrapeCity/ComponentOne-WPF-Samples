using C1.WPF.Maps;
using System.Windows;
using System.Windows.Controls;

namespace MapsExplorer
{
    public partial class EarthQuakes : UserControl
    {
        public EarthQuakes()
        {
            InitializeComponent();
            Tag = Properties.Resources.Earthquakes;

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
