using C1.WPF.Report;
using C1.WPF.TreeView;
using FlexViewerExplorer.Resources;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace FlexViewerExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlexReportViewer : UserControl
    {
        #region Variables

        // List of all report categories
        ObservableCollection<Category> _list = null;
        FlexReport _flexReport;
        #endregion

        public FlexReportViewer()
        {
            this.InitializeComponent();

            Tag = AppResources.FlexReportViewerDesc;
            _list = Category.GetAll();
            _flexReport = new FlexReport();

            foreach (Category category in _list)
            {
                var rp = category.ReportsList.FirstOrDefault(r => r.IsSelected == true);
                if (rp != null)
                {
                    category.IsExpanded = true;
                    rp.IsSelected = true;
                    break;
                }
            }

            myTreeView.ItemsSource = _list.SelectMany(x => x.ReportsList).GroupBy(x => x.Category).Select(x => new Group { Category = x.Key, Reports = x.ToList() });
        }

        private void OpenFile(string rptFile, string rptName)
        {
            try
            {
                _flexReport = new FlexReport();
                string filePath = rptFile.Substring(rptFile.IndexOf("Reports"));
                System.IO.Stream file = null;
                if (File.Exists(rptFile))
                {

                    file = new FileStream(rptFile, FileMode.Open, FileAccess.Read);
                    ChangeConnectionString(ref file);
                    _flexReport.Load(file, rptName);
                }
                else if (File.Exists(filePath))
                {
                    file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    ChangeConnectionString(ref file);
                    _flexReport.Load(file, rptName);
                }
                else
                {
                    throw new FileNotFoundException(new FileNotFoundException() + " " + filePath);
                }
                flexViewer.DocumentSource = null;
                flexViewer.DocumentSource = _flexReport;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        //change connection string
        private void ChangeConnectionString(ref Stream file)
        {
            XDocument xdoc = XDocument.Load(file);
            foreach (XElement item in xdoc.Descendants("ConnectionString"))
            {
                if (item.Value != null && item.Value != string.Empty)
                {

                    int strIndex = item.Value.LastIndexOf("\\") + 1;
                    int endIndex = item.Value.LastIndexOf(";");
                    string dbName = item.Value.ToString().Substring(strIndex, endIndex - strIndex);
                    if (File.Exists("..\\..\\Data\\" + dbName))
                    {
                        item.Value = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=..\\..\\Data\\" + dbName + ";Persist Security Info=False";
                    }
                    else
                    {
                        item.Value = "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=Data\\" + dbName + ";Persist Security Info=False";
                    }
                }
            }
            file = null;
            file = new MemoryStream();
            xdoc.Save(file);
            file.Position = 0;
        }

        #region EventHandler
        private void myTreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (myTreeView.SelectedItem == null) //re-click on same item
                {
                    return;
                }

                if (myTreeView.SelectedItem.DataContext is Group group)
                {
                    group.Category.IsExpanded = !group.Category.IsExpanded;
                }
                else if (myTreeView.SelectedItem.DataContext is Report rpt)
                {
                    OpenFile(rpt.FileName, rpt.RptName);
                }
            }
            catch (Exception)
            {

            }
        }

        private void DockControl_SlidingOpened(object sender, C1.WPF.Docking.SlidingEventArgs e)
        {
            dockControl.Width = 320;
            sliding.DockMode = C1.WPF.Docking.DockMode.Docked;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            sliding.DockMode = C1.WPF.Docking.DockMode.Sliding;
            dockControl.Width = 25;
        }
        #endregion
    }
}
