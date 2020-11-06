using System;
using System.IO;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Win32;
using C1.WPF.Chart;

namespace FlexChartExplorer
{
    public sealed partial class ImageExport : UserControl
    {
        int cnt = 100;
        Random rnd = new Random();
        string[] coef = new string[]
        {
          "AMTMNQQXUYGA",
          "CVQKGHQTPHTE",
          "FIRCDERRPVLD",
          "GIIETPIQRRUL",
          "GLXOESFTTPSV",
          "GXQSNSKEECTX",
        };

        public ImageExport()
        {
            this.InitializeComponent();
            this.Loaded += ImageExport_Loaded;
        }

        private void ImageExport_Loaded(object sender, RoutedEventArgs e)
        {
            CreateData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateData();
        }

        void CreateData()
        {
            flexChart.BeginUpdate();
            flexChart.ItemsSource = DataCreator.Create(coef, cnt);
            flexChart.EndUpdate();
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog()
            {
                Filter = "PNG|*.png|JPEG |*.jpeg|BMP|*.bmp|SVG|*.svg"
            };
            if (dialog.ShowDialog() == true)
            {
                using (Stream stream = dialog.OpenFile())
                {
                    var extension = dialog.SafeFileName.Split('.')[1];
                    ImageFormat fmt = (ImageFormat)Enum.Parse(typeof(ImageFormat), extension, true);
                    flexChart.SaveImage(stream, fmt);
                }
            }
        }
    }
}
