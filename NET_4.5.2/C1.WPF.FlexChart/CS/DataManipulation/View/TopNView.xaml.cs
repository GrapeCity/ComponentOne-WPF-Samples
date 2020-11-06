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

namespace DataManipulation
{
    /// <summary>
    /// Interaction logic for TopNView.xaml
    /// </summary>
    public partial class TopNView : UserControl
    {
        public TopNView()
        {
            InitializeComponent();
        }

        private void flexChart1_SeriesVisibilityChanged(object sender, C1.WPF.Chart.SeriesEventArgs e)
        {
            Queue<string> bindings = new Queue<string>();
            foreach (var s in flexChart1.Series)
            {
                if (s.Visibility == C1.Chart.SeriesVisibility.Visible)
                {
                    bindings.Enqueue(s.Binding);
                }
            }
            topNViewModel.Bindings = bindings.ToArray();
        }
    }
}
