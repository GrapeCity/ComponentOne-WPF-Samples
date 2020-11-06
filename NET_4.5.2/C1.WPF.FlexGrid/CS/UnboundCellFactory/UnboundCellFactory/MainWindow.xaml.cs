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

namespace UnboundCellFactory
{
    public enum FaultType
    {
        None,
        Red,
        Grey
    }
    public enum TestType
    {
        Linear,
        Exponential,
        Ploynomial
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1FlexGrid _flex;

        public MainWindow()
        {
            InitializeComponent();

            // create grid
            _flex = new C1FlexGrid();
            _flex.RowBackground = new SolidColorBrush(Colors.White);
            _flex.AlternatingRowBackground = _flex.RowBackground;
            _flex.GridLinesVisibility = C1.WPF.FlexGrid.GridLinesVisibility.All;
            LayoutRoot.Children.Add(_flex);

            // add extra column header row
            _flex.ColumnHeaders.Rows.Add(new Row());

            // set up merging
            _flex.AllowMerging = AllowMerging.ColumnHeaders;
            _flex.ColumnHeaders.Rows[0].AllowMerging = true;

            // add columns
            AddColumn("TRANSDUCERS", "No", typeof(int), 30, HorizontalAlignment.Center);
            AddColumn("TRANSDUCERS", "Use", typeof(string), 60, HorizontalAlignment.Right);
            AddColumn("TRANSDUCERS", "Name", typeof(string), 80, HorizontalAlignment.Left);
            AddColumn("TRANSDUCERS", "Parameter", typeof(string), 80, HorizontalAlignment.Left);
            AddColumn("TRANSDUCERS", "Fullscale", typeof(int), 60, HorizontalAlignment.Center);
            AddColumn("TRANSDUCERS", "Units", typeof(string), 40, HorizontalAlignment.Center);
            AddColumn("TRANSDUCERS", "Test Type", typeof(TestType), 140, HorizontalAlignment.Center);
            AddColumn("Input", "Enable", typeof(bool), 60, HorizontalAlignment.Center);
            AddColumn("Input", "Fault", typeof(FaultType), 40, HorizontalAlignment.Center);
            AddColumn("Sense", "Enable", typeof(bool), 60, HorizontalAlignment.Center);
            AddColumn("Sense", "Fault", typeof(FaultType), 40, HorizontalAlignment.Center);
            AddColumn("Excitation", "Enable", typeof(bool), 60, HorizontalAlignment.Center);
            AddColumn("Excitation", "Fault", typeof(FaultType), 40, HorizontalAlignment.Center);

            // use combo to edit 'Use' column
            var options = "FB,CT,JK,CB,TZ,LZ".Split(',');
            _flex.Columns["TRANSDUCERS.Use"].ValueConverter = new ColumnValueConverter(options, true);

            // add some data
            for (int i = 0; i < 10; i++)
            {
                var row = new Row();
                _flex.Rows.Add(row);
                row["TRANSDUCERS.No"] = i;
                row["TRANSDUCERS.Use"] = "FB";
                row["TRANSDUCERS.Name"] = "Hello";
                row["TRANSDUCERS.Parameter"] = "Disp - FB";
                row["TRANSDUCERS.Fullscale"] = 100;
                row["TRANSDUCERS.Units"] = "mm";
                row["Input.Enable"] = true;
                row["Input.Fault"] = FaultType.None;
                row["Sense.Enable"] = true;
                row["Sense.Fault"] = FaultType.None;
                row["Excitation.Enable"] = true;
                row["Excitation.Fault"] = FaultType.None;
            }

            // apply custom cell factory
            var cf = new MyCellFactory();
            _flex.CellFactory = cf;
            cf.CellValueChanged += cf_CellValueChanged;
        }

        // do something when the value of a cell changes
        void cf_CellValueChanged(object sender, CellRangeEventArgs e)
        {
            Console.WriteLine("the value of cell ({0}, {1}) has changed to {2}", 
                e.Row, e.Column, _flex[e.Row, e.Column]);

            // update "Fault" column when "Enable" changes
            var col = _flex.Columns[e.Column];
            if (col.DataType == typeof(bool) && col.ColumnName.EndsWith("Enable"))
            {
                // get current 'Enable' value
                var value = (bool)_flex[e.Row,e.Column];

                // set corresponding 'Fault' value 
                // (in this case, a function of the 'Enable' cell, but could be anything) 
                System.Diagnostics.Debug.Assert(_flex.Columns[e.Column + 1].DataType == typeof(FaultType));
                _flex[e.Row, e.Column + 1] = value ? FaultType.None : FaultType.Red;

                // refresh cell
                _flex.Invalidate(new CellRange(e.Row, e.Column + 1));

                // if this is the Input.Enable cell, then enable/disable entire row
                if (col.ColumnName == "Input.Enable")
                {
                    var row= _flex.Rows[e.Row];
                    row.Foreground = value ? _brBlack : _brGray;
                    row.IsReadOnly = !value;
                }
            }
        }
        static Brush _brBlack = new SolidColorBrush(Colors.Black);
        static Brush _brGray = new SolidColorBrush(Colors.LightGray);

        // create a column with the given headers, type, and alignment
        Column AddColumn(string h1, string h2, Type colType, int width, HorizontalAlignment ha)
        {
            var col = new Column();
            _flex.Columns.Add(col);
            var colIndex = _flex.Columns.Count - 1;
            _flex.ColumnHeaders[0, colIndex] = h1;
            _flex.ColumnHeaders[1, colIndex] = h2;
            col.DataType = colType;
            col.Width = new GridLength(width);
            col.HorizontalAlignment = ha;
            col.HeaderHorizontalAlignment = HorizontalAlignment.Center;
            col.AllowDragging = false;
            col.ColumnName = string.Format("{0}.{1}", h1, h2);
            return col;
        }
    }
}
