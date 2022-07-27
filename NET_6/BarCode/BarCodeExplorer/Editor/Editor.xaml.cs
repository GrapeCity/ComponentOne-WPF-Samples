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

namespace BarCodeExplorer
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public Editor()
        {
            InitializeComponent();
        }

        public Format CurrentCategory
        {
            get { return (Format)GetValue(CurrentCategoryProperty); }
            set
            {
                SetValue(CurrentCategoryProperty, value);
            }
        }

        public static readonly DependencyProperty CurrentCategoryProperty =
            DependencyProperty.Register(
                "CurrentCategory",
                typeof(Format),
                typeof(Editor),
                new PropertyMetadata(Format.Url, OnCurrentCategoryPropertyChanged));

        private static void OnCurrentCategoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Editor editor = d as Editor;
            if (e.NewValue != null)
            {
                Format newValue = (Format)e.NewValue;
                editor.CurrentEditorControl.Children.Clear();
                Control control = null;
                switch (newValue)
                {
                    case Format.Email:
                        control = new Email();
                        editor.DataContext = new EmailEntity() { Address = "us.sales@grapecity.com" };
                        break;
                    case Format.Url:
                        control = new Url();
                        editor.DataContext = new UrlEntity() { Url = "www.grapecity.com/en/componentone" };
                        break;
                    case Format.VCard:
                        control = new VCard();
                        editor.DataContext = new VCardEntity()
                        {
                            Prefix = "Mr",
                            FirstName = "C1",
                            LastName = "Sales",
                            HomeCountry = "US",
                            Suffix = "Jr",
                            FullName = "Mr.C1.Sales.Jr",
                            Nickname = "C1Sales",
                            Title = "Developer",
                            Photo = "myimage.jpg",
                            Role = "Software developer"
                        };
                        break;
                    case Format.Text:
                        control = new Text();
                        editor.DataContext = new TextEntity() { Text = "Welcome to use C1BarCode control" };
                        break;
                }
                control.Foreground = editor.Foreground;
                editor.CurrentEditorControl.Children.Add(control);
            }
        }
    }
}
