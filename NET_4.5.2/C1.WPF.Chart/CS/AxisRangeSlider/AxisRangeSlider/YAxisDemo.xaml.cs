using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;

namespace AxisRangeSlider
{
    /// <summary>
    /// Interaction logic for YAxisDemo.xaml
    /// </summary>
    public partial class YAxisDemo : UserControl
    {
        public YAxisDemo()
        {
            InitializeComponent();
            //Calls to ChartDataCreator and creates the main and the axis charts
            ChartDataCreator _dtcrtor = new ChartDataCreator();
            _dtcrtor.DrawChart(_c1MainChart,true);
            _dtcrtor.DrawAxisChart(_c1ChartDup, this.FindResource("EmptyTemplate") as DataTemplate,true);
            
        }
        /// <summary>
        /// Sets the y-axis scale based on the slider's values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _slider_ValueChanged(object sender, EventArgs e)
        {
            if (_c1MainChart != null)
            {
                Axis ay = _c1MainChart.View.AxisY;
                ay.Scale = _c1Slider.UpperValue - _c1Slider.LowerValue;
                ay.Value = _c1Slider.LowerValue / (1 - ay.Scale);
            }
        }
    }
}
