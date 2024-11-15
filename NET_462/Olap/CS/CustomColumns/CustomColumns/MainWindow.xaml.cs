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

namespace CustomColumns
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1OlapPage _c1OlapPage = new C1OlapPage();

        public MainWindow()
        {
            InitializeComponent();
            LayoutRoot.Children.Add(_c1OlapPage);
            Loaded += MainPage_Loaded;
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // load data from embedded zip resource
            var ds = new DataSet();
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("CustomColumns.nwind.zip"))
            {
                var zip = new C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    // load data
                    ds.ReadXml(zr);
                }
            }

            // show sales by country and category
            var olap = _c1OlapPage.OlapPanel.OlapEngine;
            olap.BeginUpdate();
            olap.Updated += olap_Updated;
            olap.ValueFields.MaxItems = 3;
            olap.DataSource = ds.Tables[0].DefaultView;
            olap.RowFields.Add("Country");
            olap.ColumnFields.Add("Category");
            olap.ValueFields.Add("Sales");
            olap.Fields["Sales"].Format = "n0";
            olap.EndUpdate();
        }
        void olap_Updated(object sender, EventArgs e)
        {
            // add a new calculated column to the output table
            var olap = _c1OlapPage.OlapEngine;
            var dt = olap.OlapTable;
            if (dt.Columns.Count >= 2)
            {
                dt.Columns.Add("Diff", typeof(double),
                    string.Format("[{0}] - [{1}]",
                        dt.Columns[1].ColumnName, dt.Columns[0].ColumnName));
                dt.Columns.Add("Prod", typeof(double),
                    string.Format("[{0}] * [{1}]",
                        dt.Columns[1].ColumnName, dt.Columns[0].ColumnName));

                // format the new columns on the grid
                var cols = _c1OlapPage.OlapGrid.Columns;
                for (int i = 0; i < 2; i++)
                {
                    var col = cols[cols.Count - 1 - i];
                    col.Format = "n2";
                    col.HeaderHorizontalAlignment = HorizontalAlignment.Center;
                }
            }
        }
    }
}
