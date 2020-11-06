using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Microsoft.Win32;

namespace PdfCreator
{
    using C1.WPF.Pdf;
    using C1.WPF.Document;

    public partial class Preview : Window
    {
        C1PdfDocumentSource pdfDocSource = new C1PdfDocumentSource();

        public Preview()
        {
            InitializeComponent();
            btnSave.Click += HandleSaveClick;
            fv.DocumentSource = pdfDocSource;

            this.PdfDocument = new C1PdfDocument();
            var rc = PdfUtils.PageRectangle(this.PdfDocument);
            PdfUtils.RenderParagraph(this.PdfDocument, "Welcome to C1Pdf", new Font("Verdana", 24), rc, rc);
        }

        public void HandleSaveClick(object sender, RoutedEventArgs e)
        {
            // get stream to save to
            var dlg = new SaveFileDialog();
            dlg.Filter = "Pdf Files (*.pdf)|*.pdf";
            dlg.DefaultExt = ".pdf";
            var dr = dlg.ShowDialog();
            if (!dr.HasValue || !dr.Value)
            {
                return;
            }

            // pdf document
            var pdf = this.PdfDocument;

            // save document
            using (var stream = dlg.OpenFile())
            {
                pdf.Save(stream);
            }
            MessageBox.Show("Pdf Document saved to " + dlg.SafeFileName);
        }

        public C1PdfDocument PdfDocument { get; set; }

        public void Show()
        {
            pdfDocSource.LoadFromStream(PdfUtils.SaveToStream(this.PdfDocument));
            base.Title = string.Format("PDF Viewer | {0}", this.PdfDocument.DocumentInfo.Title);
            base.Show();
        }
    }
}
