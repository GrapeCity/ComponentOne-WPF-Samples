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
using System.Reflection;
using System.IO;
using Microsoft.Win32;
using System.Printing;

using C1.WPF.Document;
using C1.WPF.FlexReport;

namespace FlexReportSamples
{
    /// <summary>
    /// Interaction logic for PrintPage.xaml
    /// </summary>
    public partial class PrintPage : UserControl
    {
        private C1FlexReport _report = new C1FlexReport();

        public PrintPage()
        {
            InitializeComponent();

            //
            UpdateReportsList();

            //
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueueCollection printQueuesOnLocalServer = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
            foreach (PrintQueue printer in printQueuesOnLocalServer)
            {
                cbPrinter.Items.Add(new PrintInfo() { PrintQueue = printer });
            }
            if (cbPrinter.Items.Count > 0)
                cbPrinter.SelectedIndex = 0;
        }


        private void UpdateReportsList()
        {
            string[] reports = null;
            // get list of reports from resource using stream
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("FlexReportSamples.Resources.FlexCommonTasks_XML.flxr"))
                reports = C1FlexReport.GetReportList(stream);

            // 
            cbReport.Items.Clear();
            if (reports != null && reports.Length > 0)
            {
                foreach (string s in reports)
                    cbReport.Items.Add(s);
                cbReport.SelectedIndex = 0;
                btnPrint.IsEnabled = !_report.IsBusy;
            }
            else
                btnPrint.IsEnabled = false;
        }

        private class PrintInfo
        {
            public PrintQueue PrintQueue { get; set; }

            public override string ToString()
            {
                return PrintQueue.Name;
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (_report.IsBusy)
                return;

            string selectedReport = cbReport.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedReport))
                return;
            PrintInfo pi = cbPrinter.SelectedItem as PrintInfo;
            if (pi == null)
                return;

            // load report
            try
            {
                // load from resource stream
                Assembly asm = Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream("FlexReportSamples.Resources.FlexCommonTasks_XML.flxr"))
                    _report.Load(stream, selectedReport);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to load report [{0}], exception:\r\n{1}", selectedReport, ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // render and print report
            try
            {
                _report.Render();
                C1PrintOptions po = new C1PrintOptions();
                po.PrintQueue = pi.PrintQueue;
                _report.Print(po);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to print report [{0}], exception:\r\n{1}", selectedReport, ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ExportPage.ExtractC1NWind();
        }
    }
}
