using C1.WPF.Core;
using C1.WPF.Maps;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MapsExplorer
{
    public partial class MapChart : UserControl, INotifyPropertyChanged
    {
        Countries countries = new Countries();
        VectorLayer vl;

        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Legend> Legends { get; set; } = new ObservableCollection<Legend>();

        public MapChart()
        {
            InitializeComponent();
            Tag = Properties.Resources.MapChart;

            this.Loaded += MapChart_Loaded;            
        }

        private void MapChart_Loaded(object sender, RoutedEventArgs e)
        {
            maps.Source = null;

            vl = new VectorLayer()
            {
                LabelVisibility = LabelVisibility.Hidden,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 0x97, 0x35, 0x35))
            };

            // read text data from resources
            using (Stream stream = Assembly.GetExecutingAssembly()
              .GetManifestResourceStream("MapsExplorer.Resources.gdp-ppp.txt"))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    for (; !sr.EndOfStream;)
                    {
                        string s = sr.ReadLine();

                        string[] ss = s.Split(new char[] { '\t' },
                          StringSplitOptions.RemoveEmptyEntries);

                        countries.Add(new Country() { Name = ss[1].Trim(), Value = double.Parse(ss[2], CultureInfo.InvariantCulture) });
                    }
                }
            }

            // create palette
            ColorValues cvals = new ColorValues
            {
                new ColorValue() { Color = Color.FromArgb(255, 241, 244, 255), Value = 0 },
                new ColorValue() { Color = Color.FromArgb(255, 241, 244, 255), Value = 5000 },
                new ColorValue() { Color = Color.FromArgb(255, 224, 224, 246), Value = 10000 },
                new ColorValue() { Color = Color.FromArgb(255, 203, 205, 255), Value = 20000 },
                new ColorValue() { Color = Color.FromArgb(255, 179, 182, 230), Value = 50000 },
                new ColorValue() { Color = Color.FromArgb(255, 156, 160, 240), Value = 100000 },
                new ColorValue() { Color = Color.FromArgb(255, 127, 132, 243), Value = 200000 },
                new ColorValue() { Color = Color.FromArgb(255, 89, 97, 230), Value = 500000 },
                new ColorValue() { Color = Color.FromArgb(255, 56, 64, 217), Value = 1000000 },
                new ColorValue() { Color = Color.FromArgb(255, 19, 26, 148), Value = 2000000 },
                new ColorValue() { Color = Color.FromArgb(255, 0, 3, 74), Value = 1.001 * countries.GetMax() }
            };

            countries.Converter = cvals;

            // read world map from resources
            Utils.LoadKMZFromResources(vl, "MapsExplorer.Resources.WorldMap.kmz",
              true, ProcessWorldMap);

            maps.Layers.Add(vl);

            maps.TargetZoomChanged += new EventHandler<PropertyChangedEventArgs<double>>(maps_TargetZoomChanged);

            InitLegend();
        }

        void maps_TargetZoomChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (e.NewValue >= 2)
                vl.LabelVisibility = LabelVisibility.AutoHide;
            else
                vl.LabelVisibility = LabelVisibility.Hidden;
        }

        bool ProcessWorldMap(VectorItemBase v)
        {
            string name = ToolTipService.GetToolTip(v) as string;

            Country country = countries[name];
            if (country != null)
                v.Fill = country.Fill;
            else
                v.Fill = null;

            return true;
        }

        void InitLegend()
        {
            // create legend

            Legends.Clear();

            var cvals = (ColorValues)countries.Converter;

            int cnt = cvals.Count;

            for (int i = 0; i < cnt - 1; i++)
            {
                ColorValue cv = cvals[i];
                var lgb = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
                lgb.GradientStops.Add(new GradientStop() { Color = cv.Color, Offset = 0 });
                lgb.GradientStops.Add(new GradientStop() { Color = cvals[i + 1].Color, Offset = 1 });

                Legends.Add(new Legend
                {
                    RectangleBrush = lgb,
                    Name = cv.Value.ToString()
                });
            }
            OnPropertyChanged(nameof(Legends));
        }

        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    public class Legend
    {
        public LinearGradientBrush RectangleBrush { get; set; }
        public string Name { get; set; }
    }
}