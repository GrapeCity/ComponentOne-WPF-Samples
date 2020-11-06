using FlexGridExplorer.Resources;
using System.Collections.ObjectModel;

namespace FlexGridExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem(AppResources.GettingStartedTitle,
                AppResources.GettingStartedTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new GettingStarted())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
            _allItems.Add(new SampleItem(AppResources.ColumnDefinitionTitle,
                AppResources.ColumnDefinitionTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new ColumnDefinitions())));
            _allItems.Add(new SampleItem(AppResources.SelectionModesTitle,
                AppResources.SelectionModesTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new SelectionModes())));
            _allItems.Add(new SampleItem(AppResources.EditConfirmationTitle,
                AppResources.EditConfirmationTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new EditConfirmation())));
            _allItems.Add(new SampleItem(AppResources.EditingTitle,
                AppResources.EditingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new EditingForm())));
            _allItems.Add(new SampleItem(AppResources.ConditionalFormattingTitle,
                AppResources.ConditionalFormattingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new ConditionalFormatting())));
            _allItems.Add(new SampleItem(AppResources.CustomCellsTitle,
                AppResources.CustomCellsTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomCells())));
            _allItems.Add(new SampleItem(AppResources.GroupingTitle,
                AppResources.GroupingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new Grouping())));
            _allItems.Add(new SampleItem(AppResources.RowDetailsTitle,
                AppResources.RowDetailsTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new RowDetails())));
            //_allItems.Add(new SampleItem(AppResources.FilterTitle,
            //    AppResources.FilterTitle,
            //    AppResources.FilterDescription,
            //    new Filter()));
            _allItems.Add(new SampleItem(AppResources.FullTextFilterTitle,
                AppResources.FullTextFilterTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new FullTextFilter())));
            _allItems.Add(new SampleItem(AppResources.FilterRowTitle,
                AppResources.FilterRowTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new FilterRow())));
            _allItems.Add(new SampleItem(AppResources.ColumnLayoutTitle,
                AppResources.ColumnLayoutTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new ColumnLayout())));
            _allItems.Add(new SampleItem(AppResources.StarSizingTitle,
                AppResources.StarSizingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new StarSizing())));
            _allItems.Add(new SampleItem(AppResources.CellFreezingTitle,
                AppResources.CellFreezingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CellFreezing())));
            _allItems.Add(new SampleItem(AppResources.CustomMergingTitle,
                AppResources.CustomMergingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomMerging())));
            _allItems.Add(new SampleItem(AppResources.UnboundTitle,
                AppResources.UnboundTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new Unbound())));
            _allItems.Add(new SampleItem(AppResources.OnDemandTitle,
                AppResources.OnDemandTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new OnDemand())));
            _allItems.Add(new SampleItem(AppResources.CustomAppearanceTitle,
                AppResources.CustomAppearanceTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomAppearance())));
            _allItems.Add(new SampleItem(AppResources.NewRowTitle,
                AppResources.NewRowTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new NewRow())));
            _allItems.Add(new SampleItem(AppResources.CheckListTitle,
                AppResources.CheckListTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CheckList())));
            _allItems.Add(new SampleItem(AppResources.PagingTitle,
                AppResources.PagingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new Paging())));
            _allItems.Add(new SampleItem(AppResources.FinancialTitle,
                AppResources.FinancialTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new Financial())));
            _allItems.Add(new SampleItem(AppResources.LiveUpdatesTitle,
                AppResources.LiveUpdatesTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new LiveUpdates())));
            _allItems.Add(new SampleItem(AppResources.CustomSortIconTitle,
                AppResources.CustomSortIconTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomSortIcon())));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
