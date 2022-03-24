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

namespace FlexViewerExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlexReportViewer : UserControl
    {
        #region Variables

        // List of all report categories
        System.Collections.ObjectModel.ObservableCollection<Categories> _list = null;
        FlexReport _flexReport;
        #endregion

        public FlexReportViewer()
        {
            this.InitializeComponent();

            Tag = AppResources.FlexReportViewerDesc;
            _list = Categories.GetAll();
            _flexReport = new FlexReport();
        }

        private void FillTree()
        {

            foreach (Categories category in _list)
            {
                myTreeView.Items.Add(category);
                foreach (Reports report in category.ReportsList)
                {
                    myTreeView.Items.Add(report);
                    C1TreeViewItem item = (C1TreeViewItem)myTreeView.ItemContainerGenerator.ContainerFromItem(report);
                    if (item == null)
                        return;
                    item.Visibility = Visibility.Collapsed;
                }
            }
        }
        private void ExpandCollapseTreeView(Categories categories)
        {
            foreach (Reports report in categories.ReportsList)
            {
                C1TreeViewItem item = (C1TreeViewItem)myTreeView.ItemContainerGenerator.ContainerFromItem(report);
                if (item == null)
                    return;
                item.Visibility = (item.Visibility == Visibility.Collapsed) ? Visibility.Visible : Visibility.Collapsed;
            }
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
                    ChangeConStrin(ref file);
                    _flexReport.Load(file, rptName);
                }
                else if (File.Exists(filePath))
                {
                    file = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    ChangeConStrin(ref file);
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
        private void ChangeConStrin(ref Stream file)
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
        private void MyTreeView_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (myTreeView.SelectedItem == null) //re-click on same item
                {
                    return;
                }

                if (myTreeView.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated && myTreeView.SelectedItem.DataContext is Categories)
                {

                    ExpandCollapseTreeView((myTreeView.SelectedItem.DataContext as Categories));
                    myTreeView.SelectedItem.IsExpanded = !myTreeView.SelectedItem.IsExpanded;
                    (myTreeView.SelectedItem.DataContext as Categories).ExpenderImg = ((myTreeView.SelectedItem.DataContext as Categories).ExpenderImg == @"..\Resources\expand.png") ? @"..\Resources\collapse.png" : @"..\Resources\expand.png";


                }
                else
                {
                    Reports rpt = (myTreeView.SelectedItem.DataContext as Reports);
                    OpenFile(rpt.FileName, rpt.RptName);
                }
            }
            catch (Exception)
            {

            }
        }
        private void MyTreeView_Loaded(object sender, RoutedEventArgs e)
        {
            if (!myTreeView.HasItems)
            {
                FillTree();
                //Select and open first report
                foreach (Categories category in _list)
                {
                    Reports rptSelected = category.ReportsList.Where(r => r.IsSelected == true).FirstOrDefault();
                    if (rptSelected != null)
                    {
                        C1TreeViewItem itemReport = (C1TreeViewItem)myTreeView.ItemContainerGenerator.ContainerFromItem(rptSelected);
                        ExpandCollapseTreeView(category);
                        category.ExpenderImg = @"..\Resources\collapse.png";
                        itemReport.IsSelected = true;
                        OpenFile(rptSelected.FileName, rptSelected.RptName);
                        break;
                    }
                }
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
