using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace DataManipulation
{
    public class SampleItem
    {
        public SampleItem(string name, string title, string desc, Control sample)
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
