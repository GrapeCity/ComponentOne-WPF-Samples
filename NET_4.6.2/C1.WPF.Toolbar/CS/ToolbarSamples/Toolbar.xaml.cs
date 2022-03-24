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

namespace ToolbarSamples
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : UserControl
    {
        public Toolbar()
        {
            InitializeComponent();
        }

        #region Object model

        public bool IsEnabled
        {
            get
            {
                return tb.IsEnabled;
            }
            set
            {
                tb.IsEnabled = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return tb.Background;
            }
            set
            {
                tb.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return tb.Foreground;
            }
            set
            {
                tb.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return tb.BorderBrush;
            }
            set
            {
                tb.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return tb.MouseOverBrush;
            }
            set
            {
                tb.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return tb.PressedBrush;
            }
            set
            {
                tb.PressedBrush = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return tb.FocusBrush;
            }
            set
            {
                tb.FocusBrush = value;
            }
        }

        #endregion

        #endregion
    }
}
