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
using System.Data;
using System.Reflection;
using C1.Zip;

namespace OlapQuickStart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // load data from embedded zip resource
            var ds = new DataSet();
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("OlapQuickStart.nwind.zip"))
            {
                var zip = new C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    // load data
                    ds.ReadXml(zr);
                }
            }

            _c1OlapPage.Loaded += (s, ea) =>
            {
                // bind olap page to data
                _c1OlapPage.DataSource = ds.Tables[0].DefaultView;

                // show sales by country and category
                var olap = _c1OlapPage.OlapPanel.OlapEngine;
                olap.DataSource = ds.Tables[0].DefaultView;
                olap.BeginUpdate();
                olap.RowFields.Add("Country");
                olap.ColumnFields.Add("Category");
                olap.ValueFields.Add("Sales");
                olap.Fields["Sales"].Format = "n0";
                olap.EndUpdate();
            };
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            // regenerate the olap view
            _c1OlapPage.OlapPanel.OlapEngine.Update();
        }
    }
}
