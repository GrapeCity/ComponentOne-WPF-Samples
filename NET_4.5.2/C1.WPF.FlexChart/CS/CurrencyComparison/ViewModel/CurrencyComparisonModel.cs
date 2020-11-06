using System;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace CurrencyComparison
{
    class CurrencyComparisonModel
    {
        string _currency = "US Dollars - USD,Euro - EUR, Japanese Yen - JPY, Indian Rupee - INR, Chinese Yuan - CNY,Russian Rouble - RUB, Korean Won - KRW,British Pound - GBP";
        Dictionary<string, MeasureMode> _dictMeasureMode;
        Dictionary<string, TimeFrame> _dictTimeFrame;
        List<Currency> _currencies;
        List<Color> _chartColors;

        public Dictionary<string, MeasureMode> DictMeasureMode
        {
            get
            {
                if (_dictMeasureMode == null)
                {
                    _dictMeasureMode = new Dictionary<string, MeasureMode>();
                    _dictMeasureMode.Add("Exchange Rate", MeasureMode.ExchangeRate);
                    _dictMeasureMode.Add("Percentage Change", MeasureMode.PercentageChange);
                    _dictMeasureMode.Add("Both", MeasureMode.Both);
                }

                return _dictMeasureMode;
            }
        }

        public List<Currency> Currencies
        {
            get
            {
                if (_currencies == null)
                {
                    var currencies = _currency.Split(',');
                    _currencies = new List<Currency>();
                    currencies.ToList().ForEach(cr =>
                    {
                        _currencies.Add(new Currency()
                        {
                            DisplayName = cr.Split('-')[0].Trim(),
                            Symbol = cr.Split('-')[1].Trim()
                        });
                    });
                }

                return _currencies;
            }
        }

        public List<Color> ChartColors
        {
            get
            {
                if (_chartColors == null)
                {
                    _chartColors = new List<Color>();
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#2a9fd6"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#77b300"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#9933cc"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#ff8800"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#cc0000"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#00cca3"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#3d6dcc"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#525252"));
                    _chartColors.Add((Color)ColorConverter.ConvertFromString("#323232"));
                }

                return _chartColors;
            }
        }

        public Dictionary<string, TimeFrame> DictTimeFrame
        {
            get
            {
                if (_dictTimeFrame == null)
                {
                    _dictTimeFrame = new Dictionary<string, TimeFrame>();
                    _dictTimeFrame.Add("5D", TimeFrame.FiveDays);
                    _dictTimeFrame.Add("10D", TimeFrame.TenDays);
                    _dictTimeFrame.Add("1M", TimeFrame.OneMonth);
                    _dictTimeFrame.Add("6M", TimeFrame.SixMonths);
                    _dictTimeFrame.Add("1Y", TimeFrame.OneYear);
                    _dictTimeFrame.Add("5Y", TimeFrame.FiveYears);
                    _dictTimeFrame.Add("10Y", TimeFrame.TenYears);
                }

                return _dictTimeFrame;
            }
        }

    }
}
