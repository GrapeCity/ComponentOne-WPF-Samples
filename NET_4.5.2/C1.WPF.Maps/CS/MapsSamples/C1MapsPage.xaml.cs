using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.Maps;

namespace MapsSamples
{
    /// <summary>
    /// Interaction logic for DemoMaps.xaml
    /// </summary>
    public partial class DemoMaps : UserControl
    {
        public DemoMaps()
        {
            InitializeComponent();
        }

        #region Object model

        public MultiScaleTileSource Source
        {
            get
            {
                return maps.Source;
            }
            set
            {
                maps.Source = value;
            }
        }

        public bool ShowTools
        {
            get
            {
                return maps.ShowTools;
            }
            set
            {
                maps.ShowTools = value;
            }
        }

        #region ** ClearStyle properties

        public Brush DEMO_Background
        {
            get
            {
                return maps.Background;
            }
            set
            {
                maps.Background = value;
            }
        }

        public Brush DEMO_Foreground
        {
            get
            {
                return maps.Foreground;
            }
            set
            {
                maps.Foreground = value;
            }
        }

        public Brush DEMO_BorderBrush
        {
            get
            {
                return maps.BorderBrush;
            }
            set
            {
                maps.BorderBrush = value;
            }
        }

        public Brush MouseOverBrush
        {
            get
            {
                return maps.MouseOverBrush;
            }
            set
            {
                maps.MouseOverBrush = value;
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return maps.PressedBrush;
            }
            set
            {
                maps.PressedBrush = value;
            }
        }

        #endregion

        #endregion
    }

    public class TypeNameConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.GetType().Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
