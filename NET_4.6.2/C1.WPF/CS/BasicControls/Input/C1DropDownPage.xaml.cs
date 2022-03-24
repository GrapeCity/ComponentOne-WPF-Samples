using System.Windows;
using System.Windows.Controls;
using C1.WPF;

namespace BasicControls
{
	/// <summary>
	/// Interaction logic for DemoDropDown.xaml
	/// </summary>
	public partial class DemoDropDown : UserControl
	{
        public DemoDropDown()
		{
			InitializeComponent();
		}

		private void treeSelection_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			selection.Text = treeSelection.SelectedItem != null ? 
				(string)((TreeViewItem)treeSelection.SelectedItem).Header :
				"« Pick one »";
			dropDown.IsDropDownOpen = false;
		}
        #region Object model

        public DropDownDirection DropDownDirection
        {
            get
            {
                return dropDown.DropDownDirection;
            }
            set
            {
                dropDown.DropDownDirection = value;
            }
        }

        public bool AutoClose
        {
            get
            {
                return dropDown.AutoClose;
            }
            set
            {
                dropDown.AutoClose = value;
            }
        }

        public double DropDownHeight
        {
            get
            {
                return dropDown.DropDownHeight;
            }
            set
            {
                dropDown.DropDownHeight = value;
            }
        }

        public double DropDownWidth
        {
            get
            {
                return dropDown.DropDownWidth;
            }
            set
            {
                dropDown.DropDownWidth = value;
            }
        }

        public bool IsDropDownOpen
        {
            get
            {
                return dropDown.IsDropDownOpen;
            }
            set
            {
                dropDown.IsDropDownOpen = value;
            }
        }
        #endregion
    }
}
