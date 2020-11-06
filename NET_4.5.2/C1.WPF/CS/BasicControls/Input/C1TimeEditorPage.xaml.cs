using System.Windows.Controls;
using C1.WPF.DateTimeEditors;
using System;
using System.Windows;

namespace BasicControls
{
    /// <summary>
    /// Interaction logic for DemoTimeEditor.xaml
    /// </summary>
    public partial class DemoTimeEditor : UserControl
    {
        public DemoTimeEditor()
        {
            InitializeComponent();
        }

        #region Object model

        public bool AllowNull
        {
            get
            {
                return timeEditor.AllowNull;
            }
            set
            {
                timeEditor.AllowNull = value;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return timeEditor.IsEnabled;
            }
            set
            {
                timeEditor.IsEnabled = value;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return timeEditor.IsReadOnly;
            }
            set
            {
                timeEditor.IsReadOnly = value;
            }
        }

        public C1TimeEditorFormat Format
        {
            get
            {
                return timeEditor.Format;
            }
            set
            {
                timeEditor.Format = value;
            }
        }

        public TimeSpan Increment
        {
            get
            {
                return timeEditor.Increment;
            }
            set
            {
                timeEditor.Increment = value;
            }
        }

        public TimeSpan Minimum
        {
            get
            {
                return timeEditor.Minimum;
            }
            set
            {
                timeEditor.Minimum = value;
            }
        }

        public TimeSpan Maximum
        {
            get
            {
                return timeEditor.Maximum;
            }
            set
            {
                timeEditor.Maximum = value;
            }
        }

        public CornerRadius CornerRadius
        {
            get
            {
                return timeEditor.CornerRadius;
            }
            set
            {
                timeEditor.CornerRadius = value;
            }
        }

        public bool ShowButtons
        {
            get
            {
                return timeEditor.ShowButtons;
            }
            set
            {
                timeEditor.ShowButtons = value;
            }
        }

        public int Delay
        {
            get
            {
                return timeEditor.Delay;
            }
            set
            {
                timeEditor.Delay = value;
            }
        }

        public int Interval
        {
            get
            {
                return timeEditor.Interval;
            }
            set
            {
                timeEditor.Interval = value;
            }
        }
        #endregion

    }
}
