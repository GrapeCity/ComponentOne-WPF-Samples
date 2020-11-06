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
using System.Windows.Markup;
using C1.C1Schedule;
using C1.WPF.Schedule;
using C1.WPF;
using System.Collections;

namespace CustomGroupingView
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
            // add some contacts
            Contact cnt = new Contact();
            cnt.Text = "Andy Garcia";
            c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt);
            cnt = new Contact();
            cnt.Text = "Nancy Drew";
            c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt);
            cnt = new Contact();
            cnt.Text = "Robert Clark";
            c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt);
            cnt = new Contact();
            cnt.Text = "James Doe";
            c1Scheduler1.DataStorage.ContactStorage.Contacts.Add(cnt);
            c1Scheduler1.GroupBy = "Contact";
        }
    }

    /// <summary>
    /// Returns VisualIntervalCollection for the specified day and specified SchedulerGroup which can be used
    /// as an ItemsSource for VisualIntervalsPresenter control.
    /// </summary>
    /// <remarks>
    /// If converter parameter is "Self", return list of a single VisualIntervalGroup, to use it as ItemsSource for representing all-day area.
    /// In all other cases returns VisualItervalCollection containing time slots for the single day.
    /// </remarks>
    public class GroupItemToVisualIntervalsConverter : IValueConverter
    {

        public static GroupItemToVisualIntervalsConverter Default = new GroupItemToVisualIntervalsConverter();

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            FrameworkElement el = value as FrameworkElement;
            if (el != null)
            {
                SchedulerGroupItem group = el.DataContext as SchedulerGroupItem;
                int index = -1;
                if (group != null)
                {
                    ItemsControl itm = VTreeHelper.GetParentOfType(el, typeof(ItemsControl)) as ItemsControl;
                    if (itm != null)
                    {
                        object data = itm.DataContext;

                        ItemsControl itmParent = VTreeHelper.GetParentOfType(itm, typeof(ItemsControl)) as ItemsControl;
                        if (itmParent != null)
                        {
                            index = itmParent.Items.IndexOf(data);
                            VisualIntervalGroup visualIntervalGroup = group.VisualIntervalGroups[index] as VisualIntervalGroup;
                            string param = (string)parameter;
                            if (param.ToLower() == "self")
                            {
                                // create list of a single VisualIntervalGroup
                                // (we need list to use it as ItemsSource)
                                List<object> list = new List<object>();
                                list.Add(visualIntervalGroup);
                                return list;
                            }
                            else
                            {
                                return visualIntervalGroup.VisualIntervals;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
