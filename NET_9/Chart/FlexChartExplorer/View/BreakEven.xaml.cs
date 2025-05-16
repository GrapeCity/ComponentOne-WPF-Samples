using FlexChartExplorer.Resources;
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

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for Waterfall.xaml
    /// </summary>
    public partial class BreakEven : UserControl
    {
        public BreakEven()
        {
            InitializeComponent();
            Tag = AppResources.BreakEvenTag;
        }
    }
}
