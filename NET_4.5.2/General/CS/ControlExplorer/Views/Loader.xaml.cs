using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ControlExplorer
{
    public partial class Loader : UserControl
    {
        public Loader()
        {
            InitializeComponent();
            this.DataContext = ControlExplorer.Properties.Resources.Loader_DataContextText;
            Loaded += new RoutedEventHandler(Loader_Loaded);
        }

        void Loader_Loaded(object sender, RoutedEventArgs e)
        {
            (Resources["Spinner"] as Storyboard).Begin();
        }
    }
}