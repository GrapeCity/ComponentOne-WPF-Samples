using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections.ObjectModel;

using C1.WPF.FlexReport;

namespace FlexCommonTasks
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

            // build list of reports
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("FlexCommonTasks.Resources.FlexCommonTasks_WPF.flxr"))
            {
                string[] reports = C1FlexReport.GetReportList(stream);

                using (C1FlexReport fr = new C1FlexReport())
                {
                    List<Category> categories = new List<Category>();
                    foreach (string reportName in reports)
                    {
                        stream.Seek(0, SeekOrigin.Begin);
                        fr.Load(stream, reportName);

                        string keywords = fr.ReportInfo.Keywords;
                        string[] ss = keywords.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                        for (int i = 0; i < ss.Length; i++)
                        {
                            Category category = categories.Find((item) => string.Compare(item.CategoryName, ss[i], true) == 0);
                            if (category == null)
                            {
                                category = new Category() { CategoryName = ss[i], Reports = new List<string>() };
                                categories.Add(category);
                            }
                            category.Reports.Add(reportName);
                        }
                    }
                    categories.Sort(CompareCategories);

                    //
                    icReports.ItemsSource = categories;
                }
            }
        }

        private static int CompareCategories(Category x, Category y)
        {
            bool xNew = x.CategoryName.StartsWith("New in");
            bool yNew = y.CategoryName.StartsWith("New in");
            if ((xNew && yNew) || (!xNew && !yNew))
                return string.Compare(x.CategoryName, y.CategoryName, true);
            if (xNew)
                return -1;
            if (yNew)
                return 1;
            return string.Compare(x.CategoryName, y.CategoryName, true);
        }

        public class Category
        {
            public string CategoryName { get; set; }
            public List<string> Reports { get; set; }
        }

        private void ShowReport(string reportName)
        {
            if (_report.IsBusy || _report.IsDisposed)
                return;

            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream("FlexCommonTasks.Resources.FlexCommonTasks_WPF.flxr"))
            {
                _report.Load(stream, reportName);
                fv.DocumentSource = null;
                fv.DocumentSource = _report;
            }
            fv.Pane.FocusFirstElement();
        }

        private void ShowReport_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void ShowReport_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowReport((string)e.Parameter);
        }

        private void Window_Loaded(object sender, RoutedEventArgs rea)
        {
            ShowReport("Simple List");
        }
    }
}
