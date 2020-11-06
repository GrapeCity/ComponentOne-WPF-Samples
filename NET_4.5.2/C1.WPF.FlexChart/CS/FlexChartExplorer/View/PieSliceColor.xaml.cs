using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using C1.Chart;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PieSliceColor : UserControl
    {
        DataItem[] data;
        Random rnd = new Random();

        public PieSliceColor()
        {
            this.InitializeComponent();
        }

        private void NewData()
        {
            data = new DataItem[]{
                new DataItem { Name = "US", Value = rnd.Next(10, 100) },
                new DataItem { Name = "Germany", Value = rnd.Next(10, 100) },
                new DataItem { Name = "UK", Value = rnd.Next(10, 100) },
                new DataItem { Name = "Japan",Value = rnd.Next(10, 100) },
                new DataItem { Name = "Italy", Value = rnd.Next(10, 100) },
                new DataItem { Name = "Greece", Value = rnd.Next(10, 100) }
            }.OrderBy(x => x.Name).ToArray();
            pieChart.ItemsSource = data;
        }

        private void UpdateData()
        {
            pieChart.BeginUpdate();

            var data = pieChart.ItemsSource as DataItem[];
            if (data != null)
            {
                var npts = data.Length;

                for (var i = 0; i < npts; i++)
                    data[i].Value = rnd.Next(10, 100);
            }

            pieChart.EndUpdate();
        }

        private void btnNew_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NewData();
        }

        private void btnUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateData();
        }

        private void pieChart_SliceRendering(object sender, C1.WPF.Chart.RenderSliceEventArgs e)
        {
            //get color depending on value
            var dataOrderedByValue = data.OrderBy(x => x.Value).ToArray();
            var itemOrderedIndex = Array.IndexOf(dataOrderedByValue, data[e.Index]);
            // Use color #B1DCB6 (177, 250, 182) and color #F9F9F9 (249, 249, 249) which are defined in the suggested palette as the start color and end color.
            var colorStart = Color.FromArgb(0xff, 177, 250, 182);
            var colorEnd = Color.FromArgb(0xff, 249, 249, 249);
            var stepsCount = (data.Count() - 1);
            int paletteStepR = (int)Math.Round((double)((colorEnd.A - colorStart.R) / stepsCount));
            int paletteStepG = (int)Math.Round((double)((colorEnd.G - colorStart.G) / stepsCount));
            int paletteStepB = (int)Math.Round((double)((colorEnd.B - colorStart.B) / stepsCount));

            var color = colorStart;
            if (itemOrderedIndex < dataOrderedByValue.Length - 1)
                color = Color.FromArgb(0xff, (byte)(colorEnd.A - itemOrderedIndex * paletteStepR), 
                    (byte)(colorEnd.G - itemOrderedIndex * paletteStepG), 
                    (byte)(colorEnd.B - itemOrderedIndex * paletteStepB));

            //use engine to paint the slice
            e.Engine.SetFill((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
            e.Engine.SetStroke((color.A << 24) | (color.R << 16) | (color.G << 8) | color.B);
        }

        private void pieChart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            NewData();
        }

        public class DataItem : INotifyPropertyChanged
        {
            private string _name;
            private int _value;

            public string Name
            {
                get { return _name; }
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        OnPropertyChanged("Name");
                    }
                }
            }

            public int Value
            {
                get { return _value; }
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                        OnPropertyChanged("Value");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
