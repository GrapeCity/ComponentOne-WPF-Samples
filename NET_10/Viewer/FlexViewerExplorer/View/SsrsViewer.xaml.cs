using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using FlexViewerExplorer.Resources;
using C1.WPF.Viewer;
using System.IO;
using System.Reflection;
using C1.WPF.Report;
using System.Xml.Linq;
using System.Windows.Input;
using C1.WPF.TreeView;
using C1.WPF.Document;
using System.Net;
using C1.WPF.ImportServices.ReportingServiceWPF;
using System.ComponentModel;
using C1.WPF.Ssrs;
using System.Threading;

namespace FlexViewerExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SsrsViewer : UserControl
    {
        #region Variables

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
        #endregion

        public SsrsViewer()
        {
            this.InitializeComponent();

            Tag = AppResources.SsrsViewerDesc;
            _ssrsDocSource = new C1SSRSDocumentSource();
            _credential = new NetworkCredential(_ssrsUserName, _ssrsPassword, _ssrsDomain);
            _ssrsDocSource.Credential = _credential;
            _ssrsDocSource.GenerateCompleted += _ssrsDocSource_GenerateCompleted;

            Loaded += SsrsViewer_Loaded;
            Unloaded += SsrsViewer_Unloaded;
        }

        private void SsrsViewer_Unloaded(object sender, RoutedEventArgs e)
        {
            _loaded = false;
        }

        private void SsrsViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _loaded = true;
            if (!_initialized)
            {
                _initialized = true;
                var rptCombo = flexViewer.DocumentListCombo;
                rptCombo.Items.Add("Loading...");
                rptCombo.SelectedIndex = 0;

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
            if (e.Error != null)
            {
                return;
            }

            var rptCombo = flexViewer.DocumentListCombo;
            var items = rptCombo.Items;
            items.Clear();
            rptCombo.SelectedIndex = -1;
            if (e.Error != null)
            {
                items.Add("#SSRS Connection Error#");
                rptCombo.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
                rptCombo.ToolTip = "The endpoint is not available at the moment: " + _ssrsServerUrl;
                rptCombo.SelectedIndex = 0;
                return;
            }

            var list = e.Result as List<CatalogItem>;
            if (list == null || !_loaded)
            {
                return;
            }

            _catalogItems = list;
            indicator.Visibility = Visibility.Collapsed;

            _catalogItems = list;
            for (int i = 0; i < list.Count; i++)
            {
                items.Add(list[i].Name);
            }
            //rptCombo.ItemsSource = items;

            rptCombo.SelectionChanged += ReportCombo_SelectionChanged;
            rptCombo.SelectedIndex = 0;
        }

        private void ReportCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_loaded || _ssrsDocSource.IsBusy)
            {
                return;
            }
            int index = flexViewer.DocumentListCombo.SelectedIndex;
            if (index >= 0 && index < _catalogItems.Count)
            {
                flexViewer.DocumentSource = null;
                _ssrsDocSource.DocumentLocation = new C1.Document.SSRSReportLocation(_ssrsServerUrl, _catalogItems[index].Path);
                flexViewer.DocumentSource = _ssrsDocSource;
                flexViewer.FocusPane();
            }
        }

        private void _ssrsDocSource_GenerateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //The pages of document source are now ready
            Dispatcher.Invoke(new Action(() =>
            {
                indicator.Visibility = Visibility.Collapsed;
                flexViewer.DocumentSource = null;
                flexViewer.DocumentSource = _ssrsDocSource;
            }));
        }
    }
}
