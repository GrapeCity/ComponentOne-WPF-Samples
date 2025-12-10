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

namespace PdfView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
        OpenFileDialog openFileDialog = new OpenFileDialog()
        {
            Title = "Select the PDF file",
            Filter = "PDF Files (*.pdf)|*.pdf|All Files (*.*)|*.*",
        };

        public MainWindow()
        {
            InitializeComponent();

            fv.OpenAction += FlexViewer_OpenAction;
            fv.CloseAction += FlexViewer_CloseAction;
            this.Loaded += MainWindow_Loaded;

            fv.DocumentSource = pdfDocumentSource;
            if (File.Exists(@"..\..\DefaultDocument.pdf"))
                pdfDocumentSource.LoadFromFile(@"..\..\DefaultDocument.pdf");
            else
                fv.CloseActionEnabled = false;
        }

        protected override void OnClosed(EventArgs e)
        {
            fv.DocumentSource = null;
            pdfDocumentSource.Dispose();
            pdfDocumentSource = null;
            base.OnClosed(e);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            fv.FocusPane();
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
                catch (C1.Document.PdfPasswordException)
                {
                    var password = PasswordWindow.DoEnterPassword(openFileDialog.FileName);
                    if (password == null)
                        return;
                    pdfDocumentSource.Credential.Password = password;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
