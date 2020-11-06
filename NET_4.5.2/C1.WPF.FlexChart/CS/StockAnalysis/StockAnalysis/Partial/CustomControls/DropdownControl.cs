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
    public class DropdownControl : ComboBox
    {
        public static EventHandler OnDropdownOpened;

        public DropdownControl()
        {
            this.DropDownOpened += (o, e) =>
            {
                if (OnDropdownOpened != null)
                {
                    OnDropdownOpened(this, new EventArgs());
                }
            };
        }
        

        static DropdownControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropdownControl), new FrameworkPropertyMetadata(typeof(DropdownControl)));
        }
        

        public object Display
        {
            get { return this.GetValue(DisplayProperty); }
            set { this.SetValue(DisplayProperty, value); }
        }
        public static DependencyProperty DisplayProperty = DependencyProperty.Register(
            "Display",
            typeof(object),
            typeof(DropdownControl),
            new PropertyMetadata(null)
        );

        public DataTemplate DisplayTemplate
        {
            get { return (DataTemplate)this.GetValue(DisplayTemplateProperty); }
            set { this.SetValue(DisplayTemplateProperty, value); }
        }
        public static DependencyProperty DisplayTemplateProperty = DependencyProperty.Register(
            "DisplayTemplate",
            typeof(DataTemplate),
            typeof(DropdownControl),
            new PropertyMetadata(null)
        );

        public Visual DropdownHeader
        {
            get { return (Visual)this.GetValue(DropdownHeaderProperty); }
            set { this.SetValue(DropdownHeaderProperty, value); }
        }
        public static DependencyProperty DropdownHeaderProperty = DependencyProperty.Register(
            "DropdownHeader",
            typeof(Visual),
            typeof(DropdownControl),
            new PropertyMetadata(null)
        );
        
        public Visual Content
        {
            get { return (Visual)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }
        public static DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content",
            typeof(Visual),
            typeof(DropdownControl),
            new PropertyMetadata(null)
        );
    }
}
