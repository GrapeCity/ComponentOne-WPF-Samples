using C1.WPF.Core;
using C1.WPF.PropertyGrid;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PropertyGridExplorer
{
    public class CornerRadiusEditor : BaseEditor<CornerRadius, CornerRadiusEditorControl>
    {
        public override CornerRadiusEditorControl Create(C1PropertyGrid parent)
        {
            var editor = new CornerRadiusEditorControl();
            ApplyEditorStyleProperties(parent, editor.TopLeftBox);
            ApplyEditorStyleProperties(parent, editor.TopRightBox);
            ApplyEditorStyleProperties(parent, editor.BottomRightBox);
            ApplyEditorStyleProperties(parent, editor.BottomLeftBox);
            return editor;
        }

        public override void Attach(CornerRadiusEditorControl cornerRadiusEditor, PropertyGroup group, Action<CornerRadiusEditorControl, object> valueChanged)
        {
            cornerRadiusEditor.Value = group.GetValue<CornerRadius>();
            EventHandler handler = (s, e) =>
            {
                valueChanged?.Invoke(cornerRadiusEditor, cornerRadiusEditor.Value);
            };
            cornerRadiusEditor.ValueChanged += handler;
            cornerRadiusEditor.Tag = handler;
        }

        public override void Detach(CornerRadiusEditorControl cornerRadiusEditor)
        {
            var handler = cornerRadiusEditor.Tag as EventHandler;
            cornerRadiusEditor.ValueChanged -= handler;
        }

    }

    public partial class CornerRadiusEditorControl : UserControl
    {
        private bool _updating;

        public CornerRadiusEditorControl()
        {
            InitializeComponent();
        }

        public CornerRadius Value
        {
            get { return (CornerRadius)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(nameof(Value), typeof(CornerRadius), typeof(CornerRadiusEditorControl), new PropertyMetadata((a, e) => { (a as CornerRadiusEditorControl).OnCornerRadiusChanged(); }));

        public event EventHandler ValueChanged;

        private void OnCornerRadiusChanged()
        {
            try
            {
                _updating = true;
                TopLeftBox.Value = Value.TopLeft;
                TopRightBox.Value = Value.TopRight;
                BottomRightBox.Value = Value.BottomRight;
                BottomLeftBox.Value = Value.BottomLeft;
                ValueChanged?.Invoke(this, EventArgs.Empty);
            }
            finally
            {
                _updating = false;
            }
        }

        private void OnValueChanged(object sender, PropertyChangedEventArgs<double> e)
        {
            if (!_updating)
                Value = new CornerRadius(TopLeftBox.Value, TopRightBox.Value, BottomRightBox.Value, BottomLeftBox.Value);
        }
    }
}
