using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows;

namespace LocaleDateGrid
{
    public partial class MainWindow : Window
    {
        private List<DateTimeItem> _dateTimeItems;

        public MainWindow()
        {
            InitializeComponent();

            _dateTimeItems = new List<DateTimeItem>
            {
                new DateTimeItem{ Label= AppResources.NowLabel },
                new DateTimeItem{ Label= AppResources.Plus3HoursLabel },
                new DateTimeItem{ Label= AppResources.Plus6HoursLabel },
                new DateTimeItem{ Label= AppResources.Plus12HoursLabel },
            };
            grid.ItemsSource = _dateTimeItems;
            UpdateTimes(DateTimeOffset.Now);
            _ = RunUpdates();
        }

        private void UpdateTimes(DateTimeOffset next)
        {
            _dateTimeItems[0].DateTime = next;
            _dateTimeItems[1].DateTime = next.AddHours(3);
            _dateTimeItems[2].DateTime = next.AddHours(6);
            _dateTimeItems[3].DateTime = next.AddHours(12);
        }

        private async Task RunUpdates()
        {
            while (true)
            {
                var now = DateTime.Now;
                var date = DateOnly.FromDateTime(now);
                var time = new TimeOnly(now.Hour, now.Minute, now.Second);
                var next = new DateTime(date, time) + TimeSpan.FromSeconds(1);
                var diff = next - now;
                await Task.Delay(diff > TimeSpan.Zero ? diff : TimeSpan.Zero);
                UpdateTimes(next);
            }
        }

        [ObservableObject]
        private partial class DateTimeItem
        {
            [ObservableProperty]
            public partial string Label { get; set; }

            [ObservableProperty]
            public partial DateTimeOffset DateTime { get; set; }
        }
    }
}