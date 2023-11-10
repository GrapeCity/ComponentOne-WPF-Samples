using C1.WPF.ColorPicker;
using C1.WPF.Core;
using C1.WPF.PropertyGrid;
using PropertyGridExplorer.Resources;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace PropertyGridExplorer
{
    /// <summary>
    /// Interaction logic for AutoProperties.xaml
    /// </summary>
    public partial class CustomEditors : UserControl
    {
        public CustomEditors()
        {
            InitializeComponent();
            Tag = AppResources.CustomEditorsDesc;

            //propertyGrid.AvailableEditors.Add(new CustomColorEditor());

            //propertyGrid.AutoGenerateProperties = false;
            //propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            //{
            //    MemberName = nameof(TextBox.Background),
            //    Editor = new CustomColorEditor()
            //});
            //propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            //{
            //    MemberName = nameof(TextBox.Foreground),
            //    Editor = new CustomColorEditor()
            //});
            //propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            //{
            //    MemberName = nameof(TextBox.BorderBrush),
            //    Editor = new CustomColorEditor()
            //});
            //propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            //{
            //    MemberName = nameof(TextBox.BorderThickness),
            //    Editor = new ThicknessEditor()
            //});
            //propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            //{
            //    MemberName = nameof(TextBox.Opacity),
            //    Editor = new RangeSliderEditor() { Minimum = 0, Maximum = 1 }
            //});
            //propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            //{
            //    MemberName = nameof(TextBox.Width),
            //    Editor = new RangeSliderEditor() { Minimum = 0, Maximum = 500 }
            //});
            //propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            //{
            //    MemberName = nameof(TextBox.Height),
            //    Editor = new RangeSliderEditor() { Minimum = 0, Maximum = 100 }
            //});
        }
    }


    public class CustomColorEditor : BaseAdvancedEditor<Brush, C1SpectrumColorPicker>
    {
        public override C1SpectrumColorPicker CreatePopupEditor(C1PropertyGrid parent)
        {
            return new C1SpectrumColorPicker() { MinHeight = 200, MinWidth = 250 };
        }
        
        public override void AttachPopupEditor(C1SpectrumColorPicker editor, PropertyGroup group, Action<C1SpectrumColorPicker, object> valueChanged)
        {
            editor.Color = (group.GetValue<Brush>() as SolidColorBrush).Color;
            EventHandler<PropertyChangedEventArgs<Color>> handler = (s, e) =>
            {
                valueChanged?.Invoke(editor, new SolidColorBrush(editor.Color));
            };
            editor.ColorChanged += handler;
            editor.Tag = handler;
        }

        public override void DetachPopupEditor(C1SpectrumColorPicker editor)
        {
            var handler = editor.Tag as EventHandler<PropertyChangedEventArgs<Color>>;
            editor.ColorChanged -= handler;
        }
    }

    public class RangeSliderEditorFactory : BaseEditor<double, Slider>
    {
        public double Minimum { get; set; } = 0;
        public double Maximum { get; set; } = 100;

        public override Slider Create(C1PropertyGrid parent)
        {
            return new Slider() { Minimum = Minimum, Maximum = Maximum, VerticalAlignment = VerticalAlignment.Center };
        }

        public override void Attach(Slider slider, PropertyGroup group, Action<Slider, object> valueChanged)
        {
            var value = group.GetValue<double>();
            slider.Value = double.IsFinite(value) ? value : Minimum;
            RoutedPropertyChangedEventHandler<double> handler = (s, e) =>
            {
                valueChanged?.Invoke(slider, slider.Value);
            };
            slider.ValueChanged += handler;
            slider.Tag = handler;
        }

        public override void Detach(Slider slider)
        {
            var handler = slider.Tag as RoutedPropertyChangedEventHandler<double>;
            slider.ValueChanged -= handler;
        }
    }
}
