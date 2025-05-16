using C1.WPF.Sparkline;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SparklineExplorer
{
    public partial class AppearanceSample : UserControl
    {
        public AppearanceSample()
        {
            InitializeComponent();
            this.Tag = Properties.Resources.AppeareanceDescription;
        }
    }
}
