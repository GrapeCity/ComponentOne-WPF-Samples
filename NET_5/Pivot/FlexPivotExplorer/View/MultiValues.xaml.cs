using C1.FlexPivot;
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
            Tag = "Shows how you can use FlexPivot to analyze multiple fields in one view.\r" +
                "The C1FlexPivotFieldList class has a new MaxItems property.This property allows you to determine how many fields are allowed in each field list (Rows, Columns, Filters, and Values).\r" +
                "If you set the MaxItems of the Values list to a number higher than one, users will be able to add multiple fields to the values list, and the analysis will be performed on all of them at once.\r" +
                "You can also use the MaxItems property on the Rows, Columns, and Filters lists if you want to limit the number of fields users can add to those lists. It rarely makes sense to have more than three or four Row or Column fields for example.";

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
                var fpEngine = pivotPage.FlexPivotEngine;
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
