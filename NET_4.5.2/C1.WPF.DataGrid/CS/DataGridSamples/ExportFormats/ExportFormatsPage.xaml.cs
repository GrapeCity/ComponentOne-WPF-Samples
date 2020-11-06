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
using System.IO;
using System.Xml.Linq;
using Microsoft.Win32;
using System.Reflection;
using System.Xml;
using C1.WPF;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for ExportFormats.xaml
    /// </summary>
    public partial class ExportFormats : UserControl
    {
        public ExportFormats()
        {
            InitializeComponent();

            grid.ItemsSource = Person.Generate(300);
        }

        private void printMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ScaleMode scaleMode = (ScaleMode)Enum.Parse(typeof(ScaleMode), ((MenuItem)sender).Tag.ToString(),
                false);
            grid.Print("C1DataGrid", scaleMode, new Thickness(20), 10);
        }


    }

}
