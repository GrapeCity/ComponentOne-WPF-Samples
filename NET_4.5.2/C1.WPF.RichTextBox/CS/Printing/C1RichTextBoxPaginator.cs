using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;

namespace C1.WPF.RichTextBox
{
    /// <summary>
    /// Helper class used for printing
    /// </summary>
    public class C1RichTextBoxPaginator 
        : DocumentPaginator
    {
        Thickness _margin = new Thickness();

        public C1RichTextBoxPaginator(C1RichTextBox rtb, Size pageSize, Thickness margin)
        {
            var mgr = new C1RichTextViewManager { Document = rtb.Document };

            // PageSize, Margin and ContentSize
            PageSize = pageSize;
            _margin = margin;
            var contentSize = new Size(PageSize.Width - margin.Left - margin.Right, PageSize.Height - margin.Top - margin.Bottom);

            // print the pages outside the visual tree
            _pages = mgr.OfflinePaint(contentSize).ToList();
        }

        List<UIElement> _pages;
        public override DocumentPage GetPage(int pageNumber)
        {
            var page = _pages[pageNumber];
            
            var container = new Border();
            container.Padding = _margin;
            container.Child = page;
            container.Measure(PageSize);
            container.Arrange(new Rect(PageSize));

            return new DocumentPage(container);
        }

        public override bool IsPageCountValid
        {
            get { return true; }
        }

        public override int PageCount
        {
            get { return _pages.Count; }
        }

        public override Size PageSize { get; set; }

        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }
    }
}
