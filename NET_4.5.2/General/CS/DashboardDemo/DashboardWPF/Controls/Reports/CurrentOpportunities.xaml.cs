using C1.WPF.FlexReport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

namespace DashboardWPF.Views
{
    /// <summary>
    /// Interaction logic for CurrentOpportunitiesData.xaml
    /// </summary>
    public partial class CurrentOpportunities : UserControl
    {
        C1FlexReport _flexReport;
        Stream _reportStream;

        public CurrentOpportunities()
        {
            InitializeComponent();

            _flexReport = new C1FlexReport();
            Loaded += ViewerPage_Loaded;
        }

        void ViewerPage_Loaded(object sender, RoutedEventArgs rea)
        {
            Loaded -= ViewerPage_Loaded;

            LoadReportList();
        }

        void LoadReportList()
        {
            _reportStream = new FileStream("Reports\\CurrentOpportunitiesData.flxr", FileMode.Open, FileAccess.Read);
            string[] reports = C1FlexReport.GetReportList(_reportStream);

            var docListCombo = fv.DocumentListCombo;
            var items = docListCombo.Items;
            for (int i = 0; i < reports.Length; i++)
                items.Add(reports[i]);

            docListCombo.SelectionCommitted += ReportCombo_SelectionCommitted;
            docListCombo.SelectedIndex = 0;
        }

        void ReportCombo_SelectionCommitted(object sender, C1.WPF.PropertyChangedEventArgs<object> e)
        {
            if (_flexReport.IsBusy || _flexReport.IsDisposed)
            {
                return;
            }
            var reportName = fv.DocumentListCombo.SelectedItem as string;
            if (!string.IsNullOrEmpty(reportName))
            {
                fv.DocumentSource = null;
                _reportStream.Seek(0, SeekOrigin.Begin);
                _flexReport.Load(_reportStream, reportName);
                fv.DocumentSource = _flexReport;
                fv.FocusPane();
            }
        }
    }
}
