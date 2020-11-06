using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using C1.WPF.Chart;
using C1.Chart;

namespace CurrencyComparison
{
    public static class HelperExtensions
    {
        public static List<Data> FilterTableByDate(this List<Data> sourceTable, DateTime date)
        {
            return sourceTable.Where(data => data.Date >= date).ToList();
        }

        public static List<Data> ImportData()
        {
            List<Data> sourceTable = new List<Data>();
            var rows = Utils.Read("Resources/CurrencyHistory.csv").TrimEnd().Split('\n');
            for (int i = 1; i < rows.Length; i++)
            {
                string[] columns = rows[i].TrimEnd().Split(',');
                double d;
                var data = new Data()
                {
                    Date = DateTime.ParseExact(columns[0], "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture),
                    USD = double.TryParse(columns[1], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].USD,
                    JPY = double.TryParse(columns[2], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].JPY,
                    GBP = double.TryParse(columns[3], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].GBP,
                    RUB = double.TryParse(columns[4], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].RUB,
                    CNY = double.TryParse(columns[5], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].CNY,
                    INR = double.TryParse(columns[6], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].INR,
                    KRW = double.TryParse(columns[7], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].KRW,
                    EUR = double.TryParse(columns[8], out d) ? Math.Round(d, 4) : sourceTable[sourceTable.Count - 1].EUR
                };
                sourceTable.Add(data);
            }

            return sourceTable;
        }

        public static List<Data> ConvertToBase(this List<Data> sourceTable, string baseCurrency)
        {
            var list = new List<Data>();
            foreach (var data in sourceTable)
            {
                var baseCurrencyValue = data.GetValue(baseCurrency);
                var newData = new Data()
                {
                    Date = data.Date,
                    USD = Math.Round(data.USD / baseCurrencyValue, 4),
                    JPY = Math.Round(data.JPY / baseCurrencyValue, 4),
                    GBP = Math.Round(data.GBP / baseCurrencyValue, 4),
                    RUB = Math.Round(data.RUB / baseCurrencyValue, 4),
                    CNY = Math.Round(data.CNY / baseCurrencyValue, 4),
                    INR = Math.Round(data.INR / baseCurrencyValue, 4),
                    KRW = Math.Round(data.KRW / baseCurrencyValue, 4),
                    EUR = Math.Round(data.EUR / baseCurrencyValue, 4)
                };
                list.Add(newData);
            }
            return list;
        }

        public static List<Data> ConvertToPercentage(this List<Data> sourceTable)
        {
            List<Data> list = new List<Data>();
            foreach (var data in sourceTable)
            {
                double x1, x2;
                var newData = new Data()
                {
                    Date = data.Date
                };
                x1 = sourceTable[sourceTable.Count - 1].USD;
                x2 = data.USD;
                newData.USD = Math.Round(((x2 - x1) / x1), 4);

                x1 = sourceTable[sourceTable.Count - 1].JPY;
                x2 = data.JPY;
                newData.JPY = Math.Round(((x2 - x1) / x1), 4);

                x1 = sourceTable[sourceTable.Count - 1].GBP;
                x2 = data.GBP;
                newData.GBP = Math.Round(((x2 - x1) / x1), 4);

                x1 = sourceTable[sourceTable.Count - 1].RUB;
                x2 = data.RUB;
                newData.RUB = Math.Round(((x2 - x1) / x1), 4);

                x1 = sourceTable[sourceTable.Count - 1].CNY;
                x2 = data.CNY;
                newData.CNY = Math.Round(((x2 - x1) / x1), 4);

                x1 = sourceTable[sourceTable.Count - 1].INR;
                x2 = data.INR;
                newData.INR = Math.Round(((x2 - x1) / x1), 4);

                x1 = sourceTable[sourceTable.Count - 1].KRW;
                x2 = data.KRW;
                newData.KRW = Math.Round(((x2 - x1) / x1), 4);

                x1 = sourceTable[sourceTable.Count - 1].EUR;
                x2 = data.EUR;
                newData.EUR = Math.Round(((x2 - x1) / x1), 4);

                list.Add(newData);
            }

            return list;
        }

        public static DateTime AddBusinessDays(this DateTime current, int days)
        {
            var sign = Math.Sign(days);
            var unsignedDays = Math.Abs(days);
            for (var i = 0; i < unsignedDays; i++)
            {
                do
                {
                    current = current.AddDays(sign);
                }
                while (current.DayOfWeek == DayOfWeek.Saturday ||
                    current.DayOfWeek == DayOfWeek.Sunday);
            }
            return current;
        }

        public static double GetMax(this Axis axis)
        {
            return ((IAxis)axis).GetMax();
        }

        public static double GetMin(this Axis axis)
        {
            return ((IAxis)axis).GetMin();
        }
    }
}
