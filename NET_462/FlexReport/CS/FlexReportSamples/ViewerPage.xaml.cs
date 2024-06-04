using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using System.Windows;
using System.Windows.Controls;

using C1.WPF.FlexReport;

namespace FlexReportSamples
{
    /// <summary>
    /// Interaction logic for ViewerPage.xaml
    /// </summary>
    public partial class ViewerPage : UserControl
    {
        C1FlexReport _flexReport;
        Stream _reportStream;

        public ViewerPage()
        {
            InitializeComponent();

            _flexReport = new C1FlexReport();
            Loaded += ViewerPage_Loaded;
        }

        void ViewerPage_Loaded(object sender, RoutedEventArgs rea)
        {
            Loaded -= ViewerPage_Loaded;

            ExportPage.ExtractC1NWind();
            LoadReportList();
        }

        void LoadReportList()
        {
            string[] reports;
            Assembly asm = Assembly.GetExecutingAssembly();
            _reportStream = asm.GetManifestResourceStream("FlexReportSamples.Resources.FlexCommonTasks_XML.flxr");
            reports = C1FlexReport.GetReportList(_reportStream);

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
