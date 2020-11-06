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

namespace CubeAnalysis
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // bind olap page to data
            _c1OlapPage.Loaded += (s, ea) =>
            {
                // prepare to build view 
           //   string connectionString = @"Data Source=http://c1db1.componentone.com/olap/msmdpump.dll;Provider=msolap;Initial Catalog=AdventureWorksDW2012Multidimensional-EE";
                string connectionString = @"Data Source=http://ssrs.componentone.com/OLAP/msmdpump.dll;Provider=msolap;Initial Catalog=AdventureWorksDW2012Multidimensional";

                string cubeName = "Adventure Works"; 
                try
                {
                    _c1OlapPage.OlapPanel.ConnectCube(cubeName, connectionString);

                    // show some data.
                    var olap = _c1OlapPage.OlapEngine;
                    olap.BeginUpdate();
                    olap.ColumnFields.Add("Color");
                    olap.RowFields.Add("Category");
                    olap.ValueFields.Add("Order Count");
                    olap.EndUpdate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                _c1OlapPage.OlapEngine.Updated += (s1, e) =>
                {
                    _c1OlapPage.OlapGrid.Opacity = 1;
                    info.Visibility = Visibility.Collapsed;
                };

                _c1OlapPage.OlapEngine.UpdateProgressChanged += (s1, e) =>
                {
                    _c1OlapPage.OlapGrid.Opacity = 0.4;
                    info.Visibility = Visibility.Visible;
                    progress.Value = (int)e.ProgressPercentage;
                    txt.Text = e.ProgressPercentage.ToString() + " %";
                };
            };
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            // refresh olap output
            _c1OlapPage.OlapPanel.OlapEngine.Update();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Cancel the data processing.
            _c1OlapPage.OlapPanel.OlapEngine.CancelUpdate();
        }
    }
}
