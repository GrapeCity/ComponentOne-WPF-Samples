using C1.DataCollection;
using C1.WPF.Core;
using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Windows.Controls;


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
            foreach (var value in new string[] { nameof(C1IconTemplate.TriangleUp), nameof(C1IconTemplate.TriangleNorth), nameof(C1IconTemplate.ChevronUp), nameof(C1IconTemplate.ArrowUp) })
            {
                sortIconTemplate.Items.Add(value);
            }
            sortIconTemplate.SelectedIndex = 2;
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

        private void SortIconPositionChanged(object sender, EventArgs e)
        {
            grid.SortIconPosition = (GridSortIconPosition)Enum.Parse(typeof(GridSortIconPosition), (string)sortIconPosition.Items[sortIconPosition.SelectedIndex]);
        }

        private void SortIconTemplateChanged(object sender, EventArgs e)
        {
            switch (sortIconTemplate.SelectedIndex)
            {
                case 0:
                    grid.SortAscendingIconTemplate = C1IconTemplate.TriangleUp;
                    break;
                case 1:
                    grid.SortAscendingIconTemplate = C1IconTemplate.TriangleNorth;
                    break;
                case 2:
                    grid.SortAscendingIconTemplate = C1IconTemplate.ChevronUp;
                    break;
                case 3:
                    grid.SortAscendingIconTemplate = C1IconTemplate.ArrowUp;
                    break;
            }
        }
    }
}
