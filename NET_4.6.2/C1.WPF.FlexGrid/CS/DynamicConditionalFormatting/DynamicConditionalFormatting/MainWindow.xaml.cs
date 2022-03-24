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

namespace DynamicConditionalFormatting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var list = Product.GetProducts(1200);

            // bind to range sliders
            _rngPrice.HistogramValues = from p in list select p.Price;
            _rngPrice.ValueChanged += _rngPrice_ValueChanged;

            _rngWeight.HistogramValues = from p in list select p.Weight;
            _rngWeight.ValueChanged += _rngWeight_ValueChanged;

            // bind to grids
            _flex.ItemsSource = list;

        }

        // this shouldn't be necessary, seems like a WPF limitation
        // (bindings with converters don't seem to update correctly in column templates)
        void _rngPrice_ValueChanged(object sender, EventArgs e)
        {
            var c = _flex.Columns["Price"].Index;
            _flex.Invalidate(new C1.WPF.FlexGrid.CellRange(0, c, int.MaxValue, c));
            DynamicRangeConverter DRCPrice = this.Resources["DRCPrice"] as DynamicRangeConverter;
            DRCPrice.UpperValue = _rngPrice.UpperValue;
            DRCPrice.LowerValue = _rngPrice.LowerValue;
        }
        void _rngWeight_ValueChanged(object sender, EventArgs e)
        {
            var c = _flex.Columns["Weight"].Index;
            _flex.Invalidate(new C1.WPF.FlexGrid.CellRange(0, c, int.MaxValue, c));

            DynamicRangeConverter DRCWeight = this.Resources["DRCWeight"] as DynamicRangeConverter;
            DRCWeight.UpperValue = _rngWeight.UpperValue;
            DRCWeight.LowerValue = _rngWeight.LowerValue;
        }
    }
}
