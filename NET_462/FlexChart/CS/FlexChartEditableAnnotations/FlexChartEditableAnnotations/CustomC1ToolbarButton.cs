using C1.WPF.Toolbar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexChartEditableAnnotations
{
    public class CustomC1ToolbarButton : C1ToolbarButton
    {
        private Border _border;
        public CustomC1ToolbarButton()
        {
            this.Background = Brushes.Transparent;
            this.Padding = new Thickness(0);
        }

        public static readonly DependencyProperty CheckedProperty = DependencyProperty.Register(
        "Checked",
        typeof(bool),
        typeof(CustomC1ToolbarButton),
        new FrameworkPropertyMetadata(null)
        );
        public bool Checked
        {
            get { return (bool)GetValue(CheckedProperty); }
            set
            {
                SetValue(CheckedProperty, value);

                if (value == true)
                {
                    this.Background = Brushes.LightSkyBlue;
                }
                else
                {
                    this.Background = Brushes.Transparent;
                }
            }
        }
    }
}
