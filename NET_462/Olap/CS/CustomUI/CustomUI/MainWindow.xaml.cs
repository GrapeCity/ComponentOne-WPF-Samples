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
using C1.WPF.Olap;
using System.Data;
using System.Reflection;
using C1.Zip;
using Microsoft.Win32;
using System.IO;
using C1.WPF;
using C1.WPF.FlexGrid;
using C1.WPF.Docking;

namespace CustomUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //---------------------------------------------------------------------
        #region ** fields
        ReportOptions _reportOptions;

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            // load data from embedded zip resource
            var ds = new DataSet();
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("CustomUI.Resources.nwind.zip"))
            {
                var zip = new C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    // load data
                    ds.ReadXml(zr);
                }
            }

            // bind olap page to data
            PivotBuilder.DataSource = ds.Tables[0].DefaultView;

            var olap = PivotBuilder.OlapEngine;
            // rename fields in the UI
            olap.Fields["ExtendedPrice"].Caption = "Sales";
            olap.Fields["OrderDate"].Caption = "Order Date";
            olap.Fields["PostalCode"].Caption = "Postal Code";
            //olap.Fields["Customers_002eCompanyName"].Caption = "Company Name";
            olap.Fields["ProductName"].Caption = "Product Name";
            olap.Fields["RequiredDate"].Caption = "Required Date";
            olap.Fields["ShipAddress"].Caption = "Ship Address";
            olap.Fields["ShipCity"].Caption = "Ship City";
            olap.Fields["ShipCountry"].Caption = "Ship Country";
            olap.Fields["ShipName"].Caption = "Ship Name";
            olap.Fields["ShippedDate"].Caption = "Shipped Date";
            //olap.Fields["Shippers_002eCompanyName"].Caption = "Shipper";
            olap.Fields["ShipPostalCode"].Caption = "Ship Postal Code";
            olap.Fields["ShipRegion"].Caption = "Ship Region";
            olap.Fields["UnitPrice"].Caption = "Price";

            // set formats
            olap.Fields["OrderDate"].Format = "yyyy";
            olap.Fields["RequiredDate"].Format = "yyyy";
            olap.Fields["ShippedDate"].Format = "yyyy";
            olap.Fields["ExtendedPrice"].Format = "c2";
            olap.Fields["UnitPrice"].Format = "c2";

            // show sales by country and category
            olap.DataSource = ds.Tables[0].DefaultView;
            olap.BeginUpdate();
            olap.RowFields.Add("Country");
            olap.ColumnFields.Add("OrderDate");
            olap.ValueFields.Add("ExtendedPrice");
            olap.EndUpdate();

            PivotBuilder.OlapEngine.Update();

            olap.Updated += (s, e) =>
            {
                info.Visibility = Visibility.Collapsed;
            };

            olap.UpdateProgressChanged += (s, e) =>
            {
                info.Visibility = Visibility.Visible;
            };

            // initialize printing report options
            _reportOptions = new ReportOptions()
            {
                IncludeGrid = true,
                IncludeChart = true,
                IncludeRawData = false,
                Margin = new Thickness(1),
                ChartScale = ScaleMode.SinglePage,
                GridScale = ScaleMode.PageWidth,
                RawDataScale = ScaleMode.PageWidth,
                ShowFooterSeparator = true,
                ShowHeaderSeparator = true,
                HeaderCenter = "&[ViewTitle]",
                HeaderRight = "&[Date]",
                FooterRight = "Page &[Page] of &[PageCount]"
            };

            // initialize undo/redo stack
            new OlapPanelUndoStack(PivotBuilder, btnUndo, btnRedo);

            // configure toolbar
            btnChartType_Bar.IsChecked = true;

            Application.Current.Resources.MergedDictionaries.Add(C1.WPF.Theming.C1Theme.GetCurrentThemeResources(new C1.WPF.Theming.Office2013.C1ThemeOffice2013White()));

        }



        private void PivotBuilder_Loaded(object sender, RoutedEventArgs e)
        {
            PivotBuilder.OlapEngine.Update();
            //TabSecondary.DockMode = DockMode.Sliding;
        }

        //------------------------------------------------------------------------
        #region Chart Type

        private void btnChartType_Bar_Checked(object sender, RoutedEventArgs e)
        {
            OlapChart.ChartType = ChartType.Bar;
        }

        private void btnChartType_Column_Checked(object sender, RoutedEventArgs e)
        {
            OlapChart.ChartType = ChartType.Column;
        }

        private void btnChartType_Area_Checked(object sender, RoutedEventArgs e)
        {
            OlapChart.ChartType = ChartType.Area;
        }

        private void btnChartType_Line_Checked(object sender, RoutedEventArgs e)
        {
            OlapChart.ChartType = ChartType.Line;
        }

        private void btnChartType_Scatter_Checked(object sender, RoutedEventArgs e)
        {
            OlapChart.ChartType = ChartType.Scatter;
        }

        private void btnChartType_Pie_Checked(object sender, RoutedEventArgs e)
        {
            OlapChart.ChartType = ChartType.Pie;
        }

        #endregion

        //------------------------------------------------------------------------
        #region Views

        private void menuViewSalesPerson_Click(object sender, RoutedEventArgs e)
        {
            BuildView("Salesperson");
        }

        private void menuViewProduct_Click(object sender, RoutedEventArgs e)
        {
            BuildView("ProductName");
        }

        private void menuViewCountry_Click(object sender, RoutedEventArgs e)
        {
            BuildView("Country");
        }

        #endregion

        //------------------------------------------------------------------------
        #region Price Filter

        private void menuFilterExpensive1_Click(object sender, RoutedEventArgs e)
        {
            SetPriceFilter("Expensive Products (price > $50)", 50, double.MaxValue);
        }

        private void menuFilterExpensive2_Click(object sender, RoutedEventArgs e)
        {
            SetPriceFilter("Moderately Priced Products ($20 < price < $50)", 20, 50);
        }

        private void menuFilterExpensive3_Click(object sender, RoutedEventArgs e)
        {
            SetPriceFilter("Inexpensive Products (price < $20)", 0, 20);
        }

        private void menuFilterAllPrices_Click(object sender, RoutedEventArgs e)
        {
            SetPriceFilter("All Products", 0, double.MaxValue);
        }

        #endregion

        //------------------------------------------------------------------------
        #region Grid Totals
        private void menuRowGrandTotals_Checked(object sender, RoutedEventArgs e)
        {
            PivotBuilder.ShowTotalsRows = C1.Olap.ShowTotals.GrandTotals;
            menuRowGrandTotals.IsChecked = true;
            menuRowNoTotals.IsChecked = false;
        }

        private void menuRowSubtotals_Checked(object sender, RoutedEventArgs e)
        {
            PivotBuilder.ShowTotalsRows = C1.Olap.ShowTotals.Subtotals;
            menuRowSubtotals.IsChecked = true;
            menuRowNoTotals.IsChecked = false;
        }

        private void menuRowNoTotals_Checked(object sender, RoutedEventArgs e)
        {
            menuRowGrandTotals.IsChecked = false;
            menuRowSubtotals.IsChecked = false;
            PivotBuilder.ShowTotalsRows = C1.Olap.ShowTotals.None;
            menuRowNoTotals.IsChecked = true;
        }

        private void menuColumnGrandTotals_Checked(object sender, RoutedEventArgs e)
        {
            PivotBuilder.ShowTotalsColumns = C1.Olap.ShowTotals.GrandTotals;
            menuColumnGrandTotals.IsChecked = true;
            menuColumnNoTotals.IsChecked = false;
        }

        private void menuColumnSubtotals_Checked(object sender, RoutedEventArgs e)
        {
            PivotBuilder.ShowTotalsColumns = C1.Olap.ShowTotals.Subtotals;
            menuColumnSubtotals.IsChecked = true;
            menuColumnNoTotals.IsChecked = false;
        }

        private void menuColumnNoTotals_Checked(object sender, RoutedEventArgs e)
        {
            menuColumnGrandTotals.IsChecked = false;
            menuColumnSubtotals.IsChecked = false;
            PivotBuilder.ShowTotalsColumns = C1.Olap.ShowTotals.None;
            menuColumnNoTotals.IsChecked = true;
        }
        #endregion

        //------------------------------------------------------------------------
        #region Open/Save
        private void btnSaveView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = "XML (*.xml)|*.xml";
                dlg.DefaultExt = C1.WPF.Olap.Resources.Resources.DefaultExtension;

                if (dlg.ShowDialog().Value == true)
                {
                    using (var sw = new StreamWriter(dlg.OpenFile()))
                    {
                        sw.Write(PivotBuilder.OlapEngine.ViewDefinition);
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Error", MessageBoxButton.OK);
            }

        }

        private void btnOpenView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dlg = new OpenFileDialog();
                dlg.Filter = "XML (*.xml)|*.xml";
                if (dlg.ShowDialog().Value == true)
                {
                    var vd = new FileInfo(dlg.FileName).OpenText().ReadToEnd();
                    PivotBuilder.OlapEngine.ViewDefinition = vd;
                }

            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Error", MessageBoxButton.OK);
            }
        }
        #endregion

        //------------------------------------------------------------------------
        #region Printing
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            C1.WPF.Olap.PrintManager pm = new C1.WPF.Olap.PrintManager();
            pm.Print(PivotBuilder, _reportOptions, OlapGrid, OlapChart);
        }

        private void btnPrintSettings_Click(object sender, RoutedEventArgs e)
        {
            // show editor in a dialog
            C1Window w = new C1Window();
            w.Header = C1.WPF.Olap.Resources.Resources.DocumentOptions;
            w.FontFamily = FontFamily;
            w.FontSize = FontSize;
            w.Content = new C1OlapReportOptions(_reportOptions);
            w.Width = 600;
            w.Height = 350;
            w.CenterOnScreen();
            w.ModalBackground = new SolidColorBrush(Color.FromArgb(80, 80, 80, 80));
            w.ShowModal();
        }
        #endregion

        //------------------------------------------------------------------------
        #region Export
        private void btnExportCSV_Click(object sender, RoutedEventArgs e)
        {
            Export(1);
        }

        private void btnExportHtml_Click(object sender, RoutedEventArgs e)
        {
            Export(2);
        }

        private void btnExportText_Click(object sender, RoutedEventArgs e)
        {
            Export(3);
        }

        private void Export(int extension)
        {
            try
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = C1.WPF.Olap.Resources.Resources.ExportDialogFilter;
                dlg.FilterIndex = extension;

                if (dlg.ShowDialog().Value == true)
                {
                    using (var sw = dlg.OpenFile())
                    {
                        var ext = System.IO.Path.GetExtension(dlg.SafeFileName).ToLower();
                        switch (ext)
                        {
                            case ".html":
                                OlapGrid.Save(sw, FileFormat.Html);
                                break;
                            case ".csv":
                                OlapGrid.Save(sw, FileFormat.Csv);
                                break;
                            case ".txt":
                            default:
                                OlapGrid.Save(sw, FileFormat.Text);
                                break;
                        }
                    }
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message, "Error", MessageBoxButton.OK);
            }
        }

        //private void btnExportExcel_Click(object sender, RoutedEventArgs e)
        //{

        //}
        #endregion

        //------------------------------------------------------------------------
        #region ** implementation

        // rebuild the view after a button was clicked
        void BuildView(string fieldName)
        {
            // get olap engine
            var olap = PivotBuilder.OlapEngine;

            // stop updating until done
            olap.BeginUpdate();

            // clear all fields
            olap.RowFields.Clear();
            olap.ColumnFields.Clear();
            olap.ValueFields.Clear();

            // format order dates to group by year
            var f = olap.Fields["OrderDate"];
            f.Format = "yyyy";

            // build up view
            olap.ColumnFields.Add("OrderDate");
            olap.RowFields.Add(fieldName);
            olap.ValueFields.Add("ExtendedPrice");

            // restore updates
            olap.EndUpdate();
        }

        // apply a filter to the product price
        void SetPriceFilter(string footerText, double min, double max)
        {
            // get olap engine
            var olap = PivotBuilder.OlapEngine;

            // stop updating until done
            olap.BeginUpdate();

            // make sure unit price field is active in the view
            var field = olap.Fields["UnitPrice"];
            olap.FilterFields.Add(field);

            // customize the filter
            var filter = field.Filter;
            filter.Clear();
            filter.Condition1.Operator = C1.Olap.ConditionOperator.GreaterThanOrEqualTo;
            filter.Condition1.Parameter = min;
            filter.Condition2.Operator = C1.Olap.ConditionOperator.LessThanOrEqualTo;
            filter.Condition2.Parameter = max;

            // restore updates
            olap.EndUpdate();
        }

        #endregion

        private void btnShowZeros_Click(object sender, RoutedEventArgs e)
        {
            PivotBuilder.ShowZeros = (bool)btnShowZeros.IsChecked;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PivotBuilder.OlapEngine.Update();
        }

        //------------------------------------------------------------------------
        #region Chart Settings

        private void menuPalette_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = e.Source as MenuItem;
            foreach(MenuItem mi in menuChartPalette.Items)
            {
                mi.IsChecked = false;
            }
            item.IsChecked = true;
            OlapChart.Palette = ChartPalette.GetPalette(item.Header.ToString());
        }

        private void btnChartTitle_Click(object sender, RoutedEventArgs e)
        {
            OlapChart.ShowTitle = (bool)btnChartTitle.IsChecked;
        }

        private void btnChartGridlines_Click(object sender, RoutedEventArgs e)
        {
            OlapChart.ShowGridLines = (bool)btnChartGridlines.IsChecked;
        }

        private void btnChartStacked_Click(object sender, RoutedEventArgs e)
        {
            OlapChart.Stacked = (bool)btnChartStacked.IsChecked;
        }

        private void btnChartTotals_Click(object sender, RoutedEventArgs e)
        {
            OlapChart.ChartTotals = (bool)btnChartTotals.IsChecked;
        }

        private void btnChartLegend_Click(object sender, RoutedEventArgs e)
        {
            if (btnChartLegend.IsChecked == false)
            {
                OlapChart.ShowLegend = ShowLegend.Never;
            }
            else
            {
                OlapChart.ShowLegend = ShowLegend.Automatic;
            }
        }
        #endregion

        //------------------------------------------------------------------------
        #region View tab
        private void btnViewLock_Click(object sender, RoutedEventArgs e)
        {
            foreach (C1DockTabControl d in ViewDock.Items)
            {
                d.CanUserDock = d.CanUserFloat = d.CanUserReorder = d.CanUserHide = d.CanUserSlide = !(bool)btnViewLock.IsChecked;
            }

            if ((bool)btnViewLock.IsChecked)
            {
                btnViewLock.LabelTitle = "Locked";
                btnViewLock.LargeImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Resources/Toolbar/Lock.png", UriKind.Relative));// "/Resources/Toolbar/Lock.png";
            }
            else
            {
                btnViewLock.LabelTitle = "Unlocked";
                btnViewLock.LargeImageSource = new System.Windows.Media.Imaging.BitmapImage(new Uri("/Resources/Toolbar/Unlock.png", UriKind.Relative));
            }
        }

        private void menuFieldListTab_Click(object sender, RoutedEventArgs e)
        {
            if (menuFieldListTab.IsChecked == true)
            {
                TabFieldList.Visibility = System.Windows.Visibility.Visible;
                var parent = FindParentTab(TabFieldList);
                if (parent != null)
                {
                    parent.Visibility = System.Windows.Visibility.Visible;
                    parent.DockMode = DockMode.Docked;
                }
                else
                {
                    TabSecondary.Items.Add(TabFieldList);
                }
            }
            else
            {
                TabFieldList.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void menuPivotGridTab_Click(object sender, RoutedEventArgs e)
        {
            if (menuPivotGridTab.IsChecked == true)
            {
                TabPivotGrid.Visibility = System.Windows.Visibility.Visible;
                var parent = FindParentTab(TabPivotGrid);
                if (parent != null)
                {
                    parent.Visibility = System.Windows.Visibility.Visible;
                    parent.DockMode = DockMode.Docked;
                }
                else
                {
                    TabMain.Items.Add(TabPivotGrid);
                }
            }
            else
            {
                TabPivotGrid.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void menuChartTab_Click(object sender, RoutedEventArgs e)
        {
            if (menuChartTab.IsChecked == true)
            {
                TabChart.Visibility = System.Windows.Visibility.Visible;
                var parent = FindParentTab(TabChart);
                if (parent != null)
                {
                    parent.Visibility = System.Windows.Visibility.Visible;
                    parent.DockMode = DockMode.Docked;
                }
                else
                {
                    TabMain.Items.Add(TabChart);
                }
            }
            else
            {
                TabChart.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void menuTabs_ItemClick(object sender, SourcedEventArgs e)
        {
            MenuItem item = e.Source as MenuItem;
            if (item.Header.Equals("Field List"))
            {
                if (item.IsChecked == true)
                {
                    TabFieldList.Visibility = System.Windows.Visibility.Visible;
                    var parent = FindParentTab(TabFieldList);
                    if (parent != null)
                    {
                        parent.Visibility = System.Windows.Visibility.Visible;
                        parent.DockMode = DockMode.Docked;
                    }
                    else
                    {
                        TabSecondary.Items.Add(TabFieldList);
                    }
                }
                else
                {
                    TabFieldList.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else if (item.Header.Equals("Pivot Grid"))
            {
                if (item.IsChecked == true)
                {
                    TabPivotGrid.Visibility = System.Windows.Visibility.Visible;
                    var parent = FindParentTab(TabPivotGrid);
                    if (parent != null)
                    {
                        parent.Visibility = System.Windows.Visibility.Visible;
                        parent.DockMode = DockMode.Docked;
                    }
                    else
                    {
                        TabMain.Items.Add(TabPivotGrid);
                    }
                }
                else
                {
                    TabPivotGrid.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            if (item.Header.Equals("Chart"))
            {
                if (item.IsChecked == true)
                {
                    TabChart.Visibility = System.Windows.Visibility.Visible;
                    var parent = FindParentTab(TabChart);
                    if (parent != null)
                    {
                        parent.Visibility = System.Windows.Visibility.Visible;
                        parent.DockMode = DockMode.Docked;
                    }
                    else
                    {
                        TabMain.Items.Add(TabChart);
                    }
                }
                else
                {
                    TabChart.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
        }

        private C1DockTabControl FindParentTab(C1DockTabItem tab)
        {
            foreach (C1DockTabControl d in ViewDock.Items)
            {
                if (d.Items.Contains(tab))
                    return d;
            }
            return null;
        }

        private void ViewDock_ItemDockModeChanged(object sender, ItemDockModeChangedEventArgs e)
        {
            if (e.NewValue == DockMode.Hidden || e.NewValue == DockMode.Sliding)
            {
                if (e.TabControl.Items.Contains(TabFieldList))
                {
                    menuFieldListTab.IsChecked = false;
                }

                if (e.TabControl.Items.Contains(TabChart))
                {
                    menuChartTab.IsChecked = false;
                }

                if (e.TabControl.Items.Contains(TabPivotGrid))
                {
                    menuPivotGridTab.IsChecked = false;
                }
            }
            else
            {
                if (e.TabControl.Items.Contains(TabFieldList))
                {
                    menuFieldListTab.IsChecked = true;
                }

                if (e.TabControl.Items.Contains(TabChart))
                {
                    menuChartTab.IsChecked = true;
                }

                if (e.TabControl.Items.Contains(TabPivotGrid))
                {
                    menuPivotGridTab.IsChecked = true;
                }
            }
        }

        #endregion

        private void C1Toolbar_HelpClick(object sender, EventArgs e)
        {
            MessageBox.Show("This sample builds a custom OLAP application with a ribbon-like " +
                    "toolbar containing predefined views and filters. The OLAP controls " +
                    "are displayed in docking tabs so the user can customize their " +
                    "workspace. The C1OlapPanel control can be hidden or made sliding " +
                    "so the user can focus on the grid and chart.", "About");
        }
    }
}
