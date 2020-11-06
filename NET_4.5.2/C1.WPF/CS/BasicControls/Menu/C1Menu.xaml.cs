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
using C1.WPF;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoMenu.xaml
    /// </summary>
    public partial class DemoMenu : UserControl
    {
        public DemoMenu()
        {
            InitializeComponent();
        }
        public event EventHandler SelectedItemChanged;

        #region ** ClearStyle

        public Brush DEMO_Background
        {
            get
            {
                return VisualStudioMenu.Background;
            }
            set
            {
                VisualStudioMenu.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return VisualStudioMenu.Foreground;
            }
            set
            {
                VisualStudioMenu.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return VisualStudioMenu.BorderBrush;
            }
            set
            {
                VisualStudioMenu.BorderBrush = value;
            }
        }

        public Brush HighlightedBackground
        {
            get
            {
                return VisualStudioMenu.HighlightedBackground;
            }
            set
            {
                VisualStudioMenu.HighlightedBackground = value;
            }
        }

        public Brush OpenedBackground
        {
            get
            {
                return VisualStudioMenu.OpenedBackground;
            }
            set
            {
                VisualStudioMenu.OpenedBackground = value;
            }
        }

        #endregion

        private void C1MenuItem_Click(object sender, SourcedEventArgs e)
        {
            C1MenuItem menu = sender as C1MenuItem;
            if (SelectedItemChanged != null)
            {
                SelectedItemChanged(this, new EventArgs());
            }
        }
    }
}
