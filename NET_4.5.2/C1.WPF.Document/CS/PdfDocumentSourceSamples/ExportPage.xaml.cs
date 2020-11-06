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

using C1.WPF;
using C1.WPF.Document;
using C1.WPF.Document.Export;

namespace PdfDocumentSourceSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ExportPage : UserControl
    {
        C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public ExportPage()
        {
            InitializeComponent();

            var supportedExportProviders = pdfDocumentSource.SupportedExportProviders;
            foreach (var sep in supportedExportProviders)
                cbExportFilter.Items.Add(new C1ComboBoxItem() { Content = sep.FormatName, Tag = sep });
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
                        using (Stream stream = this.GetType().Assembly.GetManifestResourceStream(@"PdfDocumentSourceSamples.Resources.DefaultDocument.pdf"))
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
            ExportProvider ep = (ExportProvider)((C1ComboBoxItem)cbExportFilter.SelectedItem).Tag;
            saveFileDialog.DefaultExt = "." + ep.DefaultExtension;
            saveFileDialog.FileName = (fpFile.SelectedFile == null ? "DefaultDocument" : System.IO.Path.GetFileName(fpFile.SelectedFile.FullName)) + "." + ep.DefaultExtension;
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
                MessageBox.Show("Document was successfully exported.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
