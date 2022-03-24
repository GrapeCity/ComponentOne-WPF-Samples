using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using C1.WPF;

namespace C1_Demo
{
    public partial class DemoInputs : UserControl
    {
        public DemoInputs()
        {
            InitializeComponent();

            DataContext = new DemoInputsViewModel();
        }

        #region ** hierarchical combo

        private void C1TreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateSelection();
                e.Handled = true;
            }
        }

        private void UpdateSelection()
        {
            if (treeSelection.SelectedItem != null)
            {
                selection.Text = treeSelection.SelectedItem.Header.ToString();
            }
            else
            {
                selection.Text = "";
            }
            PlacesDropDown.IsDropDownOpen = false;
        }

        private void C1TreeView_ItemClicked(object sender, SourcedEventArgs e)
        {
            UpdateSelection();
        }

        #endregion
    }
}
