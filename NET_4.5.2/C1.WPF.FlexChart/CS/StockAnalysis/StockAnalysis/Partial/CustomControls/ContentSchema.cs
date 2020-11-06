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
    public class ContentSchema : Control
    {
        static ContentSchema()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentSchema), new FrameworkPropertyMetadata(typeof(ContentSchema)));
        }

        public Visual Nav
        {
            get { return (Visual)this.GetValue(NavProperty); }
            set { this.SetValue(NavProperty, value); }
        }
        public static DependencyProperty NavProperty = DependencyProperty.Register(
            "Nav",
            typeof(Visual),
            typeof(ContentSchema),
            new PropertyMetadata(null)
        );

        public Visual Settings
        {
            get { return (Visual)this.GetValue(SettingsProperty); }
            set { this.SetValue(SettingsProperty, value); }
        }
        public static DependencyProperty SettingsProperty = DependencyProperty.Register(
            "Settings",
            typeof(Visual),
            typeof(ContentSchema),
            new PropertyMetadata(null)
        );
        


        public Visual AnnotitionsSettings
        {
            get { return (Visual)this.GetValue(AnnotitionsSettingsProperty); }
            set { this.SetValue(AnnotitionsSettingsProperty, value); }
        }
        public static DependencyProperty AnnotitionsSettingsProperty = DependencyProperty.Register(
            "AnnotitionsSettings",
            typeof(Visual),
            typeof(ContentSchema),
            new PropertyMetadata(null)
        );

        public Visual Chart
        {
            get { return (Visual)this.GetValue(ChartProperty); }
            set { this.SetValue(ChartProperty, value); }
        }
        public static DependencyProperty ChartProperty = DependencyProperty.Register(
            "Chart",
            typeof(Visual),
            typeof(ContentSchema),
            new PropertyMetadata(null)
        );

    }
}
