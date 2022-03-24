using C1.WPF.ColorPicker;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyGridExplorer
{
    public partial class CustomColorForm : UserControl
    {
        public CustomColorForm()
        {
            InitializeComponent();

            spectrum.SetBinding(C1SpectrumColorPicker.ColorProperty, new Binding 
            { 
                Source = this, 
                Path = new PropertyPath(nameof(Color)), 
                Mode = BindingMode.TwoWay,
            });

            btnOK.Click += (s, e) => OKClicked?.Invoke(this, e);
            btnCancel.Click += (s, e) => CancelClicked?.Invoke(this, e);
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(CustomColorForm), new PropertyMetadata(Colors.Transparent));

        public event EventHandler OKClicked;
        public event EventHandler CancelClicked;
    }
}
