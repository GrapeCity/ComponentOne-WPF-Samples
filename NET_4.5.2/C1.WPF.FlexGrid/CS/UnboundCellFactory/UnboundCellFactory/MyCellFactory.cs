using System;
using System.IO;
using System.Collections.Generic;
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
    public class MyCellFactory : CellFactory
    {
        // ** object model
        public event EventHandler<CellRangeEventArgs> CellValueChanged;
        void OnCellValueChanged(int row, int col)
        {
            if (CellValueChanged != null)
            {
                var range = new CellRange(row, col);
                CellValueChanged(this, new C1.WPF.FlexGrid.CellRangeEventArgs(null, range));
            }
        }

        // ** overrides
        public override void CreateCellContent(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            var col = grid.Columns[rng.Column];

            // use check boxes for boolean columns
            if (col.DataType == typeof(bool))
            {
                var val = grid[rng.Row, rng.Column];
                var checkCell = new CheckCell(val is bool ? (bool)val : false);
                bdr.Child = checkCell;
                return;
            }

            // use images for FaultType columns
            if (col.DataType == typeof(FaultType))
            {
                var val = grid[rng.Row, rng.Column];
                var faultCell = new FaultCell(val is FaultType ? (FaultType)val : FaultType.None);
                bdr.Child = faultCell;
                return;
            }

            // use radio buttons for TestType columns
            if (col.DataType == typeof(TestType))
            {
                var val = grid[rng.Row, rng.Column];
                var testCell = new TestTypeCell(val is TestType ? (TestType)val : TestType.Linear);
                bdr.Child = testCell;
                return;
            }

            // allow default factory
            base.CreateCellContent(grid, bdr, rng);
        }
        public override void CreateCellContentEditor(C1FlexGrid grid, Border bdr, CellRange rng)
        {
            base.CreateCellContentEditor(grid, bdr, rng);

            // listen to changes in combo columns
            var col = grid.Columns[rng.Column];
            if (col.ValueConverter != null)
            {
                var tb = bdr.Child as TextBox;
                if (tb != null)
                {
                    tb.TextChanged += tb_TextChanged;
                }
            }
        }
        public override void DisposeCell(C1FlexGrid grid, CellType cellType, FrameworkElement cell)
        {
            var bdr = cell as Border;
            if (bdr != null)
            {
                var tb = bdr.Child as TextBox;
                if (tb != null)
                {
                    tb.TextChanged -= tb_TextChanged;
                }
            }
            base.DisposeCell(grid, cellType, cell);
        }

        // ** utilities
        void tb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = sender as TextBox;
            var grid = Util.GetParentOfType(tb, typeof(C1FlexGrid)) as C1FlexGrid;
            var bdr = Util.GetParentOfType(tb, typeof(Border)) as Border;
            if (grid != null && bdr != null)
            {
                int row = (int)bdr.GetValue(Grid.RowProperty);
                int col = (int)bdr.GetValue(Grid.ColumnProperty);
                grid.FinishEditing();
                OnCellValueChanged(row, col);
            }
        }
        public static void StoreValue(FrameworkElement e, object value)
        {
            var grid = Util.GetParentOfType(e, typeof(C1FlexGrid)) as C1FlexGrid;
            var bdr = Util.GetParentOfType(e, typeof(Border)) as Border;
            if (grid != null && bdr != null)
            {
                int row = (int)bdr.GetValue(Grid.RowProperty);
                int col = (int)bdr.GetValue(Grid.ColumnProperty);
                grid.Select(row, col);
                grid[row, col] = value;

                var cf = grid.CellFactory as MyCellFactory;
                cf.OnCellValueChanged(row, col);
            }
        }
    }
    /// <summary>
    /// Cell used to display a boolean value.
    /// </summary>
    public class CheckCell : CheckBox
    {
        public CheckCell(bool isChecked)
        {
            IsChecked = isChecked;
            VerticalAlignment = VerticalAlignment.Center;
            HorizontalAlignment = HorizontalAlignment.Center;
            Checked += CheckCell_Checked;
            Unchecked += CheckCell_Checked;
        }
        void CheckCell_Checked(object sender, RoutedEventArgs e)
        {
            MyCellFactory.StoreValue(this, this.IsChecked);
        }
    }
    /// <summary>
    /// Cell used to display a FaultCell value.
    /// </summary>
    public class FaultCell : Image
    {
        static BitmapImage _imgNone, _imgGrey, _imgRed;
        FaultType _faultType;

        public FaultCell(FaultType faultType)
        {
            if (_imgNone == null)
            {
                _imgNone = Util.LoadImage("LEDgreenOn.png");
                _imgGrey = Util.LoadImage("LEDgreyOff.png");
                _imgRed = Util.LoadImage("LEDredOn.png");
            }
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Stretch = Stretch.None;//.Uniform;

            _faultType = faultType;
            UpdateImage();
        }
        public FaultType FaultType
        {
            get { return _faultType; }
            set
            {
                if (value != _faultType)
                {
                    _faultType = value;
                    UpdateImage();
                    MyCellFactory.StoreValue(this, value);
                }
            }
        }
        void UpdateImage()
        {
            switch (_faultType)
            {
                case FaultType.None:
                    this.Source = _imgNone;
                    break;
                case FaultType.Grey:
                    this.Source = _imgGrey;
                    break;
                case FaultType.Red:
                    this.Source = _imgRed;
                    break;
            }
        }
    }
    public class TestTypeCell : Grid
    {
        TestType _testType;
        public TestTypeCell(TestType testType)
        {
            _testType = testType;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            ColumnDefinitions.Add(new ColumnDefinition());
            AddButton("Lin");
            AddButton("Exp");
            AddButton("Ply");
        }
        void AddButton(string caption)
        {
            var btn = new RadioButton();
            btn.Content = caption;
            btn.Margin = new Thickness(4, 0, 4, 0);
            btn.IsChecked = (int)_testType == this.Children.Count;
            btn.Checked += btn_Checked;
            btn.SetValue(Grid.ColumnProperty, this.Children.Count);
            this.Children.Add(btn);
        }
        void btn_Checked(object sender, RoutedEventArgs e)
        {
            var btn = sender as RadioButton;
            _testType = (TestType)(int)btn.GetValue(Grid.ColumnProperty);
            MyCellFactory.StoreValue(this, _testType);
        }
    }
}
