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

namespace FlexRadarIntro
{
    /// <summary>
    /// Interaction logic for FlexRadarView.xaml
    /// </summary>
    public partial class FlexRadarView : UserControl
    {
        public FlexRadarView()
        {
            InitializeComponent();
            this.Loaded += FlexRadarView_Loaded;
        }

        private void FlexRadarView_Loaded(object sender, RoutedEventArgs e)
        {
            this.lbSamples.SelectedIndex = 0;
        }
    }
}
