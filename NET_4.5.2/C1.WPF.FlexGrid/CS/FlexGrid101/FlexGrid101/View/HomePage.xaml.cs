using FlexGrid101.Resources;
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
using System.Windows.Shapes;

namespace FlexGrid101
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();

            DataContext = GetSamples();
        }

        private List<Sample> GetSamples()
        {
            return new List<Sample>
            {
                new Sample() { Name = AppResources.GettingStartedTitle, Description = AppResources.GettingStartedDescription, SampleViewType = 1 , Thumbnail="flexgrid.png"},
                new Sample() { Name = AppResources.ColumnDefinitionTitle, Description = AppResources.ColumnDefinitionDescription, SampleViewType = 2 , Thumbnail="flexgrid_columns.png"},
                new Sample() { Name = AppResources.SelectionModesTitle, Description = AppResources.SelectionModesDescription, SampleViewType = 3 , Thumbnail="flexgrid_selection.png"},
                new Sample() { Name = AppResources.EditConfirmationTitle, Description = AppResources.EditConfirmationDescription, SampleViewType = 4 , Thumbnail="flexgrid_editConfirmation.png"},
                new Sample() { Name = AppResources.EditingTitle, Description = AppResources.EditingDescription, SampleViewType = 5 , Thumbnail="input_form.png"},
                new Sample() { Name = AppResources.ConditionalFormattingTitle, Description = AppResources.ConditionalFormattingDescription, SampleViewType = 6 , Thumbnail="flexgrid_conditionalFormatting.png"},
                new Sample() { Name = AppResources.CustomCellsTitle, Description = AppResources.CustomCellsDescription, SampleViewType = 7 , Thumbnail="flexgrid_custom.png"},
                new Sample() { Name = AppResources.GroupingTitle, Description = AppResources.GroupingDescription, SampleViewType = 8 , Thumbnail="flexgrid_grouping.png"},
                new Sample() { Name = AppResources.RowDetailsTitle, Description = AppResources.RowDetailsDescription, SampleViewType = 9 , Thumbnail="flexgrid_rowdetails.png"},
                new Sample() { Name = AppResources.FilterTitle, Description = AppResources.FilterDescription, SampleViewType = 10 , Thumbnail="flexgrid_filter.png"},
                new Sample() { Name = AppResources.FullTextFilterTitle, Description = AppResources.FullTextFilterDescription, SampleViewType = 11 , Thumbnail="filter.png"},
                new Sample() { Name = AppResources.ColumnLayoutTitle, Description = AppResources.ColumnLayoutDescription, SampleViewType = 12 , Thumbnail="flexgrid_columns.png"},
                new Sample() { Name = AppResources.StarSizingTitle, Description = AppResources.StarSizingDescription, SampleViewType = 13 , Thumbnail="flexgrid_starSizing.png"},
                new Sample() { Name = AppResources.CellFreezingTitle, Description = AppResources.CellFreezingDescription, SampleViewType = 14 , Thumbnail="flexgrid_freezing.png"},
                new Sample() { Name = AppResources.CustomMergingTitle, Description = AppResources.CustomMergingDescription, SampleViewType = 15 , Thumbnail="flexgrid_merging.png"},
                new Sample() { Name = AppResources.UnboundTitle, Description = AppResources.UnboundDescription, SampleViewType = 16 , Thumbnail="flexgrid_headers.png"},
                //new Sample() { Name = AppResources.OnDemandTitle, Description = AppResources.OnDemandDescription, SampleViewType = 17 , Thumbnail="flexgrid_loading.png"},
                new Sample() { Name = AppResources.CustomAppearanceTitle, Description = AppResources.CustomAppearanceDescription, SampleViewType = 18 , Thumbnail="flexgrid_customAppearance.png"},
                new Sample() { Name = AppResources.NewRowTitle, Description = AppResources.NewRowDescription, SampleViewType = 19 , Thumbnail="flexgrid_newRow.png"},
                //new Sample() { Name = AppResources.CheckListTitle, Description = AppResources.CheckListDescription, SampleViewType = 20 , Thumbnail="flexgrid.png"},

                new Sample() { Name = AppResources.FinancialTitle, Description = AppResources.FinancialDescription, SampleViewType = 22 , Thumbnail="flexgrid_financial.png"},
                new Sample() { Name = AppResources.GroupingPanelTitle, Description = AppResources.GroupingPanelDescription, SampleViewType = 23 , Thumbnail="flexgrid_groupingPanel.png"},
                new Sample() { Name = AppResources.CustomSortIconTitle, Description = AppResources.CustomSortIconDescription, SampleViewType = 24 , Thumbnail="flexgrid_customSort.png"},
            };
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Sample selectedItem = e.AddedItems[0] as Sample;
            if (selectedItem != null)
            {
                MainWindow frame = Application.Current.MainWindow as MainWindow;
                if (frame != null)
                {
                    Uri link;
                    if (selectedItem.Name.Contains("&"))
                    {
                        link = new Uri("/View/" + selectedItem.Name.Substring(0, selectedItem.Name.IndexOf("&")).Replace(" ", string.Empty) + ".xaml", UriKind.Relative);
                    }
                    else
                        link = new Uri("/View/" + selectedItem.Name.Replace(" ", string.Empty) + ".xaml", UriKind.Relative);
                    frame.Navigate(link);
                    frame.UpdateCaption(selectedItem.Name);
                }

            }
        }
    }
}
