using C1.WPF.Core;
using C1.WPF.PropertyGrid;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PropertyGridExplorer
{
    /// <summary>
    /// Interaction logic for ThicknessEditor.xaml
    /// </summary>
    public partial class ThicknessEditor : UserControl, ITypeEditorControl
    {
        private PropertyAttribute _property;
        private bool _updating;

        public ThicknessEditor()
        {
            InitializeComponent();
        }

        public Thickness Thickness
        {
            get { return (Thickness)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        public static readonly DependencyProperty ThicknessProperty = DependencyProperty.Register("Thickness", typeof(Thickness), typeof(ThicknessEditor), new PropertyMetadata((a, e) => { (a as ThicknessEditor).OnThicknessChanged(); }));

        public event PropertyChangedEventHandler ValueChanged;

        public void Attach(PropertyAttribute property)
        {
            _property = property;
            SetBinding(ThicknessEditor.ThicknessProperty, new Binding(property.PropertyInfo.Name)
            {
                Mode = BindingMode.TwoWay,
                Source = _property.SelectedObject,
            });
        }

        public ITypeEditorControl Create()
        {
            return new ThicknessEditor();
        }

        public void Detach(PropertyAttribute property)
        {
        }

        public bool Supports(PropertyAttribute property)
        {
            return property.PropertyInfo.PropertyType == typeof(Thickness);
        }

        private void OnThicknessChanged()
        {
            try
            {
                _updating = true;
                LeftBox.Value = Thickness.Left;
                TopBox.Value = Thickness.Top;
                RightBox.Value = Thickness.Right;
                BottomBox.Value = Thickness.Bottom;
            }
            finally
            {
                _updating = false;
            }
        }

        private void OnValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (!_updating)
                Thickness = new Thickness(LeftBox.Value, TopBox.Value, RightBox.Value, BottomBox.Value);
        }
    }
}
