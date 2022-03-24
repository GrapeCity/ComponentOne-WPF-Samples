using C1.WPF.ColorPicker;
using C1.WPF.Input;
using C1.WPF.PropertyGrid;
using System.ComponentModel;
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

            propertyGrid.AvailableEditors.Add(new CustomColorEditor());

            propertyGrid.AutoGenerateProperties = false;
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            {
                MemberName = nameof(TextBox.Background),
                Editor = new CustomColorEditor()
            });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            {
                MemberName = nameof(TextBox.Foreground),
                Editor = new CustomColorEditor()
            });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            {
                MemberName = nameof(TextBox.BorderBrush),
                Editor = new CustomColorEditor()
            });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            {
                MemberName = nameof(TextBox.BorderThickness),
                Editor = new ThicknessEditor()
            });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            {
                MemberName = nameof(TextBox.Opacity),
                Editor = new RangeSliderEditor() { Minimum = 0, Maximum = 1 }
            });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            {
                MemberName = nameof(TextBox.Width),
                Editor = new RangeSliderEditor() { Minimum = 0, Maximum = 500 }
            });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute()
            {
                MemberName = nameof(TextBox.Height),
                Editor = new RangeSliderEditor() { Minimum = 0, Maximum = 100 }
            });
        }
    }

    public class CustomColorEditor : C1HexColorBox, ITypeEditorControl
    {
        PropertyAttribute _property;

        bool ITypeEditorControl.Supports(PropertyAttribute Property)
        {

            return Property.PropertyInfo.PropertyType == typeof(Color);
        }

        ITypeEditorControl ITypeEditorControl.Create()
        {

            return new CustomColorEditor();

        }

        void ITypeEditorControl.Attach(PropertyAttribute property)
        {
            _property = property;
            this.GotFocus += new RoutedEventHandler(CustomColorEditor_GotFocus); //opens up window to show up editor when the hexbox gets focus
            var binding = new Binding(property.PropertyInfo.Name)
            {
                Mode = BindingMode.TwoWay,
                Source = _property.SelectedObject,
                Converter = new C1.WPF.Core.ColorConverter(),
                ValidatesOnExceptions = true
            };

            this.SetBinding(C1HexColorBox.ColorProperty, binding);
        }

        void CustomColorEditor_GotFocus(object sender, RoutedEventArgs e)
        {

            var window = new C1.WPF.Docking.C1Window();

            var customColorForm = new CustomColorForm();
            var binding = new Binding(nameof(C1HexColorBox.Color))
            {
                Mode = BindingMode.TwoWay,
                Source = this,
                ValidatesOnExceptions = true,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit   //update item only when ok button is clicked
            };
            customColorForm.SetBinding(CustomColorForm.ColorProperty, binding);
            customColorForm.OKClicked += (s, e) =>
            {
                customColorForm.GetBindingExpression(CustomColorForm.ColorProperty).UpdateSource(); //update source explicitly
                window.Close();

            };
            customColorForm.CancelClicked += (s, e) =>
            {
                window.Close();  //close the window
            };

            window.Header = _property.DisplayName;
            window.Content = customColorForm;
            window.Width = 300;
            window.Height = 300;
            window.CenterOnScreen();
            window.ShowModal();
        }


        void ITypeEditorControl.Detach(PropertyAttribute property)
        {
        }

        public event PropertyChangedEventHandler ValueChanged;
    }


    public class RangeSliderEditor : Slider, ITypeEditorControl
    {
        PropertyAttribute _property;
        public event PropertyChangedEventHandler ValueChanged;

        public void Attach(PropertyAttribute property)
        {
            _property = property;
            SetBinding(Slider.ValueProperty, new Binding(property.PropertyInfo.Name)
            {
                Mode = BindingMode.TwoWay,
                Source = _property.SelectedObject,
            });
        }

        public ITypeEditorControl Create()
        {
            return new RangeSliderEditor() { Minimum = Minimum, Maximum = Maximum};
        }

        public void Detach(PropertyAttribute property)
        {
            ClearValue(Slider.ValueProperty);
        }

        public bool Supports(PropertyAttribute property)
        {
            return property.PropertyInfo.PropertyType == typeof(double);
        }
    }
}
