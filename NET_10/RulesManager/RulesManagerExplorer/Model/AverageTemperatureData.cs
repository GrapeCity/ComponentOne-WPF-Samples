using System.Collections.Generic;

namespace RulesManagerExplorer
{
    public class AverageTemperatureData
    {
        public string City { get; set; }
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }

        static public List<AverageTemperatureData> GetDemoData()
        {
            return new List<AverageTemperatureData>()
            {
                new()
                {
                    City = "Tokyo",
                    January = 4,
                    February = 5,
                    March = 8,
                    April = 13,
                    May = 18,
                    June = 21,
                    July = 25,
                    August = 26,
                    September = 22,
                    October = 17,
                    November = 12,
                    December = 7
                },
                new()
                {
                    City = "Moscow",
                    January = -9,
                    February = -8,
                    March = 3,
                    April = 11,
                    May = 19,
                    June = 22,
                    July = 24,
                    August = 22,
                    September = 16,
                    October = 9,
                    November = 1,
                    December = -3
                },
                new()
                {
                    City = "Amsterdam",
                    January = 2,
                    February = 2,
                    March = 5,
                    April = 8,
                    May = 12,
                    June = 15,
                    July = 17,
                    August = 17,
                    September = 14,
                    October = 11,
                    November = 6,
                    December = 4
                },
            };
        }
    }
}
