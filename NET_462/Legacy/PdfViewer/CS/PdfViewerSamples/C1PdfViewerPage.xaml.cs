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
using C1.WPF;
using System.Reflection;
using System.IO;
using System.Data;
using System.ComponentModel;
using Microsoft.Win32;
using System.Diagnostics;

namespace PdfViewerSamples
{
    /// <summary>
    /// Interaction logic for DemoPdfViewer.xaml
    /// </summary>
    public partial class DemoPdfViewer : UserControl
    {
        public DemoPdfViewer()
        {
            InitializeComponent();
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/C1XapOptimizer.pdf", UriKind.Relative));
            pdfViewer.LoadDocument(resource.Stream);
            // Add navigate link event;
            this.pdfViewer.RequestNavigate += pdfViewer_RequestNavigate;
        }

        void pdfViewer_RequestNavigate(object sender, C1.WPF.PdfViewer.RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.ToString());
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
