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
    /// Interaction logic for DemoAccordion.xaml
    /// </summary>
    public partial class DemoAccordion : UserControl
    {
        public DemoAccordion()
        {
            InitializeComponent();
        }

        #region Object Model

        public C1ExpandDirection ExpandDirection
        {
            get
            {
                return accordion.ExpandDirection;
            }
            set
            {
                accordion.ExpandDirection = value;
            }
        }

        public bool AllowCollapseAll
        {
            get
            {
                return accordion.AllowCollapseAll;
            }
            set
            {
                accordion.AllowCollapseAll = value;
            }
        }

        public bool Fill
        {
            get
            {
                return accordion.Fill;
            }
            set
            {
                accordion.Fill = value;
            }
        }

        public Thickness DEMO_Padding
        {
            get
            {
                return accordion.Padding;
            }
            set
            {
                accordion.Padding = value;
            }
        }

        public Thickness HeaderPadding
        {
            get
            {
                return accordion.HeaderPadding;
            }
            set
            {
                accordion.HeaderPadding = value;
            }
        }
        #endregion

        #region ** ClearStyle

        public Brush DEMO_Background
        {
            get
            {
                return accordion.Background;
            }
            set
            {
                accordion.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return accordion.Foreground;
            }
            set
            {
                accordion.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return accordion.BorderBrush;
            }
            set
            {
                accordion.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return accordion.MouseOverBrush;
            }
            set
            {
                accordion.MouseOverBrush = value;
            }
        }

        public Brush ExpandedBackground
        {
            get
            {
                return accordion.ExpandedBackground;
            }
            set
            {
                accordion.ExpandedBackground = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return accordion.FocusBrush;
            }
            set
            {
                accordion.FocusBrush = value;
            }
        }

        public Brush HeaderBackground
        {
            get
            {
                return accordion.HeaderBackground;
            }
            set
            {
                accordion.HeaderBackground = value;
            }
        }

        public Brush HeaderForeground
        {
            get
            {
                return accordion.HeaderForeground;
            }
            set
            {
                accordion.HeaderForeground = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return accordion.CornerRadius;
            }
            set
            {
                accordion.CornerRadius = value;
            }
        }

        #endregion
    }
}
