using System;
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
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using C1.C1Preview;

namespace AutoSizeTable
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            C1PrintDocument doc = new C1PrintDocument();

            RenderText title = new RenderText("Auto-sized tables");
            title.Style.FontSize = 18;
            title.Style.FontBold = true;
            title.Style.Spacing.All = "5mm";
            title.Style.TextAlignHorz = AlignHorzEnum.Center;
            doc.Body.Children.Add(title);

            // add simple autosized table:
            doc.Body.Children.Add(MakeTable_Simple());

            // prepare data source:
            object bookList = MakeBookList();
            // add a bit more interesting data bound table:
            var rt = MakeTable_TitlePriceStock(bookList);
            rt.Style.Spacing.Top = "5mm";
            doc.Body.Children.Add(rt);

            // show the document
            c1DocumentViewer1.Document = doc.FixedDocumentSequence;
        }

        /// <summary>
        /// Creates a simple table with 3 auto-sized columns.
        /// </summary>
        /// <returns>Created table.</returns>
        private RenderTable MakeTable_Simple()
        {
            // create a RenderTable object:
            RenderTable rt = new RenderTable();
            // adjust table's properties so that columns are auto-sized:
            // 1) By default, table width is set to parent (page) width,
            // for auto-sizing we must change it to auto (i.e. based on content):
            rt.Width = Unit.Auto;
            // 2) Set ColumnSizingMode to Auto (default means Fixed for columns):
            rt.ColumnSizingMode = TableSizingModeEnum.Auto;
            // that's it, now the table's columns will be auto-sized.

            // Turn table grid lines on to better see autosizing, add some padding:
            rt.Style.GridLines.All = LineDef.Default;
            rt.CellStyle.Padding.All = "1mm";

            // add some data
            rt.Cells[0, 0].Text = "aaa";
            rt.Cells[0, 1].Text = "bbbbbbbbbb";
            rt.Cells[0, 2].Text = "cccccc";
            rt.Cells[1, 0].Text = "aaa aaa aaa";
            rt.Cells[1, 1].Text = "bbbbb";
            rt.Cells[1, 2].Text = "cccccc cccccc";
            rt.Cells[2, 2].Text = "zzzzzzzzzzzzzzz zz z";

            // done:
            return rt;
        }

        /// <summary>
        /// Creates an AmazonBookDescription data source for data bound objects.
        /// </summary>
        /// <returns>The data source created.</returns>
        private object MakeBookList()
        {
            // load book descriptions from xml
            XDocument doc = XDocument.Load(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("AutoSizeTable.Amazon.xml")));
            var books = from reader in doc.Descendants("book")
                        select new AmazonBookDescription
                        {
                            Title = reader.Attribute("title").Value,
                            CoverUri = reader.Attribute("coverUri").Value,
                            Id = reader.Attribute("id").Value,
                            Price = reader.Attribute("price").Value,
                            StockAmount = int.Parse(reader.Attribute("stockAmount").Value)
                        };

            // currently released version of C1Report relies on IEnumerable.Reset() being available,
            // hence we cannot use books directly (Reset on it is not supported) and use a List instead:
            var tbooks = books.AsEnumerable<AmazonBookDescription>();
            List<AmazonBookDescription> bookList = new List<AmazonBookDescription>();
            foreach (AmazonBookDescription book in tbooks)
                bookList.Add(book);
            return bookList;
        }

        /// <summary>
        /// Creates a data bound table with Title, Price and Stock columns.
        /// Price and Stock columns are auto-sized, whereas Title takes up
        /// the remaining spcae.
        /// </summary>
        /// <param name="bookList">AmazonBookDescription data source.</param>
        /// <returns>Created table.</returns>
        private RenderTable MakeTable_TitlePriceStock(object bookList)
        {
            // data bound table:
            RenderTable rt = new RenderTable();
            // Setting the sizing properties as follows make the last two columns (Price and StockAmount)
            // auto-adjust their widths, with the first column taking up the remaining space (it contais
            // long wrapped texts):
            rt.Width = Unit.Auto;
            rt.ColumnSizingMode = TableSizingModeEnum.Auto;
            rt.Cols[0].Width = "50%"; // allocate at least 50% to 1st column
            rt.Cols[0].Stretch = StretchColumnEnum.Yes; // but allow it to stretch to take up the free space
            // add grid lines and padding to better see the layout:
            rt.Style.GridLines.All = LineDef.Default;
            rt.CellStyle.Padding.All = "2mm";
            // header row:
            rt.Cells[0, 0].Text = "Title";
            rt.Cells[0, 1].Text = "Price";
            rt.Cells[0, 2].Text = "Stock Amount";
            rt.RowGroups[0, 1].Header = TableHeaderEnum.All;
            // data row:
            rt.Cells[1, 0].Text = "[Fields!Title.Value]";
            rt.Cells[1, 1].Text = "[Fields!Price.Value]";
            rt.Cells[1, 2].Text = "[Fields!StockAmount.Value]";
            // right-align price and stock columns:
            rt.Cells[1, 1].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cells[1, 2].Style.TextAlignHorz = AlignHorzEnum.Right;
            // center headers:
            rt.Rows[0].Style.TextAlignHorz = AlignHorzEnum.Center;
            // rt.RowGroups[1, 1].DataBinding.DataSource = books; -- can't do that yet
            rt.RowGroups[1, 1].DataBinding.DataSource = bookList;

            // done:
            return rt;
        }
    }

    /// <summary>
    /// Describes a book data record.
    /// </summary>
    public class AmazonBookDescription
    {
        public string Title { get; set; }
        public string CoverUri { get; set; }
        public string Id { get; set; }
        public string Price { get; set; }
        public int StockAmount { get; set; }
    }

}
