using C1.WPF.Core;
using C1.WPF.Input;
using C1.WPF.PropertyGrid;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PropertyGridExplorer
{
    public class ThicknessEditor : BaseEditor<Thickness, ThicknessEditorControl>
    {
        public override ThicknessEditorControl Create(C1PropertyGrid parent)
        {
            var editor = new ThicknessEditorControl();
            ApplyEditorStyleProperties(parent, editor.LeftBox);
            ApplyEditorStyleProperties(parent, editor.TopBox);
            ApplyEditorStyleProperties(parent, editor.RightBox);
            ApplyEditorStyleProperties(parent, editor.BottomBox);
            return editor;
        }

        public override void Attach(ThicknessEditorControl thicknessEditor, PropertyGroup group, Action<ThicknessEditorControl, object> valueChanged)
        {
            thicknessEditor.Value = group.GetValue<Thickness>();
            EventHandler handler = (s, e) =>
            {
                valueChanged?.Invoke(thicknessEditor, thicknessEditor.Value);
            };
            thicknessEditor.ValueChanged += handler;
            thicknessEditor.Tag = handler;
        }

        public override void Detach(ThicknessEditorControl thicknessEditor)
        {
            var handler = thicknessEditor.Tag as EventHandler;
            thicknessEditor.ValueChanged -= handler;
        }
    }

    public partial class ThicknessEditorControl : UserControl
    {
        private bool _updating;

        public ThicknessEditorControl()
        {
            InitializeComponent();
        }

        public Thickness Value
        {
            get { return (Thickness)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Thickness", typeof(Thickness), typeof(ThicknessEditorControl), new PropertyMetadata((a, e) => { (a as ThicknessEditorControl).OnThicknessChanged(); }));

        public event EventHandler ValueChanged;

        private void OnThicknessChanged()
        {
            try
            {
                _updating = true;
                LeftBox.Value = Value.Left;
                TopBox.Value = Value.Top;
                RightBox.Value = Value.Right;
                BottomBox.Value = Value.Bottom;
                ValueChanged?.Invoke(this, new EventArgs());
            }
            finally
            {
                _updating = false;
            }
        }

        private void OnValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (!_updating)
                Value = new Thickness(LeftBox.Value, TopBox.Value, RightBox.Value, BottomBox.Value);
        }
    }
}
