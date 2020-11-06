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

namespace TreeFactory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1.WPF.FlexGrid.C1FlexGrid _flex;
        Random _rnd = new Random(0);
        public MainWindow()
        {
            InitializeComponent();

            _flex = new C1.WPF.FlexGrid.C1FlexGrid();
            _flex.FontFamily = new System.Windows.Media.FontFamily("Arial");
            _flex.FontSize = 15;
            _flex.GridLinesVisibility = C1.WPF.FlexGrid.GridLinesVisibility.All;
            _flex.AreRowGroupHeadersFrozen = false;
            _flex.MergeManager = null;
            _flex.GroupRowBackground = new SolidColorBrush(Colors.White);
            LayoutRoot.Children.Add(_flex);

            for (int i = 0; i < 10; i++)
            {
                var c = new C1.WPF.FlexGrid.Column();
                c.Width = new GridLength(200);
                c.Header = "Column " + i.ToString();
                _flex.Columns.Add(c);
            }

            // add rows
            AddRows(0);

            // apply custom cell factory
            _flex.CellFactory = new TreeCellFactory();
        }

        void AddRows(int level)
        {
            var count = _rnd.Next(0, 5);
            for (int i = 0; i < count; i++)
            {
                var gr = new C1.WPF.FlexGrid.GroupRow();
                _flex.Rows.Add(gr);
                gr.Level = level;
                gr[0] = string.Format("row {0}, level {1}", i, level);
                if (level < 3)
                {
                    AddRows(level + 1);
                }
            }
        }
    }
}
