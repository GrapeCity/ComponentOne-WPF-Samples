using C1.ImportServices.ReportingService4;
using C1.Ssrs;
using C1.WPF.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSRSViewerSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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

        public MainWindow()
        {
            InitializeComponent();

            _ssrsDocSource = new C1SSRSDocumentSource();
            _credential = new NetworkCredential(_ssrsUserName, _ssrsPassword, _ssrsDomain);
            _ssrsDocSource.Credential = _credential;

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

                var rptCombo = fv.DocumentListCombo;
                rptCombo.Items.Add("Loading...");
                rptCombo.SelectedIndex = 0;

                var bw = new BackgroundWorker();
                bw.DoWork += ListReports_DoWork;
                bw.RunWorkerCompleted += ListReports_RunWorkerCompleted;
                bw.RunWorkerAsync(null);
            }
        }

        void ListReports_DoWork(object sender, DoWorkEventArgs e)
        {
            using (ServerConnection rs = new ServerConnection())
            {
                rs.SetConnectionOptions(_ssrsServerUrl, new ConnectionOptions(_credential));
                var items = rs.ListChildren(@"/", true, CancellationToken.None);
                var sortedItems = from itm in items where itm.Type == ItemTypeEnum.Report && !itm.Hidden orderby itm.Name select itm;
                e.Result = sortedItems.ToList();
            }
        }

        void ListReports_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var rptCombo = fv.DocumentListCombo;
            var items = rptCombo.Items;
            items.Clear();
            if (e.Error != null)
            {
                items.Add("#SSRS Connection Error#");
                rptCombo.Foreground = new SolidColorBrush(Colors.Red);
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
            for (int i = 0; i < list.Count; i++)
            {
                items.Add(list[i].Name);
            }

            rptCombo.SelectionCommitted += ReportCombo_SelectionCommitted;
            rptCombo.SelectedIndex = 0;
        }

        void ReportCombo_SelectionCommitted(object sender, C1.WPF.PropertyChangedEventArgs<object> e)
        {
            if (!_loaded || _ssrsDocSource.IsBusy)
            {
                return;
            }
            int index = fv.DocumentListCombo.SelectedIndex;
            if (index >= 0 && index < _catalogItems.Count)
            {
                fv.DocumentSource = null;
                _ssrsDocSource.DocumentLocation = new C1.Document.SSRSReportLocation(_ssrsServerUrl, _catalogItems[index].Path);
                fv.DocumentSource = _ssrsDocSource;
                fv.FocusPane();
            }
        }
    }
}
