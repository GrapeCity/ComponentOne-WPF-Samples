using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using C1.WPF.Calendar;
using C1.WPF.Core;
using C1.WPF.DateTimeEditors;
using C1.WPF.Input;
using CalendarExplorer.Resources;

namespace CalendarExplorer
{
    public partial class WeekSelection : UserControl
    {
        public WeekSelection()
        {
            InitializeComponent();

            Tag = AppResources.WeekSelectionDescription;

            calendar.SelectionChanging += OnSelectionChanging;
        }

        private void OnSelectionChanging(object sender, CalendarSelectionChangingEventArgs e)
        {
            var calendar = sender as C1Calendar;
            var firstDayOfWeek = calendar.FirstDayOfWeek ?? CultureInfo.CurrentUICulture.DateTimeFormat.FirstDayOfWeek;
            var selection = e.SelectedDates.ToArray();
            foreach (var date in selection)
            {
                var dayOffset = (date.DayOfWeek - firstDayOfWeek + 7) % 7;
                for (int i = 0; i < 7; i++)
                {
                    var newDate = date.AddDays(i - dayOffset);
                    if (!e.SelectedDates.Contains(newDate))
                        e.SelectedDates.Add(newDate);
                }
            }
        }
    }
}