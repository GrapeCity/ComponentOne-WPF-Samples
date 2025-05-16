using FlexReportExplorer.Resources;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using C1.WPF.Report;
using C1.WPF.Document.Export;
using C1.Zip;

namespace FlexReportExplorer
{
    public partial class Export : UserControl
    {
        private FlexReport _report = new FlexReport();

        public Export()
        {
            InitializeComponent();

            Tag = AppResources.ExportDesc;
            Loaded += UserControl_Loaded;

            //
            UpdateReportsList();

            // build list of supported export filters
            cbExportFilter.ItemsSource = _report.SupportedExportProviders;
            cbExportFilter.SelectedIndex = 0;
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
                cbReport.ItemsSource = reports;
                cbReport.SelectedIndex = 0;
                btnExport.IsEnabled = !_report.IsBusy;
            }
            else
                btnExport.IsEnabled = false;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (_report.IsBusy)
                return;

            string selectedReport = cbReport.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedReport))
                return;
            if (cbExportFilter.SelectedIndex < 0 || cbExportFilter.SelectedIndex >= _report.SupportedExportProviders.Length)
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

            // prepare ExportFilter object
            ExportProvider ep = _report.SupportedExportProviders[cbExportFilter.SelectedIndex];
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = string.Format("{0}.{1}", selectedReport, ep.DefaultExtension);
            sfd.Filter = string.Format("{0} Files (*.{1})|*.{1}", ep.FormatName, ep.DefaultExtension);
            sfd.CheckPathExists = true;
            if (!sfd.ShowDialog().Value)
                return;
            ExportFilter ef = ep.NewExporter() as ExportFilter;
            ef.FileName = sfd.FileName;
            ef.Preview = cbOpenDocument.IsChecked.Value;

            //
            try
            {
                _report.RenderToFilter(ef);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to export report [{0}], exception:\r\n{1}", selectedReport, ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static void ExtractC1NWind()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("FlexReportExplorer.Resources.C1NWind.xml.zip"))
            {
                C1ZipFile zf = new C1ZipFile();
                zf.Open(stream);
                using (Stream src = zf.Entries[0].OpenReader())
                {
                    if (!File.Exists("C1NWind.xml") || new FileInfo("C1NWind.xml").Length != zf.Entries[0].SizeUncompressed)
                    {
                        using (Stream dst = new FileStream("C1NWind.xml", FileMode.Create))
                        {
                            byte[] buf = new byte[16 * 1024];
                            int len;
                            while ((len = src.Read(buf, 0, buf.Length)) > 0)
                            {
                                dst.Write(buf, 0, len);
                            }
                        }
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ExtractC1NWind();
        }
    }
}
