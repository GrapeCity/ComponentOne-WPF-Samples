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

using C1.WPF.FlexReport;

namespace C1DataSourceBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private C1FlexReport _report = new C1FlexReport();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            c1DataSource1.ViewSources["Products"].FilterDescriptors[0].Value = ((Categories)cbxCategory.SelectedItem).CategoryID;
            _report.Load(@"..\..\ReportDefinition.flxr", "Report");
            _report.DataSource.Recordset = c1DataSource1.ViewSources["Products"].DataView;
            c1FlexViewer.DocumentSource = _report;
        }

        private void cbxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                c1DataSource1.ViewSources["Products"].FilterDescriptors[0].Value = ((Categories)cbxCategory.SelectedItem).CategoryID;
                c1DataSource1.ViewSources["Products"].Refresh();
                _report.Render();
            }
        }
    }
}
