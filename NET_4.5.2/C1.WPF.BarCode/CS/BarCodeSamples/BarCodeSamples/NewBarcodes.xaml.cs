using C1.BarCode;
using C1.WPF.BarCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BarCodesSample
{
    /// <summary>
    /// Interaction logic for NewBarcode.xaml
    /// </summary>
    public partial class NewBarcode : UserControl
    {
        List<CodeType> _barcodes = new List<CodeType>
        {
            CodeType.Bc412, CodeType.Code11, CodeType.HIBCCode128, CodeType.HIBCCode39, CodeType.Iata25, CodeType.IntelligentMailPackage, CodeType.ISBN, CodeType.ISMN, CodeType.ISSN, CodeType.ITF14, CodeType.MicroQRCode, CodeType.Pharmacode, CodeType.Plessey, CodeType.PZN, CodeType.SSCC18, CodeType.Telepen
        };
        public NewBarcode()
        {
            InitializeComponent();
            Loaded += NewBarcode_Loaded;
        }

        private void NewBarcode_Loaded(object sender, RoutedEventArgs e)
        {
            BarcodeType.ItemsSource = _barcodes;
            BarcodeType.SelectedIndex = 0;
        }

        private void BarcodeType_SelectedItemChanged(object sender, C1.WPF.PropertyChangedEventArgs<object> e)
        {
            barCode.Text = "";
            CodeType codetype = (CodeType)BarcodeType.SelectedItem;
            barCode.CodeType = codetype;
            // Change the available text for selected barcode type
            switch(codetype)
            {
                case CodeType.HIBCCode128:
                case CodeType.HIBCCode39:
                    BarcodeText.Text = @"A123PROD78905/0123456789DATA";
                    break;
                case CodeType.IntelligentMailPackage:
                    BarcodeText.Text = "9212391234567812345671";
                    break;
                case CodeType.PZN:
                    BarcodeText.Text = "01234562";
                    break;
                case CodeType.Pharmacode:
                    BarcodeText.Text = "131070";
                    break;
                case CodeType.SSCC18:
                    BarcodeText.Text = "1234t5+678912345678";
                    break;
                case CodeType.Bc412:
                    BarcodeText.Text = "AQ1557";
                    break;
                case CodeType.MicroQRCode:
                    BarcodeText.Text = "12345";
                    break;
                case CodeType.Iata25:
                    BarcodeText.Text = "0123456789";
                    break;
                case CodeType.ITF14:
                    BarcodeText.Text = "1234567891011";
                    break;
                default:
                    BarcodeText.Text = "9790123456785";
                    break;
            }
            barCode.Text = BarcodeText.Text;
        }

        private void ExportToImage()
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                DefaultExt = "jpeg",
                Title = "Choose filename and location",
                Filter = "Jpeg files|*.jpeg|Bmp Files|*.bmp|PNG Files|*.png",
                AddExtension = true
            };
            if (saveDialog.ShowDialog() == true)
            {
                var strs = saveDialog.SafeFileName.Split(new char[] { '.' });
                if (strs.Length > 1)
                {
                    var fileExtension = strs[strs.Length - 1];
                    using (FileStream fstream = File.OpenWrite(saveDialog.FileName))
                    {
                        using (MemoryStream stream = new MemoryStream())
                        {
                            barCode.Save(stream, (ImageFormat)Enum.Parse(typeof(ImageFormat), fileExtension, true));
                            stream.WriteTo(fstream);
                        }
                        fstream.Flush();
                    }
                }

            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            ExportToImage();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(BarcodeText.Text))
            {
                MessageBox.Show("Please type something in Data part!");
                return;
            }
            barCode.Text = BarcodeText.Text;
        }
    }
}
