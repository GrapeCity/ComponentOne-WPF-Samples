using C1.WPF.Pivot;
using System.Windows.Controls;

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
            Tag = Properties.Resources.Template;

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
                var fpEngine = pivotPage.C1PivotEngine;
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
