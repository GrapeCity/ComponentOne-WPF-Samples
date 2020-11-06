using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexChartCustomization
{
    public class ViewModel
    {
        List<DataItem> _data;
        List<DataItem> _cosData;

        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<DataItem>();
                    _data.Add(new DataItem() { X = 1, Y = 50 });
                    _data.Add(new DataItem() { X = 2, Y = 30 });
                    _data.Add(new DataItem() { X = 3, Y = 40 });
                    _data.Add(new DataItem() { X = 4, Y = 60 });
                    _data.Add(new DataItem() { X = 5, Y = 90 });
                    _data.Add(new DataItem() { X = 6, Y = 80 });
                    _data.Add(new DataItem() { X = 7, Y = 56 });
                    _data.Add(new DataItem() { X = 8, Y = 56 });
                    _data.Add(new DataItem() { X = 9, Y = 50 });
                    _data.Add(new DataItem() { X = 10, Y = 70 });
                }

                return _data;
            }
        }

        public List<DataItem> CosData
        {
            get
            {
                if (_cosData == null)
                {
                    _cosData = new List<DataItem>();
                    for (int i = 0; i < 300; i++)
                    {
                        _cosData.Add(new DataItem { X = 0.16 * i, Y = Math.Cos(0.12 * i) });
                    }
                }

                return _cosData;
            }
        }

        static List<SmartPhoneVendor> vendors = null;

        private static BitmapImage loadImageSource(string resourceName)
        {
            Uri uri = new Uri("pack://application:,,,/Resources/" + resourceName, UriKind.Absolute);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = uri;
            bitmap.EndInit();
            return bitmap;
        }

        public static List<SmartPhoneVendor> SmartPhoneVendors
        {
            get
            {
                if (vendors == null)
                {
                    vendors = new List<SmartPhoneVendor>();

                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "Apple",
                        Color = Color.FromRgb(136, 189, 230),
                        Shipments = 215.2,
                        Country = "USA",
                        ImageSource = loadImageSource("apple-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "Huawei",
                        Color = Color.FromRgb(251, 178, 88),
                        Shipments = 139,
                        Country = "China",
                        ImageSource = loadImageSource("huawei-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "Lenovo",
                        Color = Color.FromRgb(144, 205, 151),
                        Shipments = 50.7,
                        Country = "China",
                        ImageSource = loadImageSource("lenovo-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "LG",
                        Color = Color.FromRgb(246, 170, 201),
                        Shipments = 55.1,
                        Country = "Korea",
                        ImageSource = loadImageSource("lg-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "Oppo",
                        Color = Color.FromRgb(191, 165, 84),
                        Shipments = 92.5,
                        Country = "China",
                        ImageSource = loadImageSource("oppo-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "Samsung",
                        Color = Color.FromRgb(188, 153, 199),
                        Shipments = 310.3,
                        Country = "Korea",
                        ImageSource = loadImageSource("samsung-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "Vivo",
                        Color = Color.FromRgb(237, 221, 70),
                        Shipments = 71.7,
                        Country = "China",
                        ImageSource = loadImageSource("vivo-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "Xiaomi",
                        Color = Color.FromRgb(240, 126, 110),
                        Shipments = 61,
                        Country = "China",
                        ImageSource = loadImageSource("xiaomi-50.png"),
                    });
                    vendors.Add(new SmartPhoneVendor()
                    {
                        Name = "ZTE",
                        Color = Color.FromRgb(140, 140, 140),
                        Shipments = 61.9,
                        Country = "China",
                        ImageSource = loadImageSource("zte-50.png"),
                    });
                }
                return vendors;
            }
        }

        public static List<TemperatureRecord> USStatesTemperatureRecords
        {
            get
            {
                List<TemperatureRecord> records = new List<TemperatureRecord>();
                records.Add(new TemperatureRecord() { Place = "Alabama", High = 112 });
                records.Add(new TemperatureRecord() { Place = "Alaska", High = 100 });
                records.Add(new TemperatureRecord() { Place = "Arizona", High = 128 });
                records.Add(new TemperatureRecord() { Place = "Arkansas", High = 120 });
                records.Add(new TemperatureRecord() { Place = "California", High = 134 });
                records.Add(new TemperatureRecord() { Place = "Colorado", High = 114 });
                records.Add(new TemperatureRecord() { Place = "Connecticut", High = 106 });
                records.Add(new TemperatureRecord() { Place = "Delaware", High = 110 });
                records.Add(new TemperatureRecord() { Place = "District of Columbia", High = 106 });
                records.Add(new TemperatureRecord() { Place = "Florida", High = 109 });
                records.Add(new TemperatureRecord() { Place = "Georgia", High = 112 });
                records.Add(new TemperatureRecord() { Place = "Hawaii", High = 98 });
                records.Add(new TemperatureRecord() { Place = "Idaho", High = 118 });
                records.Add(new TemperatureRecord() { Place = "Illinois", High = 117 });
                records.Add(new TemperatureRecord() { Place = "Indiana", High = 116 });
                records.Add(new TemperatureRecord() { Place = "Iowa", High = 118 });
                records.Add(new TemperatureRecord() { Place = "Kansas", High = 121 });
                records.Add(new TemperatureRecord() { Place = "Kentucky", High = 114 });
                records.Add(new TemperatureRecord() { Place = "Louisiana", High = 114 });
                records.Add(new TemperatureRecord() { Place = "Maine", High = 105 });
                records.Add(new TemperatureRecord() { Place = "Maryland", High = 109 });
                records.Add(new TemperatureRecord() { Place = "Massachusetts", High = 107 });
                records.Add(new TemperatureRecord() { Place = "Michigan", High = 112 });
                records.Add(new TemperatureRecord() { Place = "Minnesota", High = 115 });
                records.Add(new TemperatureRecord() { Place = "Mississippi", High = 115 });
                records.Add(new TemperatureRecord() { Place = "Missouri", High = 118 });
                records.Add(new TemperatureRecord() { Place = "Montana", High = 117 });
                records.Add(new TemperatureRecord() { Place = "Nebraska", High = 118 });
                records.Add(new TemperatureRecord() { Place = "Nevada", High = 125 });
                records.Add(new TemperatureRecord() { Place = "New Hampshire", High = 106 });
                records.Add(new TemperatureRecord() { Place = "New Jersey", High = 110 });
                records.Add(new TemperatureRecord() { Place = "New Mexico", High = 122 });
                records.Add(new TemperatureRecord() { Place = "New York", High = 109 });
                records.Add(new TemperatureRecord() { Place = "North Carolina", High = 110 });
                records.Add(new TemperatureRecord() { Place = "North Dakota", High = 121 });
                records.Add(new TemperatureRecord() { Place = "Ohio", High = 113 });
                records.Add(new TemperatureRecord() { Place = "Oklahoma", High = 120 });
                records.Add(new TemperatureRecord() { Place = "Oregon", High = 117 });
                records.Add(new TemperatureRecord() { Place = "Pennsylvania", High = 111 });
                records.Add(new TemperatureRecord() { Place = "Rhode Island", High = 104 });
                records.Add(new TemperatureRecord() { Place = "South Carolina", High = 113 });
                records.Add(new TemperatureRecord() { Place = "South Dakota", High = 120 });
                records.Add(new TemperatureRecord() { Place = "Tennessee", High = 113 });
                records.Add(new TemperatureRecord() { Place = "Texas", High = 120 });
                records.Add(new TemperatureRecord() { Place = "Utah", High = 117 });
                records.Add(new TemperatureRecord() { Place = "Vermont", High = 105 });
                records.Add(new TemperatureRecord() { Place = "Virginia", High = 110 });
                records.Add(new TemperatureRecord() { Place = "Washington", High = 118 });
                records.Add(new TemperatureRecord() { Place = "West Virginia", High = 112 });
                records.Add(new TemperatureRecord() { Place = "Wisconsin", High = 114 });
                records.Add(new TemperatureRecord() { Place = "Wyoming", High = 115 });
                return records;
            }
        }
    }
}
