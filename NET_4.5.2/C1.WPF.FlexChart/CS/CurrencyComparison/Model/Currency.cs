using System;

namespace CurrencyComparison
{
    public class Currency
    {
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
        public ChartSeries ExchangeRateSeries { get; set; }
        public ChartSeries PercentageChangeSeries { get; set; }
    }

    public class Data
    {
        public DateTime Date { get; set; }
        public double USD { get; set; }
        public double JPY { get; set; }
        public double GBP { get; set; }
        public double RUB { get; set; }
        public double CNY { get; set; }
        public double INR { get; set; }
        public double KRW { get; set; }
        public double EUR { get; set; }

        public double GetValue(string property)
        {
            switch (property)
            {
                case "USD":
                    return USD;
                case "JPY":
                    return JPY;
                case "GBP":
                    return GBP;
                case "RUB":
                    return RUB;
                case "CNY":
                    return CNY;
                case "INR":
                    return INR;
                case "KRW":
                    return KRW;
                case "EUR":
                default:
                    return EUR;
            }
        }
    }
}
