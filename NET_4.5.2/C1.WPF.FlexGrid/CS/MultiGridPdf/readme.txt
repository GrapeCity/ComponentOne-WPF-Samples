MultiGridPdf
------------------------------------------------------------------------------------
Shows how you to create a PDF document that contains multiple C1FlexGrid controls.

The sample uses a helper class called GridExport that renders a C1FlexGrid into
C1Pdf documents.

The GridExport class has a SavePdf method that renders a FlexGrid directly into 
a PDF stream. This method is very easy to use.

It also has a RenderGrid method that renders a FlexGrid into an existing C1PdfDocument. 
This second method allows you to combine multiple types of content into a single
C1PdfDocument. For example, you may add several grids to the document, then combine
that with charts, text, images, and other UIElement objects.

The code below shows how you can use the GridExport class to render two grids into 
a single PDF file:

<code>
     var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "PDF files (*.pdf)|*.pdf";
           // var t = dlg.ShowDialog();
            if (dlg.ShowDialog()==true)
             {
                 // create pdf document
                 var pdf = new C1PdfDocument();

                 // render all grids into pdf document
                 var options = new PdfExportOptions();
                 options.ScaleMode = ScaleMode.ActualSize;
                 GridExport.RenderGrid(pdf, _flex1, options);
                 pdf.NewPage();
                 GridExport.RenderGrid(pdf, _flex2, options);
                 pdf.NewPage();
                 GridExport.RenderGrid(pdf, _flex3, options);
                 pdf.NewPage();
                 GridExport.RenderGrid(pdf, _flex4, options);

                 // save document
                 using (var stream = dlg.OpenFile())
                 {
                     pdf.Save(stream);
                 }
</code>

Note that the GridExport source code is included in this sample. If the class doesn't
fit your needs, you can customize it.
