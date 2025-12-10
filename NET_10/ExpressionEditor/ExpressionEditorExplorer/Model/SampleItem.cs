using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExpressionEditorExplorer
{
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
