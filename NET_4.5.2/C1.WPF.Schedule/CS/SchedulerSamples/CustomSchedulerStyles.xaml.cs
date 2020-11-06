using System;
using System.Windows;
using System.Windows.Controls;

using C1.C1Schedule;
using C1.WPF.Schedule;

namespace SchedulerSamples
{
    /// <summary>
	/// Interaction logic for CustomSchedulerStyles.xaml
    /// </summary>
    public partial class CustomSchedulerStyles : UserControl
    {
		public CustomSchedulerStyles()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();
			
			scheduler1.DataStorage.ContactStorage.ListChanged += new System.ComponentModel.ListChangedEventHandler(ContactStorage_ListChanged);
			scheduler1.LayoutUpdated += new EventHandler(scheduler1_LayoutUpdated);
			scheduler1.BeforeViewChange += new EventHandler<BeforeViewChangeEventArgs>(scheduler1_BeforeViewChange);
		}

		// Change view from Month View mode to 2 Week mode if selection 
		// contains 2 full weeks.
		void scheduler1_BeforeViewChange(object sender, BeforeViewChangeEventArgs e)
		{
			if (e.Dates.Length == 14 && e.Style == scheduler1.MonthStyle)
			{
				Style s = (Style)scheduler1.Theme["CustomTwoWeekStyle"];
				if (s != null)
				{
					e.Style = s;
				}
			}
		}

		// Fill contact information after adding new contact from the Appointment form.
		void ContactStorage_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
		{
			if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemAdded)
			{
				Contact c = scheduler1.DataStorage.ContactStorage.Contacts[e.NewIndex];
				c.MenuCaption = "test contact";
			}
		}

		void views_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (scheduler1 == null)
			{
				return;
			}
			ChangeStyle();
		}

		// Refresh buttons' state if Scheduler's layout has been changed
		// by Scheduler's commands defined in templates.
		void scheduler1_LayoutUpdated(object sender, EventArgs e)
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
			else if (scheduler1.Style == (Style)scheduler1.Theme["CustomTwoWeekStyle"])
			{
				views.SelectedIndex = 3;
			}
            else if (scheduler1.Style == scheduler1.MonthStyle)
            {
                views.SelectedIndex = 4;
            }
            else
            {
                views.SelectedIndex = 5;
            }
		}

		// Change style according to the user's selection.
		private void ChangeStyle()
		{
			if (!IsLoaded)
                return;
            switch (views.SelectedIndex)
            {
                case 0:
                    scheduler1.ChangeStyle(scheduler1.OneDayStyle);
                    break;
                case 1:
                    scheduler1.ChangeStyle(scheduler1.WorkingWeekStyle);
                    break;
                case 2:
                    scheduler1.ChangeStyle(scheduler1.WeekStyle);
                    break;
                case 3:
                    scheduler1.ChangeStyle((Style)scheduler1.Theme["CustomTwoWeekStyle"]);
                    break;
                case 4:
                    scheduler1.ChangeStyle(scheduler1.MonthStyle);
                    break;
                case 5:
                    scheduler1.ChangeStyle(scheduler1.TimeLineStyle);
                    break;
            }
		}
	}
}