using C1.C1Preview;
using C1.C1Preview.DataBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for DaataBoundDoc.xaml
    /// </summary>
    public partial class DataBoundDoc : UserControl
    {
        public DataBoundDoc()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Data element type used by btnBindToList_Click.
        /// </summary>
        public class Customer
        {
            private int _id;
            private string _name;
            private int _orderId = -1;
            public Customer(int id, string name)
            {
                _id = id;
                _name = name;
            }
            public Customer(int id, string name, int orderId)
            {
                _id = id;
                _name = name;
                _orderId = orderId;
            }
            public int Id { get { return _id; } }
            public string Name { get { return _name; } }
            public int OrderId { get { return _orderId; } }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            cmbSelection.SelectedIndex = 4;
        }

        /// <summary>
        /// Builds a document with a RenderTable bound to a list of objects in memory.
        /// The list consists of objects of type Customer (see above). For each customer,
        /// several records are created representing different orders (OrderId) of that customer.
        /// In the document, an outer RenderTable contains a row group bound to that list of
        /// customers/orders, with grouping on customer Ids.
        /// For each unique customer in the outer table, a row group (2 rows) is created.
        /// The first row lists customer's Id and Name.
        /// The 2nd row contains a nested RenderTable that is also bound to the customers/orders,
        /// and represents the detail - OrderId's for the current customer.
        /// Unlike the outer table, in the nested detail table a COLUMN GROUP is bound to the customers/orders list.
        /// As the result, for each OrderId of the current customer, a new COLUMN is created, showing the OrderId.
        /// </summary>
        private void BindToListHorizontalDetail()
        {
            // build sample list of customers,
            // for each customer create several order ID's for master/detail:
            Random rnd = new Random(DateTime.Now.Second);
            List<Customer> customerOrders = new List<Customer>();
            for (int i = 0; i < 100; i++)
            {
                // create up to 12 dummy orders for each customer, with order Ids in the 1-200 range:
                int orderCount = rnd.Next(1, 12);
                for (int orderIdx = 0; orderIdx < orderCount; ++orderIdx)
                    customerOrders.Add(new Customer(i + 1, "Customer " + (i + 1).ToString(), rnd.Next(1, 200)));
            }

            // make the document:
            C1PrintDocument doc = new C1PrintDocument();
            doc.Style.FontName = "Verdana";
            doc.Style.FontSize = 12;

            // document title:
            doc.Body.Children.Add(new RenderText("List of Customer orders, customers are listed vertically, orders horizontally"));
            doc.Body.Children.Add(new RenderEmpty("5mm")); // empty space after title

            // outer render table will loop through master records:
            RenderTable rt = new RenderTable();
            rt.Style.GridLines.All = LineDef.Default;

            // group master by Id.
            // Each master group will contain 2 rows:
            // - header row with customer Id and Name;
            // - detail row, with customer's OrderId's IN A ROW (spread horizontally in a nested table).
            TableVectorGroup gMaster = rt.RowGroups[0, 2];
            gMaster.DataBinding.DataSource = customerOrders;
            gMaster.DataBinding.Grouping.Expressions.Add("Fields!Id.Value");
            // 1st row:
            rt.Cells[0, 0].Text = "Id: [Fields!Id.Value]";
            rt.Cells[0, 1].Text = "Name: [Fields!Name.Value]";

            // 2nd row: a nested table:
            RenderTable rt2 = new RenderTable();
            rt2.Style.BackColor = Colors.BlanchedAlmond;
            rt2.Style.GridLines.Vert = new LineDef("1pt", Colors.DarkOliveGreen);
            rt2.Style.FontSize = rt.Style.FontSize - 2;
            rt2.CellStyle.Padding.All = "1mm";
            TableVectorGroup dDetail = rt2.ColGroups[1, 1];
            dDetail.DataBinding.DataSource = customerOrders;
            rt2.Cells[0, 0].Text = "[Fields!Name.Value]'s orders: ";
            rt2.Cells[0, 1].Text = "[Fields!OrderId.Value]";
            // auto-size the first column (with customer's name), spread the rest:
            rt2.Cols[0].SizingMode = TableSizingModeEnum.Auto;

            rt.Cells[1, 0].RenderObject = rt2;
            rt.Cells[1, 0].SpanCols = 2;

            doc.Body.Children.Add(rt);

            this.c1DocumentViewer1.Document = doc.FixedDocumentSequence;
        }

        /// <summary>
        /// Builds a document bound to a list of objects in memory.
        /// </summary>
        private void BindToList()
        {
            C1PrintDocument doc = new C1PrintDocument();

            // build sample list of customers
            List<Customer> customers = new List<Customer>();
            for (int i = 0; i < 100; i++)
                customers.Add(new Customer(i + 1, "Customer " + (i + 1).ToString()));

            doc.Style.FontName = "Verdana";
            doc.Style.FontSize = 16;

            // document title:
            doc.Body.Children.Add(new RenderText("List of Customer objects"));
            doc.Body.Children.Add(new RenderEmpty("5mm")); // empty space after title

            // this object will be repeated for each element in the customers list
            RenderText rt = new RenderText();
            rt.Text = "Id: [Fields!Id.Value]\rName: [Fields!Name.Value]";
            rt.Style.Borders.All = LineDef.DefaultBold;
            rt.SplitVertBehavior = SplitBehaviorEnum.SplitIfLarge;
            // bind the RenderText to the customers list:
            rt.DataBinding.DataSource = customers;
            doc.Body.Children.Add(rt);

            this.c1DocumentViewer1.Document = doc.FixedDocumentSequence;
        }

        /// <summary>
        /// Builds a document with a data bound RenderText bound to an empty data source
        /// (list with 0 elements) - shows that a data bound object does not show up at all
        /// when bound to an empty data source.
        /// </summary>
        private void BindToEmptyList()
        {
            C1PrintDocument doc = new C1PrintDocument();

            // document title:
            doc.Style.FontName = "Verdana";
            doc.Style.FontSize = 20;
            doc.Body.Children.Add(new RenderText("The following RenderText object is bound to an empty list so it is not displayed.", AlignHorzEnum.Center));
            doc.Body.Children.Add(new RenderEmpty("5mm"));

            // object bound to an empty data source:
            RenderText rt = new RenderText("This RenderText object has data binding with empty recordset");
            // bind object to empty list
            rt.DataBinding.DataSource = new int[0];
            rt.Style.Borders.All = new LineDef("1mm", Colors.Blue);
            rt.Style.BackColor = Colors.YellowGreen;
            doc.Body.Children.Add(rt);

            this.c1DocumentViewer1.Document = doc.FixedDocumentSequence;
        }

        /// <summary>
        /// Builds a simple data bound document, listing product IDs and names
        /// in a single data bound RenderText.
        /// </summary>
        private void BindToMdb()
        {
            C1PrintDocument doc = new C1PrintDocument();

            doc.Style.FontName = "Verdana";
            doc.Style.FontSize = 16;

            // define data schema
            DataSource ds = CreateDemoDataSource();
            DataSet dsCities = new DataSet(ds, "select * from Products");
            doc.DataSchema.DataSources.Add(ds);
            doc.DataSchema.DataSets.Add(dsCities);

            // document title:
            RenderText rt = new RenderText();
            rt.Text = string.Format("ConnectString = {0}\rCommandText={1}", ds.ConnectionProperties.ConnectString, dsCities.Query.CommandText);
            rt.Style.BackColor = Colors.LightBlue;
            rt.Style.Borders.All = new LineDef("1mm", Colors.Blue);
            rt.Style.Spacing.Bottom = "1cm";
            doc.Body.Children.Add(rt);

            // data bound RenderText, listing product IDs and names:
            rt = new RenderText();
            rt.DataBinding.DataSource = dsCities;
            rt.Text = "Id = [Fields!ProductID.Value]  Name = [Fields!ProductName.Value]";
            rt.Style.Borders.Bottom = LineDef.Default;
            doc.Body.Children.Add(rt);

            c1DocumentViewer1.Document = doc.FixedDocumentSequence;
        }

        /// <summary>
        /// Builds a document with a data bound table, with two levels of master-detail relations.
        /// </summary>
        private void DataBoundTable()
        {
            this.Cursor = Cursors.Wait;

            C1PrintDocument doc = new C1PrintDocument();

            // Set up some styles:
            doc.Style.FontName = "Verdana";
            doc.Style.FontSize = 10;
            C1.C1Preview.Style boldFontStyle = doc.Style.Children.Add();
            boldFontStyle.FontName = "Verdana";
            boldFontStyle.FontSize = 10;
            boldFontStyle.FontBold = true;
            C1.C1Preview.Style smallFontStyle = doc.Style.Children.Add();
            smallFontStyle.FontName = "Verdana";
            smallFontStyle.FontSize = 7;
            C1.C1Preview.Style detailCaptionStyle = doc.Style.Children.Add();
            detailCaptionStyle.TextAlignHorz = AlignHorzEnum.Center;
            detailCaptionStyle.TextAlignVert = AlignVertEnum.Center;
            detailCaptionStyle.FontName = "Verdana";
            detailCaptionStyle.FontSize = 8;
            detailCaptionStyle.FontUnderline = true;
            C1.C1Preview.Style detailStyle = doc.Style.Children.Add();
            detailStyle.FontName = "Verdana";
            detailStyle.FontSize = 11;
            C1.C1Preview.Style dateStyle = detailStyle.Children.Add();
            dateStyle.TextAlignHorz = AlignHorzEnum.Center;
            C1.C1Preview.Style descriptionStyle = detailStyle.Children.Add();
            descriptionStyle.TextAlignHorz = AlignHorzEnum.Left;
            C1.C1Preview.Style currencyStyle = detailStyle.Children.Add();
            currencyStyle.TextAlignHorz = AlignHorzEnum.Right;
            C1.C1Preview.Style quantityStyle = detailStyle.Children.Add();
            quantityStyle.TextAlignHorz = AlignHorzEnum.Right;

            // define data schema:
            // orders sorted by customer sorted by customer country:
            DataSource ds = CreateDemoDataSource();
            DataSet dsCities = new DataSet(ds,
                "select o.*, c.Country, c.CompanyName, p.ProductName, od.* from orders o, customers c, products p, `order details` od " +
                "where o.CustomerID = c.CustomerID and o.OrderID = od.OrderID and od.ProductID = p.ProductID " +
                "order by c.Country, c.CompanyName, o.OrderDate");
            // add data source and data set to the document - this will preserve the data binding
            // if the document is saved as c1d/c1dx:
            doc.DataSchema.DataSources.Add(ds);
            doc.DataSchema.DataSets.Add(dsCities);

            // document caption:
            RenderText txt = new RenderText();
            txt.Text = "List of orders grouped by country and customer";
            txt.Style.FontName = "Tahoma";
            txt.Style.FontSize = 16;
            txt.Style.FontBold = true;
            doc.Body.Children.Add(txt);

            // main body of the document is a data bound table, with 3 nested data bound row groups:
            RenderTable rt = new RenderTable();
            // the following two lines make table and table columns auto-sized:
            rt.Width = Unit.Auto;
            rt.ColumnSizingMode = TableSizingModeEnum.Auto;
            // spread out data a bit:
            rt.CellStyle.Padding.All = "0.5mm";
            // turn lines on for structure clarity:
            rt.Style.GridLines.All = new LineDef("0.5pt", Colors.Silver);

            // top level master: country:
            rt.Cells[0, 0].Text = "Country:";
            rt.Cells[0, 1].SpanCols = 2;
            rt.Cells[0, 1].Text = "[Fields!Country.Value]";
            rt.Cells[0, 1].Style.Parents = boldFontStyle;
            // top level master: orders count for country:
            rt.Cells[0, 3].RenderObject = CreateAggregate1("Total orders:", "CountryOrderCount", smallFontStyle, boldFontStyle, false);
            // top level master: orders total for country:
            rt.Cells[0, 4].RenderObject = CreateAggregate1("Country total:", "CountryTotal", smallFontStyle, boldFontStyle, true);
            // country header back color:
            rt.Rows[0].Style.BackColor = Colors.LightGoldenrodYellow;

            // second level master: company:
            rt.Cells[1, 0].Text = "Company:";
            rt.Cells[1, 1].Text = "[Fields!CompanyName.Value]";
            rt.Cells[1, 1].Style.Parents = boldFontStyle;
            rt.Cells[1, 1].SpanCols = 2;
            // second level master: orders count for company:
            rt.Cells[1, 3].RenderObject = CreateAggregate1("Total orders:", "CompanyOrderCount", smallFontStyle, boldFontStyle, false);
            // second level master: orders total for company:
            rt.Cells[1, 4].RenderObject = CreateAggregate1("Company total:", "CompanyTotal", smallFontStyle, boldFontStyle, true);
            // company header back color:
            rt.Rows[1].Style.BackColor = Colors.Lavender;

            // detail: column captions:
            rt.Cells[2, 0].Text = "Date";
            rt.Cells[2, 0].Style.Parents = detailCaptionStyle;
            rt.Cells[2, 1].Text = "Product";
            rt.Cells[2, 1].Style.Parents = detailCaptionStyle;
            rt.Cells[2, 2].Text = "Unit Price";
            rt.Cells[2, 2].Style.Parents = detailCaptionStyle;
            rt.Cells[2, 3].Text = "Quantity";
            rt.Cells[2, 3].Style.Parents = detailCaptionStyle;
            rt.Cells[2, 4].Text = "Total";
            rt.Cells[2, 4].Style.Parents = detailCaptionStyle;

            // detail: data:
            // 1) detail: order date:
            rt.Cells[3, 0].Text = "[FormatDateTime(Fields!OrderDate.Value, DateFormat.ShortDate)]";
            rt.Cells[3, 0].Style.Parents = dateStyle;
            // 2) detail: product name:
            rt.Cells[3, 1].Text = "[Fields!ProductName.Value]";
            rt.Cells[3, 1].Style.Parents = descriptionStyle;
            // 3) detail: unit price:
            rt.Cells[3, 2].Text = "[string.Format(\"{0:C}\",Fields!UnitPrice.Value)]";
            rt.Cells[3, 2].Style.Parents = currencyStyle;
            // 4) detail: quantity:
            rt.Cells[3, 3].Text = "[Fields!Quantity.Value]";
            rt.Cells[3, 3].Style.Parents = quantityStyle;
            // 5) detail: order total:
            rt.Cells[3, 4].Text = "[string.Format(\"{0:C}\",Fields!UnitPrice.Value * Fields!Quantity.Value)]";
            rt.Cells[3, 4].Style.Parents = currencyStyle;
            // New in 2009 v3 release of C1Report - style expressions.
            // Use it to highlight orders worth $1000 or more:
            rt.Cells[3, 4].Style.TextColorExpr = "iif(Fields!UnitPrice.Value * Fields!Quantity.Value >= 1000, Colors.Blue, Colors.Black)";

            // top-level master footer: country total (duplicated in cell (0,4)):
            rt.Cells[5, 0].SpanCols = 3;
            rt.Cells[5, 0].Text = "Total of all orders for [Fields!Country.Value]:";
            rt.Cells[5, 0].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cells[5, 3].SpanCols = 2;
            rt.Cells[5, 3].Style.Parents = currencyStyle;
            rt.Cells[5, 3].Text = "[string.Format(\"{0:C}\", Aggregates!CountryTotal.Value)]";

            // Rows 6-8 create a visual break in the table after each country.
            // The 2 extra rows (6 & 8) are needed to make sure no extra grid lines
            // appear if this break is immediately followed by a page break.
            rt.Cells[6, 0].SpanCols = 5;
            rt.Cells[6, 0].RenderObject = new RenderEmpty("1pt");
            rt.Cells[6, 0].Style.Borders.Left = rt.Cells[6, 0].Style.Borders.Right = LineDef.Empty;
            rt.Cells[6, 0].Style.Borders.Bottom = LineDef.Empty;
            rt.RowGroups[6, 1].MinVectorsBefore = 1;
            //
            rt.Cells[7, 0].SpanCols = 5;
            rt.Cells[7, 0].RenderObject = new RenderEmpty("3mm");
            rt.Cells[7, 0].Style.Borders.All = LineDef.Empty;
            //
            rt.Cells[8, 0].SpanCols = 5;
            rt.Cells[8, 0].RenderObject = new RenderEmpty("1pt");
            rt.Cells[8, 0].Style.Borders.Left = rt.Cells[8, 0].Style.Borders.Right = LineDef.Empty;
            rt.Cells[8, 0].Style.Borders.Top = LineDef.Empty;
            rt.RowGroups[8, 1].MinVectorsAfter = 1;

            // grand total for all countries:
            rt.Cells[9, 0].SpanCols = 2;
            rt.Cells[9, 0].Text = "Grand Total:";
            rt.Cells[9, 0].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cells[9, 2].SpanCols = 3;
            rt.Cells[9, 2].Text = "[string.Format(\"{0:C}\", Aggregates!GrandTotal.Value)]";
            rt.Cells[9, 2].Style.Parents = currencyStyle;

            // define databinding within table:
            // top level master row group - country - includes all defined rows (0-9):
            TableVectorGroup g = rt.RowGroups[0, 9];
            g.DataBinding.DataSource = dsCities;
            // group by country:
            g.DataBinding.Grouping.Expressions.Add("Fields!Country.Value");
            // add outline group header for each contry:
            g.DataBinding.OutlineText = "[Fields!Country.Value]";
            g.SplitBehavior = SplitBehaviorEnum.SplitIfNeeded;
            // make sure country header is followed by company and at least one detail row:
            rt.RowGroups[0, 1].MinVectorsAfter = 3;
            // add country level aggregates (attached to top level master DataBinding):
            // (because there are several OrderID fields in the 'select' we must qualify it)
            doc.DataSchema.Aggregates.Add(new Aggregate("CountryOrderCount", "Fields(\"o.OrderID\").Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Count));
            doc.DataSchema.Aggregates.Add(new Aggregate("CountryTotal", "Fields!UnitPrice.Value * Fields!Quantity.Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            // document level aggregate for the grand total:
            doc.DataSchema.Aggregates.Add(new Aggregate("GrandTotal", "Fields!UnitPrice.Value * Fields!Quantity.Value", g.DataBinding, RunningEnum.Document, AggregateFuncEnum.Sum));

            // second level master row group - company - includes rows 1-4:
            g = rt.RowGroups[1, 4];
            g.DataBinding.DataSource = dsCities;
            // group by company:
            g.DataBinding.Grouping.Expressions.Add("Fields!CompanyName.Value");
            // add outline group header for each company:
            g.DataBinding.OutlineText = "[Fields!CompanyName.Value]";
            g.SplitBehavior = SplitBehaviorEnum.SplitIfNeeded;
            // ensure company header is followed by at least one detail row:
            rt.RowGroups[1, 2].MinVectorsAfter = 1;
            rt.RowGroups[1, 2].SplitBehavior = SplitBehaviorEnum.SplitIfLarge;
            // add company level aggregates (attached to second level master DataBinding):
            doc.DataSchema.Aggregates.Add(new Aggregate("CompanyTotal", "Fields!UnitPrice.Value * Fields!Quantity.Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            // see note about OrderID above; but really, because we are just counting, any field would do here - e.g. CustomerID
            doc.DataSchema.Aggregates.Add(new Aggregate("CompanyOrderCount", "Fields(\"o.OrderID\").Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Count));

            // finally, detail level data binding (just one row):
            g = rt.RowGroups[3, 1];
            g.DataBinding.DataSource = dsCities;
            // add outline entry for each product name:
            g.DataBinding.OutlineText = "[Fields!ProductName.Value]";

            // add table to the document:
            doc.Body.Children.Add(rt);

            // set up a progress window:
            ProgressWindow pw = new ProgressWindow();
            pw.Owner = Application.Current.MainWindow;
            doc.Body.Children[0].UserData = pw;
            pw.Show();
            doc.LongOperation += new LongOperationEventHandler(doc_LongOperation);

            // preview the document (this will cause the document to generate):
            c1DocumentViewer1.Document = doc.FixedDocumentSequence;

            // we're done; reset cursor:
            this.Cursor = null;
        }

        /// <summary>
        /// Called periodically while the document is generating. Updates the progress window,
        /// closes it when the document is ready.
        /// </summary>
        private void doc_LongOperation(object sender, LongOperationEventArgs e)
        {
            DispatcherHelper.DoEvents();
            C1PrintDocument doc = (C1PrintDocument)sender;
            ProgressWindow pw = doc.Body.Children[0].UserData as ProgressWindow;
            if (pw != null)
            {
                if (!doc.IsGenerating)
                {
                    doc.Body.Children[0].UserData = null;
                    pw.Close();
                }
                else
                    pw.progressBar1.Value = e.Complete * 100;
            }
            DispatcherHelper.DoEvents();
        }

        /// <summary>
        /// Creates a RenderParagraph containing a caption (label) and an expression
        /// referencing an aggregate with the specified name, delimited by a newline.
        /// The aggregate must be added to the document's DataSchema.Aggregates separately.
        /// </summary>
        /// <param name="caption">The caption text</param>
        /// <param name="aggregateName">The aggregate name.</param>
        /// <param name="captionStyle">Style for the caption text.</param>
        /// <param name="aggregateStyle">Style for the aggregate value.</param>
        /// <param name="currency">If true, value is formatted as currency.</param>
        /// <returns>The created RenderParagraph object.</returns>
        private RenderObject CreateAggregate1(string caption, string aggregateName,
            C1.C1Preview.Style captionStyle, C1.C1Preview.Style aggregateStyle, bool currency)
        {
            RenderParagraph result = new RenderParagraph();
            ParagraphText pt = new ParagraphText(caption + "\r");
            pt.Style.Parents = captionStyle;
            result.Content.Add(pt);
            if (currency)
                pt = new ParagraphText("[string.Format(\"{0:C}\",Aggregates!" + aggregateName + ".Value)]");
            else
                pt = new ParagraphText("[Aggregates!" + aggregateName + ".Value]");
            pt.Style.Parents = aggregateStyle;
            result.Content.Add(pt);
            return result;
        }

        /// <summary>
        /// Creates the demo data source using c1nwind.mdb located in user's home dir, such as:
        /// C:\Users\dima\Documents\ComponentOne Samples\Common\c1nwind.mdb 
        /// </summary>
        private DataSource CreateDemoDataSource()
        {
            DataSource result = new DataSource();
            result.ConnectionProperties.DataProvider = DataProviderEnum.OLEDB;
            result.ConnectionProperties.ConnectString = GetConnectionString();
            return result;
        }

        /// <summary>
        /// Gets connection string for c1nwind.mdb.
        /// </summary>
        private string GetConnectionString()
        {
            string cs = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=c1nwind.mdb";

            //
            int i = cs.IndexOf("Data Source", 0, StringComparison.OrdinalIgnoreCase);
            if (i < 0)
                return string.Empty;
            while (i < cs.Length && cs[i] != '=') i++;
            if (i >= cs.Length)
                return string.Empty;
            int j = i;
            while (i < cs.Length && cs[i] != ';') i++;

            //
            string mdbName = cs.Substring(j + 1, i - j - 1).Trim();
            if (mdbName.Length <= 0)
                return string.Empty;
            mdbName = System.IO.Path.GetFileName(mdbName);
            if (string.Compare(mdbName, "nwind.mdb", true) == 0)
                mdbName = "c1nwind.mdb";

            //
            cs = cs.Substring(0, j + 1) +
                Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                @"\ComponentOne Samples\Common\" +
                mdbName +
                cs.Substring(i);

            return cs;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cmbSelection.SelectedIndex)
            {
                case 0:
                    BindToList();
                    break;
                case 1:
                    BindToListHorizontalDetail();
                    break;
                case 2:
                    BindToEmptyList();
                    break;
                case 3:
                    BindToMdb();
                    break;
                case 4:
                    DataBoundTable();
                    break;
            }
        }
    }

    //
    // Code below is taken from:
    // http://www.cnblogs.com/sheva/archive/2006/08/24/485790.html
    //
    /// <summary>
    /// Encapsulates a WPF dispatcher with added functionalities.
    /// </summary>
    public class DispatcherHelper
    {
        private static DispatcherOperationCallback exitFrameCallback = new
             DispatcherOperationCallback(ExitFrame);

        /// <summary>
        /// Processes all UI messages currently in the message queue.
        /// </summary>
        public static void DoEvents()
        {
            // Create new nested message pump.
            DispatcherFrame nestedFrame = new DispatcherFrame();

            // Dispatch a callback to the current message queue, when getting called,
            // this callback will end the nested message loop.
            // note that the priority of this callback should be lower than that of UI event messages.
            DispatcherOperation exitOperation = Dispatcher.CurrentDispatcher.BeginInvoke(
                DispatcherPriority.Background, exitFrameCallback, nestedFrame);

            // pump the nested message loop, the nested message loop will immediately
            // process the messages left inside the message queue.
            Dispatcher.PushFrame(nestedFrame);

            // If the "exitFrame" callback is not finished, abort it.
            if (exitOperation.Status != DispatcherOperationStatus.Completed)
            {
                exitOperation.Abort();
            }
        }

        private static Object ExitFrame(Object state)
        {
            DispatcherFrame frame = state as DispatcherFrame;

            // Exit the nested message loop.
            frame.Continue = false;
            return null;
        }
    }

}
