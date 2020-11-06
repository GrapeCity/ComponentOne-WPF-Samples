using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public class SampleItem
    {
        public SampleItem(string name, string title, Lazy<UserControl> sample)
        {
            Name = name;
            Title = title;
            Sample = sample;
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description => Sample.Value.Tag?.ToString();
        public Lazy<UserControl> Sample { get; set; }
    }
}
