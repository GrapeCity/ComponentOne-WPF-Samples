using System;
using System.Collections.Generic;

namespace WeatherChart
{
    public class WeatherChartModel
    {
        Random rnd = new Random();

        public List<DataItem> Data
        {
            get
            {
                var pointsCount = rnd.Next(1, 30);
                var temperaturePoints = new List<DataItem>();
                for (DateTime date = new DateTime(2016, 1, 1); date.Year == 2016; date = date.AddDays(1))
                {
                    var newItem = new DataItem();
                    newItem.Date = date;
                    if (date.Month <= 8)
                        newItem.MaxTemp = rnd.Next(3 * date.Month, 4 * date.Month);
                    else
                        newItem.MaxTemp = rnd.Next((13 - date.Month - 2) * date.Month, (13 - date.Month) * date.Month);
                    newItem.MinTemp = newItem.MaxTemp - rnd.Next(6, 8);
                    newItem.MeanTemp = (newItem.MaxTemp + newItem.MinTemp) / 2;
                    newItem.MeanPressure = rnd.Next(980, 1050);
                    newItem.Precipitation = rnd.Next(5) == 1 ? rnd.Next(0, 20) : 0;
                    temperaturePoints.Add(newItem);
                }

                return temperaturePoints;
            }
        }

        public string Description
        {
            get
            {
                return "This view shows multiple plots with range selection.";
            }
        }
    }
}
