using C1.PivotEngine;
using C1.WPF.Core;
using C1.WPF.Pivot;
using C1.WPF.Ribbon;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for FlexPivotDemo.xaml
    /// </summary>
    public partial class TopNDemo : UserControl
    {
        bool isLoaded = false;
        C1PivotEngine fpEngine;

        public TopNDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.TopNDemoDesc;

            this.Loaded += (s, ea) =>
            {
                if (!pivotGrid.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                // bind to data source
                var panel = new FlexPivotPanel();
                panel.DataSource = Utils.PivotDataSet.Tables[0].DefaultView;
                pivotGrid.ItemsSource = panel;

                fpEngine = panel.C1PivotEngine;
                // apply update to view
                fpEngine.BeginUpdate();
                fpEngine.RowFields.Add("Country");
                fpEngine.ColumnFields.Add("Salesperson");
                fpEngine.ValueFields.Add("ExtendedPrice");
                fpEngine.Fields["ExtendedPrice"].Format = "c0";
                fpEngine.EndUpdate();
            };
        }

        private void rdoBtn_Checked(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
                return;
            var filter = fpEngine.RowFields["Country"].Filter;
            if (sender == rdoBtnAll)
                filter.TopN = 0;
            else
            {
                filter.TopN = 3;
                if (sender == rdoBtnTop3)
                    filter.TopNRule = TopNRule.TopN;
                else
                    filter.TopNRule = TopNRule.BottomN;
            }
        }
    }
}
