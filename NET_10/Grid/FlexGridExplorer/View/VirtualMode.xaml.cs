using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class VirtualMode : UserControl
    {
        public VirtualMode()
        {
            InitializeComponent();
            Tag = AppResources.VirtualModeDescription;
            grid.RowHeaderColumns[0].Width = GridLength.Auto;
            grid.RowHeaderColumns[0].MinWidth = 40;
            grid.ItemsSource = new VirtualModeDataCollection();
        }
    }

    public class RowHeaderNumbersCellFactory : GridCellFactory
    {
        bool _useDataIndex, _useZeroBasedIndex;

        public bool UseDataIndex
        {
            get
            {
                return _useDataIndex;
            }
            set
            {
                _useDataIndex = value;
                Grid?.Refresh();
            }
        }

        public bool UseZeroBasedIndex
        {
            get
            {
                return _useZeroBasedIndex;
            }
            set
            {
                _useZeroBasedIndex = value;
                Grid?.Refresh();
            }
        }

        public override bool AllowCustomCell(GridCellType cellType, GridCellRange range)
        {
            return true;
        }

        public override object GetCellKind(GridCellType cellType, GridCellRange range)
        {
            if (cellType == GridCellType.RowHeader)
            {
                if (!UseDataIndex || Grid.Rows[range.Row] is GridBoundRow)
                    return typeof(RowHeaderNumbersCellFactory);
            }
            return base.GetCellKind(cellType, range);
        }

        public override GridCellView CreateCell(GridCellType cellType, GridCellRange range, object cellKind)
        {
            if (cellType == GridCellType.RowHeader && cellKind as Type == typeof(RowHeaderNumbersCellFactory))
            {
                return new GridTextCellView() 
                { 
                    HorizontalTextAlignment = HorizontalAlignment.Center, 
                    VerticalTextAlignment = VerticalAlignment.Center 
                };
            }
            return base.CreateCell(cellType, range, cellKind);
        }

        public override void BindCell(GridCellType cellType, GridCellRange range, GridCellView cell)
        {
            if (cellType == GridCellType.RowHeader && cell is GridTextCellView textCellView)
            {
                var index = range.Row;
                if (UseDataIndex)
                    index = (Grid.Rows[index] as GridBoundRow).DataIndex;
                textCellView.Text = (index + (UseZeroBasedIndex ? 0 : 1)).ToString("N0");
            }
            else
            {
                base.BindCell(cellType, range, cell);
            }
        }
    }
}
