using C1.WPF.Core;
using C1.WPF.Accordion;
using C1.WPF.Input;
using C1.WPF.PropertyGrid;
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
using System.ComponentModel;

namespace PropertyGridExplorer
{
    /// <summary>
    /// Interaction logic for AutoProperties.xaml
    /// </summary>
    public partial class AutoProperties : UserControl
    {
        public AutoProperties()
        {
            InitializeComponent();
            Tag = Properties.Resources.PropertyGridAutoPropsDesc;
        }

        private void CheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var chkBox = sender as CheckBox;
            if (chkBox.IsChecked == true)
            {
                propertyGrid.SelectedObject = txtbox;
            }
            else
            {
                propertyGrid.SelectedObject = null;
            }    
        }        
    }
}
