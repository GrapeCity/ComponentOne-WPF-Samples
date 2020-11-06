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

namespace TutorialsWPF
{
    public partial class ServerSideFiltering : Window
    {
        public ServerSideFiltering()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Update the filter value. It will trigger automatic loading.
            c1DataSource1.ViewSources["Products"].FilterDescriptors[0].Value = ((Category)comboBox1.SelectedItem).CategoryID;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            c1DataSource1.ClientCache.SaveChanges();
        }
    }
}
