using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using C1.WPF.FlexGrid;

namespace MultiGridPdf
{
    public class PdfExportOptions
    {
        public PdfExportOptions()
        {
            Margin = new Thickness(96);
            ScaleMode = ScaleMode.PageWidth;
            DocumentTitle = "ComponentOne FlexGrid";
        }
        public Thickness Margin { get; set; }
        public ScaleMode ScaleMode { get; set; }
        public string DocumentTitle { get; set; }
        public bool KnownPageCount { get; set; }
    }
}
