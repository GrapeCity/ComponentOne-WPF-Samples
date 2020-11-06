using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeatherChart
{
    public class DataItem
    {
        public int MaxTemp { get; set; }
        public int MinTemp { get; set; }
        public int MeanTemp { get; set; }
        public int MeanPressure { get; set; }
        public int Precipitation { get; set; }
        public DateTime Date { get; set; }
    }
}
