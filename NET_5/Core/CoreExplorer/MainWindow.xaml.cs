﻿using System.Windows;

namespace CoreExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Title = CoreExplorer.Resources.AppResources.Title;
        }
    }
}
