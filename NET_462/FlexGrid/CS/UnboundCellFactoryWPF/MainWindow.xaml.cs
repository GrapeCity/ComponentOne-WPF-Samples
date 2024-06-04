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
using C1.WPF.Extended;

namespace UnboundCellFactoryWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // populate grid
            var c = new Column();
            c.Header = c.ColumnName = "Row#";
            c.IsReadOnly = true;
            _flex.Columns.Add(c);

            c = new Column();
            c.Header = c.ColumnName = "Cell Type";
            c.IsReadOnly = true;
            _flex.Columns.Add(c);

            c = new Column();
            c.Header = c.ColumnName = "Data";
            c.Width = new GridLength(250, GridUnitType.Pixel);
            _flex.Columns.Add(c);

            for (int i = 0; i < 50; i++)
            {
                AddRow(typeof(TextBox));
                AddRow(typeof(CheckBox));
                AddRow(typeof(ComboBox));
                AddRow(typeof(Slider));
                AddRow(typeof(C1ColorPicker));
            }

            // initialize custom cell factory
            _flex.Rows.MinSize = 35;
            _flex.CellFactory = new UnboundCellFactory(_flex);
            _ccf.Click += _ccf_Checked;
        }

        // add a new row to the grid, with an editor of the specified type
        void AddRow(Type cellType)
        {
            var r = new Row();
            r.Tag = cellType;
            _flex.Rows.Add(r);
            _flex[r.Index, "Row#"] = r.Index;
            _flex[r.Index, "Cell Type"] = cellType.Name;
        }

        // toggle custom cell factory
        void _ccf_Checked(object sender, RoutedEventArgs e)
        {
            _flex.CellFactory = _ccf.IsChecked.Value ? new UnboundCellFactory(_flex) : null;
        }
    }
}
