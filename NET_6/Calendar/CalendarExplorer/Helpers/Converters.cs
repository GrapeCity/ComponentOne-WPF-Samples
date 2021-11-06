using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace CalendarExplorer
{
    public static class LocalDataStorage
    {
        internal static IDictionary<DateTime, string> NoteDictionary = new ConcurrentDictionary<DateTime, string>();
        internal static IDictionary<DateTime, string> WeatherDictionary = new ConcurrentDictionary<DateTime, string>();


        internal static void Note(DateTime date)
        {
            NoteDictionary[date] = RandomEventText();
        }

        internal static string GetNote(DateTime date)
        {
            if (NoteDictionary.ContainsKey(date)) return NoteDictionary[date];

            return "";
        }

        public static string GetWeather(DateTime date)
        {
            if (WeatherDictionary.ContainsKey(date))
            {
                return WeatherDictionary[date];
            }

            var weatherText = RandomWhetherText();

            WeatherDictionary[date] = weatherText;

            return weatherText;
        }

        private static string RandomEventText()
        {
            var randomTexts = new List<string>
            {
                "Birthday party at 14h", "Go to cooking class at 18h15", "Get to work at 7h", "Go to bed at 21h",
                "Go to school at 8h"
            };

            return RandomText(randomTexts);
        }

        private static string RandomWhetherText()
        {
            var randomTexts = new List<string>
            {
                "18.5°C - Mostly cloudy ☁", "25.5°C - Sunny ⛅", "18.5°C - Cloudy ☁", "16.5°C - Rainy ⛈",
                "13.5°C - Mostly cloudy ☁", "18.5°C - Mostly cloudy ☁"
            };

            return RandomText(randomTexts);
        }

        private static string RandomText(List<string> texts)
        {
            return texts[new Random().Next(texts.Count - 1)];
        }
    }

    public class NoteFromDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date) return LocalDataStorage.GetNote(date);

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Today;
        }
    }

    public class RandomWeatherFromDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date) return LocalDataStorage.GetWeather(date);

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DateTime.Today;
        }
    }
}