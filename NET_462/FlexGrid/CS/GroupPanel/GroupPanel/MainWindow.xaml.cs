using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Data;
using C1.WPF.FlexGrid;

namespace GroupPanel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // create a data source
            var list = new List<Product>();
            for (int i = 0; i < 200; i++)
            {
                list.Add(new Product());
            }

            // assign the data source to both grids
            var cvs = new CollectionViewSource() { Source = list };
            _flex.ItemsSource = cvs.View;
            _flexCustomGrouping.ItemsSource = cvs.View;

            // customize groups for the _flexCustomGrouping control
            _groupPanelCustomGrouping.PropertyGroupCreated += _groupPanel_PropertyGroupCreated;
        }

        /// <summary>
        /// Customize group descriptors created by the C1FlexGridGroupPanel.
        /// </summary>
        void _groupPanel_PropertyGroupCreated(object sender, PropertyGroupCreatedEventArgs e)
        {
            var pgd = e.PropertyGroupDescription;
            switch (pgd.PropertyName)
            {
                case "Introduced":
                    pgd.Converter = new DateTimeGroupConverter();
                    break;
                case "Price":
                    pgd.Converter = new AmountGroupConverter(1000);
                    break;
                case "Cost":
                    pgd.Converter = new AmountGroupConverter(300);
                    break;
            }
        }
    }
}
