using C1.Schedule;
using C1.WPF.Schedule;
using C1.WPF.Theming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchedulerTableViews
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //----------------------------------------
        #region ** fields
        private const string USHolidaysFile = "US32Holidays.ics";
        private const string USHolidaysDownloadUri = "https://www.officeholidays.com/ics-fed/usa";
        private static string TEMP_DIR = System.Environment.GetEnvironmentVariable("tmp");

        private SchedulerTableViews.C1NwindDataSetTableAdapters.AppointmentsTableAdapter appointmentsTableAdapter = new SchedulerTableViews.C1NwindDataSetTableAdapters.AppointmentsTableAdapter();
        private SchedulerTableViews.C1NwindDataSetTableAdapters.AppointeesTableAdapter appointeesTableAdapter = new SchedulerTableViews.C1NwindDataSetTableAdapters.AppointeesTableAdapter();
        private C1NwindDataSet dataSet = new C1NwindDataSet();

        private C1.WPF.Toolbar.C1ToolbarTabControl _tabControl;
        #endregion

        //----------------------------------------
        #region ** Initializing
        public MainWindow()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);

            InitializeComponent();

            // get data from the data base
            this.appointeesTableAdapter.Fill(dataSet.Appointees);
            this.appointmentsTableAdapter.Fill(dataSet.Appointments);

            // set mappings and DataSource for the ContactStorage
            ContactStorage cstorage = scheduler.DataStorage.ContactStorage;
            cstorage.Mappings.IndexMapping.MappingName = "EmployeeID";
            cstorage.Mappings.TextMapping.MappingName = "FirstName";
            cstorage.DataMember = "Appointees";
            cstorage.DataSource = dataSet.Appointees;

            // set correct MenuCaption for contacts
            foreach (Contact cnt in scheduler.DataStorage.ContactStorage.Contacts)
            {
                C1NwindDataSet.AppointeesRow row = dataSet.Appointees.FindByEmployeeID((int)cnt.Key[0]);
                if (row != null)
                {
                    cnt.MenuCaption = row["FirstName"].ToString() + " " + row["LastName"].ToString();
                }
            }

            // set mappings and DataSource for the AppointmentStorage
            AppointmentStorage storage = scheduler.DataStorage.AppointmentStorage;
            storage.Mappings.AppointmentProperties.MappingName = "Properties";
            storage.Mappings.Body.MappingName = "Body";
            storage.Mappings.End.MappingName = "End";
            storage.Mappings.IdMapping.MappingName = "Id";
            storage.Mappings.Location.MappingName = "Location";
            storage.Mappings.Start.MappingName = "Start";
            storage.Mappings.Subject.MappingName = "Subject";
            storage.DataMember = "Appointments";
            storage.DataSource = dataSet.Appointments;

            themeList.SelectedItem = "Colorful";
            agendaRange.SelectedItem = "Week";

            Closing += MainWindow_Closing;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // add US holidays from public calendar
            var fileName = TEMP_DIR + USHolidaysFile;
            // use cached file if it already exists in working folder and not old
            if (!System.IO.File.Exists(fileName) || DateTime.Today.Subtract(System.IO.File.GetCreationTime(fileName)).TotalDays > 90)
            {
                // get newer version
                System.Net.WebClient webClient = new System.Net.WebClient();
                webClient.DownloadFile(USHolidaysDownloadUri, fileName);
            }
            try
            {
                scheduler.DataStorage.Import(fileName, C1.Schedule.FileFormatEnum.iCal);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
            }
        }

        // On closing update data adapters in order to save data to the database.
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // this.appointeesTableAdapter.Update(dataSet.Appointees);
            // this.appointmentsTableAdapter.Update(dataSet.Appointments);
        }

        #endregion

        //----------------------------------------
        #region ** Switching Views
        private void scheduler_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            switch (scheduler.ViewType)
            {
                case C1.WPF.Schedule.ViewType.Day:
                    dayButton.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.Month:
                    monthButton.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.TimeLine:
                    timelineButton.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.Week:
                    weekButton.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.WorkingWeek:
                    workWeekButton.IsChecked = true;
                    break;
            }
        }

        private void gotoToday_Click(object sender, RoutedEventArgs e)
        {
            HideTableView();
            // Go to today date.
            scheduler.SelectedDateTime = DateTime.Today;
        }

        private void next7days_Click(object sender, RoutedEventArgs e)
        {
            HideTableView();
            // Show next 7 days.
            List<DateTime> days = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                days.Add(DateTime.Today.AddDays(i));
            }
            scheduler.BeginUpdate();
            if (scheduler.Style != scheduler.TimeLineStyle)
            {
                scheduler.ChangeStyle(scheduler.OneDayStyle);
            }
            scheduler.VisualStartTime = DateTime.Today;
            scheduler.VisualTimeSpan = TimeSpan.FromDays(7);
            scheduler.EndUpdate();
        }

        private void calendarView_Click(object sender, RoutedEventArgs e)
        {
            HideTableView();
        }

        private void listButton_Checked(object sender, RoutedEventArgs e)
        {
            ShowTableView(false);
        }

        private void activeButton_Checked(object sender, RoutedEventArgs e)
        {
            ShowTableView(true);
        }

        private void HideTableView()
        {
            calendarView.Visibility = Visibility.Visible;
            tableView.Visibility = Visibility.Collapsed;
            // update toggle buttons state
            listButton.IsChecked = activeButton.IsChecked = false;
        }

        private void ShowTableView(bool active)
        {
            calendarView.Visibility = Visibility.Collapsed;
            tableView.Active = active;
            tableView.Visibility = Visibility.Visible;
            // update toggle buttons state
            dayButton.IsChecked = workWeekButton.IsChecked = weekButton.IsChecked = monthButton.IsChecked = timelineButton.IsChecked = false;
        }
        #endregion

        //----------------------------------------
        #region ** Settings
        private void themeList_SelectionChanged(object sender, C1.WPF.SelectionChangedEventArgs<int> e)
        {
            if (themeList.SelectedItem == null)
                return;

            themeDropDown.IsDropDownOpen = false;
            scheduler.BeginUpdate();

            C1Theme theme = null;

            switch (themeList.SelectedItem.ToString())
            {
                case "Black":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016Black();
                    break;
                case "White":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016White();
                    break;
                case "Colorful":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016Colorful();
                    break;
                case "Dark Gray":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016DarkGray();
                    break;
            }
            C1Theme.ApplyTheme(this, theme);
            var rd = theme.GetNewResourceDictionary();
            scheduler.Theme = calendar.Theme = rd;

            // apply themed FlexGrid style to inherited controls
            var flexGridStyle = rd[typeof(C1.WPF.FlexGrid.C1FlexGrid)] as Style;
            if (flexGridStyle != null)
            {
                agendaView.Style = flexGridStyle;
                tableView.Style = flexGridStyle;
            }
            var adornerLayer = AdornerLayer.GetAdornerLayer(this);
            if (adornerLayer != null)
            {
                // this will aplly theme to everything displayed in adorner, including any C1Window instances
                C1Theme.ApplyTheme(adornerLayer, theme);
            }
            //     scheduler_LayoutUpdated(null, null);
            scheduler.EndUpdate();
        }

        private void showAgendaChk_Checked(object sender, RoutedEventArgs e)
        {
            if (agendaView != null)
            {
                agendaView.Visibility = Visibility.Visible;
            }
        }

        private void showAgendaChk_Unchecked(object sender, RoutedEventArgs e)
        {
            if (agendaView != null)
            {
                agendaView.Visibility = Visibility.Collapsed;
            }
        }

        private void agendaRange_SelectionChanged(object sender, C1.WPF.SelectionChangedEventArgs<int> e)
        {
            if (agendaRange.SelectedItem != null && agendaView != null)
            {
                switch(agendaRange.SelectedItem.ToString())
                {
                    case "Today":
                        this.agendaView.ViewType = AgendaViewType.Today;
                        break;
                    case "Week":
                        this.agendaView.ViewType = AgendaViewType.Week;
                        break;
                    default:
                        this.agendaView.ViewType = AgendaViewType.DateRange;
                        break;
                }
                agendaDropDown.IsDropDownOpen = false;
            }
        }
        #endregion

        //----------------------------------------
        #region ** Show/Hide contextual tab for Appointment editing
        // Only show contextual Appointment tab if some appointment is selected in the active view.
        private void scheduler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var hitTest = VisualTreeHelper.HitTest(scheduler, e.GetPosition(scheduler));
            var fe = hitTest?.VisualHit as FrameworkElement;
            if ( fe != null && fe.DataContext is IntervalAppointment)
            {
                UpdateAppointmentTab(((IntervalAppointment)fe.DataContext).Appointment);
            }
            else
            {
                UpdateAppointmentTab();
            }
        }

        private void tableView_SelectedAppointmentsChanged(object sender, EventArgs e)
        {
            UpdateAppointmentTab();
        }

        private void UpdateAppointmentTab(Appointment app = null)
        {
            Appointment oldApp = appointmentTab.DataContext as Appointment;

            if ( app == null)
            { 
                if (tableView.IsVisible)
                {
                    if ( tableView.SelectedAppointments.Count > 0)
                        app = tableView.SelectedAppointments[0];
                }
                else if (agendaView.IsKeyboardFocusWithin)
                {
                    if (agendaView.SelectedAppointments.Count > 0)
                        app = agendaView.SelectedAppointments[0];
                }
            }

            if ( oldApp == app)
            {
                // no change
                return;
            }

            if (oldApp != null)
            {
                // unsubscribe from events
                ((INotifyPropertyChanged)oldApp).PropertyChanged -= currentAppointment_PropertyChanged;
            }

            appointmentTab.DataContext = app;
            if (app != null)
            {
                appointmentTab.Visibility = Visibility.Visible;
                UpdateImportanceButtons(app);
                ((INotifyPropertyChanged)app).PropertyChanged += currentAppointment_PropertyChanged;

                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (System.Action)(() =>
                {
                    // select tab via BeginInvoke, so that control can fully initialize it before selection
                    appointmentTab.IsSelected = true;
                }));
            }
            else
            {
                appointmentTab.IsSelected = false;
                appointmentTab.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        //----------------------------------------
        #region ** Working with Appointments
        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (tableView.IsVisible)
            {
                if (tableView.SelectedAppointments.Count > 0)
                    C1Scheduler.DeleteAppointmentCommand.Execute(tableView.SelectedAppointments.ToList<Appointment>(), scheduler);
            }
            else
            {
                scheduler.DeleteAppointment(appointmentTab.DataContext as Appointment);
            }
        }

        private void currentAppointment_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateImportanceButtons(sender as Appointment);
        }

        private void highImportance_Checked(object sender, RoutedEventArgs e)
        {
            Appointment app = appointmentTab.DataContext as Appointment;
            if (app != null)
            {
                app.Importance = ImportanceEnum.High;
                UpdateImportanceButtons(app);
            }
        }
        private void highImportance_Unchecked(object sender, RoutedEventArgs e)
        {
            Appointment app = appointmentTab.DataContext as Appointment;
            if (app != null)
            {
                if (lowImportanceButton.IsChecked.HasValue && lowImportanceButton.IsChecked.Value)
                {
                    app.Importance = ImportanceEnum.Low;
                }
                else
                {
                    app.Importance = ImportanceEnum.Normal;
                }
                UpdateImportanceButtons(app);
            }
        }
        private void lowImportance_Checked(object sender, RoutedEventArgs e)
        {
            Appointment app = appointmentTab.DataContext as Appointment;
            if (app != null)
            {
                app.Importance = ImportanceEnum.Low;
                UpdateImportanceButtons(app);
            }
        }
        private void lowImportance_Unchecked(object sender, RoutedEventArgs e)
        {
            Appointment app = appointmentTab.DataContext as Appointment;
            if (app != null)
            {
                if (highImportanceButton.IsChecked.HasValue && highImportanceButton.IsChecked.Value)
                {
                    app.Importance = ImportanceEnum.High;
                }
                else
                {
                    app.Importance = ImportanceEnum.Normal;
                }
                UpdateImportanceButtons(app);
            }
        }

        private void UpdateImportanceButtons(Appointment app)
        {
            highImportanceButton.IsChecked = app.Importance == ImportanceEnum.High;
            lowImportanceButton.IsChecked = app.Importance == ImportanceEnum.Low;
        }
        #endregion
    }
}
