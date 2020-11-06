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

namespace CustomDialogs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Language = System.Windows.Markup.XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.Name);
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state
            sched1_StyleChanged(null, null);
        }

        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            sched1.VisibleDates.BeginUpdate();
            sched1.VisibleDates.Clear();
            sched1.VisibleDates.Add(DateTime.Today);
            sched1.VisibleDates.EndUpdate();
        }

        private void sched1_StyleChanged(object sender, RoutedEventArgs e)
        {
            // update toolbar buttons state according to the current C1Scheduler view
            if (sched1.Style == sched1.WorkingWeekStyle)
            {
                btnWorkWeek.IsChecked = true;
            }
            else if (sched1.Style == sched1.WeekStyle)
            {
                btnWeek.IsChecked = true;
            }
            else if (sched1.Style == sched1.MonthStyle)
            {
                btnMonth.IsChecked = true;
            }
            else if (sched1.Style == sched1.OneDayStyle)
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
