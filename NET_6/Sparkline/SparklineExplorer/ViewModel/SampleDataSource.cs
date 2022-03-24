using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SparklineExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>();

            AllItems.Add(new SampleItem(
                Properties.Resources.SparklineDemoTitle,
                Properties.Resources.SparklineDemoTitle,
                new SparklineDemo()));
            AllItems.Add(new SampleItem(
                Properties.Resources.AppeareanceTitle,
                Properties.Resources.AppeareanceTitle,
                new AppearanceSample()));
            AllItems.Add(new SampleItem(
                Properties.Resources.RegionSalesTitle,
                Properties.Resources.RegionSalesTitle,
                new RegionSales()));
            AllItems.Add(new SampleItem(
                Properties.Resources.GridTitle,
                Properties.Resources.GridTitle,
                new FlexGridIntegration()));
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }

    public class SampleItem
    {
        public SampleItem(string name, string title, Control sample)
        {
            Name = name;
            Title = title;
            Sample = sample;
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description => Sample.Tag?.ToString();
        public Control Sample { get; set; }
    }
}
