using System;
using System.Collections.Generic;
using System.Linq;
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
using C1.WPF.Maps;

namespace MapsSamples
{
    public enum Location
    {
        USA,
        Japan
    }

    /// <summary>
    /// Interaction logic for MapShape.xaml
    /// </summary>
    public partial class MapShape : UserControl
    {
        Dictionary<Location, string> Locations = new Dictionary<Location, string>();

        public MapShape()
        {
            InitializeComponent();
            Loaded += MapShape_Loaded;
        }

        void InitialLocations()
        {
            if (Locations.Count == 0)
            {
                Locations.Add(Location.USA, "USA");
                Locations.Add(Location.Japan, "Japan");
            }
        }
        void MapShape_Loaded(object sender, RoutedEventArgs e)
        {
            InitialLocations();
            this.countriesCombo.ItemsSource = Locations;
            countriesCombo.SelectedIndex = 0;
            maps.Source = null;
            InitialMapsByLocaltion(maps, Location.USA);
        }

        void ProcessMap(C1VectorItemBase vector, C1ShapeAttributes attribute, Location location)
        {
            InitialVectorItemBase(vector, attribute, location);
        }

        void InitialMapsByLocaltion(C1Maps maps, Location location)
        {
            C1VectorLayer layer = null;
            if (maps.Layers.Count == 0)
            {
                layer = new C1VectorLayer();
                maps.Layers.Add(layer);
            }
            else
            {
                layer = maps.Layers[0] as C1VectorLayer;
                layer.Children.Clear();
            }
            Point center = new Point();
            double zoom = 0;
            switch (location)
            {
                case Location.USA:
                    Utils.LoadShapeFromResource(layer, "MapsSamples.Resources.states.shp", "MapsSamples.Resources.states.dbf", location,
                true, ProcessMap);
                    center = new Point(-115, 50);
                    zoom = 2;
                    break;
                case Location.Japan:
                    Utils.LoadShapeFromResource(layer, "MapsSamples.Resources.jp_toku_kuni_pgn.shp", "MapsSamples.Resources.jp_toku_kuni_pgn.dbf", location,
                    true, ProcessMap);
                    center = new Point(135, 37);
                    zoom = 4;
                    break;
            }

            maps.MinZoom = zoom;
            maps.Zoom = zoom;
            maps.Center = center;
        }

        void InitialVectorItemBase(C1VectorItemBase vector, C1ShapeAttributes attribute, Location location)
        {
            string toolTip = string.Empty;
            vector.Stroke = new SolidColorBrush(Colors.LightGray);
            switch (location)
            {
                case Location.USA:
                    vector.Fill = new SolidColorBrush(Colors.Purple);
                    toolTip = (string)attribute["STATE_NAME"];
                    break;
                case Location.Japan:
                    vector.Fill = new SolidColorBrush(Colors.Brown);
                    toolTip = (string)attribute["NAME_UTF"];
                    break;
            }
            ToolTipService.SetToolTip(vector, toolTip);
        }

        private void countriesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Location location = comboBox.SelectedItem == null ? Location.USA : location = (Location)((KeyValuePair<Location, string>)countriesCombo.SelectedItem).Key;
                InitialMapsByLocaltion(maps, location);
            }
        }
    }
}
