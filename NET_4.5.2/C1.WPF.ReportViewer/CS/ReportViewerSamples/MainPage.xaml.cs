using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Reflection;
using C1.WPF;

namespace ReportViewerSamples
{
    public partial class DemoReportViewer : UserControl
    {
        public DemoReportViewer()
        {
            InitializeComponent();

            reports.ItemsSource = new[]
            {
                "Alphabetical List of Products",
                "Casual Products Report",
                "Company Sales",
                "Employee Sales",
                "Sales by Category",
                "Sales Order Detail",
                "Suppliers",
                "Territory Sales Drilldown",
            };

            reports.SelectedItemChanged += OnSelectedItemChanged;
            reports.SelectedIndex = 3;
        }

        void OnSelectedItemChanged(object sender, PropertyChangedEventArgs<object> args)
        {
            var report = (string)reports.SelectedItem;
            var format = pdfFormat.IsChecked == true ? ".pdf" : ".mhtml";
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/" + report + format, UriKind.Relative));
            if(resource != null)
            {
                reportViewer.LoadDocument(resource.Stream);
            }
        }

        private void FormatClick(object sender, RoutedEventArgs e)
        {
            OnSelectedItemChanged(null, null);
        }
    }
}
