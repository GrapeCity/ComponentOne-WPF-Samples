using C1.Document;
using C1.WPF.Document.Export;
using C1.WPF.Input;
using DocumentExplorer.Resources;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DocumentExplorer
{
    public partial class Export : UserControl
    {
        C1.WPF.Document.C1PdfDocumentSource pdfDocumentSource = new ();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public Export()
        {
            InitializeComponent();

            Tag = AppResources.ExportDesc;

            var supportedExportProviders = pdfDocumentSource.SupportedExportProviders;
            cbExportFilter.ItemsSource = supportedExportProviders;
            cbExportFilter.SelectedIndex = 0;

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            // load document
            while (true)
            {
                try
                {
                    if (fpFile.SelectedFile == null)
                    {
                        using (Stream stream = this.GetType().Assembly.GetManifestResourceStream(@"DocumentExplorer.Resources.DefaultDocument.pdf"))
                            pdfDocumentSource.LoadFromStream(stream);
                    }
                    else
                        pdfDocumentSource.LoadFromFile(fpFile.SelectedFile.FullName);
                    break;
                }
                catch (PdfPasswordException)
                {
                    var password = PasswordWindow.DoEnterPassword(fpFile.SelectedFile.FullName);
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

            //
            OutputRange outputRange;
            if (rbtnPagesAll.IsChecked.Value)
                outputRange = OutputRange.All;
            else
            {
                if (!OutputRange.TryParse(tbPagesRange.Text, out outputRange))
                {
                    MessageBox.Show("Invalid range of pages.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // execute action
            var ep = (C1.Document.Export.ExportProvider)cbExportFilter.SelectedValue;
            saveFileDialog.DefaultExt = "." + ep.DefaultExtension;
            saveFileDialog.FileName = (fpFile.SelectedFile == null ? "DefaultDocument" : System.IO.Path.GetFileNameWithoutExtension(fpFile.SelectedFile.FullName)) + "." + ep.DefaultExtension;
            saveFileDialog.Filter = String.Format("{0} (*.{1})|*.{1}|All files (*.*)|*.*", ep.FormatName, ep.DefaultExtension);
            bool? dr = saveFileDialog.ShowDialog();
            if (!dr.HasValue || !dr.Value)
                return;

            try
            {
                var exporter = ep.NewExporter();
                exporter.ShowOptions = false;
                exporter.Preview = true;
                exporter.FileName = saveFileDialog.FileName;
                exporter.Range = outputRange;
                pdfDocumentSource.Export(exporter);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
