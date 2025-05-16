using C1.DataCollection;
using C1.WPF.Core;
using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Windows.Controls;
using System.Windows;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for CustomSortIcon.xaml
    /// </summary>
    public partial class CustomSortIcon : UserControl
    {
        public CustomSortIcon()
        {
            InitializeComponent();
            Tag = AppResources.CustomSortIconDescription;
            foreach (var value in Enum.GetValues(typeof(GridSortIconPosition)))
            {
                sortIconPosition.Items.Add(value.ToString());
            }
            sortIconPosition.SelectedIndex = 1;
            foreach (var value in new string[] { "Custom 1", "Custom 2", nameof(C1IconTemplate.TriangleUp), nameof(C1IconTemplate.TriangleNorth), nameof(C1IconTemplate.ChevronUp), nameof(C1IconTemplate.ArrowUp) })
            {
                sortIconTemplate.Items.Add(value);
            }
            sortIconTemplate.SelectedIndex = 0;
            foreach (var value in new string[] { nameof(HorizontalAlignment.Left), nameof(HorizontalAlignment.Center), nameof(HorizontalAlignment.Right), nameof(HorizontalAlignment.Stretch) })
            {
                headerAlignment.Items.Add(value);
            }
            headerAlignment.SelectedIndex = 0;
            lblIconPos.Text = AppResources.SortIconPosition;
            lblIconTemplate.Text = AppResources.SortIconTemplate;

            Load();
        }

        private async void Load()
        {
            var data = new C1DataCollection<Customer>(Customer.GetCustomerList(100));
            await data.SortAsync(new SortDescription("FirstName"), new SortDescription("LastName", SortDirection.Descending));
            grid.ItemsSource = data;
        }

        private void sortIconTemplate_SelectedIndexChanged(object sender, PropertyChangedEventArgs<int> e)
        {
            switch (e.NewValue)
            {
                case 0:
                    grid.SortAscendingIconTemplate = Resources["SortAscendingIcon"] as C1IconTemplate;
                    grid.SortDescendingIconTemplate = null;
                    grid.SortIndeterminateIconTemplate = null;
                    break;
                case 1:
                    grid.SortAscendingIconTemplate = Resources["Sort2AscendingIcon"] as C1IconTemplate;
                    grid.SortDescendingIconTemplate = Resources["Sort2DescendingIcon"] as C1IconTemplate;
                    grid.SortIndeterminateIconTemplate = Resources["Sort2Icon"] as C1IconTemplate;
                    break;
                case 2:
                    grid.SortAscendingIconTemplate = C1IconTemplate.TriangleUp;
                    grid.SortDescendingIconTemplate = null;
                    grid.SortIndeterminateIconTemplate = null;
                    break;
                case 3:
                    grid.SortAscendingIconTemplate = C1IconTemplate.TriangleNorth;
                    grid.SortDescendingIconTemplate = null;
                    grid.SortIndeterminateIconTemplate = null;
                    break;
                case 4:
                    grid.SortAscendingIconTemplate = C1IconTemplate.ChevronUp;
                    grid.SortDescendingIconTemplate = null;
                    grid.SortIndeterminateIconTemplate = null;
                    break;
                case 5:
                    grid.SortAscendingIconTemplate = C1IconTemplate.ArrowUp;
                    grid.SortDescendingIconTemplate = null;
                    grid.SortIndeterminateIconTemplate = null;
                    break;
            }
        }

        private void headerAlignment_SelectedIndexChanged(object sender, PropertyChangedEventArgs<int> e)
        {
            foreach (var column in grid.Columns)
            {

                switch (e.NewValue)
                {
                    case 0:
                        column.HeaderHorizontalAlignment = HorizontalAlignment.Left;
                        break;
                    case 1:
                        column.HeaderHorizontalAlignment = HorizontalAlignment.Center;
                        break;
                    case 2:
                        column.HeaderHorizontalAlignment = HorizontalAlignment.Right;
                        break;
                    case 3:
                        column.HeaderHorizontalAlignment = HorizontalAlignment.Stretch;
                        break;
                }
            }
            grid.Refresh(GridCellType.ColumnHeader);
        }

        private void sortIconPosition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is string item && Enum.IsDefined(typeof(GridSortIconPosition), item))
            {
                grid.SortIconPosition = (GridSortIconPosition)Enum.Parse(typeof(GridSortIconPosition), item);
            }            
        }
    }
}
