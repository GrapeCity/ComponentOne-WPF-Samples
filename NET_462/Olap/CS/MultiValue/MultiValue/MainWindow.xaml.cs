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

namespace MultiValue
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        // initialize after page has loaded (would be nicer if it wasn't this way...)
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // create some data to analyze
            var data = Product.GetProducts(10000);

            // assign data to olap page
            _olapPage.DataSource = data;

            // build an initial view
            var olap = _olapPage.OlapEngine;
            olap.BeginUpdate();

            // build the view
            olap.ShowTotalsColumns = olap.ShowTotalsRows = C1.Olap.ShowTotals.Subtotals;
            olap.ValueFields.MaxItems = 100;
            olap.RowFields.Add("Line");
            olap.ColumnFields.Add("Color");
            olap.ValueFields.Add("Price", "Weight", "Rating");

            // create a static style for the "Price" field
            var fs = olap.Fields["Price"].Style;
            fs.BackColor = Color.FromArgb(0xff, 240, 255, 240);

            // create conditional styles for all value fields
            foreach (var f in olap.ValueFields)
            {
                // high value cells are green and bold
                var fsh = f.StyleHigh;
                fsh.ConditionType = C1.Olap.ConditionType.Percentage;
                fsh.Value = 0.9;
                fsh.BackColor = Color.FromArgb(0xff, 0, 125, 0);
                fsh.ForeColor = Colors.White;
                fsh.FontBold = true;

                // low value cells are red and bold
                var fsl = f.StyleLow;
                fsl.ConditionType = C1.Olap.ConditionType.Percentage;
                fsl.Value = 0.1;
                fsl.BackColor = Color.FromArgb(0xff, 125, 0, 0);
                fsl.ForeColor = Colors.White;
                fsl.FontBold = true;
            }

            // view is ready
            olap.EndUpdate();
        }
    }
}
