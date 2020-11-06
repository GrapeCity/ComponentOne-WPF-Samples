using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using C1.WPF;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoComboBox.xaml
    /// </summary>
    public partial class DemoComboBox : UserControl
    {
        public DemoComboBox()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            comboBox.Items.Add("Alabama");
            comboBox.Items.Add("Alaska");
            comboBox.Items.Add("American Samoa");
            comboBox.Items.Add("Arizona");
            comboBox.Items.Add("Arkansas");
            comboBox.Items.Add("California");
            comboBox.Items.Add("Colorado");
            comboBox.Items.Add("Connecticut");
            comboBox.Items.Add("Delaware");
            comboBox.Items.Add("District of Columbia");
            comboBox.Items.Add("Florida");
            comboBox.Items.Add("Georgia");
            comboBox.Items.Add("Guam");
            comboBox.Items.Add("Hawaii");
            comboBox.Items.Add("Idaho");
            comboBox.Items.Add("Illinois");
            comboBox.Items.Add("Indiana");
            comboBox.Items.Add("Iowa");
            comboBox.Items.Add("Kansas");
            comboBox.Items.Add("Kentucky");
            comboBox.Items.Add("Louisiana");
            comboBox.Items.Add("Maine");
            comboBox.Items.Add("Maryland");
            comboBox.Items.Add("Massachusetts");
            comboBox.Items.Add("Michigan");
            comboBox.Items.Add("Minnesota");
            comboBox.Items.Add("Mississippi");
            comboBox.Items.Add("Missouri");
            comboBox.Items.Add("Montana");
            comboBox.Items.Add("Nebraska");
            comboBox.Items.Add("Nevada");
            comboBox.Items.Add("New Hampshire");
            comboBox.Items.Add("New Jersey");
            comboBox.Items.Add("New Mexico");
            comboBox.Items.Add("New York");
            comboBox.Items.Add("North Carolina");
            comboBox.Items.Add("North Dakota");
            comboBox.Items.Add("Northern Marianas Islands");
            comboBox.Items.Add("Ohio");
            comboBox.Items.Add("Oklahoma");
            comboBox.Items.Add("Oregon");
            comboBox.Items.Add("Pennsylvania");
            comboBox.Items.Add("Puerto Rico");
            comboBox.Items.Add("Rhode Island");
            comboBox.Items.Add("South Carolina");
            comboBox.Items.Add("South Dakota");
            comboBox.Items.Add("Tennessee");
            comboBox.Items.Add("Texas");
            comboBox.Items.Add("Utah");
            comboBox.Items.Add("Vermont");
            comboBox.Items.Add("Virginia");
            comboBox.Items.Add("Virgin Islands");
            comboBox.Items.Add("Washington");
            comboBox.Items.Add("West Virginia");
            comboBox.Items.Add("Wisconsin");
            comboBox.Items.Add("Wyoming");

        }

        #region Object model

        public C1.WPF.Condition Condition
        {
            get
            {
                return comboBox.Condition;
            }
            set
            {
                comboBox.Condition = value;
            }
        }

        public DropDownDirection DropDownDirection
        {
            get
            {
                return comboBox.DropDownDirection;
            }
            set
            {
                comboBox.DropDownDirection = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return comboBox.IsEnabled;
            }
            set
            {
                comboBox.IsEnabled = value;
            }
        }

        public bool IsEditable
        {
            get
            {
                return comboBox.IsEditable;
            }
            set
            {
                comboBox.IsEditable = value;
            }
        }

        public bool AutoComplete
        {
            get
            {
                return comboBox.AutoComplete;
            }
            set
            {
                comboBox.AutoComplete = value;
            }
        }

        public double DropDownWidth
        {
            get
            {
                return comboBox.DropDownWidth;
            }
            set
            {
                comboBox.DropDownWidth = value;
            }
        }

        public double DropDownHeight
        {
            get
            {
                return comboBox.DropDownHeight;
            }
            set
            {
                comboBox.DropDownHeight = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return comboBox.CornerRadius;
            }
            set
            {
                comboBox.CornerRadius = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return comboBox.Background;
            }
            set
            {
                comboBox.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return comboBox.Foreground;
            }
            set
            {
                comboBox.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return comboBox.BorderBrush;
            }
            set
            {
                comboBox.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return comboBox.MouseOverBrush;
            }
            set
            {
                comboBox.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return comboBox.PressedBrush;
            }
            set
            {
                comboBox.PressedBrush = value;
            }
        }

        public Brush SelectedBackground
        {
            get
            {
                return comboBox.SelectedBackground;
            }
            set
            {
                comboBox.SelectedBackground = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return comboBox.FocusBrush;
            }
            set
            {
                comboBox.FocusBrush = value;
            }
        }

        public Brush CaretBrush
        {
            get
            {
                return comboBox.CaretBrush;
            }
            set
            {
                comboBox.CaretBrush = value;
            }
        }

   /*     public Brush SelectionBackground // these props are not available in WPF version
        {
            get
            {
                return comboBox.SelectionBackground;
            }
            set
            {
                comboBox.SelectionBackground = value;
            }
        }

        public Brush SelectionForeground
        {
            get
            {
                return comboBox.SelectionForeground;
            }
            set
            {
                comboBox.SelectionForeground = value;
            }
        }*/

        public Brush ButtonBackground
        {
            get
            {
                return comboBox.ButtonBackground;
            }
            set
            {
                comboBox.ButtonBackground = value;
            }
        }

        public Brush ButtonForeground
        {
            get
            {
                return comboBox.ButtonForeground;
            }
            set
            {
                comboBox.ButtonForeground = value;
            }
        }

        #endregion

        #endregion
    }
}
