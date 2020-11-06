using System;
using System.Collections.Generic;
using System.IO;
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

namespace MapsExplorer
{
    public partial class Cities : UserControl
    {
        VectorLayer vl;

        public Cities()
        {
            InitializeComponent();
            Tag = "Cities";
            Color fc = Color.FromArgb(0xff, 0xC0, 0x50, 0x4d);
            maps.Foreground = new SolidColorBrush(fc);

            vl = new VectorLayer();

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("MapsExplorer.Resources.Cities100K.txt");
            if (stream != null)
            {
                List<City> cities = City.Read(stream);
                vl.ItemTemplate = (DataTemplate)Resources["templCity"];
                IEnumerable<City> source = from city in cities orderby city.Population descending select city;
                vl.ItemsSource = source.Take(500);
            }

            maps.Layers.Add(vl);
        }

        private void C1VectorPlacemark_Loaded(object sender, RoutedEventArgs e)
        {
            VectorPlacemark pl = (VectorPlacemark)sender;
            City city = (City)pl.DataContext;
            ToolTipService.SetToolTip(pl, city.Name + ":\n" + city.Population.ToString());
        }
    }
}
