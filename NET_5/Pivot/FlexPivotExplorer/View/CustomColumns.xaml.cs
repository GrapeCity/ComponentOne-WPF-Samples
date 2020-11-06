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
using C1.WPF.Pivot;
using C1.Zip;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for CustomColumns.xaml
    /// </summary>
    public partial class CustomColumns : UserControl
    {
        bool isLoaded = false;
        public CustomColumns()
        {
            InitializeComponent();
            Tag = "Shows a custom calculated column in FlexPivotGrid\r" +
                "This project shows sales by country and category.It also shows a couple calculated columns that show the difference in product sales a custom total calculation.";
            //Loaded += MainPage_Loaded;

            pivotPage.Loaded += (s, ea) =>
            {
                if (!pivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                // show sales by country and category
                var fpEngine = pivotPage.FlexPivotPanel.FlexPivotEngine;
                fpEngine.BeginUpdate();
                //fpEngine.
                fpEngine.Updated += fpEngine_Updated;
                fpEngine.ValueFields.MaxItems = 3;
                fpEngine.DataSource = Utils.PivotDataSet.Tables[0].DefaultView;
                fpEngine.RowFields.Add("Country");
                fpEngine.ColumnFields.Add("Salesperson");
                fpEngine.ValueFields.Add("ExtendedPrice");
                fpEngine.Fields["ExtendedPrice"].Format = "n0";
                fpEngine.Fields["ExtendedPrice"].Caption = "Sales";
                fpEngine.EndUpdate();
            };

        }

        void fpEngine_Updated(object sender, EventArgs e)
        {
            // add a new calculated column to the output table
            var fpEngine = pivotPage.FlexPivotEngine;
            var dt = fpEngine.FlexPivotTable;
            if (dt.Columns.Count >= 2)
            {
                dt.Columns.Add("Diff", typeof(double),
                    string.Format("[{0}] - [{1}]",
                        dt.Columns[1].ColumnName, dt.Columns[0].ColumnName));
                dt.Columns.Add("Prod", typeof(double),
                    string.Format("[{0}] * [{1}]",
                        dt.Columns[1].ColumnName, dt.Columns[0].ColumnName));

                // format the new columns on the grid
                var cols = pivotPage.FlexPivotGrid.Columns;
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
