using System.Windows.Controls;
using C1.WPF;
using System.Windows;
using System.Windows.Media;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoMaskedTextBox.xaml
    /// </summary>
    public partial class DemoMaskedTextBox : UserControl
    {
        public DemoMaskedTextBox()
        {
            InitializeComponent();
        }

        private void SetTextAndPos(TextBox tb, string text)
        {
            int pos = tb.SelectionStart;
            tb.Text = text;
            tb.Select(pos, 0);
        }

        #region Object model

        public bool IsEnabled
        {
            get
            {
                return maskedTextBox.IsEnabled;
            }
            set
            {
                maskedTextBox.IsEnabled = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return maskedTextBox.IsReadOnly;
            }
            set
            {
                maskedTextBox.IsReadOnly = value;
            }
        }

        public string Text
        {
            get
            {
                return maskedTextBox.Text;
            }
            set
            {
                maskedTextBox.Text = value;
            }
        }

        public string Mask
        {
            get
            {
                return maskedTextBox.Mask;
            }
            set
            {
                maskedTextBox.Mask = value;
            }
        }

        public string Value
        {
            get
            {
                return maskedTextBox.Value;
            }
            set
            {
                maskedTextBox.Value = value;
            }
        }

        public char PromptChar
        {
            get
            {
                return maskedTextBox.PromptChar;
            }
            set
            {
                maskedTextBox.PromptChar = value;
            }
        }

        public MaskFormat TextMaskFormat
        {
            get
            {
                return maskedTextBox.TextMaskFormat;
            }
            set
            {
                maskedTextBox.TextMaskFormat = value;
            }
        }

        public bool ReplaceMode
        {
            get
            {
                return maskedTextBox.ReplaceMode;
            }
            set
            {
                maskedTextBox.ReplaceMode = value;
            }
        }

        public TextAlignment TextAlignment
        {
            get
            {
                return maskedTextBox.TextAlignment;
            }
            set
            {
                maskedTextBox.TextAlignment = value;
            }
        }

        public object Watermark
        {
            get
            {
                return maskedTextBox.Watermark;
            }
            set
            {
                maskedTextBox.Watermark = value;
            }
        }
        
        public CornerRadius CornerRadius
        {
            get
            {
                return maskedTextBox.CornerRadius;
            }
            set
            {
                maskedTextBox.CornerRadius = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return maskedTextBox.Background;
            }
            set
            {
                maskedTextBox.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return maskedTextBox.Foreground;
            }
            set
            {
                maskedTextBox.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return maskedTextBox.BorderBrush;
            }
            set
            {
                maskedTextBox.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return maskedTextBox.MouseOverBrush;
            }
            set
            {
                maskedTextBox.MouseOverBrush = value;
            }
        }

        public Brush FocusBrush
        {
            get
            {
                return maskedTextBox.FocusBrush;
            }
            set
            {
                maskedTextBox.FocusBrush = value;
            }
        }

        public Brush SelectionBackground
        {
            get
            {
                return maskedTextBox.SelectionBackground;
            }
            set
            {
                maskedTextBox.SelectionBackground = value;
            }
        }

        public Brush SelectionForeground
        {
            get
            {
                return maskedTextBox.SelectionForeground;
            }
            set
            {
                maskedTextBox.SelectionForeground = value;
            }
        }

        #endregion

        #endregion
    }
}
