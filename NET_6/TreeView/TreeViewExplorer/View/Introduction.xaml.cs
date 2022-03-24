using System.Windows.Controls;
using System.Windows.Media;

namespace TreeViewExplorer
{
    /// <summary>
    /// Interaction logic for DemoTreeView.xaml
    /// </summary>
    public partial class DemoTreeView : UserControl
    {
        public DemoTreeView()
        {
            InitializeComponent();
            Tag = Properties.Resources.Introduction;
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

        private void cbShowCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (WindowExplorerTreeView == null) return;
            WindowExplorerTreeView.ShowCheckboxes = !WindowExplorerTreeView.ShowCheckboxes;
        }

        private void cbSelectionMode_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            if (WindowExplorerTreeView == null) return;
            if (e.NewValue == 0)
                WindowExplorerTreeView.SelectionMode = C1.WPF.Core.C1SelectionMode.Single;

            if (e.NewValue == 1)
                WindowExplorerTreeView.SelectionMode = C1.WPF.Core.C1SelectionMode.Extended;

            if (e.NewValue == 2)
                WindowExplorerTreeView.SelectionMode = C1.WPF.Core.C1SelectionMode.Multiple;
        }
    }
}
