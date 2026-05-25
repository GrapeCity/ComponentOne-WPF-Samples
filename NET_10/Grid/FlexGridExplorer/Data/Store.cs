using FlexGridExplorer.Resources;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows;

namespace FlexGridExplorer
{
    public class Store
    {
        [Display(Name = nameof(AppResources.StoreLabel), ResourceType = typeof(AppResources))]
        public int ID { get; set; }

        [Display(Name = nameof(AppResources.City), ResourceType = typeof(AppResources))]
        public string City { get; set; }

        [Display(Name = nameof(AppResources.LocationLabel), ResourceType = typeof(AppResources))]
        public Point Location { get; set; }

        [Display(Name = nameof(AppResources.CountryLabel), ResourceType = typeof(AppResources))]
        public string Country { get; set; }

        public static Store FromString(string s)
        {
            Store shop = null;
            if (!string.IsNullOrEmpty(s))
            {
                var record = s.Split('\t');
                if (record.Length == 4)
                {
                    shop = new Store
                    {
                        ID = int.Parse(record[0]),
                        City = record[1],
                        Location = PointFromString(record[2]),
                        Country = record[3]
                    };
                }
            }

            return shop;
        }

        private static Point PointFromString(string s)
        {
            var record = s.Split(',');
            return new Point(double.Parse(record[1], CultureInfo.InvariantCulture),
              double.Parse(record[0], CultureInfo.InvariantCulture));
        }
    }
}
