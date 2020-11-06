using C1.WPF.Sparkline;
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

namespace SparklineSamples
{
    /// <summary>
    /// Interaction logic for SparklineDemo.xaml
    /// </summary>
    public partial class SparklineDemo : UserControl
    {
        private SampleData sampleData = new SampleData();

        public SparklineDemo()
        {
            InitializeComponent();
            sparklineType.ItemsSource = Enum.GetValues(typeof(SparklineType));
            sparklineType.SelectedItem = sparkline.SparklineType;
            sparkline.Data = sampleData.DefaultData;
        }

        private void sparklineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sparkline.SparklineType = (SparklineType)sparklineType.SelectedItem;
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox.IsChecked.Value)
            {
                sparkline.DateAxisData = sampleData.DefaultDateAxisData;
            }
            else
            {
                if (sparkline.DateAxisData != null)
                    sparkline.DateAxisData = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 12;
            double[] vals = new double[cnt];
            Random rnd = new Random();
            for (int i = 0; i < cnt; i++)
            {
                vals[i] = rnd.Next(-50, 50);
            }
            sparkline.Data = vals;
        }
    }
}
