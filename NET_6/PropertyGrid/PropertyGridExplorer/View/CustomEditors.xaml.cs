using C1.WPF.ColorPicker;
using C1.WPF.PropertyGrid;
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
            Tag = Properties.Resources.CustomEditorsDesc;

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


    public class CustomColorEditor : BaseEditor<Brush, C1HexColorBox>
    {
        public bool ShowSharpPrefix { get; set; }

        public override C1HexColorBox Create(C1PropertyGrid parent)
        {
            var editor = new C1HexColorBox() { ShowSharpPrefix = ShowSharpPrefix };
            ApplyEditorStyleProperties(parent, editor);
            return editor;
        }

        public override void Attach(C1HexColorBox hexColorBox, PropertyGroup group, Action<C1HexColorBox, object> valueChanged)
        {
            hexColorBox.Color = group.GetValue<SolidColorBrush>()?.Color ?? Colors.Transparent;
            RoutedEventHandler handler = (sender, e) =>
            {

                var window = new C1.WPF.Docking.C1Window();

                var customColorForm = new CustomColorForm();
                var binding = new Binding(nameof(C1HexColorBox.Color))
                {
                    Mode = BindingMode.TwoWay,
                    Source = hexColorBox,
                    ValidatesOnExceptions = true,
                    UpdateSourceTrigger = UpdateSourceTrigger.Explicit   //update item only when ok button is clicked
                };
                customColorForm.SetBinding(CustomColorForm.ColorProperty, binding);
                customColorForm.OKClicked += (s, e) =>
                {
                    customColorForm.GetBindingExpression(CustomColorForm.ColorProperty).UpdateSource(); //update source explicitly
                    window.Close();
                    valueChanged?.Invoke(hexColorBox, new SolidColorBrush(hexColorBox.Color));

                };
                customColorForm.CancelClicked += (s, e) =>
                {
                    window.Close();  //close the window
                };

                window.Header = group.Properties.First().DisplayName;
                window.Content = customColorForm;
                window.Width = 300;
                window.Height = 300;
                window.ShowMaximizeButton = false;
                window.ShowMinimizeButton = false;
                window.CenterOnScreen();
                window.ShowModal();
            };
            hexColorBox.GotFocus += handler;
            hexColorBox.Tag = handler;
        }

        public override void Detach(C1HexColorBox hexColorBox)
        {
            var handler = hexColorBox.Tag as RoutedEventHandler;
            hexColorBox.GotFocus -= handler;
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
