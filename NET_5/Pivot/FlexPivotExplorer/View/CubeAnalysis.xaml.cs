using C1.WPF.Core;
using C1.WPF.Ribbon;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for FlexPivotDemo.xaml
    /// </summary>
    public partial class CubeAnalysis : UserControl
    {
        bool isLoaded = false;

        public CubeAnalysis()
        {
            InitializeComponent();
            Tag = Properties.Resources.CubeAnalysisDesc;

            flexPivotPage.Loaded += (s, ea) =>
            {
                if (!flexPivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                var fpEngine = flexPivotPage.FlexPivotPanel.FlexPivotEngine;

                // prepare to build view 
                string connectionString = @"Data Source=http://ssrs.componentone.com/OLAP/msmdpump.dll;Provider=msolap;Initial Catalog=AdventureWorksDW2012Multidimensional";
                string cubeName = "Adventure Works";
                try
                {
                    flexPivotPage.FlexPivotPanel.ConnectCube(cubeName, connectionString);

                    // show some data.                    
                    fpEngine.BeginUpdate();
                    fpEngine.ColumnFields.Add("Date.Fiscal Year");
                    fpEngine.RowFields.Add("Category");
                    fpEngine.ValueFields.Add("Internet Revenue Trend", "Internet Revenue Status");
                    fpEngine.EndUpdate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                fpEngine.Updated += (s1, e) =>
                {
                    flexPivotPage.FlexPivotGrid.Opacity = 1;
                    info.Visibility = Visibility.Collapsed;
                };

                fpEngine.UpdateProgressChanged += (s1, e) =>
                {
                    flexPivotPage.FlexPivotGrid.Opacity = 0.4;
                    info.Visibility = Visibility.Visible;
                    //progress.Value = (int)e.ProgressPercentage;
                    txt.Text = e.ProgressPercentage.ToString() + " %";
                };
            };
        }
    }
}
