using System.Windows.Controls;

namespace SchedulerExplorer
{
    public class SampleItem
    {
        public SampleItem(string title, string name, string desc, Control sample)
        {
            Name = name;
            Title = title;
            Description = desc;
            Sample = sample;
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Control Sample { get; set; }
    }
}