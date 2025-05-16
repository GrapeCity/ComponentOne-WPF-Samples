using C1.PivotEngine;
using System.Windows.Controls;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for MultiValues.xaml
    /// </summary>
    public partial class MultiValues : UserControl
    {
        bool isLoaded = false;
        public MultiValues()
        {
            InitializeComponent();
            Tag = Properties.Resources.MultiValue;

            pivotPage.Loaded += (s, ea) =>
            {
                if (!pivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                // create some data to analyze
                var data = Product.GetProducts(10000);

                // assign data to olap page
                pivotPage.DataSource = data;

                // build an initial view
                var fpEngine = pivotPage.C1PivotEngine;
                fpEngine.BeginUpdate();

                // build the view
                fpEngine.ShowTotalsColumns = fpEngine.ShowTotalsRows = ShowTotals.Subtotals;
                fpEngine.ValueFields.MaxItems = 100;
                fpEngine.RowFields.Add("Line");
                fpEngine.ColumnFields.Add("Color");
                fpEngine.ValueFields.Add("Price", "Weight", "Rating");

                // view is ready
                fpEngine.EndUpdate();
            };
        }
    }
}
