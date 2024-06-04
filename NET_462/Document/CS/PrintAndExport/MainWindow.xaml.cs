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

namespace PrintAndExport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
        PrintDialog printDialog = new PrintDialog() { UserPageRangeEnabled = true };
        SaveFileDialog saveFileDialog = new SaveFileDialog();

        public MainWindow()
        {
            InitializeComponent();

            fpFile.SelectedFile = new System.IO.FileInfo(@"..\..\DefaultDocument.pdf");

            cbAction.Items.Add(new C1ComboBoxItem() { Content = "Print..." });
            var supportedExportProviders = pdfDocumentSource.SupportedExportProviders;
            foreach (var sep in supportedExportProviders)
                cbAction.Items.Add(new C1ComboBoxItem() { Content = String.Format("Export to {0}...", sep.FormatName), Tag = sep });
            cbAction.SelectedIndex = 0;
        }

        private void DoPrint(C1PdfDocumentSource pds)
        {
            printDialog.MaxPage = (uint)pds.PageCount;
            bool? dr = printDialog.ShowDialog();
            if (!dr.HasValue || !dr.Value)
                return;

            try
            {
                var po = new C1PrintOptions();
                po.PrintQueue = printDialog.PrintQueue;
                po.PrintTicket = printDialog.PrintTicket;
                if (printDialog.PageRangeSelection == PageRangeSelection.UserPages)
                    po.OutputRange = new OutputRange(printDialog.PageRange.PageFrom, printDialog.PageRange.PageTo);
                pds.Print(po);
                MessageBox.Show(this, "Document was successfully printed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DoExport(C1PdfDocumentSource pds, ExportProvider ep)
        {
            saveFileDialog.DefaultExt = "." + ep.DefaultExtension;
            saveFileDialog.FileName = System.IO.Path.GetFileNameWithoutExtension(fpFile.SelectedFile.FullName) + "." + ep.DefaultExtension;
            saveFileDialog.Filter = String.Format("{0} (*.{1})|*.{1}|All files (*.*)|*.*", ep.FormatName, ep.DefaultExtension);
            bool? dr = saveFileDialog.ShowDialog(this);
            if (!dr.HasValue || !dr.Value)
                return;

            try
            {
                var exporter = ep.NewExporter();
                exporter.ShowOptions = false;
                exporter.Preview = true;
                exporter.FileName = saveFileDialog.FileName;
                pds.Export(exporter);
                MessageBox.Show(this, "Document was successfully exported.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            if (fpFile.SelectedFile == null)
            {
                MessageBox.Show(this, "Please select a PDF file.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // load document
            while (true)
            {
                try
                {
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
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // execute action
            if (cbAction.SelectedIndex == 0)
                DoPrint(pdfDocumentSource);
            else
                DoExport(pdfDocumentSource, (ExportProvider)((C1ComboBoxItem)cbAction.SelectedItem).Tag);
        }
    }
}
