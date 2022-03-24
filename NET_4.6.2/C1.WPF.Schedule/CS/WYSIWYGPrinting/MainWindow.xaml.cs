using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.Schedule;

namespace WYSIWYGPrinting
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ** fields
        #endregion

        #region ** ctor
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region ** printing
        private void print_Click(object sender, RoutedEventArgs e)
        {
            // print the date range currently shown in the C1Scheduler. You might allow end-user to select date range via dialog.
            var printDialog = new PrintDialog();
            // setup default printer and page orientation
            printDialog.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
            printDialog.PrintTicket = printDialog.PrintQueue.DefaultPrintTicket;
            if (scheduler1.ViewType == ViewType.Month || scheduler1.ViewType == ViewType.TimeLine)
            {
                printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Landscape;
            }
            // show print dialog
            if (printDialog.ShowDialog() == true)
            {
                // print via paginator
                var paginator = new C1SchedulerPaginator(scheduler1, printScheduler1,
                                    new Size(printDialog.PrintableAreaWidth, printDialog.PrintableAreaHeight),
                                    new Thickness(40, 60, 40, 60) /* hardcoded margin */,
                                    printDialog.PrintTicket.PageResolution.X.HasValue ? printDialog.PrintTicket.PageResolution.X.Value : 96,
                                    printDialog.PrintTicket.PageResolution.Y.HasValue ? printDialog.PrintTicket.PageResolution.Y.Value : 96
                                );
                if (paginator.PageCount > 0)
                {
                    printDialog.PrintDocument(paginator, "C1Scheduler printing test");
                }
            }
        }
        #endregion

        #region ** navigation
        private void Day_Click(object sender, RoutedEventArgs e)
        {
            scheduler1.ViewType = C1.WPF.Schedule.ViewType.Day;
        }
        private void WorkWeek_Click(object sender, RoutedEventArgs e)
        {
            scheduler1.ViewType = C1.WPF.Schedule.ViewType.WorkingWeek;
        }
        private void Week_Click(object sender, RoutedEventArgs e)
        {
            scheduler1.ViewType = C1.WPF.Schedule.ViewType.Week;
        }
        private void Month_Click(object sender, RoutedEventArgs e)
        {
            if (scheduler1 != null)
            {
                scheduler1.ViewType = C1.WPF.Schedule.ViewType.Month;
            }
        }
        private void TimeLine_Click(object sender, RoutedEventArgs e)
        {
            scheduler1.ViewType = C1.WPF.Schedule.ViewType.TimeLine;
        }

        private void scheduler1_StyleChanged(object sender, RoutedEventArgs e)
        {
            switch (scheduler1.ViewType)
            {
                case C1.WPF.Schedule.ViewType.Day:
                    day.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.WorkingWeek:
                    workWeek.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.Week:
                    week.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.Month:
                    month.IsChecked = true;
                    break;
                case C1.WPF.Schedule.ViewType.TimeLine:
                    timeLine.IsChecked = true;
                    break;
            }
        }
        #endregion
    }
}
