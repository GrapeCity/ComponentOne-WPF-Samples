using C1.WPF.Input;
using InputSamples.Strings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace InputSamples
{
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
            return IsTagMode((DisplayMode)value) ? Resources.InputTagContent : Resources.InputText;
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
            return IsTagMode((DisplayMode)value) ? Resources.AddTag : Resources.AddText;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
