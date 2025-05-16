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
using Microsoft.Win32;
using System.Reflection;

namespace PdfViewerSamples
{
    /// <summary>
    /// Interaction logic for AnnotationDemo.xaml
    /// </summary>
    public partial class AnnotationDemo : UserControl
    {
        public AnnotationDemo()
        {
            InitializeComponent();

            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/CorporateProfile.pdf", UriKind.Relative));
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
