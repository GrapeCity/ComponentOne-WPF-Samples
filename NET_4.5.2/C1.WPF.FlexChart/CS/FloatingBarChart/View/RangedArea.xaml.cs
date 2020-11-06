using C1.WPF.Chart;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using C1.Chart;

namespace FloatingBarChart
{
    /// <summary>
    /// Interaction logic for FloatBar.xaml
    /// </summary>
    public partial class RangedArea : UserControl
    {
        Series seriesA, seriesB;
        float columnWidthPercentage = 0.6f;
        public RangedArea()
        {
            InitializeComponent();
            seriesA = new Series();
            seriesA.Binding = "ClassALow,ClassAHigh";
            seriesA.SeriesName = "Class A";

            seriesB = new Series();
            seriesB.Binding = "ClassBLow,ClassBHigh";
            seriesB.SeriesName = "Class B";
            
            flexChart.Series.Add(seriesA);
            flexChart.Series.Add(seriesB);
            flexChart.ChartType = ChartType.Area;
            flexChart.LabelRendering += FlexChart_LabelRendering;
            flexChart.AxisY.MajorUnit = 10;
            flexChart.DataLabel.Position = LabelPosition.Bottom;
            flexChart.DataLabel.Content = "{seriesName}";
            flexChart.Options.ClusterSize = new ElementSize() { SizeType = ElementSizeType.Percentage, Value = columnWidthPercentage * 100 };
        }

        private void FlexChart_LabelRendering(object sender, RenderDataLabelEventArgs e)
        {
            SubjectScoreRange range = (SubjectScoreRange)e.Item;
            e.Text = e.Text.Equals("Class A") ? range.ClassALow + " - " + range.ClassAHigh : range.ClassBLow + " - " + range.ClassBHigh;
        }
       
    }
}
