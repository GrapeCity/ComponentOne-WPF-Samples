using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using C1.WPF.FlexGrid;

namespace MultiGridPrinting
{
    /// <summary>
    /// <see cref="DocumentPaginator"/> class used to render <see cref="C1FlexGrid"/> controls.
    /// </summary>
    /// <remarks>
    /// <para>This class is based on the following article:</para>
    /// <para>http://www.switchonthecode.com/tutorials/wpf-printing-part-2-pagination</para>
    /// </remarks>
    public class FlexPaginator : DocumentPaginator
    {
        Thickness _margin;
        Size _pageSize;
        ScaleMode _scaleMode;
        List<FrameworkElement> _pages;

        public FlexPaginator(C1FlexGrid flex, ScaleMode scaleMode, Size pageSize, Thickness margin, int maxPages)
        {
            // save parameters
            _margin = margin;
            _scaleMode = scaleMode;
            _pageSize = pageSize;

            // adjust page size for margins before building grid images
            pageSize.Width -= (margin.Left + margin.Right);
            pageSize.Height -= (margin.Top + margin.Bottom);

            // get grid images for each page
            _pages = flex.GetPageImages(scaleMode, pageSize, maxPages);
            
        }
        //Adding pages to the list of framework elements
        public void AddPages(C1FlexGrid flex)
        {
            
            _pages.AddRange(flex.GetPageImages(_scaleMode, _pageSize, 100));
            
        }
        public override DocumentPage GetPage(int pageNumber)
        {
            // create page element
            var pageTemplate = new PageTemplate();

            // set margins
            pageTemplate.SetPageMargin(_margin);

            // set content
            pageTemplate.PageContent.Child = _pages[pageNumber];
            pageTemplate.PageContent.Stretch = _scaleMode == ScaleMode.ActualSize
                ? System.Windows.Media.Stretch.None
                : System.Windows.Media.Stretch.Uniform;

            // set footer text
            pageTemplate.FooterRight.Text = string.Format("Page {0} of {1}",
                pageNumber + 1, _pages.Count);

            // arrange the elements on the page
            pageTemplate.Arrange(new Rect(0, 0, _pageSize.Width, _pageSize.Height));

            // return new document page
            return new DocumentPage(pageTemplate);
        }
        public override int PageCount
        {
            get { return _pages.Count; }
        }
        public override IDocumentPaginatorSource Source
        {
            get { return null; }
        }
        public override Size PageSize
        {
            get { return _pageSize; }
            set { throw new NotImplementedException(); }
        }
        public override bool IsPageCountValid
        {
            get { return true; }
        }
    }
}
