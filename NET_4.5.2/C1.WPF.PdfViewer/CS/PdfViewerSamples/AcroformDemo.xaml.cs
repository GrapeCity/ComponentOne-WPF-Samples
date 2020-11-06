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
using System.Reflection;
using Microsoft.Win32;

namespace PdfViewerSamples
{
    /// <summary>
    /// Interaction logic for AcroformDemo.xaml
    /// </summary>
    public partial class AcroformDemo : UserControl
    {
        public AcroformDemo()
        {
            InitializeComponent();

            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/C1PdfAcroForm.pdf", UriKind.Relative));
            pdfViewer.LoadDocument(resource.Stream);
        }

        void LoadPdf(object sender, EventArgs args)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "PDF files|*.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                using (var fileStream = dialog.OpenFile())
                {
                    try
                    {
                        pdfViewer.LoadDocument(fileStream);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Could not load PDF file");
                    }
                }
            }
        }

    }
}
