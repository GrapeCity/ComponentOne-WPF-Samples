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
            _allItems.Add(new SampleItem(AppResources.HierarchicalRowsTitle,
                AppResources.HierarchicalRowsTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new HierarchicalRows())));
            _allItems.Add(new SampleItem(AppResources.GroupingTitle,
                AppResources.GroupingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new Grouping())));
            _allItems.Add(new SampleItem(AppResources.RowDetailsTitle,
                AppResources.RowDetailsTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new RowDetails())));
            _allItems.Add(new SampleItem(AppResources.FullTextFilterTitle,
                AppResources.FullTextFilterTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new FullTextFilter())));
            _allItems.Add(new SampleItem(AppResources.FilterRowTitle,
                AppResources.FilterRowTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new FilterRow())));
            _allItems.Add(new SampleItem(AppResources.CustomFiltersTitle,
                AppResources.CustomFiltersTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomFilters())));
            _allItems.Add(new SampleItem(AppResources.AdvancedFilteringTitle,
                AppResources.AdvancedFilteringTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new AdvancedFiltering())));
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
            _allItems.Add(new SampleItem(AppResources.MouseHoverTitle,
                AppResources.MouseHoverTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new MouseHover())));
            _allItems.Add(new SampleItem(AppResources.ExportTitle,
                AppResources.ExportTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new Export())));
            _allItems.Add(new SampleItem(AppResources.ColumnOptionsTitle,
                AppResources.ColumnOptionsTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new ColumnOptions())));
            _allItems.Add(new SampleItem(AppResources.ExcelExportTitle,
                AppResources.ExcelExportTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new ExcelExport())));
            _allItems.Add(new SampleItem(AppResources.VirtualModeTitle,
                AppResources.VirtualModeTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new VirtualMode())));
            _allItems.Add(new SampleItem(AppResources.PinColumnTitle,
                AppResources.PinColumnTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new PinColumn())));
            _allItems.Add(new SampleItem(AppResources.AdvancedCustomCellsTitle,
                AppResources.AdvancedCustomCellsTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new AdvancedCustomCells())));
            _allItems.Add(new SampleItem(AppResources.ValidationTitle,
                AppResources.ValidationTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new Validation())));
            _allItems.Add(new SampleItem(AppResources.DataTableSampleTitle,
                AppResources.DataTableSampleTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new DataTableSample())));
            _allItems.Add(new SampleItem(AppResources.SelectedItemsTitle,
                AppResources.SelectedItemsTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new SelectedItems())));
            _allItems.Add(new SampleItem(AppResources.SummaryRowTitle,
                AppResources.SummaryRowTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new SummaryRow())));
            _allItems.Add(new SampleItem(AppResources.TransposedGridTitle,
                AppResources.TransposedGridTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new TransposedGrid())));

            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
