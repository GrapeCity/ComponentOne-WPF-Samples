using C1.BarCode;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using C1.WPF.BarCode;

namespace BarCodesSample
{
    /// <summary>
    /// Interaction logic for DemoPage.xaml
    /// </summary>
    public partial class DemoPage : UserControl
    {
        public DemoPage()
        {
            InitializeComponent();
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
