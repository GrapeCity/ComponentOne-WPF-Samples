
using FlexReportExplorer.Resources;
using System;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using Microsoft.Win32;

using C1.WPF.Document;
using C1.WPF.Report;

namespace FlexReportExplorer
{
    public partial class Print : UserControl
    {
        private FlexReport _report = new FlexReport();

        public Print()
        {
            InitializeComponent();
            Tag = AppResources.PrintDesc;
            Loaded += UserControl_Loaded;

            //
            UpdateReportsList();

            //
            LocalPrintServer printServer = new LocalPrintServer();
            PrintQueueCollection printQueuesOnLocalServer = printServer.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Local, EnumeratedPrintQueueTypes.Connections });
            //foreach (PrintQueue printer in printQueuesOnLocalServer)
            //{
            //    cbPrinter.Items.Add(new PrintInfo() { PrintQueue = printer });
            //}
            cbPrinter.ItemsSource = printQueuesOnLocalServer;
            if (cbPrinter.Items.Count > 0)
                cbPrinter.SelectedIndex = 0;
        }

        private void UpdateReportsList()
        {
            string[] reports = null;
            // get list of reports from resource using stream
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("FlexReportExplorer.Resources.FlexCommonTasks_XML.flxr"))
                reports = FlexReport.GetReportList(stream);

            // 
            cbReport.Items.Clear();
            if (reports != null && reports.Length > 0)
            {
                //foreach (string s in reports)
                //    cbReport.Items.Add(s);
                cbReport.ItemsSource = reports;
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
            PrintQueue pi = cbPrinter.SelectedItem as PrintQueue;
            if (pi == null)
                return;

            // load report
            try
            {
                // load from resource stream
                Assembly asm = Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream("FlexReportExplorer.Resources.FlexCommonTasks_XML.flxr"))
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
                po.PrintQueue = pi;
                _report.Print(po);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to print report [{0}], exception:\r\n{1}", selectedReport, ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Export.ExtractC1NWind();
        }

    }
}
