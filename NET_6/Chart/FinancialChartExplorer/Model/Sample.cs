using System.Windows.Controls;
using System.Collections.Generic;

namespace FinancialChartExplorer
{
    public class Sample
    {
        public Sample(string name, Control view)
        {
            Name = name;
            View = view;
        }

        public string Name { get; set; }

        public string Description => View.Tag?.ToString();

        public bool IsSelected { get; set; }

        public Control View { get; set; }
    }

    public class SampleGroup
    {
        List<Sample> _samples = new List<Sample>();

        public SampleGroup(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public List<Sample> Samples
        {
            get { return _samples; }
        }
    }
}
