using C1.WPF.BarCode;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BarCodeExplorer
{
    /// <summary>
    /// Interaction logic for AccordionSample.xaml
    /// </summary>
    public partial class Demo : UserControl
    {
        public Demo()
        {
            InitializeComponent();
            Tag = Properties.Resources.DemoDesc;
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
    }
}
