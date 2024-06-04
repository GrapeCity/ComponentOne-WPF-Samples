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

namespace StockAnalysis.Partial.CustomControls
{
    public class MyColorPicker : Control
    {
        static MyColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyColorPicker), new FrameworkPropertyMetadata(typeof(MyColorPicker)));
        }

        public System.Windows.Media.Color SelectedColor
        {
            get { return (Color)this.GetValue(SelectedColorProperty); }
            set { this.SetValue(SelectedColorProperty, value); }
        }
        public static DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor",
            typeof(Color),
            typeof(MyColorPicker),
            new PropertyMetadata(Colors.Black)
        );
        
        public string Description
        {
            get { return (string)this.GetValue(DescriptionProperty); }
            set { this.SetValue(DescriptionProperty, value); }
        }
        public static DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description",
            typeof(string),
            typeof(MyColorPicker),
            new PropertyMetadata(null)
        );
    }
}
