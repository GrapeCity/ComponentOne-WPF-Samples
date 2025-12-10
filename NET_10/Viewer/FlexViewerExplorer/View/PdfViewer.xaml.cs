using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using System.Windows;
using FlexViewerExplorer.Resources;
using C1.WPF.Viewer;
using System.IO;
using System.Reflection;
using C1.WPF.Document;
using Microsoft.Win32;

namespace FlexViewerExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PdfViewer : UserControl
    {
        C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Title = "Select the PDF file",
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
        };

        public PdfViewer()
        {
            this.InitializeComponent();

            Tag = AppResources.PdfViewerDesc;

            viewer.DocumentSource = pdfDocumentSource;
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("FlexViewerExplorer.Resources.DefaultDocument.pdf");            
            pdfDocumentSource.LoadFromStream(stream);

            viewer.OpenAction += Viewer_OpenAction;
            viewer.CloseAction += Viewer_CloseAction;
        }

        private void Viewer_CloseAction(object sender, EventArgs e)
        {
            pdfDocumentSource.ClearContent();
            viewer.CloseActionEnabled = false;
            viewer.Focus();
        }

        private void Viewer_OpenAction(object sender, EventArgs e)
        {
            viewer.Focus();

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
                catch (C1.Document.PdfPasswordException)
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

            viewer.CloseActionEnabled = true;
        }
    }
}
