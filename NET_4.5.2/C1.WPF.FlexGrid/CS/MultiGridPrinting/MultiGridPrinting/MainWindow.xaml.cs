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
using System.ComponentModel;
using C1.WPF.FlexGrid;

namespace MultiGridPrinting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ICollectionView _data = Product.GetProducts(30);
        public MainWindow()
        {
            InitializeComponent();

            // populate grids
            _flex.ItemsSource = _data;
            _flex.Columns["Line"].AllowMerging = true;
            _flex.Columns["Color"].AllowMerging = true;
            _flex2.ItemsSource = _data;

            // for testing
            _flex.AllowResizing = AllowResizing.Both;
        }

        private void _btnPrintGrids_Click(object sender, RoutedEventArgs e)
        {
            var pd = new PrintDialog();
            if (pd.ShowDialog().Value)
            {
                // get margins, scale mode
                var margin =
                    _cmbMargins.SelectedIndex == 0 ? 96.0 / 4 :
                    _cmbMargins.SelectedIndex == 1 ? 96.0 / 2 :
                    96.0;
                var scaleMode =
                    _cmbZoom.SelectedIndex == 0 ? ScaleMode.ActualSize :
                    _cmbZoom.SelectedIndex == 1 ? ScaleMode.PageWidth :
                    ScaleMode.SinglePage;

                // calculate page size
                var sz = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);
                
                // create paginator
                var paginator = new FlexPaginator(_flex, scaleMode, sz, new Thickness(margin), 100);
                paginator.AddPages(_flex2);
                pd.PrintDocument(paginator, "C1FlexGrid printing example");
            }
        }

        private void _chkGroup_Click(object sender, RoutedEventArgs e)
        {
            using (_data.DeferRefresh())
            {
                var gd = _data.GroupDescriptions;
                gd.Clear();
                if (_chkGroup.IsChecked.Value)
                {
                    _data.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Line"));
                    _data.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Color"));
                }
            }
        }

        private void _chkMerge_Click(object sender, RoutedEventArgs e)
        {
            _flex.AllowMerging = _chkMerge.IsChecked.Value ? AllowMerging.Cells : AllowMerging.None;
        }

        private void _chkFreeze_Click(object sender, RoutedEventArgs e)
        {
            if (_chkFreeze.IsChecked.Value)
            {
                _flex.Rows.Frozen = Math.Min(12, _flex.Selection.Row);
                _flex.Columns.Frozen = Math.Min(2, _flex.Selection.Column); ;
            }
            else
            {
                _flex.Rows.Frozen = 0;
                _flex.Columns.Frozen = 0;
            }
        }
    }
}
