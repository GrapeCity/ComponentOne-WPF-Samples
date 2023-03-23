using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
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


        public override object GetCellContentType(GridCellType cellType, GridCellRange range)
        {
            if (cellType == GridCellType.RowHeader)
            {
                if (!UseDataIndex || Grid.Rows[range.Row] is GridBoundRow)
                    return typeof(RowHeaderNumbersCellFactory);
            }
            return base.GetCellContentType(cellType, range);
        }

        public override FrameworkElement CreateCellContent(GridCellType cellType, GridCellRange range, object cellContentType)
        {
            if (cellType == GridCellType.RowHeader && cellContentType as Type == typeof(RowHeaderNumbersCellFactory))
            {
                return new TextBlock() { Margin = new Thickness(4), VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
            }
            return base.CreateCellContent(cellType, range, cellContentType);
        }

        public override void BindCellContent(GridCellType cellType, GridCellRange range, FrameworkElement cellContent)
        {
            if (cellType == GridCellType.RowHeader && cellContent is TextBlock textBlock)
            {
                var index = range.Row;
                if (UseDataIndex)
                    index = (Grid.Rows[index]as GridBoundRow).DataIndex;
                textBlock.Text = (index + (UseZeroBasedIndex ? 0 : 1)).ToString("N0");
            }
            else
            {
                base.BindCellContent(cellType, range, cellContent);
            }
        }
    }

}
