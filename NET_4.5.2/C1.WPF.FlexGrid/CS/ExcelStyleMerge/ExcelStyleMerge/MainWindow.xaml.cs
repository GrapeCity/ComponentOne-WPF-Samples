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
using C1.WPF.FlexGrid;

namespace ExcelStyleMerge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ExcelStyleMergeManager _xlMergeManager;
        public MainWindow()
        {
            InitializeComponent();
            _xlMergeManager = new ExcelStyleMergeManager(_flex);
            _flex.MergeManager = _xlMergeManager;
            _flex.AllowMerging = AllowMerging.Cells;
            for (int r = 0; r < 50; r++)
            {
                _flex.Rows.Add(new Row());
            }
            for (int c = 0; c < 10; c++)
            {
                _flex.Columns.Add(new Column());
            }
            for (int r = 0; r < _flex.Rows.Count; r++)
            {
                for (int c = 0; c < _flex.Columns.Count; c++)
                {
                    _flex[r, c] = string.Format("r{0}, c{1}", r, c);
                }
            }
        }

        private void _btnMerge_Click(object sender, RoutedEventArgs e)
        {
            _xlMergeManager.AddMergedRange(_flex.Selection);
        }

        private void _btnSplit_Click(object sender, RoutedEventArgs e)
        {
            _xlMergeManager.RemoveMergedRange(_flex.Selection);
        }
    }
}
