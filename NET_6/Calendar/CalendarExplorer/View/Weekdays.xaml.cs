using System;
using System.Collections.Generic;
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
    public partial class Weekdays : UserControl
    {
        public Weekdays()
        {
            InitializeComponent();

            Tag = AppResources.WeekdaysDescription;

            calendar.SelectionChanging += OnSelectionChanging;
        }

        private void OnSelectionChanging(object sender, CalendarSelectionChangingEventArgs e)
        {
            foreach (var date in e.SelectedDates.ToArray())
            {
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    e.SelectedDates.Remove(date);
            }
        }
    }
}