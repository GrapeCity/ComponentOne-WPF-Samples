using C1.WPF.Grid;
using C1.WPF.Menu;
using FlexGridExplorer.Resources;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FlexGridExplorer;

public partial class PinColumn : UserControl
{
    public PinColumn()
    {
        InitializeComponent();

        Tag = AppResources.PinColumnDescription;

        grid.ItemsSource = Customer.GetCustomerList(100);

    }

    private void OnColumnOptionsLoading(object sender, GridColumnOptionsLoadingEventArgs e)
    {
        var grid = sender as FlexGrid;
        
        // Create a custom menu item.
        var menuItem = new PinMenuItem
        {
            CanPin = e.Column.Index > grid.FrozenColumns - 1,
            CanPinRight = grid.Columns.Count - e.Column.Index > grid.FrozenRightColumns,
            HeaderTemplate = FindResource("PinMenuItemKey") as DataTemplate,
            HorizontalContentAlignment = HorizontalAlignment.Stretch,
            PinCommand = new PinCommand
            {
                Grid = grid,
                ColumnIndex = e.Column.Index,
                CloseMenuAction = () => e.Close()
            }
        };

        e.Menu.Items.Insert(0, menuItem);

        if (e.Menu.Items.Count > 1)
        {
            e.Menu.Items.Insert(1, new C1MenuSeparator());
        }
    }
}

#region pin-column menu item

public class PinIconTemplateSelector : DataTemplateSelector
{
    public DataTemplate PinTemplate { get; set; }
    public DataTemplate UnpinTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        if (item is bool isPinAction)
        {
            return isPinAction ? PinTemplate : UnpinTemplate;
        }
        return UnpinTemplate;
    }
}

public class PinMenuItem : C1MenuItem
{
    public ICommand PinCommand { get; init; }
    public bool CanPin { get; init; }
    public bool CanPinRight { get; init; }
}

public enum PinSideButton
{
    Left,
    Right
}

public class PinCommand : ICommand
{
    public FlexGrid Grid
    {
        get
        {
            this._grid.TryGetTarget(out FlexGrid grid);
            return grid;
        }
        init 
        {
            _grid = new WeakReference<FlexGrid>(value);
        }
    }
    WeakReference<FlexGrid> _grid;

    public int ColumnIndex { get; init; }

    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    bool IsFrozen => ColumnIndex < Grid.FrozenColumns;
    bool IsFrozenRight => Grid.Columns.Count - 1 - ColumnIndex < Grid.FrozenRightColumns;

    public Action CloseMenuAction { get; init; }

    public void Execute(object parameter)
    {
        PinSideButton side = (PinSideButton)parameter;
        // Left pin side button.
        if (side == PinSideButton.Left)
        {
            if (IsFrozen)
            {
                Grid.FrozenColumns--;
                Grid.Columns.Move(ColumnIndex, Grid.FrozenColumns);
            }
            else
            {
                Grid.Columns.Move(ColumnIndex, Grid.FrozenColumns);
                Grid.FrozenColumns++;

                if (IsFrozenRight)
                {
                    Grid.FrozenRightColumns--;
                }
            }
        }
        else if (side == PinSideButton.Right)
        {
            if (IsFrozenRight)
            {
                Grid.FrozenRightColumns--;
                Grid.Columns.Move(ColumnIndex, Grid.Columns.Count - 1 - Grid.FrozenRightColumns);
            }
            else
            {
                Grid.Columns.Move(ColumnIndex, Grid.Columns.Count - 1 - Grid.FrozenRightColumns);
                Grid.FrozenRightColumns++;

                if (IsFrozen)
                {
                    Grid.FrozenColumns--;
                }
            }
        }

        CloseMenuAction?.Invoke();
    }
}

#endregion