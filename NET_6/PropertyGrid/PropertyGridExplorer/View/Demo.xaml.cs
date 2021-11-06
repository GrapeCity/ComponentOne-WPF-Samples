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
    /// Interaction logic for Demo.xaml
    /// </summary>
    public partial class Demo : UserControl
    {
        public Demo()
        {
            InitializeComponent();
            Tag = Properties.Resources.PropertyGridDemoDesc;

            propertyGrid.PropertyBoxesLoaded += _propertyGrid_PropertyBoxesLoaded;
            this.targetButton.Click += new RoutedEventHandler(targetButton_Click);
            (this.stackControl1.Children[0] as CheckBox).IsChecked = true;
            this.UpdateCheckContent();
        }

        void targetButton_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            this.targetButton.Content = "content changed";
        }

        private void CheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (e.OriginalSource != sender) return;
            this.UpdateCheckContent();
        }

        private void UpdateCheckContent()
        {
            CheckBox[] elements = this.stackControl1.Children.Cast<CheckBox>().Where(x => x.IsChecked.Value).ToArray();
            List<object> listelement = new List<object>();
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].IsChecked.Value)
                {
                    listelement.Add(elements[i].Content);
                }
            }
            this.propertyGrid.SelectedObject = null;
            if (listelement.Count > 0)
            {
                for (int i = 0; i < listelement.Count; i++)
                {
                    this.propertyGrid.SelectedObjects.Add(listelement[i]);
                }
            }
        }

        private void _propertyGrid_PropertyBoxesLoaded(object sender, EventArgs e)
        {
        }

        private void _propertyGrid_PropertyBoxAdded(object sender, PropertyBoxChangedEventArgs e)
        {
            if (e.PropertyBox.PropertyAttribute.MemberName == "Opacity")
            {
                (e.PropertyBox.CurrentEditor as C1NumericBox).Value = 0;
            }
        }
    }    
}
