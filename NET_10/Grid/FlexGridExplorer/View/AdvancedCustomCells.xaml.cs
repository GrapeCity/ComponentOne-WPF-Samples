using C1.WebStandards.Svg;
using C1.WPF.Core.Svg;
using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexGridExplorer
{
    public partial class AdvancedCustomCells : UserControl
    {
        public AdvancedCustomCells()
        {
            InitializeComponent();
            Tag = AppResources.AdvancedCustomCellsDescription;
        }
    }

    public class MyCustomCellFactory : GridCellFactory
    {
        private Random _rand = new Random();
        private bool?[,] _cells;

        public MyCustomCellFactory()
        {
            AllowCustomCells = true;

            _cells = new bool?[3, 3];

            for (int row = 0; row < 3; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    _cells[row, column] = _rand.NextDouble() > 0.5;
                }
            }
        }

        public override object GetCellKind(GridCellType cellType, GridCellRange range)
        {
            return typeof(MyCustomCell);
        }

        public override GridCellView CreateCell(GridCellType cellType, GridCellRange range, object cellKind)
        {
            return new MyCustomCell();
        }

        public override void BindCell(GridCellType cellType, GridCellRange range, GridCellView cell)
        {
            (cell as MyCustomCell).IsCross = _cells[range.Row, range.Column] ?? false;
        }

        public override void UnbindCell(GridCellType cellType, GridCellRange range, GridCellView cell)
        {
        }

        protected override void OnCellTapped(GridControlTapEventArgs e)
        {
            base.OnCellTapped(e);
            _cells[e.CellRange.Row, e.CellRange.Column] = !_cells[e.CellRange.Row, e.CellRange.Column];
            Grid.Refresh(GridCellType.Cell, new GridCellRange(e.CellRange.Row, e.CellRange.Column));
        }
    }

    public class MyCustomCell : GridCellView
    {
        private readonly C1SvgDoc _crossSvg = C1SvgDoc.Parse(Application.GetResourceStream(new Uri("Resources/cross.svg", UriKind.Relative)).Stream);
        private readonly C1SvgDoc _circleSvg = C1SvgDoc.Parse(Application.GetResourceStream(new Uri("Resources/circle.svg", UriKind.Relative)).Stream);

        private bool _isCross;
        public bool IsCross
        {
            get => _isCross;
            set
            {
                _isCross = value;
                InvalidateVisual();
            }
        }

        protected override void OnRenderBackground(DrawingContext drawingContext, Rect backgroundArea)
        {
            base.OnRenderBackground(drawingContext, backgroundArea);
            backgroundArea.Inflate(-8, -8);//Adds some padding
            if (IsCross)
            {
                drawingContext.DrawSvg(_crossSvg, Foreground, backgroundArea);
            }
            else
            {
                drawingContext.DrawSvg(_circleSvg, Foreground, backgroundArea);
            }
        }
    }
}
