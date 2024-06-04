using C1.WPF.Theming;
using C1.WPF.Theming.Cosmopolitan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RibbonThemes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        bool isLoading = true;
        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }

        #region Events Handler
        private void themeGallery_SelectionChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (!cmbTheme.IsDropDownOpen && !isLoading)
                return;
            cmbTheme.IsDropDownOpen = false;
            if (!isLoading)
            {
                UpdateDarkTheme();
            }
            isLoading = false;

            ApplyTheme((string)e.NewValue);
        }

        private void UpdateDarkTheme()
        {
            if ("Cosmopolitan Dark".Equals(themeGallery.SelectedItem) || "Office2016 Black".Equals(themeGallery.SelectedItem))
            {
                btnUnderline.SmallImageSource = new BitmapImage(new Uri("Resources/Home/Font/underline_white.png", UriKind.Relative));
                menuUnderline.ImageSource = new BitmapImage(new Uri("Resources/Home/Font/underline_white.png", UriKind.Relative));
                menuDoubleUnderline.ImageSource = new BitmapImage(new Uri("Resources/Home/Font/double_underline_white.png", UriKind.Relative));

                btnBottomBorder.SmallImageSource = new BitmapImage(new Uri("Resources/Home/Font/bottom_border_white.png", UriKind.Relative));

                var borders1 = new List<ImageHeaderData>();
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/bottom_border_white.png", Header = "Bottom Border" });
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_border_white.png", Header = "Top Border" });
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/left_border_white.png", Header = "Left Border" });
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/right_border_white.png", Header = "Right Border" });
                bordersGalleryCategory1.ItemsSource = borders1;

                var borders2 = new List<ImageHeaderData>();
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/no_border_white.png", Header = "No Border" });
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/all_borders_white.png", Header = "All Borders" });
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/outside_borders_white.png", Header = "Outside Borders" });
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/thick_box_border_white.png", Header = "Thick Box Border" });
                bordersGalleryCategory2.ItemsSource = borders2;

                var borders3 = new List<ImageHeaderData>();
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/bottom_double_border_white.png", Header = "Bottom Double Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/thick_bottom_border_white.png", Header = "Thick Bottom Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_bottom_border_white.png", Header = "Top And Bottom Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_thick_bottom_border_white.png", Header = "Top And Thick Bottom Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_double_bottom_border_white.png", Header = "Top And Double Bottom Border" });
                bordersGalleryCategory3.ItemsSource = borders3;
            }
            else
            {
                btnUnderline.SmallImageSource = new BitmapImage(new Uri("Resources/Home/Font/underline.png", UriKind.Relative));
                menuUnderline.ImageSource = new BitmapImage(new Uri("Resources/Home/Font/underline.png", UriKind.Relative));
                menuDoubleUnderline.ImageSource = new BitmapImage(new Uri("Resources/Home/Font/double_underline.png", UriKind.Relative));

                btnBottomBorder.SmallImageSource = new BitmapImage(new Uri("Resources/Home/Font/bottom_border.png", UriKind.Relative));

                var borders1 = new List<ImageHeaderData>();
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/bottom_border.png", Header = "Bottom Border" });
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_border.png", Header = "Top Border" });
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/left_border.png", Header = "Left Border" });
                borders1.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/right_border.png", Header = "Right Border" });
                bordersGalleryCategory1.ItemsSource = borders1;

                var borders2 = new List<ImageHeaderData>();
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/no_border.png", Header = "No Border" });
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/all_borders.png", Header = "All Borders" });
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/outside_borders.png", Header = "Outside Borders" });
                borders2.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/thick_box_border.png", Header = "Thick Box Border" });
                bordersGalleryCategory2.ItemsSource = borders2;

                var borders3 = new List<ImageHeaderData>();
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/bottom_double_border.png", Header = "Bottom Double Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/thick_bottom_border.png", Header = "Thick Bottom Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_bottom_border.png", Header = "Top And Bottom Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_thick_bottom_border.png", Header = "Top And Thick Bottom Border" });
                borders3.Add(new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_double_bottom_border.png", Header = "Top And Double Bottom Border" });
                bordersGalleryCategory3.ItemsSource = borders3;
            }
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                image.Visibility = Visibility.Collapsed;
                Image image1 = image.Tag as Image;
                if (image1 != null)
                {
                    image1.Visibility = Visibility.Visible;
                }
            }
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            FindReplaceDialog frd = new FindReplaceDialog();
            // assign window style
            frd.Style = TryFindResource(typeof(Window)) as Style;
            frd.ShowDialog();
        }

        private void replace_Click(object sender, RoutedEventArgs e)
        {
            FindReplaceDialog frd = new FindReplaceDialog(1);
            // assign window style
            frd.Style = TryFindResource(typeof(Window)) as Style;
            frd.ShowDialog();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        #endregion

        #region Implementation
        private void InitializeData()
        {
            #region Application Menu Items

            // Save As items
            saveas1.Header = new ImageHeaderToolTipData() { ToolTip = "Excel Workbook", ToolTipDescription = "Save the workbook in the default file format." };
            saveas2.Header = new ImageHeaderToolTipData() { ToolTip = "Excel Macro-Enabled Workbook", ToolTipDescription = "Save the workbook in the XML-based and macro-enabled file format." };
            saveas3.Header = new ImageHeaderToolTipData() { ToolTip = "Excel Binary Workbook", ToolTipDescription = "Save the workbook in a binary file format optimized for fast loading and saving." };
            saveas4.Header = new ImageHeaderToolTipData() { ToolTip = "Excel 97-2003 Workbook", ToolTipDescription = "Save a copy of the workbook that is fully compatible with Excel 97-2003." };
            saveas5.Header = new ImageHeaderToolTipData() { ToolTip = "Find add-ins for other file formats" };
            saveas6.Header = new ImageHeaderToolTipData() { ToolTip = "Other Formats", ToolTipDescription = "Open the Save As dialog box to select from all possible file types." };

            // Print
            print1.Header = new ImageHeaderToolTipData() { ToolTip = "Print", ToolTipDescription = "Select a printer, number or copies, and other printing options before printing." };
            print2.Header = new ImageHeaderToolTipData() { ToolTip = "Quick Print", ToolTipDescription = "Send the workbook directly to the default printer without making changes." };
            print3.Header = new ImageHeaderToolTipData() { ToolTip = "Print Preview", ToolTipDescription = "Preview and make changes to pages before printing." };

            // Prepare
            prepare1.Header = new ImageHeaderToolTipData() { ToolTip = "Properties", ToolTipDescription = "View and edit workbook properties, such as Title, Author, and Keywords." };
            prepare2.Header = new ImageHeaderToolTipData() { ToolTip = "Inspect Document", ToolTipDescription = "Check the workbook for hidden metadata or personal information." };
            prepare3.Header = new ImageHeaderToolTipData() { ToolTip = "Encrypt Document", ToolTipDescription = "Increase the security of the workbook by adding encryption." };
            prepare4.Header = new ImageHeaderToolTipData() { ToolTip = "Restrict Permission", ToolTipDescription = "Grant people access while restricting their ability to edit, copy, and print." };
            prepare5.Header = new ImageHeaderToolTipData() { ToolTip = "Add a Digital Signature", ToolTipDescription = "Ensure the integrity of workbook by adding an invisible digital signature." };
            prepare6.Header = new ImageHeaderToolTipData() { ToolTip = "Mark as Final", ToolTipDescription = "Let readers know the workbook is final and make it read-only." };
            prepare7.Header = new ImageHeaderToolTipData() { ToolTip = "Run Compatibility Checker", ToolTipDescription = "Check for features not supported by earlier versions of Excel." };

            // Send
            send1.Header = new ImageHeaderToolTipData() { ToolTip = "E-mail", ToolTipDescription = "Send a copy of workbook in an e-mail message as an attachment." };
            send2.Header = new ImageHeaderToolTipData() { ToolTip = "Internet Fax", ToolTipDescription = "Use an Internet fax service to fax the workbook." };

            // Publish
            publish1.Header = new ImageHeaderToolTipData() { ToolTip = "Excel Services", ToolTipDescription = "Save for Excel Services, Specify what is shown in the browser, and set options." };
            publish2.Header = new ImageHeaderToolTipData() { ToolTip = "Document Management Server", ToolTipDescription = "Share the workbook by saving it to a document management server." };
            publish3.Header = new ImageHeaderToolTipData() { ToolTip = "Create Document Workspace", ToolTipDescription = "Create a new site for the workbook and keep the local copy synchronized." };

            #endregion

            #region Recent Documents
            List<RecentDocuments> docs = new List<RecentDocuments>
            {
                new RecentDocuments { Index = 1, Name = "Sales Data.xlxs", IsPinned = true },
                new RecentDocuments { Index = 2, Name = "Sales Division Structer.xlxs", IsPinned = false },
                new RecentDocuments { Index = 3, Name = "Sales Plan.xlxs", IsPinned = false },
                new RecentDocuments { Index = 4, Name = "Book1.xlxs", IsPinned = false },
                new RecentDocuments { Index = 5, Name = "Book2.xlxs", IsPinned = false }
            };
            recentDocs.ItemsSource = docs;
            #endregion

            #region Theme
            List<string> themes = new List<string>
            {
                "Material",
                "Material Dark",
                "Cosmopolitan",
                "Cosmopolitan Dark",
                "Office2013 White",
                "Office2013 LightGray",
                "Office2013 DarkGray",
                "Office2016 Colorful",
                "Office2016 DarkGray",
                "Office2016 White",
                "Office2016 Black"
            };
            cmbTheme.DataContext = themes;
            themeGallery.SelectedItem = "Material";
            #endregion

            #region Font Family
            List<FontFamily> themeFonts = new List<FontFamily>
            {
                new FontFamily("Calibri Light"),
                new FontFamily("Calibri")
            };
            themeFontGalleryCategory.ItemsSource = themeFonts;

            allFontGalleryCategory.ItemsSource = Fonts.SystemFontFamilies;
            fontGallery.SelectedItem = SystemFonts.MessageFontFamily;
            #endregion

            #region Font Size
            List<string> fontSizes = new List<string>
            {
                "8",
                "9",
                "10",
                "11",
                "12",
                "14",
                "16",
                "18",
                "20",
                "22",
                "24",
                "26",
                "28",
                "36",
                "48",
                "72"
            };
            fontSizeGalleryCategory.ItemsSource = fontSizeGalleryCategory1.ItemsSource = fontSizeGalleryCategory2.ItemsSource = fontSizes;
            fontSizeGallery.SelectedItem = fontSizeGallery1.SelectedItem = fontSizeGallery2.SelectedItem = "11";
            #endregion

            #region Border
            List<ImageHeaderData> borders1 = new List<ImageHeaderData>
            {
                new ImageHeaderData { ImageSource = "Resources/Home/Font/bottom_border.png", Header = "Bottom Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/top_border.png", Header = "Top Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/left_border.png", Header = "Left Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/right_border.png", Header = "Right Border" }
            };
            bordersGalleryCategory1.ItemsSource = borders1;

            List<ImageHeaderData> borders2 = new List<ImageHeaderData>
            {
                new ImageHeaderData { ImageSource = "Resources/Home/Font/no_border.png", Header = "No Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/all_borders.png", Header = "All Borders" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/outside_borders.png", Header = "Outside Borders" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/thick_box_border.png", Header = "Thick Box Border" }
            };
            bordersGalleryCategory2.ItemsSource = borders2;

            List<ImageHeaderData> borders3 = new List<ImageHeaderData>
            {
                new ImageHeaderData { ImageSource = "Resources/Home/Font/bottom_double_border.png", Header = "Bottom Double Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/thick_bottom_border.png", Header = "Thick Bottom Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_bottom_border.png", Header = "Top And Bottom Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_thick_bottom_border.png", Header = "Top And Thick Bottom Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/top_and_double_bottom_border.png", Header = "Top And Double Bottom Border" }
            };
            bordersGalleryCategory3.ItemsSource = borders3;

            List<ImageHeaderData> darwborders = new List<ImageHeaderData>
            {
                new ImageHeaderData { ImageSource = "Resources/Home/Font/draw_border.png", Header = "Draw Border" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/draw_border_grid.png", Header = "Draw Border Grid" },
                new ImageHeaderData { ImageSource = "Resources/Home/Font/erase_border.png", Header = "Erase Border" }
            };
            drawBordersGalleryCategory.ItemsSource = darwborders;
            #endregion

            #region Colors
            List<ColorData> themecolors = new List<ColorData>
            {
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "White, Background 1" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "Black, Text 1" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEECE1")), ToolTip = "Tan, Background 2" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1F497D")), ToolTip = "DarkBlue, Text 2" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F81BD")), ToolTip = "Blue, Accent 1" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C0504D")), ToolTip = "Red, Accent 2" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9BBB59")), ToolTip = "Olive Green,  Accent 3" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8064A2")), ToolTip = "Purple, Accent 4" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4BACC6")), ToolTip = "Aqua, Accent 5" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F79646")), ToolTip = "Orange, Accent 6" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2F2F2")), ToolTip = "White, Background 1, Darker 5%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F7F7F")), ToolTip = "Black, Text 1, Lighter 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDD0C3")), ToolTip = "Tan, Background 2, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C6D9F0")), ToolTip = "DarkBlue, Text 2, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DBE5F1")), ToolTip = "Blue, Accent 1, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2DCDB")), ToolTip = "Red, Accent 2, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EBF1DD")), ToolTip = "Olive Green,  Accent 3, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E0EC")), ToolTip = "Purple, Accent 4, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DBEEF3")), ToolTip = "Aqua, Accent 5, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDE5DA")), ToolTip = "Orange, Accent 6, Lighter 80%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8D8D8")), ToolTip = "White, Background 1, Darker 15%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#595959")), ToolTip = "Black, Text 1, Lighter 35%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4BD97")), ToolTip = "Tan, Background 2, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8DB3E2")), ToolTip = "DarkBlue, Text 2, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B8CCE4")), ToolTip = "Blue, Accent 1, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5B9B7")), ToolTip = "Red, Accent 2, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D7E3BC")), ToolTip = "Olive Green,  Accent 3, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCC1D9")), ToolTip = "Purple, Accent 4, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B7DDE8")), ToolTip = "Aqua, Accent 5, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FBD5B5")), ToolTip = "Orange, Accent 6, Lighter 60%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BFBFBF")), ToolTip = "White, Background 1, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F3F3F")), ToolTip = "Black, Text 1, Lighter 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#938953")), ToolTip = "Tan, Background 2, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#548DD4")), ToolTip = "DarkBlue, Text 2, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#95B3D7")), ToolTip = "Blue, Accent 1, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D99694")), ToolTip = "Red, Accent 2, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C3D69B")), ToolTip = "Olive Green,  Accent 3, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B2A1C7")), ToolTip = "Purple, Accent 4, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92CDDC")), ToolTip = "Aqua, Accent 5, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FAC08F")), ToolTip = "Orange, Accent 6, Lighter 40%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#A5A5A5")), ToolTip = "White, Background 1, Darker 35%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#262626")), ToolTip = "Black, Text 1, Lighter 15%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#494429")), ToolTip = "Tan, Background 2, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#17365D")), ToolTip = "DarkBlue, Text 2, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#366092")), ToolTip = "Blue, Accent 1, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#953734")), ToolTip = "Red, Accent 2, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76923C")), ToolTip = "Olive Green,  Accent 3, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5F497A")), ToolTip = "Purple, Accent 4, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#31859B")), ToolTip = "Aqua, Accent 5, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E36C09")), ToolTip = "Orange, Accent 6, Darker 25%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7F7F7F")), ToolTip = "White, Background 1, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0C0C0C")), ToolTip = "Black, Text 1, Lighter 5%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1D1B10")), ToolTip = "Tan, Background 2, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0F243E")), ToolTip = "DarkBlue, Text 2, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#244061")), ToolTip = "Blue, Accent 1, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#632423")), ToolTip = "Red, Accent 2, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F6128")), ToolTip = "Olive Green,  Accent 3, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3F3151")), ToolTip = "Purple, Accent 4, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#205867")), ToolTip = "Aqua, Accent 5, Darker 50%" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#974806")), ToolTip = "Orange, Accent 6, Darker 50%" }
            };
            themeColorsGalleryCategory1.ItemsSource 
                = themeColorsGalleryCategory2.ItemsSource
                = themeColorsGalleryCategory3.ItemsSource 
                = themeColorsGalleryCategory4.ItemsSource 
                = themecolors;

            List<ColorData> standardcolors = new List<ColorData>
            {
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C00000")), ToolTip = "Dark Red" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000")), ToolTip = "Red" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC000")), ToolTip = "Orange" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF00")), ToolTip = "Yellow" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92D050")), ToolTip = "Light Green" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B050")), ToolTip = "Green" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00B0F0")), ToolTip = "Light Blue" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0070C0")), ToolTip = "Blue" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#002060")), ToolTip = "Dark Blue" },
                new ColorData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7030A0")), ToolTip = "Purple" }
            };
            standardColorsGalleryCategory1.ItemsSource 
                = standardColorsGalleryCategory2.ItemsSource 
                = standardColorsGalleryCategory3.ItemsSource
                = standardColorsGalleryCategory4.ItemsSource 
                = standardcolors;
            #endregion

            #region Lines
            List<LineData> lines = new List<LineData>
            {
                new LineData { StrokeDashArray = null, Tag = "None" },
                new LineData { StrokeDashArray = null, StrokeThickness = 1, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 1 }, StrokeThickness = 1, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 2, 2 }, StrokeThickness = 1, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 4, 1 }, StrokeThickness = 1, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 2, 2, 8, 2 }, StrokeThickness = 1, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 3, 3, 3, 3, 9, 3 }, StrokeThickness = 1, Tag = "Single" },
                new LineData { StrokeDashArray = null, StrokeThickness = 2, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 4, 1 }, StrokeThickness = 2, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 2, 2, 8, 2 }, StrokeThickness = 2, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 2, 2, 2, 2, 8, 2 }, StrokeThickness = 2, Tag = "Single" },
                new LineData { StrokeDashArray = new DoubleCollection { 2, 2, 6, 2 }, StrokeThickness = 3, Tag = "Single" },
                new LineData { StrokeDashArray = null, StrokeThickness = 3, Tag = "Single" },
                new LineData { StrokeDashArray = null, StrokeThickness = 6, Tag = "Double" }
            };
            lineGalleryCategory.ItemsSource = lines;
            #endregion

            #region Number Format
            List<ImageHeaderData> numberFormats = new List<ImageHeaderData>
            {
                new ImageHeaderData { ImageSource = "Resources/Home/Number/general.png", Header = "General" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/number.png", Header = "Number" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/currency.png", Header = "Currency" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/accounting.png", Header = "Accounting" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/short_date.png", Header = "Short Date" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/long_date.png", Header = "Long Date" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/time.png", Header = "Time" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/percentage.png", Header = "Percentage" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/fraction.png", Header = "Fraction" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/scientific.png", Header = "Scientific" },
                new ImageHeaderData { ImageSource = "Resources/Home/Number/text.png", Header = "Text" }
            };
            numberFormatGalleryCategory.ItemsSource = numberFormats;
            numberFormatGallery.SelectedItem = numberFormats.First();
            #endregion

            #region Styles
            List<ImageHeaderData> topbottoms = new List<ImageHeaderData>
            {
                new ImageHeaderData { ImageSource = "Resources/Home/Styles/top_10_items.png", Header = "Top 10 Items..." },
                new ImageHeaderData { ImageSource = "Resources/Home/Styles/top_10_items.png", Header = "Top 10 %..." },
                new ImageHeaderData { ImageSource = "Resources/Home/Styles/bottom_10_items.png", Header = "Bottom 10 Items..." },
                new ImageHeaderData { ImageSource = "Resources/Home/Styles/bottom_10_percent.png", Header = "Bottom 10 %..." },
                new ImageHeaderData { ImageSource = "Resources/Home/Styles/above_average.png", Header = "Above Average..." },
                new ImageHeaderData { ImageSource = "Resources/Home/Styles/below_average.png", Header = "Below Average..." }
            };
            topbottomGalleryCategory.ItemsSource = topbottoms;

            List<ImageHeaderToolTipData> databars = new List<ImageHeaderToolTipData>
            {
                new ImageHeaderToolTipData { ImageSource = "Resources/Home/Styles/blue_data_bars.png", ToolTip = "Blue Data Bars", ToolTipDescription = "Add a colored data bar to represent the value in a cell. The higher the value, the longer the bar." },
                new ImageHeaderToolTipData { ImageSource = "Resources/Home/Styles/green_data_bars.png", ToolTip = "Green Data Bars", ToolTipDescription = "Add a colored data bar to represent the value in a cell. The higher the value, the longer the bar." },
                new ImageHeaderToolTipData { ImageSource = "Resources/Home/Styles/red_data_bars.png", ToolTip = "Red Data Bars", ToolTipDescription = "Add a colored data bar to represent the value in a cell. The higher the value, the longer the bar." },
                new ImageHeaderToolTipData { ImageSource = "Resources/Home/Styles/orange_data_bars.png", ToolTip = "Orange Data Bars", ToolTipDescription = "Add a colored data bar to represent the value in a cell. The higher the value, the longer the bar." },
                new ImageHeaderToolTipData { ImageSource = "Resources/Home/Styles/purple_data_bars.png", ToolTip = "Purple Data Bars", ToolTipDescription = "Add a colored data bar to represent the value in a cell. The higher the value, the longer the bar." }
            };
            databarsGalleryCategory.ItemsSource = databars;

            List<CellStyleData> cellstyles1 = new List<CellStyleData>
            {
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A9090")), ToolTip = "Normal" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFC7CE")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9C0055")), ToolTip = "Bad" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C6EFCE")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#007B74")), ToolTip = "Good" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEB9C")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9C6500")), ToolTip = "Neutral" }
            };
            cellStyleGalleryCategory1.ItemsSource = cellstyles1;

            List<CellStyleData> cellstyles2 = new List<CellStyleData>
            {
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DCE6F1")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "20% - Accent1" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F2DCDB")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "20% - Accent2" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EBF1DE")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "20% - Accent3" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E4E6")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "20% - Accent4" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DAEEF3")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "20% - Accent5" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FDE9D9")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "20% - Accent6" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B8CCE4")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "40% - Accent1" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E6B8B7")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "40% - Accent2" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D8E4BC")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "40% - Accent3" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#CCC0DA")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "40% - Accent4" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B7DEE8")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "40% - Accent5" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCD5B4")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "40% - Accent6" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9CB4CE")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "60% - Accent1" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DA9694")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "60% - Accent2" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C4D79B")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "60% - Accent3" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#B1A0C7")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "60% - Accent4" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#92CDDC")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "60% - Accent5" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FABF8F")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "60% - Accent6" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F81BD")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "Accent1" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C0504D")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "Accent2" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9BBB59")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "Accent3" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8064A2")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "Accent4" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4BACC6")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "Accent5" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F79646")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), ToolTip = "Accent6" }
            };
            cellStyleGalleryCategory2.ItemsSource = cellstyles2;

            List<CellStyleData> cellstyles3 = new List<CellStyleData>
            {
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "Comma" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "Comma [0]" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "Current" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "Current [0]" },
                new CellStyleData { Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")), Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000")), ToolTip = "Percent" }
            };
            cellStyleGalleryCategory3.ItemsSource = cellstyles3;
            #endregion
        }
        private void ApplyTheme(string name)
        {
            C1Theme theme = null;
            C1Theme ribbonTheme = null;
            switch (name)
            {
                case "Cosmopolitan":
                    theme = new C1.WPF.Theming.Cosmopolitan.C1ThemeCosmopolitan();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonCosmopolitan();
                    break;
                case "Cosmopolitan Dark":
                    theme = new C1.WPF.Theming.CosmopolitanDark.C1ThemeCosmopolitanDark(); ;
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonCosmopolitanDark();
                    break;
                case "Office2013 White":
                    theme = new C1.WPF.Theming.Office2013.C1ThemeOffice2013White();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonOffice2013White();
                    break;
                case "Office2013 LightGray":
                    theme = new C1.WPF.Theming.Office2013.C1ThemeOffice2013LightGray();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonOffice2013LightGray();
                    break;
                case "Office2013 DarkGray":
                    theme = new C1.WPF.Theming.Office2013.C1ThemeOffice2013DarkGray();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonOffice2013DarkGray();
                    break;
                case "Office2016 Colorful":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016Colorful();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonOffice2016Colorful();
                    break;
                case "Office2016 DarkGray":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016DarkGray();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonOffice2016DarkGray();
                    break;
                case "Office2016 White":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016White();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonOffice2016White();
                    break;
                case "Office2016 Black":
                    theme = new C1.WPF.Theming.Office2016.C1ThemeOffice2016Black();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonOffice2016Black();
                    break;
                case "Material":
                    theme = new C1.WPF.Theming.Material.C1ThemeMaterial();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonMaterial();
                    break;
                case "Material Dark":
                    theme = new C1.WPF.Theming.MaterialDark.C1ThemeMaterialDark();
                    ribbonTheme = new C1.WPF.Theming.Ribbon.C1ThemeRibbonMaterialDark();
                    break;
                default:
                    break;
            }
            // apply ribbon theme 
            ribbonTheme.Apply(ribbon);
            ribbonTheme.Apply(this);
            root.Style = TryFindResource(new ComponentResourceKey(typeof(Ribbon), "RibbonWindowStyle")) as Style;

            // apply overall theme
            if (theme != null)
            {
                this.Resources.MergedDictionaries.Add(C1Theme.GetCurrentThemeResources(theme));
                var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
                if (adornerLayer != null)
                {
                    // this will aplly theme to everything displayed in adorner, including any C1Window instances
                    C1Theme.ApplyTheme(adornerLayer, theme);
                }
            }

            // add theme resource dictionaries to application resources as shown below, so that they can be accessible for other windows
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(theme.GetNewResourceDictionary());
            Application.Current.Resources.MergedDictionaries.Add(ribbonTheme.GetNewResourceDictionary());
        }
        #endregion
    }
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {

                if ((bool)value)
                {
                    if (parameter.ToString() == "1")
                    {
                        return Visibility.Visible;
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }

                }
                else
                {
                    if (parameter.ToString() == "1")
                    {
                        return Visibility.Collapsed;
                    }
                    else
                    {
                        return Visibility.Visible;
                    }
                }

            }
            else
            {
                return Visibility.Collapsed;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
            {
                if ((Visibility)value==Visibility.Visible)
                {
                    if (parameter.ToString() == "1")
                    {
                        return true ;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    if (parameter.ToString() == "1")
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
