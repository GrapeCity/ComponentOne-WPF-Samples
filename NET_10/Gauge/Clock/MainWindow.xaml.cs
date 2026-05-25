using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Animation;

namespace Clock
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            clockSecs.UpdateAnimation.Easing = new ElasticEase() { Oscillations = 1, Springiness = 4 };
            UpdateClock(DateTime.Now);
            _ = RunClock();
        }

        private async Task RunClock()
        {
            while (true)
            {
                var now = DateTime.Now;
                var date = DateOnly.FromDateTime(now);
                var time = new TimeOnly((now.Hour / 12) * 12 + (int)clockHours.Value / 60, (int)clockMins.Value, (int)clockSecs.Value);
                var next = new DateTime(date, time) + TimeSpan.FromSeconds(1);
                var diff = next - now;
                await Task.Delay(diff > TimeSpan.Zero ? diff : TimeSpan.Zero);
                UpdateClock(next);
            }
        }

        private void UpdateClock(DateTime datetime)
        {
            clockHours.Value = (datetime.Hour % 12) * 60 + datetime.Minute;
            clockMins.Value = datetime.Minute + datetime.Second / 60.0;
            clockSecs.Value = datetime.Second;
        }
    }

    public class HoursConverter : IValueConverter
    {
        public bool ShowRomanNumbers { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dValue && targetType == typeof(string))
            {
                if (ShowRomanNumbers)
                    return ToRoman((int)dValue / 60);
                else
                    return (dValue / 60).ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static string ToRoman(int number)
        {
            if (number <= 0 || number > 3999)
                throw new ArgumentOutOfRangeException(nameof(number), "Value must be between 1 and 3999.");

            var map = new (int value, string symbol)[]
            {
                (1000, "M"),
                (900,  "CM"),
                (500,  "D"),
                (400,  "CD"),
                (100,  "C"),
                (90,   "XC"),
                (50,   "L"),
                (40,   "XL"),
                (10,   "X"),
                (9,    "IX"),
                (5,    "V"),
                (4,    "IV"),
                (1,    "I")
            };

            var result = new System.Text.StringBuilder();

            foreach (var (value, symbol) in map)
            {
                while (number >= value)
                {
                    result.Append(symbol);
                    number -= value;
                }
            }

            return result.ToString();
        }
    }
}