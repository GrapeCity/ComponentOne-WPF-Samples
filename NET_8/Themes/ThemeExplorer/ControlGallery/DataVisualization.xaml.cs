using C1.BarCode;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThemeExplorer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class DataVisualization : UserControl
    {
        int npts = 50;
        Random rnd = new Random();
        List<DataItem> _data;
        readonly Dictionary<CodeType, string> BarCodes = new()
        {
            {
                CodeType.Ansi39,
                "ANSI 3 of 9 (Code 39) uses upper case, numbers, - , * $ / + %."
            },
            {
                CodeType.Ansi39x,
                "ANSI Extended 3 of 9 (Extended Code 39) uses the complete ASCII character set."
            },
            {
                CodeType.Code_2_of_5,
                "Code 2 of 5 uses only numbers."
            },
            {
                CodeType.Code25intlv,
                "Interleaved 2 of 5 uses only numbers."
            },
            {
                CodeType.Matrix_2_of_5,
                "Matrix 2 of 5 is a higher density barcode consisting of 3 black bars and 2 white bars & it only uses numbers."
            },
            {
                CodeType.Code39,
                "Code 39 uses numbers,  % * $ /. , - +, and upper case."
            },
            {
                CodeType.Code39x,
                "Extended Code 39 uses the complete ASCII character set."
            },
            {
                CodeType.Code_128_A,
                "Code 128 A uses control characters, numbers, punctuation, and upper case."
            },
            {
                CodeType.Code_128_B,
                "Code 128 B uses punctuation, numbers, upper case and lower case."
            },
            {
                CodeType.Code_128_C,
                "Code 128 C uses only numbers."
            },
            {
                CodeType.Code_128auto,
                "Code 128 Auto uses the complete ASCII character set. " +
        "It automatically selects between Code 128 A, B and C to give the smallest barcode."
            },
            {
                CodeType.Code_93,
                "Code 93 uses uppercase, % $ * / , + -,  and numbers."
            },
            {
                CodeType.Code93x,
                "Extended Code 93 uses the complete ASCII character set."
            },
            {
                CodeType.MSI,
                "MSI Code uses only numbers."
            },
            {
                CodeType.PostNet,
                "PostNet uses only numbers with a check digit."
            },
            {
                CodeType.Codabar,
                "Codabar uses A B C D + - : , / and numbers."
            },
            {
                CodeType.EAN_8,
                "EAN-8 uses only numbers (7 numbers and a check digit)."
            },
            {
                CodeType.EAN_13,
                "EAN-13 uses only numbers (12 numbers and a check digit). " +
        "If there are only 12 numbers in the string, it calculates a checksum and adds it to the thirteenth position. " +
        "If there are 13, it validates the checksum and throws an error if it is incorrect."
            },
            { CodeType.UPC_A, "UPC-A uses only numbers (11 numbers and a check digit)." },
            {
                CodeType.UPC_E0,
                "UPC-E0 uses only numbers. " +
        "Used for zero-compression UPC symbols. " +
        "For the Caption property, you may enter either a six-digit UPC-E code or a complete 11-digit (includes code type, which must be 0 (zero)) UPC-A code. " +
        "If an 11-digit code is entered, the Barcode control will convert it to a six-digit UPC-E code, if possible. " +
        "If it is not possible to convert from the 11-digit code to the six-digit code, nothing is displayed."
            },
            {
                CodeType.UPC_E1,
                "UPC-E1 uses only numbers.  Used typically for shelf labeling in the retail environment. " +
        "The length of the input string for U.P.C. E1 is six numeric characters."
            },
            {
                CodeType.RM4SCC,
                "Royal Mail RM4SCC uses only letters and numbers (with a check digit). This is the barcode used by the Royal Mail in the United Kingdom."
            },
            {
                CodeType.UCCEAN128,
                "UCC/EAN –128 uses the complete ASCII character Set. This is a special version of Code 128 used in HIBC applications."
            },
            {
                CodeType.QRCode,
                "QRCode is a 2D symbology that is capable of handling numeric, alphanumeric and byte data as well as Japanese kanji and kana characters. " +
        "This symbology can encode up to 7,366 characters."
            },
            {
                CodeType.Code49,
                "Code 49 is a 2D high-density stacked barcode. Encodes the complete ASCII character set."
            },
            {
                CodeType.JapanesePostal,
                "This is the barcode used by the Japanese Postal system. " +
        "Encodes alpha and numeric characters consisting of 20 digits including a 7-digit postal code number, optionally followed by block and house number information. " +
        "The data to be encoded can include hyphens."
            },
            {
                CodeType.Pdf417,
                "Pdf417 is a popular high-density 2-dimensional symbology that encodes up to 1108 bytes of information. " +
        "This barcode consists of a stacked set of smaller barcodes. " +
        " Encodes the full ASCII character set. Capable of encoding as many as 2725 data characters."
            },
            {
                CodeType.EAN128FNC1,
                "EAN128FNC1 is a UCC/EAN-128 (EAN128) type barcode that allows you to insert FNC1 character at any place and adjust the bar size etc, which is not available in UCC/EAN-128. " +
        "To insert FNC1 character, set “\n” for C#, or “vbLf” for VB to Text property at runtime."
            },
            {
                CodeType.RSS14,
                "RSS14 is a Reduced Space Symbology that encodes Composite Component (CC) extended EAN and UPC information in less space. " +
        "This version is a 14-digit EAN.UCC item identification for use with omnidirectional point-of-sale scanners."
            },
            {
                CodeType.RSS14Truncated,
                "RSS14Truncated is a Reduced Space Symbology that encodes Composite Component (CC) extended EAN and UPC information in less space. " +
        "This version is a 14-digit EAN.UCC item identification plus Indicator digits for use on small items, not for point-of-sale scanners."
            },
            {
                CodeType.RSS14Stacked,
                " RSS14Stacked is a Reduced Space Symbology that encodes Composite Component (CC) extended EAN and UPC information in less space. " +
        "This version is the same as RSS14Truncated, but stacked in two rows when RSS14Truncated is too wide."
            },
            {
                CodeType.RSS14StackedOmnidirectional,
                "RSS14StackedOmnidirectional is a Reduced Space Symbology that encodes Composite Component (CC) extended EAN and UPC information in less space. " +
        "This version is the same as RSS14, but stacked in two rows when RSS14 is too wide."
            },
            {
                CodeType.RSSExpanded,
                "RSSExpanded is a Reduced Space Symbology that encodes Composite Component (CC) extended EAN and UPC information in less space. " +
        "This version is a 14-digit EAN.UCC item identification plus AI element strings (expiration date, weight, etc.) for use with omnidirectional point-of-sale scanners."
            },
            {
                CodeType.RSSExpandedStacked,
                "RSSExpandedStacked is a Reduced Space Symbology that encodes Composite Component (CC) extended EAN and UPC information in less space. " +
        "This version is the same as RSSExpanded, but stacked in two rows when RSSExpanded is too wide."
            },
            {
                CodeType.RSSLimited,
                "RSS Limited is a Reduced Space Symbology that encodes Composite Component (CC) extended EAN and UPC information in less space. " +
        "This version is a 14-digit EAN.UCC item identification with indicator digits of 0 or 1 in a small symbol that is not scanned by point-of-sale scanners."
            },
            {
                CodeType.DataMatrix,
                "Data Matrix is a high density, two-dimensional barcode with square modules arranged in a square or rectangular matrix pattern."
            },
            {
                CodeType.MicroPDF417,
                "MicroPDF417 is two-dimensional (2D), multi-row symbology, derived from PDF417. " +
        "Micro-PDF417 is designed for applications that need to encode data in a two-dimensional (2D) symbol (up to 150 bytes, 250 alphanumeric characters, or 366 numeric digits) with the minimal symbol size."
            },
            {
                CodeType.MicroQRCode,
                "Micro QR Code is a variant of QR Code 2005. " +
        "Compared with other regular QR Codes, it has only one position detection pattern which reduces the barcode size so that it can be used to applications where the space for barcode image is severely restricted."
            },
            {
                CodeType.IntelligentMail,
                "ITF14 barcode is the GS1 implementation of an Interleaved 2 of 5 bar code to encode a Global Trade Item  Number. " +
        "It is continuous, self-checking, bidirectionally decodable and it will always encode 14 digits.  ITF-14 is used on packaging levels of a product in general. "
            },
            {
                CodeType.ITF14,
                "Code 11, also known as USD-8, is a high-density barcode symbology developed by Intermec in 1977 to numeric digits. " +
        "It was primarily used to label telecommunication equipments. This symbology is discrete and is able to encode numeric digits through 0-9, dash (-), and start/stop characters."
            },
            {
                CodeType.Code11,
                "Serial Shipping Container Code-18 (SSCC-18) Barcode. " +
        "A type of barcode that can print in the lower 2-inch (or local equivalent) extended area of the Thermal 4\" x 8\" or 4\" x 8¼\" (or local equivalent) label."
            },
            {
                CodeType.SSCC18,
                "Telepen is a name of a barcode symbology designed in 1972 in the UK to express all 128 ASCII characters without using shift characters for code switching, and using only two different widths for bars and spaces."
            },
            {
                CodeType.Telepen,
                "Pharmacode, also known as Pharmaceutical Binary Code, is a barcode standard, used in the pharmaceutical industry as a packing control system."
            },
            {
                CodeType.Pharmacode,
                "PZN or Pharma-Zentral-Nummer is a barcode standard used in the German pharmaceutical industry for designating medicines and health-care products."
            },
            {
                CodeType.PZN,
                "Health Industry Bar Code 128 implementation."
            },
            {
                CodeType.HIBCCode128,
                "Health Industry Bar Code 39 implementation."
            },
            {
                CodeType.HIBCCode39,
                "The International Standard Book Number (ISBN) is special commercial book identifier which encodes 9 numeric digits apart from the start number \"978\", \"979\"."
            },
            {
                CodeType.ISBN,
                "The International Standard Serial Number (ISSN) is an eight-digit number used for printed or electronic periodical publications like magazines, etc. " +
        "This ISSN system was drafted as an International Standard in 1971 and published as ISO 3297 in 1975."
            },
            {
                CodeType.ISSN,
                "The International Standard Music Number or ISMN (ISO 10957) is a thirteen-character alphanumeric identifier for printed music developed by ISO."
            },
            {
                CodeType.ISMN,
                "Represents an IATA 2 of 5 barcode"
            },
            {
                CodeType.Iata25,
                "The BC412 barcode was invented by IBM to meet the needs of the semiconductor wafer identification application."
            },
            {
                CodeType.Bc412,
                "MSI barcode, also known as Modified Plessey, is a numeric symbology developed by the MSI Data Corporation, which is used primarily for marking retail shelves for inventory control. " +
        "Though continuous and self-checking, MSI Plessey provides several module checksum situations."
            },
            {
                CodeType.Plessey,
                "Intelligent Mail Package Barcode."
            },
            {
                CodeType.IntelligentMailPackage,
                ""
            }
        };

        public DataVisualization()
        {
            DataContext = this;
            InitializeComponent();

            sparkline.Data = new List<double>() { 1.0, -2.0, -1.0, 6.0, 4.0, -4.0, 3.0, 8.0 };
            sparkline.ShowMarkers = true;
            sparkline.DisplayXAxis = true;

            pieChart.ItemsSource = Person.Generate(80);

            var keys = BarCodes.Keys.ToList();
            BarcodeType.ItemsSource = keys;
            BarcodeType.SelectedIndex = 0;
            BarCodeTypeDescription.Text = BarCodes[BarCodes.Keys.First()];
            BarcodeText.Text = "9012345678";
            BarCode.Text = "9012345678";

        }

        private void BarcodeType_SelectedIndexChanged(object sender, C1.WPF.Core.PropertyChangedEventArgs<int> e)
        {
            var selectedItem = BarcodeType.SelectedItem;
            if (selectedItem != null)
            {
                var codeType = (CodeType)selectedItem;
                BarCodeTypeDescription.Text = BarCodes[codeType];
            }
        }

        private void GenerateBarCodeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedItem = BarcodeType.SelectedItem;
            if (selectedItem == null || string.IsNullOrEmpty(BarcodeText.Text))
            {
                MessageBox.Show("A Bard Code type needs to be selected and a Bar Code text must be provided.");
                return;
            }
            BarCode.CodeType = (CodeType)selectedItem;
            BarCode.Text = BarcodeText.Text;
        }
        public List<DataItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = new List<DataItem>();
                    var dateStep = 0;
                    for (var i = 0; i < npts; i++)
                    {
                        var date = DateTime.Today.AddDays(dateStep += 9);
                        _data.Add(new DataItem()
                        {
                            Downloads = date.Month == 4 || date.Month == 8 ? (int?)null : rnd.Next(10, 20),
                            Sales = date.Month == 4 || date.Month == 8 ? (int?)null : rnd.Next(0, 10),
                            Date = date
                        });
                    }
                }

                return _data;
            }
        }

        /// <summary>
        /// Gets or sets Value property for gauges.
        /// </summary>
        public double GaugeValue
        {
            get => (double)GetValue(GaugeValueProperty);
            set => SetValue(GaugeValueProperty, value);
        }
        /// <summary>
        /// Identifies GaugeValue dependency property.
        /// </summary>
        public static readonly DependencyProperty GaugeValueProperty =
            DependencyProperty.Register(
                "GaugeValue",
                typeof(double),
                typeof(DataVisualization),
                new FrameworkPropertyMetadata(25.0));

        /// <summary>
        /// Gets or sets a value indicating whether user interaction is allowed for gauges.
        /// </summary>
        public bool IsGaugeReadOnly
        {
            get => (bool)GetValue(IsGaugeReadOnlyProperty);
            set => SetValue(IsGaugeReadOnlyProperty, value);
        }
        /// <summary>
        /// Identifies IsGaugeReadOnly dependency property.
        /// </summary>
        public static readonly DependencyProperty IsGaugeReadOnlyProperty =
            DependencyProperty.Register(
                "IsGaugeReadOnly",
                typeof(bool),
                typeof(DataVisualization),
                new FrameworkPropertyMetadata(false));
    }

    /// <summary>
    /// Converter for IsGaugeReadOnly property.
    /// </summary>
    public class IsReadOnlyConverter : IValueConverter
    {
        /// <inheritdoc cref="IValueConverter.Convert(object, Type, object, CultureInfo)"/>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
        /// <inheritdoc cref="IValueConverter.ConvertBack(object, Type, object, CultureInfo)"/>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }

    public class DataItem
    {
        public int? Downloads { get; set; }
        public int? Sales { get; set; }
        public DateTime Date { get; set; }
    }
}