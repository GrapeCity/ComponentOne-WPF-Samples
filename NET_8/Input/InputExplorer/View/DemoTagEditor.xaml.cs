using C1.WPF.Input;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for InputView.xaml
    /// </summary>
    public partial class DemoTagEditor : UserControl
    {
        public DemoTagEditor()
        {
            InitializeComponent();
            Tag = Properties.Resources.TagEditorDemoDes;

            TagEditor.InsertTag("WPF");
        }

        private void OnAddButtonClick(object sender, RoutedEventArgs e)
        {
            string text = AddInput.Text;
            if (!string.IsNullOrEmpty(text))
            {
                TagEditor.InsertTag(text);
                if (TagEditor.DisplayMode == DisplayMode.Text)
                    TagEditor.UpdateTextFromTags();
                AddInput.Text = "";
            }
        }
    }

    public abstract class DispalyModeConverter : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        public bool IsTagMode(DisplayMode mode)
        {
            return mode == DisplayMode.Tag;
        }
    }

    public class TextDisplayModeConverter : DispalyModeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IsTagMode((DisplayMode)value) ? InputExplorer.Properties.Resources.InputTagContent : InputExplorer.Properties.Resources.InputText;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ContentDisplayModeConverter : DispalyModeConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return IsTagMode((DisplayMode)value) ? InputExplorer.Properties.Resources.AddTag : InputExplorer.Properties.Resources.AddText;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
