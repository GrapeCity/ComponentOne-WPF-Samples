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
using System.Printing;

using C1.WPF;
using C1.WPF.Document;
using C1.WPF.Document.Export;

namespace PdfDocumentSourceSamples
{
    /// <summary>
    /// Interaction logic for PrintPage.xaml
    /// </summary>
    public partial class PrintPage : UserControl
    {
        C1PdfDocumentSource pdfDocumentSource = new C1PdfDocumentSource();
        PrintDialog printDialog = new PrintDialog() { UserPageRangeEnabled = true };

        public PrintPage()
        {
            InitializeComponent();

            using (LocalPrintServer lps = new LocalPrintServer())
            {
                PrintQueueCollection pqc = lps.GetPrintQueues(new EnumeratedPrintQueueTypes[] { EnumeratedPrintQueueTypes.Connections, EnumeratedPrintQueueTypes.Local });
                foreach (var pq in pqc)
                {
                    cbPrinter.Items.Add(new C1ComboBoxItem() { Content = pq.FullName, Tag = pq });
                }
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

            try
            {
                C1PrintOptions po = new C1PrintOptions();
                po.PrintQueue = (PrintQueue)((C1ComboBoxItem)cbPrinter.SelectedItem).Tag;
                po.OutputRange = outputRange;
                pdfDocumentSource.Print(po);
                MessageBox.Show("Document was successfully printed.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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

            PrintQueue pq = (PrintQueue)((C1ComboBoxItem)cbPrinter.SelectedItem).Tag;
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
