using C1.BarCode;
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

namespace BarCodesSample
{
    /// <summary>
    /// Class defined to store the properties of a category
    /// </summary>
    class Category
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public Format Format { get; set; }
        public Category(string key, string value, Format format)
        {
            Key = key;
            Value = value;
            Format = format;
        }
    }

    /// <summary>
    /// Interaction logic for EncodingFormat.xaml
    /// </summary>
    public partial class EncodingFormat : UserControl
    {
        Dictionary<string, CodeType> CodeTypes = new Dictionary<string, CodeType>();
        List<Category> Categories = new List<Category>();

        public EncodingFormat()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CreateCodeTypes();
            CreateCategories();
            this.barCodeTypes.ItemsSource = CodeTypes;
            this.categories.ItemsSource = Categories;
            this.barCodeTypes.SelectedIndex = 0;
            this.categories.SelectedIndex = 3;
        }

        private void CreateCategories()
        {
            if (Categories.Count == 0)
            {
                Categories.Add(new Category("Resources/Email.png", "Email", Format.Email));
                Categories.Add(new Category("Resources/Text.png", "Text", Format.Text));
                Categories.Add(new Category("Resources/Url.png", "Url", Format.Url));
                Categories.Add(new Category("Resources/VCard.png", "VCard", Format.VCard));
            }
        }

        private void CreateCodeTypes()
        {
            if (CodeTypes.Count == 0)
            {
                CodeTypes.Add("Resources/QRCode.png", CodeType.QRCode);
                CodeTypes.Add("Resources/DataMatrix.png", CodeType.DataMatrix);
                CodeTypes.Add("Resources/PDF417.png", CodeType.Pdf417);
                CodeTypes.Add("Resources/Code39X.png", CodeType.Code39x);
            }
        }

        private void barCodeTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;
            KeyValuePair<string, CodeType> selectedItem = (KeyValuePair<string, CodeType>)listbox.SelectedItem;
            CurrentCodeType = selectedItem.Value;
        }

        private void encodingTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listbox = sender as ListBox;
            Category selectedItem = (Category)listbox.SelectedItem;
            CurrentCategory = selectedItem.Format;
        }
        #region
        public Visibility ShowStatus
        {
            get { return (Visibility)GetValue(ShowStatusProperty); }
            set { SetValue(ShowStatusProperty, value); }
        }

        public static readonly DependencyProperty ShowStatusProperty =
            DependencyProperty.Register("ShowStatus", typeof(Visibility), typeof(EncodingFormat), new PropertyMetadata(Visibility.Collapsed));

        public Format CurrentCategory
        {
            get { return (Format)GetValue(CurrentCategoryProperty); }
            set { SetValue(CurrentCategoryProperty, value); }
        }

        public static readonly DependencyProperty CurrentCategoryProperty =
            DependencyProperty.Register("CurrentCategory", typeof(Format), typeof(EncodingFormat), new PropertyMetadata(Format.VCard, OnCurrentCategoryPropertyChanged));

        public CodeType CurrentCodeType
        {
            get { return (CodeType)GetValue(CurrentCodeTypeProperty); }
            set { SetValue(CurrentCodeTypeProperty, value); }
        }

        public static readonly DependencyProperty CurrentCodeTypeProperty =
            DependencyProperty.Register("CurrentCodeType", typeof(CodeType), typeof(EncodingFormat), new PropertyMetadata(CodeType.QRCode, OnCurrentCodeTypePropertyChanged));

        private static void OnCurrentCodeTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EncodingFormat encodingFormat = d as EncodingFormat;
            if (encodingFormat.CurrentCodeType == CodeType.QRCode 
                && encodingFormat.CurrentCategory == Format.Url)
            {
                encodingFormat.ShowStatus = Visibility.Visible;
            }
            else
            {
                encodingFormat.ShowStatus = Visibility.Collapsed;
            }
        }

        private static void OnCurrentCategoryPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            EncodingFormat category = d as EncodingFormat;

            if (category.CurrentCodeType == CodeType.QRCode && category.CurrentCategory == Format.Url)
            {
                category.ShowStatus = Visibility.Visible;
            }
            else
            {
                category.ShowStatus = Visibility.Collapsed;
            }
        }
       
        #endregion
    }
}
