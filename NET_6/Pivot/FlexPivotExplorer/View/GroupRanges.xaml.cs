using C1.PivotEngine;
using C1.WPF.Core;
using C1.WPF.Ribbon;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for GroupRanges.xaml
    /// </summary>
    public partial class GroupRanges : UserControl
    {
        bool isLoaded = false;

        public GroupRanges()
        {
            InitializeComponent();
            Tag = Properties.Resources.GroupRangesDesc;

            flexPivotPage.Loaded += (s, ea) =>
            {
                if (!flexPivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                var fpEngine = flexPivotPage.FlexPivotPanel.C1PivotEngine;
                // apply update to view
                fpEngine.BeginUpdate();
                fpEngine.DataSource = Utils.PivotDataSet.Tables[0].DefaultView;
                // set value field limit to 4 (**default is 1)
                fpEngine.ValueFields.MaxItems = 4;

                fpEngine.RowFields.Add("UnitPrice");
                fpEngine.ValueFields.Add("UnitPrice");
                fpEngine.RowFields[0].Caption = "Price group";
                fpEngine.RowFields[0].Range.RangeType = RangeType.Numeric;
                fpEngine.RowFields[0].Range.NumericStep = 50;
                fpEngine.ColumnFields.Add("Country");
                fpEngine.EndUpdate();
            };
        }
    }
}
