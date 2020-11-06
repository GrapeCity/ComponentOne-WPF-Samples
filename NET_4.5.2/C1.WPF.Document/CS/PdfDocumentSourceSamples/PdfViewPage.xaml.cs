using System;
using System.IO;
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

using C1.WPF.Document;

namespace PdfDocumentSourceSamples
{
    /// <summary>
    /// Interaction logic for PdfViewPage.xaml
    /// </summary>
    public partial class PdfViewPage : UserControl
    {
        C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Title = "Select the PDF file",
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
        };

        public PdfViewPage()
        {
            InitializeComponent();

            fv.OpenAction += FlexViewer_OpenAction;
            fv.CloseAction += FlexViewer_CloseAction;

            fv.DocumentSource = pdfDocumentSource;
            Stream stream = this.GetType().Assembly.GetManifestResourceStream(@"PdfDocumentSourceSamples.Resources.DefaultDocument.pdf");
            pdfDocumentSource.LoadFromStream(stream);
        }

        private void FlexViewer_OpenAction(object sender, EventArgs e)
        {
            fv.FocusPane();

            if (!openFileDialog.ShowDialog().Value)
                return;

            // load document
            while (true)
            {
                try
                {
                    pdfDocumentSource.LoadFromFile(openFileDialog.FileName);
                    break;
                }
                catch (PdfPasswordException)
                {
                    var password = PasswordWindow.DoEnterPassword(openFileDialog.FileName);
                    if (password == null)
                        return;
                    pdfDocumentSource.Credential.Password = password;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            fv.CloseActionEnabled = true;
        }

        private void FlexViewer_CloseAction(object sender, EventArgs e)
        {
            pdfDocumentSource.ClearContent();
            fv.CloseActionEnabled = false;
            fv.FocusPane();
        }
    }
}
