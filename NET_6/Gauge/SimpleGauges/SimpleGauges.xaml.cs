using C1.WPF.Gauge;
using C1.WPF.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleGauges
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SimpleGaugesDemo : UserControl
    {
        public SimpleGaugesDemo()
        {
            InitializeComponent();
            this.DataContext = new SampleViewModel() { Value = 25, TextVisibility = GaugeTextVisibility.All };
            Tag = Properties.Resources.Tag;
        }
    }
}
