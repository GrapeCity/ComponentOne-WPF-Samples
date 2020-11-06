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
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Threading;

using C1.WPF.FlexReport;
using C1.WPF.FlexViewer;

namespace Binding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Uri ODataUri = new Uri(@"https://services.odata.org/V3/OData/OData.svc/");
        private C1FlexReport _report;

        public MainWindow()
        {
            InitializeComponent();

            _report = new C1FlexReport();
            _report.BusyStateChanged += _report_BusyStateChanged;
            
        }

        private delegate void BusyStateUpdateDelegate(C1FlexReport report);
        private void BusyStateUpdateMethod(C1FlexReport report)
        {
            cbReport.IsEnabled = !report.IsBusy;
        }

        private void _report_BusyStateChanged(object sender, EventArgs e)
        {
            Dispatcher.BeginInvoke(new BusyStateUpdateDelegate(BusyStateUpdateMethod), DispatcherPriority.Normal, sender);
        }

        private void BuildCategoriesReport()
        {
            // request data from OData service
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12; // force Tls12 
            var service = new DemoODataService.DemoService(ODataUri);
            // select all categrues
            var categories = service.Categories.Execute().ToList();

            // load report definition from resources
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("Binding.Resources.Reports.flxr"))
                _report.Load(stream, "Categories");

            // assign dataset to the report
            _report.DataSource.Recordset = categories;
        }

        private void BuildProductsReport()
        {
            // request data from OData service
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12; // force Tls12 
            var service = new DemoODataService.DemoService(ODataUri);
            // select all categories and products of each category
            var categories = service.Categories.Expand(c => c.Products).Execute();
            var products = (
                from c in categories
                from p in c.Products
                select new
                {
                    CategoryID = c.ID,
                    CategoryName = c.Name,
                    ID = p.ID,
                    Name = p.Name,
                    Description = p.Description,
                    ReleaseDate = p.ReleaseDate,
                    DiscontinuedDate = p.DiscontinuedDate,
                    Rating = p.Rating,
                    Price = p.Price,
                }).ToList();

            // load report definition from resources
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("Binding.Resources.Reports.flxr"))
                _report.Load(stream, "Products");

            // assign dataset to the report
            _report.DataSource.Recordset = products;
        }

        private void ShowReport(string reportName)
        {
            try
            {
                // build report
                switch (reportName)
                {
                    case "Categories":
                        BuildCategoriesReport();
                        break;
                    case "Products":
                        BuildProductsReport();
                        break;
                }

                // assign report to the preview pane
                flexViewerPane.DocumentSource = null;
                flexViewerPane.DocumentSource = _report;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Failed to show \"{0}\" report, error:\r\n{1}", reportName, ex.Message), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void cbReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
                return;
            var si = cbReport.SelectedItem as ComboBoxItem;
            if (si != null)
                ShowReport((string)si.Content);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbReport_SelectionChanged(null, null);
        }
    }
}
