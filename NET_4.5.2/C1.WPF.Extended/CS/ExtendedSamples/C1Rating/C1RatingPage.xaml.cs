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
using C1.WPF;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using C1.WPF.Extended;

namespace ExtendedSamples
{
    /// <summary>
    /// Interaction logic for DemoRating.xaml
    /// </summary>
    public partial class DemoRating : UserControl
    {
 
        List<double> ratedList = new List<double>();
        public DemoRating()
        {
            InitializeComponent();
            ratedList.Add(2.0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ratedList.Add(rating.Value);
            int count = ratedList.Count;
            double total = 0.0;
            foreach (var item in ratedList)
            {
                total += item;
            }
            var ratedstring = String.Format("{0:N1} ", total / count);
            displayRatingText.Text = ratedstring;
            displayrating.Value = double.Parse(ratedstring);
            rating.Value = 0.0;
            commentstb.Text = "";
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            rating.Value = 0.0;
            commentstb.Text = "";
        }
    }
      
}
