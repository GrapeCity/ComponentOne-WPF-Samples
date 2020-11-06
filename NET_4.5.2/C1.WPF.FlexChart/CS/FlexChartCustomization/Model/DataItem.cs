using System;
using System.Windows.Media;

namespace FlexChartCustomization
{
    public class DataItem
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class SmartPhoneVendor
    {
        public string Name { get; set; }
        public double Shipments { get; set; }
        public string Country { get; set; }
        public Color Color { get; set; }
        public ImageSource ImageSource { get; set; }
    }

    public class TemperatureRecord
    {
        public string Place { get; set; }
        public int High { get; set; }
    }
}
