using C1.WPF.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Printing
{
    /// <summary>
    /// Interaction logic for PrintingControl.xaml
    /// </summary>
    public partial class PrintingControl : UserControl
    {
        ICollectionView _data = Product.GetProducts(100);
        /// <summary>
        /// 
        /// </summary>
        public PrintingControl()
        {
            InitializeComponent();
            Tag = Properties.Resources.Tag;

            // populate grid
            _flex.ItemsSource = _data;
            _flex.Columns["Line"].AllowMerging = true;
            _flex.Columns["Color"].AllowMerging = true;

            // for testing
            _flex.AllowResizing = GridAllowResizing.Both;
        }

        #region ** event handlers

        // basic printing
        void _btnBasicPrint_Click(object sender, RoutedEventArgs e)
        {
            // get margins, scale mode
            var margin =
                _cmbMargins.SelectedIndex == 0 ? 96.0 / 4 :
                _cmbMargins.SelectedIndex == 1 ? 96.0 / 2 :
                96.0;
            var scaleMode =
                _cmbZoom.SelectedIndex == 0 ? GridScaleMode.ActualSize :
                _cmbZoom.SelectedIndex == 1 ? GridScaleMode.PageWidth :
                _cmbZoom.SelectedIndex == 2 ? GridScaleMode.SinglePage : GridScaleMode.Selection;
            bool showPrintDialog = _chkShowPrintDialog.IsChecked.HasValue && _chkShowPrintDialog.IsChecked.Value;

            if (_cmbOrientation.SelectedIndex > 0 || showPrintDialog)
            {
                // setup advanced print parameters according to user selection
                var pp = new GridPrintParameters();
                pp.Margin = new Thickness(margin);
                pp.ScaleMode = scaleMode;
                pp.MaxPages = 20;
                pp.ShowPrintDialog = showPrintDialog;
                pp.ShowPrintPreview = _chkPrintPreview.IsChecked.HasValue && _chkPrintPreview.IsChecked.Value;
                if (_cmbOrientation.SelectedIndex > 0)
                {
                    pp.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
                    pp.PrintTicket = pp.PrintQueue.DefaultPrintTicket;
                    pp.PrintTicket.PageOrientation = _cmbOrientation.SelectedIndex == 1 ?
                        System.Printing.PageOrientation.Portrait : System.Printing.PageOrientation.Landscape;
                }
                pp.DocumentName = Properties.Resources.PrintingTitle;
                // print the grid
                _flex.Print(pp);
            }
            else
            {
                // print the grid with default printing options and showing PrintDialog
                _flex.Print("FlexGrid printing example", scaleMode, new Thickness(margin), 20);
            }
        }

        // advanced printing
        void _btnAdvancedPrint_Click(object sender, RoutedEventArgs e)
        {
            // get margins, scale mode
            var margin =
                _cmbMargins.SelectedIndex == 0 ? 96.0 / 4 :
                _cmbMargins.SelectedIndex == 1 ? 96.0 / 2 :
                96.0;
            var scaleMode =
                _cmbZoom.SelectedIndex == 0 ? GridScaleMode.ActualSize :
                _cmbZoom.SelectedIndex == 1 ? GridScaleMode.PageWidth :
                _cmbZoom.SelectedIndex == 2 ? GridScaleMode.SinglePage : GridScaleMode.Selection;

            var pd = new PrintDialog();
            if (_cmbOrientation.SelectedIndex > 0)
            {
                // setup PrintDialog parameters according to user selection
                pd.PrintQueue = System.Printing.LocalPrintServer.GetDefaultPrintQueue();
                pd.PrintTicket = pd.PrintQueue.DefaultPrintTicket;
                pd.PrintTicket.PageOrientation = _cmbOrientation.SelectedIndex == 1 ?
                    System.Printing.PageOrientation.Portrait : System.Printing.PageOrientation.Landscape;
            }

            bool showPrintDialog = _chkShowPrintDialog.IsChecked.Value;
            if (!showPrintDialog || pd.ShowDialog().Value)
            {
                // calculate page size
                var sz = new Size(pd.PrintableAreaWidth, pd.PrintableAreaHeight);

                // create paginator
                var paginator = new FlexPaginator(_flex, scaleMode, sz, new Thickness(margin), 100);
                pd.PrintDocument(paginator, "C1FlexGrid printing example");

                _flex.DisposePageImages();
            }
        }

        // toggle grouping
        void _chkGroup_Click(object sender, RoutedEventArgs e)
        {
            //using (_data.DeferRefresh())
            //{
            //    var gd = _data.GroupDescriptions;
            //    gd.Clear();
            //    if (_chkGroup.IsChecked.Value)
            //    {
            //        _data.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Line"));
            //        _data.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Color"));
            //    }
            //}
        }

        // toggle merging
        void _chkMerge_Click(object sender, RoutedEventArgs e)
        {
            _flex.AllowMerging = _chkMerge.IsChecked.Value ? GridAllowMerging.Cells : GridAllowMerging.None;
        }

        // toggle freezing
        void _chkFreeze_Click(object sender, RoutedEventArgs e)
        {
            if (_chkFreeze.IsChecked.Value)
            {
                var sel = _flex.Selection ?? new GridCellRange(0, 0);

                _flex.FrozenRows = Math.Min(12, sel.Row);
                _flex.FrozenColumns = Math.Min(2, sel.Column); ;
            }
            else
            {
                _flex.FrozenRows = 0;
                _flex.FrozenColumns = 0;
            }
        }
        #endregion

        #region ** printing

        /// <summary>
        /// <see cref="DocumentPaginator"/> class used to render <see cref="FlexGrid"/> controls.
        /// </summary>
        /// <remarks>
        /// <para>This class is based on the following article:</para>
        /// <para>http://www.switchonthecode.com/tutorials/wpf-printing-part-2-pagination</para>
        /// </remarks>
        public class FlexPaginator : DocumentPaginator
        {
            Thickness _margin;
            Size _pageSize;
            GridScaleMode _scaleMode;
            List<FrameworkElement> _pages;

            public FlexPaginator(FlexGrid flex, GridScaleMode scaleMode, Size pageSize, Thickness margin, int maxPages)
            {
                // save parameters
                _margin = margin;
                _scaleMode = scaleMode;
                _pageSize = pageSize;

                // adjust page size for margins before building grid images
                ContentSize = new Size(PageSize.Width - margin.Left - margin.Right, PageSize.Height - margin.Top - margin.Bottom);
                ContentLocation = new Point(margin.Left, margin.Top);

                // get grid images for each page
                _pages = flex.GetPageImages(scaleMode, ContentSize, maxPages);
            }

            public override DocumentPage GetPage(int pageNumber)
            {
                // create page element
                var pageTemplate = new PageTemplate();

                // set margins
                pageTemplate.SetPageMargin(_margin);

                // set content
                pageTemplate.PageContent.Child = _pages[pageNumber];
                pageTemplate.PageContent.Stretch = Stretch.None;

                // set footer text
                pageTemplate.FooterRight.Text = string.Format("Page {0} of {1}",
                    pageNumber + 1, _pages.Count);

                // arrange the elements on the page
                pageTemplate.Arrange(new Rect(0, 0, _pageSize.Width, _pageSize.Height));

                // return new document page
                return new DocumentPage(pageTemplate, _pageSize,
                   new Rect(ContentLocation, ContentSize), new Rect(ContentLocation, ContentSize));
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
            /// <summary>
            /// Gets or sets size which is used to layout page content.
            /// </summary>
            public Size ContentSize
            {
                get; private set;
            }
            /// <summary>
            ///  Gets or sets content location.
            /// </summary>
            public Point ContentLocation
            {
                get; private set;
            }
        }

        #endregion
    }

}
