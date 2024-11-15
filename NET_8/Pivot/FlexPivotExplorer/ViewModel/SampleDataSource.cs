﻿using System;
using System.Collections.ObjectModel;

namespace FlexPivotExplorer
{
    public class SampleDataSource
    {
        private ObservableCollection<SampleItem> _allItems = new ObservableCollection<SampleItem>();

        public SampleDataSource()
        {
            _allItems.Add(new SampleItem(Properties.Resources.PivotTitle,
                Properties.Resources.PivotTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new FlexPivotDemo())));
            _ = _allItems[0].Sample.Value; //Force first page is loaded immediately

            _allItems.Add(new SampleItem(Properties.Resources.CellFactoryTitle,
                Properties.Resources.CellFactoryTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomCellFactory())));

            _allItems.Add(new SampleItem(Properties.Resources.ColumnTitle,
                Properties.Resources.ColumnTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomColumns())));

            _allItems.Add(new SampleItem(Properties.Resources.PageTitle,
                Properties.Resources.PageTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomPage())));

            _allItems.Add(new SampleItem(Properties.Resources.MultiValueTitle,
                Properties.Resources.MultiValueTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new MultiValues())));

            _allItems.Add(new SampleItem(Properties.Resources.TemplateTitle,
                Properties.Resources.TemplateTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CustomTemplate())));

            _allItems.Add(new SampleItem(Properties.Resources.DataEngineTitle,
                Properties.Resources.DataEngineTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new DataEngine())));

            _allItems.Add(new SampleItem(Properties.Resources.CubeAnalysisTitle,
                Properties.Resources.CubeAnalysisTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new CubeAnalysis())));

            _allItems.Add(new SampleItem(Properties.Resources.TopNDemoTitle,
                Properties.Resources.TopNDemoTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new TopNDemo())));
            
            _allItems.Add(new SampleItem(Properties.Resources.GroupRangesTitle,
                Properties.Resources.GroupRangesTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new GroupRanges())));

            _allItems.Add(new SampleItem(Properties.Resources.ConditionalFormattingTitle,
                Properties.Resources.ConditionalFormattingTitle,
                new System.Lazy<System.Windows.Controls.UserControl>(() => new ConditionalFormatting())));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get { return _allItems; }
        }
    }
}
