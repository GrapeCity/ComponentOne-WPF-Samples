using C1.WPF;
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

namespace ExtendedSamples
{
    /// <summary>
    /// Interaction logic for DemoExpander.xaml
    /// </summary>
    public partial class DemoExpander : UserControl
    {
        public DemoExpander()
        {
            InitializeComponent();
        }

        #region ** Object Model

        public C1ExpandDirection ExpandDirection
        {
            get
            {
                return expander.ExpandDirection;
            }
            set
            {
                expander.ExpandDirection = value;
            }
        }

        public bool IsExpanded
        {
            get
            {
                return expander.IsExpanded;
            }
            set
            {
                expander.IsExpanded = value;
            }
        }

        public bool IsExpandable
        {
            get
            {
                return expander.IsExpandable;
            }
            set
            {
                expander.IsExpandable = value;
            }
        }

        #endregion

        #region ** ClearStyle

        public Brush DEMO_Background
        {
            get
            {
                return expander.Background;
            }
            set
            {
                expander.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return expander.Foreground;
            }
            set
            {
                expander.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return expander.BorderBrush;
            }
            set
            {
                expander.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return expander.MouseOverBrush;
            }
            set
            {
                expander.MouseOverBrush = value;
            }
        }

        public Brush ExpandedBackground
        {
            get
            {
                return expander.ExpandedBackground;
            }
            set
            {
                expander.ExpandedBackground = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return expander.FocusBrush;
            }
            set
            {
                expander.FocusBrush = value;
            }
        }

        public Brush HeaderBackground
        {
            get
            {
                return expander.HeaderBackground;
            }
            set
            {
                expander.HeaderBackground = value;
            }
        }

        public Brush HeaderForeground
        {
            get
            {
                return expander.HeaderForeground;
            }
            set
            {
                expander.HeaderForeground = value;
            }
        }

        #endregion

    }
}
