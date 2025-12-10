using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ExtendedFeatures
{
    public class SampleDataSource
    {
        ObservableCollection<SampleGroup> _groups = new ObservableCollection<SampleGroup>();

        public SampleDataSource()
        {
            var heatMapGroup = new SampleGroup("Heat Map");
            heatMapGroup.Samples.Add(new Sample(
                "Color Scale",
                "Shows Heatmap plot with gradient color scale. The data from two-dimensional array is shown as a table. The color of table's cell depends on the corresponding data value.",
                new Samples.Temperature())
            { IsSelected = true });
            heatMapGroup.Samples.Add(new Sample(
                "Color Axis",
                "Shows Heatmap plot with gradient color axis. The data from function f(x,y) is displayed a heatmap with gradient colouring. The correspondence with color and function value is shown on the auxliary color axis.",
                new Samples.ColorAxis()));
            heatMapGroup.Samples.Add(new Sample(
                "Discrete Color Scale",
                "Shows Heatmap plot with disrete color scale. In case of disrete color scale, the color depends on which interval the corresponding data value belongs to. All data points from the same interval have the same color. The list of user-defined intervals is shown on the chart legend.",
                new Samples.Loading()));

            var contourGroup = new SampleGroup("Contour chart");
            contourGroup.Samples.Add(new Sample(
                "Color Scale",
                "Shows a Contour chart with gradient color scale",
                new Samples.GradientContour()
                ));

            Groups.Add(heatMapGroup);
            Groups.Add(contourGroup);
        }

        public ObservableCollection<SampleGroup> Groups
        {
            get
            {
                return _groups;
            }
        }

    }
}
