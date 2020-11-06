using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using C1.C1Schedule;
using C1.WPF.Schedule;

namespace SchedulerSamples
{
    /// <summary>
    /// Interaction logic for DefaultSchedulerWindow.xaml
    /// </summary>
    public partial class DefaultSchedulerWindow : UserControl
    {
		public DefaultSchedulerWindow()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();
            // initialize time scale combo
            scale.Items.Add(TimeSpan.FromMinutes(10));
            scale.Items.Add(TimeSpan.FromMinutes(15));
            scale.Items.Add(TimeSpan.FromMinutes(20));
            scale.Items.Add(TimeSpan.FromMinutes(30));
            scale.Items.Add(TimeSpan.FromMinutes(60));
            scale.Items.Add(TimeSpan.FromHours(2));
            scale.SelectedIndex = 3;

            scheduler1.DataStorage.ContactStorage.ListChanged += new System.ComponentModel.ListChangedEventHandler(ContactStorage_ListChanged);
			scheduler1.LayoutUpdated += new EventHandler(scheduler1_LayoutUpdated);
		}

		// Fill contact information after adding new contact from the Appointment form.
		void ContactStorage_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
		{
			if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
			{
				Contact c = scheduler1.DataStorage.ContactStorage.Contacts[e.NewIndex];
				// you have to fill at least MenuCaption as 
				// it's mapped to the Employees.LastName field which doesn't allow empty strings.
				c.MenuCaption = "test contact";
			}
		}

		// refresh buttons' state if Scheduler's layout has been changed
		// by Scheduler's commands defined in default templates.
		void scheduler1_LayoutUpdated(object sender, EventArgs e)
		{
            if (scheduler1.Style == scheduler1.MonthStyle)
            {
                views.SelectedIndex = 3;
                scale.IsEnabled = false;
            }
            else
            {
                if (scheduler1.Style == scheduler1.OneDayStyle)
                {
                    views.SelectedIndex = 0;
                }
                else if (scheduler1.Style == scheduler1.WorkingWeekStyle)
                {
                    views.SelectedIndex = 1;
                }
                else if (scheduler1.Style == scheduler1.WeekStyle)
                {
                    views.SelectedIndex = 2;
                }
                else
                {
                    views.SelectedIndex = 4;
                }
                scale.IsEnabled = true;
            }
        }

