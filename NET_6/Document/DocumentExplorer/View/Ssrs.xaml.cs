using C1.DataCollection;
using C1.WPF.Document;
using C1.WPF.Document.Export;
using C1.WPF.ImportServices.ReportingServiceWPF;
using C1.WPF.Input;
using C1.WPF.Ssrs;
using DocumentExplorer.Resources;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DocumentExplorer
{
    public partial class Ssrs : UserControl
    {
        static readonly string
        _ssrsServerUrl = "http://ssrs.componentone.com:8000/ReportServer",
        _ssrsUserName = "ssrs_demo",
        _ssrsPassword = "bjqveS5gh89BH1Q",
        _ssrsDomain = string.Empty;

        NetworkCredential _credential;
        C1SSRSDocumentSource _ssrsDocSource;
        List<CatalogItem> _catalogItems;
        bool _initialized;
        bool _loaded;

        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public Ssrs()
        {
            InitializeComponent();

            Tag = AppResources.SsrsDesc;

            _ssrsDocSource = new C1SSRSDocumentSource();
            _credential = new NetworkCredential(_ssrsUserName, _ssrsPassword, _ssrsDomain);
            _ssrsDocSource.Credential = _credential;
            _ssrsDocSource.GenerateCompleted += _ssrsDocSource_GenerateCompleted;
            cbExportFilter.IsEnabled = false;

            Loaded += SsrsPage_Loaded;
            Unloaded += SsrsPage_Unloaded;
        }        

        void SsrsPage_Unloaded(object sender, RoutedEventArgs e)
        {
            _loaded = false;
        }

        void SsrsPage_Loaded(object sender, RoutedEventArgs rea)
        {
            _loaded = true;
            if (!_initialized)
            {
                _initialized = true;

                cbReports.Items.Add("Loading...");
                cbReports.SelectedIndex = 0;

                indicator.Visibility = Visibility.Visible;

                var bw = new BackgroundWorker();
                bw.DoWork += Reports_DoWork;
                bw.RunWorkerCompleted += Reports_RunWorkerCompleted;
                bw.RunWorkerAsync(null);
            }
        }

        void Reports_DoWork(object sender, DoWorkEventArgs e)
        {
            //Connecting to Ssrs server
            using (ServerConnection rs = new ServerConnection())
            {
                rs.SetConnectionOptions(_ssrsServerUrl, new ConnectionOptions(_credential));
                var items = rs.ListChildren(@"/", true, CancellationToken.None);
                var sortedItems = from itm in items where itm.Type == ItemTypeEnum.Report && !itm.Hidden orderby itm.Name select itm;
                e.Result = sortedItems.ToList();
            }
        }

        void Reports_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Populate the combo box with available reports
            var items = cbReports.Items;
            cbReports.SelectedIndex = -1;
            items.Clear();
            if (e.Error != null)
            {
                items.Add("#SSRS Connection Error#");
                cbReports.Foreground = new SolidColorBrush(Colors.Red);
                cbReports.ToolTip = "The endpoint is not available at the moment: " + _ssrsServerUrl;
                cbReports.SelectedIndex = 0;
                return;
            }

            var list = e.Result as List<CatalogItem>;
            if (list == null || !_loaded)
            {
                return;
            }

            _catalogItems = list;
            cbReports.ItemsSource = _catalogItems.AsDataCollection();

            this.cbReports.SelectionCommitted += CbReports_SelectionCommitted;
            cbReports.SelectedIndex = 0;
        }

        private void CbReports_SelectionCommitted(object sender, C1.WPF.Core.PropertyChangedEventArgs<object> e)
        {
            //Set the selected report to document source, and generate the pages
            if (!_loaded || _ssrsDocSource.IsBusy)
            {
                return;
            }
            indicator.Visibility = Visibility.Visible;
            btnExport.IsEnabled = false;
            cbExportFilter.IsEnabled = false;

            int index = cbReports.SelectedIndex;
            if (index >= 0 && index < _catalogItems.Count)
            {
                _ssrsDocSource.DocumentLocation = new SSRSReportLocation(_ssrsServerUrl, _catalogItems[index].Path);
                //Generate the pages for the document, so that export can work
                _ssrsDocSource.GenerateAsync();
            }
        }

        private void _ssrsDocSource_GenerateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //The pages of document source are now ready
            Dispatcher.Invoke(new Action(() =>
            {
                indicator.Visibility = Visibility.Collapsed;
                cbExportFilter.IsEnabled = true;
                btnExport.IsEnabled = true;

                var supportedExportProviders = _ssrsDocSource.SupportedExportProviders;
                cbExportFilter.ItemsSource = supportedExportProviders.AsDataCollection();
                cbExportFilter.SelectedIndex = 0;
            }));
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            OutputRange outputRange;
            if (rbtnPagesAll.IsChecked.Value)
                outputRange = OutputRange.All;
            else
            {
                if (!OutputRange.TryParse(tbPagesRange.Text, out outputRange))
                {
                    MessageBox.Show("Invalid range of pages.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // execute action
            ExportProvider ep = (ExportProvider)cbExportFilter.SelectedItem;
            saveFileDialog.DefaultExt = "." + ep.DefaultExtension;
            saveFileDialog.FileName = "DefaultDocument." + ep.DefaultExtension;
            saveFileDialog.Filter = String.Format("{0} (*.{1})|*.{1}|All files (*.*)|*.*", ep.FormatName, ep.DefaultExtension);
            bool? dr = saveFileDialog.ShowDialog();
            if (!dr.HasValue || !dr.Value)
                return;

            try
            {
                var exporter = ep.NewExporter();
                exporter.ShowOptions = false;
                exporter.Preview = true;
                exporter.FileName = saveFileDialog.FileName;
                exporter.Range = outputRange;
                _ssrsDocSource.Export(exporter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
