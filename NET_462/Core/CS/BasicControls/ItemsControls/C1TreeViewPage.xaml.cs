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

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoTreeView.xaml
    /// </summary>
    public partial class DemoTreeView : UserControl
    {
        public DemoTreeView()
        {
            InitializeComponent();

            WindowExplorerTreeView.ShowLines = true;
            WindowExplorerTreeView.LineThickness = 1.0;
        }

        #region Object model

        public bool IsEnabled
        {
            get
            {
                return WindowExplorerTreeView.IsEnabled;
            }
            set
            {
                WindowExplorerTreeView.IsEnabled = value;
            }
        }


        public bool AllowDragDrop
        {
            get
            {
                return WindowExplorerTreeView.AllowDragDrop;
            }
            set
            {
                WindowExplorerTreeView.AllowDragDrop = value;
            }
        }

        public bool ShowLines
        {
            get 
            { 
                return WindowExplorerTreeView.ShowLines; 
            }
            set 
            { 
                WindowExplorerTreeView.ShowLines = value; 
            }
        }

        public double LineThickness
        {
            get 
            { 
                return WindowExplorerTreeView.LineThickness; 
            }
            set 
            { 
                WindowExplorerTreeView.LineThickness = value; 
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return WindowExplorerTreeView.Background;
            }
            set
            {
                WindowExplorerTreeView.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return WindowExplorerTreeView.Foreground;
            }
            set
            {
                WindowExplorerTreeView.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return WindowExplorerTreeView.BorderBrush;
            }
            set
            {
                WindowExplorerTreeView.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return WindowExplorerTreeView.MouseOverBrush;
            }
            set
            {
                WindowExplorerTreeView.MouseOverBrush = value;
            }
        }

        public Brush SelectedBackground
        {
            get
            {
                return WindowExplorerTreeView.SelectedBackground;
            }
            set
            {
                WindowExplorerTreeView.SelectedBackground = value;
            }
        }

        #endregion

        #endregion

    }
}