		void views_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (scheduler1 == null)
			{
				return;
			}
			switch (views.SelectedIndex)
			{
				case 0:
					SetStyle(scheduler1.OneDayStyle);
					break;
				case 1:
					SetStyle(scheduler1.WorkingWeekStyle);
					break;
				case 2:
					SetStyle(scheduler1.WeekStyle);
					break;
				case 3:
					SetStyle(scheduler1.MonthStyle);
					break;
                case 4:
                    SetStyle(scheduler1.TimeLineStyle);
                    break;
            }
		}
		private void SetStyle(Style style)
		{
			if (!IsLoaded || scheduler1.Style == style)
			{
				return;
			}
			scheduler1.BeginUpdate();
			try
			{
				scheduler1.ChangeStyle(style);
			}
			finally
			{
				scheduler1_LayoutUpdated(null, null);
				// Always call EndUpdate to apply all changes.
				scheduler1.EndUpdate();
			}
		}

		// Detect whether or not this application has the requested permission
		private bool IsPermissionGranted(CodeAccessPermission requestedPermission)
		{
			try
			{
				// Try and get this permission
				requestedPermission.Demand();
				return true;
			}
			catch
			{
				return false;
			}
		}

        void Import_Click(Object sender, RoutedEventArgs e)
		{
			if (IsPermissionGranted(new FileIOPermission(PermissionState.Unrestricted)))
			{
				// Import previously save data from .xml, .ics or .dat file.
				C1Scheduler.ImportCommand.Execute(null, scheduler1);
			}
			else
			{
				try
				{
					IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
					using (IsolatedStorageFileStream stream =
						new IsolatedStorageFileStream("C1ScheduleData.xml", FileMode.Open, storage))
					{
						scheduler1.DataStorage.Import(stream, FileFormatEnum.XML);
					}
				}
				catch
				{
					MessageBox.Show("There is no permission to load data.",
							"ComponentOne WPF Sample Explorer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				MessageBox.Show("There is no permission to load data. Data have been loaded from the isolated storage.",
					"ComponentOne WPF Sample Explorer", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		void Export_Click(Object sender, RoutedEventArgs e)
		{
			if (IsPermissionGranted(new FileIOPermission(PermissionState.Unrestricted)))
			{
				// Export all data to file.
				C1Scheduler.ExportCommand.Execute(null, scheduler1);
			}
			else
			{
				try
				{
					IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
					using (IsolatedStorageFileStream stream =
						new IsolatedStorageFileStream("C1ScheduleData.xml", FileMode.Create, storage))
					{
						scheduler1.DataStorage.Export(stream, FileFormatEnum.XML);
					}
				}
				catch
				{
					MessageBox.Show("There is no permission to save data.",
						Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
					return;
				}
				MessageBox.Show("There is no permission to save file. Data have been saved to the isolated storage.",
					Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
		}

		private void themes_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (!IsLoaded)
				return;
			scheduler1.BeginUpdate();

			string themeName = themes.SelectedItem.ToString();
			switch (themeName)
			{
                // embedded themes
				case "Office 2007 Blue":
					scheduler1.Theme = C1SchedulerResources.Office2007Blue;
                    calendar1.Theme = C1CalendarResources.Office2007Blue;
					break;
				case "Office 2007 Silver":
					scheduler1.Theme = C1SchedulerResources.Office2007Silver;
                    calendar1.Theme = C1CalendarResources.Office2007Silver;
					break;
				case "Office 2007 Black":
					scheduler1.Theme = C1SchedulerResources.Office2007Black;
                    calendar1.Theme = C1CalendarResources.Office2007Black;
					break;
                case "Office 2010 Blue":
                    scheduler1.Theme = C1SchedulerResources.Office2010Blue;
                    calendar1.Theme = C1CalendarResources.Office2010Blue;
                    break;
                case "Office 2010 Silver":
                    scheduler1.Theme = C1SchedulerResources.Office2010Silver;
                    calendar1.Theme = C1CalendarResources.Office2010Silver;
                    break;
                case "Office 2010 Black":
                    scheduler1.Theme = C1SchedulerResources.Office2010Black;
                    calendar1.Theme = C1CalendarResources.Office2010Black;
                    break;
                case "Office 2016 Black":
                    scheduler1.Theme = C1SchedulerResources.Office2016Black;
                    calendar1.Theme = C1CalendarResources.Office2016Black;
                    break;
                case "Office 2016 White":
                    scheduler1.Theme = C1SchedulerResources.Office2016White;
                    calendar1.Theme = C1CalendarResources.Office2016White;
                    break;
                case "Office 2016 Colorful":
                    scheduler1.Theme = C1SchedulerResources.Office2016Colorful;
                    calendar1.Theme = C1CalendarResources.Office2016Colorful;
                    break;
                case "Office 2016 DarkGray":
                    scheduler1.Theme = C1SchedulerResources.Office2016DarkGray;
                    calendar1.Theme = C1CalendarResources.Office2016DarkGray;
                    break;
                // legacy themes
                case "Dusk Blue":
                    scheduler1.Theme = calendar1.Theme = C1.WPF.Schedule.Legacy.Themes.DuskBlue;
                    break;
                case "Dusk Green":
                    scheduler1.Theme = calendar1.Theme = C1.WPF.Schedule.Legacy.Themes.DuskGreen;
                    break;
                case "Vista":
                    scheduler1.Theme = calendar1.Theme = C1.WPF.Schedule.Legacy.Themes.Vista;
                    break;
                case "Media Player":
                    scheduler1.Theme = calendar1.Theme = C1.WPF.Schedule.Legacy.Themes.MediaPlayer;
                    break;
            }
            scheduler1_LayoutUpdated(null, null);
			scheduler1.EndUpdate();
		}
    }
}