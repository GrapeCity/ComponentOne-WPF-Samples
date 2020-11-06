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

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for DeferredScrollingPage.xaml
    /// </summary>
    public partial class DeferredScrolling : UserControl
    {
        public DeferredScrolling()
        {
            InitializeComponent();

            dataGrid.ItemsSource = Person.Generate(5000);
        }
    }
}
