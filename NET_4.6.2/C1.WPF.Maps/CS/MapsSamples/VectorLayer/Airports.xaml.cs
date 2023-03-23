using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using C1.WPF.Maps;
using C1.WPF.Maps.GeoJson;

namespace MapsSamples
{
    public partial class Airports : UserControl
    {
        private C1VectorLayer layerLand;
        private C1VectorLayer layerAirports;


        public Airports()
        {
            InitializeComponent();
            Loaded += Airports_Loaded;
        }

        private void Airports_Loaded(object sender, RoutedEventArgs e)
        {
            maps.Source = null;
            Background = new SolidColorBrush(Color.FromArgb(255, 138, 180, 248));

            layerLand = ReadGeoJsonFromResource("land.geojson", new SolidColorBrush(Color.FromArgb(255, 187, 226, 198)), Brushes.Transparent);
            layerAirports = ReadGeoJsonFromResource("airports.geojson", Brushes.LightGray, Brushes.Gray);

            maps.Layers.Add(layerLand);
            maps.Layers.Add(layerAirports);

            maps.Zoom = 1.2;
        }

        C1VectorLayer ReadGeoJsonFromResource(string name, Brush fill, Brush stroke)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MapsSamples.Resources." + name);

            var items = GeoJsonReader.Read(stream);

            var vl = new C1VectorLayer();
            vl.BeginUpdate();
            foreach (var item in items)
            {
                if (item != null)
                {
                    item.Fill = fill;
                    item.Stroke = stroke;

                    if (item is C1VectorPlacemark)
                    {
                        var lbl = ((System.Collections.IDictionary)item.Tag)["name_en"] as string;
                        if (!string.IsNullOrEmpty(lbl))
                        {
                            var pm = (C1VectorPlacemark)item;
                            pm.Geometry = new EllipseGeometry() { RadiusX = 3, RadiusY = 3 };
                            pm.ToolTip = pm.Label = lbl;
                            pm.LabelPosition = LabelPosition.Top;
                            vl.Children.Add(item);
                        }
                    }
                    else
                        vl.Children.Add(item);
                }
            }
            vl.LabelVisibility = LabelVisibility.Hidden;
            vl.EndUpdate();
            return vl;
        }

        private void cbLabels_Checked(object sender, RoutedEventArgs e)
        {
            layerAirports.LabelVisibility = cbLabels.IsChecked.Value ?
                LabelVisibility.AutoHide : LabelVisibility.Hidden;
        }
    }
}
