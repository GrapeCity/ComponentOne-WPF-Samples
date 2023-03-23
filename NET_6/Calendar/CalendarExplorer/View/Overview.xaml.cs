using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using C1.WPF.Calendar;
using C1.WPF.Core;
using C1.WPF.DateTimeEditors;
using C1.WPF.Input;
using CalendarExplorer.Resources;

namespace CalendarExplorer
{
    /// <summary>
    ///     Interaction logic for Overview.xaml
    /// </summary>
    public partial class Overview : UserControl
    {
        private ObservableCollection<DateTime> _boldDates;

        public Overview()
        {
            InitializeComponent();

            Tag = AppResources.OverviewDescription;

            cbDayOfWeek.ItemsSource = new List<DayOfWeek>
            {
                DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday,
                DayOfWeek.Saturday, DayOfWeek.Sunday
            };
            cbDayOfWeek.SelectedItemChanged += CbDayOfWeek_SelectedItemChanged;

            dpMinDate.SelectedDateChanged += DpMinDate_SelectedDateChanged;
            dpMaxDate.SelectedDateChanged += DpMaxDate_SelectedDateChanged;

            cbkShowHeader.IsChecked = true;
            cbkShowHeader.Checked += CbkShowHeader_Checked;
            cbkShowHeader.Unchecked += CbkShowHeader_Unchecked;

            cbkShowNavigationButton.IsChecked = true;
            cbkShowNavigationButton.Checked += CbkShowNavigationButton_Checked;
            cbkShowNavigationButton.Unchecked += CbkShowNavigationButton_Unchecked;

            cbkShowAdjacentDay.IsChecked = true;
            cbkShowAdjacentDay.Checked += CbkShowAdjacentDay_Checked;
            cbkShowAdjacentDay.Unchecked += CbkShowAdjacentDay_Unchecked;

            cbOrientation.ItemsSource = new List<Orientation> { Orientation.Vertical, Orientation.Horizontal };
            cbOrientation.SelectedItemChanged += CbOrientation_SelectedItemChanged;


            cbDayOfWeekFormat.ItemsSource = new List<string>() { "d", "dd", "ddd", "dddd" };
            cbDayOfWeekFormat.SelectedItemChanged += CbDayOfWeekFormat_SelectedItemChanged; ;


            cbHeaderFormat.ItemsSource = new List<string> { "M yyyy", "MMM yyyy", "MMMM yyyy" };
            cbHeaderFormat.SelectedItemChanged += CbHeaderFormat_SelectedItemChanged;

            cbCalendarType.ItemsSource = new List<CalendarType> { CalendarType.Default, CalendarType.Gregorian, CalendarType.Japanese };
            cbCalendarType.SelectedItemChanged += CbCalendarType_SelectedItemChanged;

            cbSelectionMode.ItemsSource = Enum.GetValues(typeof(SelectionMode)).Cast<SelectionMode>();
            cbSelectionMode.SelectedItemChanged += cbSelectionMode_SelectedItemChanged;

            //calendar.DisplayDate = new DateTime(2023, 1, 1);
            calendar.SelectionChanging += Calendar_SelectionChanging;
            calendar.SelectionChanged += Calendar_SelectionChanged;

            _boldDates = new ObservableCollection<DateTime> {DateTime.Today.AddDays(-3), DateTime.Today.AddDays(1) };
            calendar.BoldedDates = _boldDates;

        }

        private void Calendar_SelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
        {

        }

        private void Calendar_SelectionChanging(object sender, CalendarSelectionChangingEventArgs e)
        {

        }

        private void cbSelectionMode_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {

            var selectionMode = (sender as C1ComboBox).SelectedItem;

            if (selectionMode != null)
            {
                calendar.SelectionMode = (SelectionMode)selectionMode;
            }

        }

        private void CbCalendarType_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {

            var calendarType = (sender as C1ComboBox).SelectedItem;

            if (calendarType != null)
            {
                calendar.CalendarType = (CalendarType)calendarType;
            }

        }

        private void CbDayOfWeekFormat_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            calendar.DayOfWeekFormat = (string)((C1ComboBox)sender).SelectedItem;
        }

        private void CbHeaderFormat_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            calendar.HeaderMonthFormat = (string)e.NewValue;
        }

        private void CbkShowAdjacentDay_Unchecked(object sender, RoutedEventArgs e)
        {
            calendar.ShowAdjacentDay = false;
        }

        private void CbkShowAdjacentDay_Checked(object sender, RoutedEventArgs e)
        {
            calendar.ShowAdjacentDay = true;
        }

        private void CbOrientation_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {

            var orientation = ((C1ComboBox)sender).SelectedItem;

            if (orientation != null)
            {
                calendar.Orientation = (Orientation)orientation;
            }
        }


        private void CbkShowNavigationButton_Unchecked(object sender, RoutedEventArgs e)
        {
            calendar.ShowNavigationButtons = false;
        }

        private void CbkShowNavigationButton_Checked(object sender, RoutedEventArgs e)
        {
            calendar.ShowNavigationButtons = true;
        }

        private void CbkShowHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            calendar.ShowHeader = false;
        }

        private void CbkShowHeader_Checked(object sender, RoutedEventArgs e)
        {
            calendar.ShowHeader = true;
        }

        private void CbDayOfWeek_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (cbDayOfWeek.SelectedItem != null)
            {
                calendar.FirstDayOfWeek = (DayOfWeek)cbDayOfWeek.SelectedItem;
            }
            else
            {
                calendar.FirstDayOfWeek = null;
            }
        }

        private void DpMaxDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            calendar.MaxDate = (sender as C1DatePicker).SelectedDate;
        }

        private void DpMinDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            calendar.MinDate = (sender as C1DatePicker).SelectedDate;
        }
    }
}