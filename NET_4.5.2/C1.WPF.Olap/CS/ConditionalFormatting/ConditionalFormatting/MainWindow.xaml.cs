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
using C1.C1Zip;
using System.Reflection;

namespace ConditionalFormatting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // load Invoices data from embedded resource
            var ds = new DataSet();
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("ConditionalFormatting.nwind.zip"))
            {
                var zip = new C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    // load data
                    ds.ReadXml(zr);
                }
            }

            // bind olap page to data
            _c1OlapPage.Loaded += (s, ea) =>
            {
                // prepare to build view 
                _c1OlapPage.DataSource = ds.Tables[0].DefaultView;
                var olap = _c1OlapPage.OlapPanel.OlapEngine;
                olap.BeginUpdate();

                // show sales by country and category/product
                olap.ColumnFields.Add("Country");
                olap.RowFields.Add("Category", "Product");
                olap.Fields["Product"].Width = 250;
                var value = olap.Fields["Sales"];
                olap.ValueFields.Add(value);
                value.Format = "n0";

                // show values in the top 90% percentile in green
                var sh = value.StyleHigh;
                sh.Value = .90;
                sh.ConditionType = C1.Olap.ConditionType.Percentage;
                sh.ForeColor = Color.FromArgb(0xff, 0, 50, 0);
                sh.BackColor = Color.FromArgb(0xff, 200, 250, 200);
                sh.FontBold = true;

                // show values in the bottom 2% percentile in red
                var sl = value.StyleLow;
                sl.Value = .02;
                sl.ConditionType = C1.Olap.ConditionType.Percentage;
                sl.ForeColor = Color.FromArgb(0xff, 50, 0, 0);
                sl.BackColor = Color.FromArgb(0xff, 250, 200, 200);
                sl.FontBold = true;

                // view is ready
                olap.EndUpdate();
            };
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            // refresh olap output
            _c1OlapPage.OlapPanel.OlapEngine.Update();
        }
    }
}
