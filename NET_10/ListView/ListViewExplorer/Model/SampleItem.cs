using System;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public class SampleItem
    {
        Lazy<Control> _getSample;
        public SampleItem(string name, string title, Func<Control> sample)
        {
            Name = name;
            Title = title;
            _getSample = new Lazy<Control>(sample);
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public Control Sample
        {
            get
            {
                return _getSample.Value;
            }
        }
        public string Description => Sample.Tag?.ToString();
    }
}
