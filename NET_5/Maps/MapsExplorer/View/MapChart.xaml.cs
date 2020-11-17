using C1.WPF.Core;
using C1.WPF.Maps;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MapsExplorer
{
    public partial class MapChart : UserControl
    {
        Countries countries = new Countries();
        VectorLayer vl;

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
            ColorValues cvals = new ColorValues();
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 241, 244, 255), Value = 0 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 241, 244, 255), Value = 5000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 224, 224, 246), Value = 10000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 203, 205, 255), Value = 20000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 179, 182, 230), Value = 50000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 156, 160, 240), Value = 100000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 127, 132, 243), Value = 200000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 89, 97, 230), Value = 500000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 56, 64, 217), Value = 1000000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 19, 26, 148), Value = 2000000 });
            cvals.Add(new ColorValue() { Color = Color.FromArgb(255, 0, 3, 74), Value = 1.001 * countries.GetMax() });

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

            legend.Items.Clear();

            ColorValues cvals = (ColorValues)countries.Converter;

            int cnt = cvals.Count;
            double sz = 20;

            for (int i = 0; i < cnt - 1; i++)
            {
                ColorValue cv = cvals[i];
                ListBoxItem lbi = new ListBoxItem()
                {
                    Height = sz,
                    Margin = new Thickness(0),
                    Padding = new Thickness(0)
                };
                StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };
                LinearGradientBrush lgb = new LinearGradientBrush() { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
                lgb.GradientStops.Add(new GradientStop() { Color = cv.Color, Offset = 0 });
                lgb.GradientStops.Add(new GradientStop() { Color = cvals[i + 1].Color, Offset = 1 });

                sp.Children.Add(new Rectangle()
                {
                    Width = sz,
                    Height = sz,
                    Fill = lgb,
                    Stroke = new SolidColorBrush(Colors.LightGray),
                    StrokeThickness = 0.5
                });
                sp.Children.Add(new TextBlock()
                {
                    Height = sz,
                    Text = cv.Value.ToString(),
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(5, 0, 0, 0)
                });
                lbi.Content = sp;
                legend.Items.Add(lbi);
            }
            legend.Items.Add(new ListBoxItem() { Height = sz });
        }

        private void legend_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                ((ListBoxItem)item).IsSelected = false;
            }
        }
    }
}