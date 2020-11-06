using System;
using System.Collections.Generic;
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
using System.Data;
using C1.WPF.Schedule;
using C1.C1Schedule;
using System.ComponentModel;
using System.Collections.Specialized;

namespace MultiUser
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        #region ** fields
        private MultiUser.NwindDataSetTableAdapters.AppointmentsTableAdapter appointmentsTableAdapter = new NwindDataSetTableAdapters.AppointmentsTableAdapter();
        private MultiUser.NwindDataSetTableAdapters.EmployeesTableAdapter employeesTableAdapter = new NwindDataSetTableAdapters.EmployeesTableAdapter();
		private MultiUser.NwindDataSetTableAdapters.CustomersTableAdapter customersTableAdapter = new NwindDataSetTableAdapters.CustomersTableAdapter();
		private NwindDataSet dataSet = new NwindDataSet();
        #endregion

        #region ** initializing
        public Window1()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();
			
			Scheduler.ReminderFire += new EventHandler<ReminderActionEventArgs>(scheduler_ReminderFire);

			Scheduler.GroupItems.CollectionChanged += new NotifyCollectionChangedEventHandler(GroupItems_CollectionChanged);
			
			// get data from the data base
            this.employeesTableAdapter.Fill(dataSet.Employees);
			this.customersTableAdapter.Fill(dataSet.Customers);
			this.appointmentsTableAdapter.Fill(dataSet.Appointments);
			
			// set mappings and DataSource for the AppointmentStorages
			AppointmentStorage storage = Scheduler.DataStorage.AppointmentStorage;
			storage.Mappings.AppointmentProperties.MappingName = "Properties";
			storage.Mappings.Body.MappingName = "Description";
			storage.Mappings.End.MappingName = "End";
			storage.Mappings.IdMapping.MappingName = "AppointmentId";
			storage.Mappings.Location.MappingName = "Location";
			storage.Mappings.Start.MappingName = "Start";
			storage.Mappings.Subject.MappingName = "Subject";
			storage.Mappings.OwnerIndexMapping.MappingName = "Owner";
			storage.DataSource = dataSet.Appointments;

			// set mappings and DataSource for the OwnerStorage
			ContactStorage ownerStorage = Scheduler.DataStorage.OwnerStorage;
			((INotifyCollectionChanged)ownerStorage.Contacts).CollectionChanged += new NotifyCollectionChangedEventHandler(Owners_CollectionChanged);
			ownerStorage.Mappings.IndexMapping.MappingName = "EmployeeId";
			ownerStorage.Mappings.TextMapping.MappingName = "FirstName";
			ownerStorage.DataSource = dataSet.Employees;
			
			// set mappings and DataSource for the ContactStorage
			ContactStorage cntStorage = Scheduler.DataStorage.ContactStorage;
			((INotifyCollectionChanged)cntStorage.Contacts).CollectionChanged += new NotifyCollectionChangedEventHandler(Contacts_CollectionChanged);
			cntStorage.Mappings.IdMapping.MappingName = "CustomerId";
			cntStorage.Mappings.TextMapping.MappingName = "CompanyName";
			cntStorage.DataSource = dataSet.Customers;

            btnDay.IsChecked = true;
            Scheduler.StyleChanged += new System.EventHandler<RoutedEventArgs>(Scheduler_StyleChanged);
        }

		void GroupItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			foreach (SchedulerGroupItem group in Scheduler.GroupItems)
			{
				if (group.Owner != null)
				{
					// set SchedulerGroupItem.Tag property to the data row. That allows to use data row fields in xaml for binding
					int index = (int)group.Owner.Key[0];
					MultiUser.NwindDataSet.EmployeesRow row = dataSet.Employees.Rows.Find(index) as MultiUser.NwindDataSet.EmployeesRow;
					group.Tag = row;
				}
			}
		}

		void Owners_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (Contact cnt in e.NewItems)
				{
					MultiUser.NwindDataSet.EmployeesRow row = dataSet.Employees.Rows.Find(cnt.Key[0]) as MultiUser.NwindDataSet.EmployeesRow;
					if (row != null)
					{
						// set Contact.MenuCaption to the FirstName + LastName string
						cnt.MenuCaption = row["FirstName"].ToString() + " " + row["LastName"].ToString(); 
					}
				}
			}
		}
		void Contacts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add)
			{
				foreach (Contact cnt in e.NewItems)
				{
					MultiUser.NwindDataSet.CustomersRow row = dataSet.Customers.Rows.Find(cnt.Key[0]) as MultiUser.NwindDataSet.CustomersRow;
					if (row != null)
					{
						// set Contact.MenuCaption to the CompanyName + ContactName string
						cnt.MenuCaption = row["CompanyName"].ToString() + " (" + row["ContactName"].ToString() + ")";
					}
				}
			}
		}

        #endregion

        #region ** misc
        // save changes
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.appointmentsTableAdapter.Update(dataSet.Appointments);
            base.OnClosing(e);
        }
        // prevent showing reminders
        private void scheduler_ReminderFire(object sender, ReminderActionEventArgs e)
        {
            e.Handled = true;
        }
        private void Scheduler_StyleChanged(object sender, RoutedEventArgs e)
        {
            if (Scheduler.Style == Scheduler.TimeLineStyle)
            {
                // update group header (use different headers for TimeLine and other views)
                Scheduler.GroupHeaderTemplate = (DataTemplate)Resources["myCustomTimeLineGroupHeaderTemplate"];
                btnTimeLine.IsChecked = true;
            }
            else
            {
                // update group header (use different headers for TimeLine and other views)
                Scheduler.GroupHeaderTemplate = (DataTemplate)Resources["myCustomGroupHeaderTemplate"];
                // update toolbar buttons state according to the current C1Scheduler view
                if (Scheduler.Style == Scheduler.WorkingWeekStyle)
                {
                    btnWorkWeek.IsChecked = true;
                }
                else if (Scheduler.Style == Scheduler.WeekStyle)
                {
                    btnWeek.IsChecked = true;
                }
                else if (Scheduler.Style == Scheduler.MonthStyle)
                {
                    btnMonth.IsChecked = true;
                }
                else
                {
                    btnDay.IsChecked = true;
                }
            }
        }
        #endregion
    }
}
