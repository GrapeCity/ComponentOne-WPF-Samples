using System;
using System.Windows;
using System.ComponentModel;

namespace SimpleGauges
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            this.Title = SimpleGauges.Properties.Resources.Title;
        }
    }
}
