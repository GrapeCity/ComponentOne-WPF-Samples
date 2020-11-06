using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TreeMap : UserControl
    {
        object[] _data;
        List<string> _chartTypes;
        List<string> _palettes;
        List<int> _maxDepths;

        public TreeMap()
        {
            this.InitializeComponent();
            Tag = "This view shows main functionality of TreeMap control." + Environment.NewLine + Environment.NewLine +
               "TreeMap charts are compact way of visualizing hierarchical data in form of nested rectangles with area of each rectangle depicting the quantity of each category.";
        }

        public object[] Data
        {
            get
            {
                if (_data == null)
                    _data = DataCreator.CreateHierarchicalData();
                return _data;
            }
        }

        public List<string> ChartTypes
        {
            get
            {
                if(_chartTypes==null)
                    _chartTypes = Enum.GetNames(typeof(TreeMapType)).ToList<string>();
                return _chartTypes;
            }
        }

        public List<string> Palettes
        {
            get
            {
                if (_palettes == null)
                    _palettes = Enum.GetNames(typeof(Palette)).ToList<string>();
                return _palettes;
            }
        }

        public List<int> MaxDepths
        {
            get
            {
                if (_maxDepths == null)
                    _maxDepths = new List<int>() { 1, 2, 3, 4 };
                return _maxDepths;
            }
        }

        private void cbLabels_Checked(object sender, EventArgs e)
        {
            if (treeMap != null)
                treeMap.DataLabel.Content = cbLabels.IsChecked == true ? "{type}" : "";
        }
    }
}
