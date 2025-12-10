using C1.Document;
using C1.WPF.Input;
using DocumentExplorer.Resources;
using System;
using System.IO;
using System.Printing;
using System.Windows;
using System.Windows.Controls;

namespace DocumentExplorer
{
    public partial class Print : UserControl
    {
        C1.WPF.Document.C1PdfDocumentSource pdfDocumentSource = new C1.WPF.Document.C1PdfDocumentSource();
        PrintDialog printDialog = new PrintDialog() { UserPageRangeEnabled = true };

        public Print()
        {
            InitializeComponent();
            Tag = AppResources.PrintDesc;

            using (LocalPrintServer lps = new LocalPrintServer())
            {
                PrintQueueCollection pqc = lps.GetPrintQueues(new EnumeratedPrintQueueTypes[] { EnumeratedPrintQueueTypes.Connections, EnumeratedPrintQueueTypes.Local });
                cbPrinter.ItemsSource = pqc;
            }
            cbPrinter.SelectedIndex = 0;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (cbPrinter.SelectedItem == null)
            {
                MessageBox.Show("Please select a printer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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

            try
            {
                C1.WPF.Document.C1PrintOptions po = new ();
                po.PrintQueue = (PrintQueue)cbPrinter.SelectedItem;
                po.OutputRange = outputRange;
                pdfDocumentSource.Print(po);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnPrinterSettings_Click(object sender, RoutedEventArgs e)
        {
            if (cbPrinter.SelectedItem == null)
                return;

            PrintQueue pq = (PrintQueue)cbPrinter.SelectedItem;
            printDialog.PrintQueue = pq;
            if (!printDialog.ShowDialog().Value)
                return;

            if (printDialog.PageRangeSelection == PageRangeSelection.AllPages)
                rbtnPagesAll.IsChecked = true;
            else
            {
                rbtnPagesRange.IsChecked = true;
                tbPagesRange.Text = string.Format("{0} - {1}", printDialog.PageRange.PageFrom, printDialog.PageRange.PageTo);
            }
        }
    }
}
