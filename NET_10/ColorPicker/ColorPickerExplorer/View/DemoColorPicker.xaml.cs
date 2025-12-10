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
using C1.WPF.ColorPicker;
using ColorPickerExplorer.Resources;

namespace ColorPickerExplorer
{
    /// <summary>
    /// Interaction logic for DemoColorPicker.xaml
    /// </summary>
    public partial class DemoColorPicker : UserControl
    {
        public DemoColorPicker()
        {
            InitializeComponent();
            Tag = AppResources.Tag;
        }


        #region Object model

        public ColorPalette Palette
        {
            get
            {
                return colorPicker.Palette;
            }
            set
            {
                colorPicker.Palette = value;
            }
        }

        public C1ColorPickerMode Mode
        {
            get
            {
                return colorPicker.Mode;
            }
            set
            {
                colorPicker.Mode = value;
            }
        }

        public bool ShowTransparentColor
        {
            get
            {
                return colorPicker.ShowTransparentColor;
            }
            set
            {
                colorPicker.ShowTransparentColor = value;
            }
        }

        public bool ShowRecentColors
        {
            get
            {
                return colorPicker.ShowRecentColors;
            }
            set
            {
                colorPicker.ShowRecentColors = value;
            }
        }

        public bool ShowAlphaChannel
        {
            get
            {
                return colorPicker.ShowAlphaChannel;
            }
            set
            {
                colorPicker.ShowAlphaChannel = value;
            }
        }

        #endregion

        #region ** ClearStyle

        public Brush DEMO_Background
        {
            get
            {
                return colorPicker.Background;
            }
            set
            {
                colorPicker.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return colorPicker.Foreground;
            }
            set
            {
                colorPicker.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return colorPicker.BorderBrush;
            }
            set
            {
                colorPicker.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return colorPicker.MouseOverBrush;
            }
            set
            {
                colorPicker.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return colorPicker.PressedBrush;
            }
            set
            {
                colorPicker.PressedBrush = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return colorPicker.FocusBrush;
            }
            set
            {
                colorPicker.FocusBrush = value;
            }
        }

        public Brush InputBackground
        {
            get
            {
                return colorPicker.InputBackground;
            }
            set
            {
                colorPicker.InputBackground = value;
            }
        }

        public Brush InputForeground
        {
            get
            {
                return colorPicker.InputForeground;
            }
            set
            {
                colorPicker.InputForeground = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return colorPicker.CornerRadius;
            }
            set
            {
                colorPicker.CornerRadius = value;
            }
        }
        #endregion
    }
}
