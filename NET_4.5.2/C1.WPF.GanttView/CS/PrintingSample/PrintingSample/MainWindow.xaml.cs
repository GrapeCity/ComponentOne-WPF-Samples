using C1.WPF.FlexGrid;
using C1.WPF.GanttView;
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

namespace PrintingSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void _btnBasicPrint_Click(object sender, RoutedEventArgs e)
        {
            ReportOptions ro = new ReportOptions();

            var margin =
                _cmbMargins.SelectedIndex == 0 ? 96.0 / 4 :
                _cmbMargins.SelectedIndex == 1 ? 96.0 / 2 :
                96.0;

            ro.Margin = new Thickness(margin);

            if (_chkDates.IsChecked.Value)
            {
                ro.PrintEntrieProject = false;
                ro.StartDate = StartDate.SelectedDate.Value;
                ro.EndDate = EndDate.SelectedDate.Value;
            }

            C1.WPF.GanttView.PrintManager pm = new C1.WPF.GanttView.PrintManager();
            pm.Print(gv, ro);
        }

        private void _chkDates_Click(object sender, RoutedEventArgs e)
        {
            StartDate.IsEnabled = !StartDate.IsEnabled;
            EndDate.IsEnabled = !EndDate.IsEnabled;
        }

        private void OnPrintCustomerStyleClick(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            ReportOptions ro = new ReportOptions();

            var margin =
                _cmbMargins.SelectedIndex == 0 ? 96.0 / 4 :
                _cmbMargins.SelectedIndex == 1 ? 96.0 / 2 :
                96.0;

            ro.Margin = new Thickness(margin);
            
            if (_chkDates.IsChecked.Value)
            {
                ro.PrintEntrieProject = false;
                ro.StartDate = StartDate.SelectedDate.Value;
                ro.EndDate = EndDate.SelectedDate.Value;
            }
            print.Print(gv, ro, "CustomerPrintStyle\\CustomTitle.xaml");
        }

        private void OnPreviewCustomerStyleClick(object sender, RoutedEventArgs e)
        {
            PrintManager print = new PrintManager();
            ReportOptions ro = new ReportOptions();

            var margin =
                _cmbMargins.SelectedIndex == 0 ? 96.0 / 4 :
                _cmbMargins.SelectedIndex == 1 ? 96.0 / 2 :
                96.0;

            ro.Margin = new Thickness(margin);

            if (_chkDates.IsChecked.Value)
            {
                ro.PrintEntrieProject = false;
                ro.StartDate = StartDate.SelectedDate.Value;
                ro.EndDate = EndDate.SelectedDate.Value;
            }
            print.Preview(gv, ro, "CustomerPrintStyle\\CustomTitle.xaml");
        }
    }
}
