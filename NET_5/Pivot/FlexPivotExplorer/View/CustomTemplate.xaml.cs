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
using C1.WPF.Pivot;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for CustomTemplate.xaml
    /// </summary>
    public partial class CustomTemplate : UserControl
    {
        bool isLoaded = false;
        public CustomTemplate()
        {
            InitializeComponent();
            Tag = "Shows how to customize the FlexPivotPage component by creating a custom template based on the default one. \r" +
                "The sample creates a custom template(located in App.xaml) for the FlexPivotPage, this template is a customized version of the default one, the changes made to the template are:\r" +
                "The FlexPivotPanel is located on the right side of the FlexPivotPage.\r" +
                "The FlexPivotChart has been removed from the TabPanel at the top of the page and is shown below the FlexPivotGrid";

            pivotPage.Loaded += (s, ea) =>
            {
                if (!pivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                // set columns totals chart
                pivotPage.ChartType = ChartType.Column;
                pivotPage.ChartTotals = true;
                // show sales by country and order date
                var fpEngine = pivotPage.FlexPivotEngine;
                fpEngine.DataSource = Utils.PivotDataSet.Tables[0].DefaultView;
                fpEngine.BeginUpdate();
                fpEngine.RowFields.Add("Country");
                fpEngine.ColumnFields.Add("OrderDate");
                fpEngine.ValueFields.Add("ExtendedPrice");
                fpEngine.Fields["OrderDate"].Format = "MMM";

                fpEngine.EndUpdate();
            };
        }
    }
}
