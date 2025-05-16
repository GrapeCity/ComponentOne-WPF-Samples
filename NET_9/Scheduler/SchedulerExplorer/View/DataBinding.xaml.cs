using SchedulerExplorer.Resources;
using System.Windows;
using System.Windows.Controls;

namespace SchedulerExplorer
{
    /// <summary>
    ///     Interaction logic for DataBinding.xaml
    /// </summary>
    public partial class DataBinding : UserControl
    {
        AppointmentBOList _list = null;

        public DataBinding()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            // create collection of custom business objects
            _list = new AppointmentBOList();
            InitializeComponent();
            Tag = AppResources.DataBindingDescription;
            scheduler1.StyleChanged += new System.EventHandler<RoutedEventArgs>(scheduler1_StyleChanged);
            scheduler1_StyleChanged(null, null);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // add a new AppointmentBusinessObject (that creates a new appointment in a C1Scheduler control)
            AppointmentBusinessObject newObject = new AppointmentBusinessObject();
            newObject.Subject = "The test business object " + _list.Count.ToString();
            _list.Add(newObject);
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            // clear the list of business objects (that clears all appointments from the C1Scheduler control)
            _list.Clear();
        }

        /// <summary>
        /// Gets a collection of custom business objects, used as a data source for C1Scheduler's AppointmentStorage.
        /// </summary>
        public AppointmentBOList Appointments
        {
            get
            {
                return _list;
            }
        }

        private void scheduler1_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            if (scheduler1.Style == scheduler1.WorkingWeekStyle)
            {
                btnWorkWeek.IsChecked = true;
            }
            else if (scheduler1.Style == scheduler1.WeekStyle)
            {
                btnWeek.IsChecked = true;
            }
            else if (scheduler1.Style == scheduler1.MonthStyle)
            {
                btnMonth.IsChecked = true;
            }
            else if (scheduler1.Style == scheduler1.OneDayStyle)
            {
                btnDay.IsChecked = true;
            }
            else
            {
                btnTimeLine.IsChecked = true;
            }
        }
    }
}