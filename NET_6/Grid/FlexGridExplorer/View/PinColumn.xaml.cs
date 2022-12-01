using C1.WPF.Core;
using C1.WPF.Grid;
using C1.WPF.Menu;
using FlexGridExplorer.Resources;
using System.Windows.Controls;

namespace FlexGridExplorer
{
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
            var column = e.Column;
            var isFrozen = e.Column.Index < grid.FrozenColumns;
            var menuItem = new C1MenuItem();
            menuItem.Header = isFrozen ? AppResources.UnpinColumn : AppResources.PinColumn;
            menuItem.IconTemplate = Resources[isFrozen ? "UnpinIconTemplate" : "PinIconTemplate"] as C1IconTemplate;
            menuItem.Tag = e.Column;
            menuItem.Click += (s, e2) =>
            {
                if (isFrozen)
                {
                    grid.FrozenColumns--;
                    grid.Columns.Move(column.Index, column.Grid.FrozenColumns);
                }
                else
                {
                    grid.Columns.Move(column.Index, column.Grid.FrozenColumns);
                    grid.FrozenColumns++;
                }
                e.Close();
            };

            e.Menu.Items.Insert(0, menuItem);

            if (e.Menu.Items.Count > 1)
                e.Menu.Items.Insert(1, new C1MenuSeparator());
        }
    }
}
