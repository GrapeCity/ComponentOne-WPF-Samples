using System;
using System.Collections.Generic;
using System.Drawing;
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
using C1.C1Preview;
using C1.C1Preview.DataBinding;
using C1.C1Preview.Export;
using C1.WPF;
using Microsoft.Win32;

namespace SimpleReports
{
    /// <summary>
    /// Interaction logic for CustomReport.xaml
    /// </summary>
    public partial class CustomReport : UserControl
    {
        private readonly string[] _reportsList = new string[]
           {
            "Alphabetical List of Products",
            "Customer Labels",
            "Employees",
            "Product Catalog",
            "Products By Category",
            "Sales Chart",
            "Employee Sales by Country",
            "Data bound RenderTable with grouping and aggregates",
            "Cross-tab Reports",
            "Balance Sheet",
            "Price Comparison"
           };
        C1PrintDocument _printDocument;
        Cursor _defaultCursor;
        public CustomReport()
        {
            InitializeComponent();

            _defaultCursor = this.Cursor;

            _printDocument = new C1PrintDocument();
            c1DocumentViewer1.FitToWidth();

            BuildComboItems();
        }

        private void BuildComboItems()
        {
            for (int i = 0; i < _reportsList.Length; i++)
            {
                comboReports.Items.Add(new C1ComboBoxItem() { Content = _reportsList[i] });
            }
            comboReports.SelectedIndexChanged += ComboReports_SelectedIndexChanged;
            comboReports.SelectedIndex = 5;

        }

        private async void ComboReports_SelectedIndexChanged(object sender, PropertyChangedEventArgs<int> e)
        {
            if (comboReports.SelectedIndex < 0)
            {
                return;
            }

            indicator.IsActive = true;
            await Task.Delay(100);
            switch (comboReports.SelectedIndex)
            {
                case 0:
                    await AlphabeticalListOfProducts();
                    break;

                case 1:
                    await CustomerLabels();
                    break;

                case 2:
                    await Employees();
                    break;

                case 3:
                    await ProductCatalog();
                    break;

                case 4:
                default:
                    await ProductsByCategory();
                    break;

                case 5:
                    await SalesChart();
                    break;

                case 6:
                    await EmployeeSalesByCountry();
                    break;

                case 7:
                    await DataBoundTable();
                    break;

                case 8:
                    await CrossTabReports();
                    break;

                case 9:
                    await BalanceSheet();
                    break;

                case 10:
                    await PriceComparison();
                    break;
            }

            c1DocumentViewer1.Document = _printDocument.FixedDocumentSequence;
            indicator.IsActive = false;
        }

        private async Task AlphabeticalListOfProducts()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Verdana";
            _printDocument.Style.FontSize = 10;

            // set group header style
            C1.C1Preview.Style headerLetterStyle = _printDocument.Style.Children.Add();
            headerLetterStyle.FontSize = 11;
            headerLetterStyle.FontBold = true;
            headerLetterStyle.GridLines.Top = new LineDef("0.5pt", Colors.Black);

            // set group subheader style
            C1.C1Preview.Style headerNameStyle = _printDocument.Style.Children.Add();
            headerNameStyle.FontSize = 9;
            headerNameStyle.FontBold = true;

            // set group subheader style (right aligned)
            C1.C1Preview.Style headerNameStyleRight = _printDocument.Style.Children.Add();
            headerNameStyleRight.FontSize = 9;
            headerNameStyleRight.FontBold = true;
            headerNameStyleRight.TextAlignHorz = AlignHorzEnum.Right;

            // set data style
            C1.C1Preview.Style dataStyle = _printDocument.Style.Children.Add();
            dataStyle.FontSize = 8;
            dataStyle.TextAlignHorz = AlignHorzEnum.Left;

            // set data style (right aligned)
            C1.C1Preview.Style dataStyleRight = _printDocument.Style.Children.Add();
            dataStyleRight.FontSize = 8;
            dataStyleRight.TextAlignHorz = AlignHorzEnum.Right;

            // set document caption
            var rtCaption = new RenderText();
            rtCaption.Text = "Alphabetical List of Products";
            rtCaption.Style.FontName = "Tahoma";
            rtCaption.Style.FontSize = 16;
            rtCaption.Style.Padding.Bottom = "2mm";

            _printDocument.Body.Children.Add(rtCaption);

            // set current date (short format)
            var rtCurrentDate = new RenderText();
            rtCurrentDate.Text = DateTime.Now.ToShortDateString();
            rtCurrentDate.Style.Padding.Bottom = "2mm";

            _printDocument.Body.Children.Add(rtCurrentDate);

            // define data schema
            DataSource ds = CreateDemoDataSource();

            DataSet dsProducts = new DataSet(ds,
                "SELECT Left(p.ProductName, 1) AS FirstLetter, p.ProductName, p.QuantityPerUnit, p.UnitsInStock, c.CategoryName " +
                "FROM Categories c, Products p " +
                "WHERE c.CategoryID = p.CategoryID " +
                "ORDER BY p.ProductName");

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(ds);
            _printDocument.DataSchema.DataSets.Add(dsProducts);

            RenderTable rt = new RenderTable();

            TableVectorGroup tvg = rt.RowGroups[0, 5];
            tvg.DataBinding.DataSource = dsProducts;

            // group by first letter of product name
            tvg.DataBinding.Grouping.Expressions.Add("Fields!FirstLetter.Value");

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // make table and table columns auto-sized
            rt.Width = Unit.Auto;
            rt.ColumnSizingMode = TableSizingModeEnum.Auto;

            // set header row
            rt.Cells[0, 0].Text = "[Fields!FirstLetter.Value]";
            rt.Cells[0, 0].Style.Parents = headerLetterStyle;
            rt.Cells[0, 1].SpanCols = 4;
            rt.Cells[0, 1].Style.Parents = headerLetterStyle;

            // set subheader row
            rt.Cells[1, 1].Text = "Product Name:";
            rt.Cells[1, 1].Style.Parents = headerNameStyle;

            rt.Cells[1, 2].Text = "Category Name:";
            rt.Cells[1, 2].Style.Parents = headerNameStyle;

            rt.Cells[1, 3].Text = "Quantity Per Unit:";
            rt.Cells[1, 3].Style.Parents = headerNameStyle;

            rt.Cells[1, 4].Text = "Units In Stock:";
            rt.Cells[1, 4].Style.Parents = headerNameStyleRight;

            // set data row
            rt.Cells[2, 1].Text = "[Fields!ProductName.Value]";
            rt.Cells[2, 1].Style.Parents = dataStyle;

            rt.Cells[2, 2].Text = "[Fields!CategoryName.Value]";
            rt.Cells[2, 2].Style.Parents = dataStyle;

            rt.Cells[2, 3].Text = "[Fields!QuantityPerUnit.Value]";
            rt.Cells[2, 3].Style.Parents = dataStyle;

            rt.Cells[2, 4].Text = "[Fields!UnitsInStock.Value]";
            rt.Cells[2, 4].Style.Parents = dataStyleRight;

            // create row group
            tvg = rt.RowGroups[2, 1];
            tvg.DataBinding.DataSource = dsProducts;

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task CustomerLabels()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set landscape page orientation
            _printDocument.PageLayout.PageSettings.Landscape = true;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // set page style
            _printDocument.Style.FontName = "Verdana";
            _printDocument.Style.FontSize = 7;

            // create render area containing areas with data
            RenderArea raContainer = new RenderArea();
            raContainer.Stacking = StackingRulesEnum.InlineLeftToRight;

            _printDocument.Body.Children.Add(raContainer);

            // define data schema
            DataSource ds = CreateDemoDataSource();

            DataSet dsCustomers = new DataSet(ds,
                "SELECT CompanyName, Address, City, PostalCode, Country " +
                "FROM Customers " +
                "ORDER BY CompanyName");

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(ds);
            _printDocument.DataSchema.DataSets.Add(dsCustomers);

            // create render area contains data
            RenderArea raItem = new RenderArea();

            // set borders
            raItem.Style.Borders.Right = new LineDef("0.1pt", Colors.LightGray, System.Drawing.Drawing2D.DashStyle.Dot);
            raItem.Style.Borders.Bottom = new LineDef("0.1pt", Colors.LightGray, System.Drawing.Drawing2D.DashStyle.Dot);

            // set size
            raItem.Width = "40mm";
            raItem.Height = "20mm";

            // do not split the area into different pages
            raItem.SplitVertBehavior = SplitBehaviorEnum.Never;

            // set the data source
            raItem.DataBinding.DataSource = dsCustomers;

            // group by first letter of product name
            raItem.DataBinding.Grouping.Expressions.Add("Fields!CompanyName.Value");

            raContainer.Children.Add(raItem);

            // add text
            RenderText rt = new RenderText();
            rt.Text = "[Fields!CompanyName.Value]\r\n[Fields!Address.Value]\r\n[Fields!City.Value] [Fields!PostalCode.Value]\r\n[Fields!Country.Value]";
            rt.Style.Padding.All = "1mm";

            // set the text's data source to the data source of the containing RenderArea,
            // it`s indicates that the render object is bound to the current group in the specified object
            rt.DataBinding.DataSource = raContainer.DataBinding.DataSource;

            // Add the text to the area
            raItem.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task Employees()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Verdana";
            _printDocument.Style.FontSize = 10;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // set group header style
            C1.C1Preview.Style headerStyle = _printDocument.Style.Children.Add();
            headerStyle.FontSize = 9;
            headerStyle.FontBold = true;
            headerStyle.GridLines.Bottom = new LineDef("1pt", Colors.Black);

            // set country header style
            C1.C1Preview.Style countryStyle = _printDocument.Style.Children.Add();
            countryStyle.FontSize = 11;
            countryStyle.FontBold = true;

            // set city header style
            C1.C1Preview.Style cityStyle = _printDocument.Style.Children.Add();
            cityStyle.FontSize = 10;
            cityStyle.FontUnderline = true;

            // set data name
            C1.C1Preview.Style dataNameStyle = _printDocument.Style.Children.Add();
            dataNameStyle.FontSize = 8;
            dataNameStyle.FontBold = true;
            dataNameStyle.TextAlignHorz = AlignHorzEnum.Center;

            // set data style
            C1.C1Preview.Style dataStyle = _printDocument.Style.Children.Add();
            dataStyle.FontSize = 8;

            // set document caption
            var rtCaption = new RenderText();
            rtCaption.Text = "Employees";
            rtCaption.Style.FontName = "Tahoma";
            rtCaption.Style.FontSize = 16;
            rtCaption.Style.Padding.All = "2mm";
            rtCaption.Style.BackColor = Colors.LightGray;

            _printDocument.Body.Children.Add(rtCaption);

            // define data schema
            DataSource ds = CreateDemoDataSource();

            DataSet dsEmployers = new DataSet(ds,
                "SELECT EmployeeID, LastName, FirstName, Title, TitleOfCourtesy, BirthDate, HireDate, Address, City, Region, PostalCode, Country, HomePhone, Extension, Notes, ReportsTo, Photo " +
                "FROM Employees " +
                "ORDER BY Country, City, FirstName, LastName");

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(ds);
            _printDocument.DataSchema.DataSets.Add(dsEmployers);

            // create table
            RenderTable rt = new RenderTable();

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // set header row
            rt.Cells[0, 0].Text = "Country";
            rt.Cells[0, 0].Style.Spacing.Top = "2mm";
            rt.Cells[0, 0].Style.Parents = headerStyle;

            rt.Cells[0, 1].Text = "City";
            rt.Cells[0, 1].Style.Parents = headerStyle;

            rt.Cells[0, 2].Text = "Address";
            rt.Cells[0, 2].Style.Parents = headerStyle;

            rt.Cells[0, 3].Text = "Home Phone";
            rt.Cells[0, 3].Style.Parents = headerStyle;

            // set country row
            rt.Cells[1, 0].Text = "[Fields!Country.Value]";
            rt.Cells[1, 0].Style.Parents = countryStyle;

            // set city row
            rt.Cells[2, 1].Text = "[Fields!City.Value]";
            rt.Cells[2, 1].Style.Parents = cityStyle;

            // set data rows
            rt.Cells[3, 0].Text = "[Fields!FirstName.Value] [Fields!LastName.Value]";
            rt.Cells[3, 0].Style.Parents = dataNameStyle;
            rt.Cells[3, 0].SpanCols = 2;

            rt.Cells[3, 2].Text = "[Fields!Address.Value]";
            rt.Cells[3, 2].Style.Parents = dataStyle;

            rt.Cells[3, 3].Text = "[Fields!HomePhone.Value]";
            rt.Cells[3, 3].Style.Parents = dataStyle;

            RenderImage ri = new RenderImage(_printDocument);

            _printDocument.ThrowExceptionOnError = true;
            _printDocument.AddWarningsWhenErrorInScript = true;

            ri.FormatDataBindingInstanceScript = @"
                Dim ri as RenderImage = DirectCast(RenderObject, RenderImage)
                Dim picData as Byte() = DirectCast(RenderObject.Original.DataBinding.Parent.Fields!Photo.Value, Byte())
                Const bmData As Integer = 78
                Dim ms as IO.MemoryStream = New IO.MemoryStream(picData, bmData, picData.Length - bmData)
                ri.Image = Image.FromStream(ms)
                ";

            // set image parameters
            ri.Width = "30mm";
            ri.Height = "30mm";
            ri.Style.ImageAlign.AlignHorz = ImageAlignHorzEnum.Center;
            ri.Style.ImageAlign.AlignVert = ImageAlignVertEnum.Center;

            rt.Cells[4, 0].RenderObject = ri;
            rt.Cells[4, 0].SpanCols = 2;
            rt.Cells[4, 0].SpanRows = 2;

            rt.Cells[4, 2].Text = "[Fields!Title.Value]";
            rt.Cells[4, 2].Style.Parents = dataStyle;

            rt.Cells[4, 3].Text = "[FormatDateTime(Fields!BirthDate.Value, DateFormat.ShortDate)]    [FormatDateTime(Fields!HireDate.Value, DateFormat.ShortDate)]";
            rt.Cells[4, 3].Style.Parents = dataStyle;

            // add area for notes
            RenderArea raNotes = new RenderArea();
            raNotes.Style.Spacing.Right = "20mm";

            var rtNoteTitle = new RenderText();
            rtNoteTitle.Text = "[Fields!FirstName.Value]`s notes:";
            rtNoteTitle.Style.FontSize = 7;

            // add note text
            var rtNote = new RenderText();
            rtNote.Text = "[Fields!Notes.Value]";
            rtNote.Style.Borders.All = new LineDef("1pt", Colors.LightGray);
            rtNote.Style.Padding.All = "2mm";
            rtNote.Style.FontSize = 6;
            rtNote.Style.FontItalic = true;

            // do not split the area into different pages
            raNotes.SplitVertBehavior = SplitBehaviorEnum.Never;

            raNotes.Children.Add(rtNoteTitle);
            raNotes.Children.Add(rtNote);

            rt.Cells[5, 2].RenderObject = raNotes;
            rt.Cells[5, 2].SpanCols = 2;

            // auto-size columns, spread the rest columns
            rt.Cols[0].SizingMode = TableSizingModeEnum.Auto;
            rt.Cols[1].SizingMode = TableSizingModeEnum.Auto;

            // create group by city
            TableVectorGroup tvg = rt.RowGroups[2, 6];
            tvg.DataBinding.DataSource = dsEmployers;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!City.Value");

            // create group by country
            tvg = rt.RowGroups[0, 8];
            tvg.DataBinding.DataSource = dsEmployers;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!Country.Value");

            // add data rows
            tvg = rt.RowGroups[3, 5];
            tvg.DataBinding.DataSource = dsEmployers;
            tvg.SplitBehavior = SplitBehaviorEnum.Never;

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // define a cell group style
            rt.UserCellGroups.Add(new UserCellGroup(new Rectangle(0, 3, 4, 3)));
            rt.UserCellGroups[0].Style.Borders.All = new LineDef("0.5pt", Colors.Black);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task ProductCatalog()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Tahoma";
            _printDocument.Style.FontSize = 10;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // set category header style
            C1.C1Preview.Style categoryNameStyle = _printDocument.Style.Children.Add();
            categoryNameStyle.FontSize = 15;
            categoryNameStyle.FontBold = true;
            categoryNameStyle.TextAlignVert = AlignVertEnum.Bottom;

            // set description style
            C1.C1Preview.Style descriptionStyle = _printDocument.Style.Children.Add();
            descriptionStyle.FontSize = 7;
            descriptionStyle.FontItalic = true;

            // set header style
            C1.C1Preview.Style headerStyle = _printDocument.Style.Children.Add();
            headerStyle.FontSize = 10;
            headerStyle.FontBold = true;
            headerStyle.FontItalic = true;
            headerStyle.BackColor = Colors.LightGray;

            // normal data style
            C1.C1Preview.Style normalDataStyle = _printDocument.Style.Children.Add();
            normalDataStyle.FontSize = 9;

            // center aligned data style
            C1.C1Preview.Style centerAlignedDataStyle = _printDocument.Style.Children.Add();
            centerAlignedDataStyle.FontSize = 9;
            centerAlignedDataStyle.TextAlignHorz = AlignHorzEnum.Center;

            // bold data style
            C1.C1Preview.Style boldDataStyle = _printDocument.Style.Children.Add();
            boldDataStyle.FontSize = 9;
            boldDataStyle.FontBold = true;

            // right aligned data style
            C1.C1Preview.Style rightAlignedDataStyle = _printDocument.Style.Children.Add();
            rightAlignedDataStyle.FontSize = 9;
            rightAlignedDataStyle.TextAlignHorz = AlignHorzEnum.Right;

            // add document caption
            var rtCaption = new RenderText();
            rtCaption.Style.FontSize = 16;
            rtCaption.Style.TextAlignHorz = AlignHorzEnum.Center;
            rtCaption.Text = "Northwind Traders";

            _printDocument.Body.Children.Add(rtCaption);

            // add second document caption
            var rtCaption2 = new RenderText();
            rtCaption2.Style.FontSize = 10;
            rtCaption2.Style.TextColor = Colors.DarkRed;
            rtCaption2.Style.TextAlignHorz = AlignHorzEnum.Center;
            rtCaption2.Text = "Fall Catalog";

            _printDocument.Body.Children.Add(rtCaption2);

            // add empty space
            _printDocument.Body.Children.Add(new RenderEmpty("38mm"));

            // add text block
            var raBlock = new RenderArea();
            raBlock.Style.Borders.All = new LineDef("0.5pt", Colors.Black);
            raBlock.Style.Padding.All = "3mm";
            raBlock.Style.TextColor = Colors.DarkRed;
            raBlock.Style.TextAlignHorz = AlignHorzEnum.Center;
            raBlock.SplitVertBehavior = SplitBehaviorEnum.Never;
            raBlock.Width = "60%";

            // block is horizontally centered
            raBlock.X = "parent.x + (parent.width - width)/2";

            // add caption to block
            var rtBlockCaption = new RenderText();
            rtBlockCaption.Style.FontSize = 11;
            rtBlockCaption.Style.Padding.Bottom = "1mm";
            rtBlockCaption.Text = "Commitment to Quality";

            // add text to block
            var rtBlocktext = new RenderText();
            rtBlocktext.Style.FontSize = 8;
            rtBlocktext.Text = "Northwind Traders is committed to bringing you products of the highest quality from all over the world. " +
                "If at any time you are not completely satisfied with any of our products, " +
                "you may return them to us for a full refund.";

            raBlock.Children.Add(rtBlockCaption);
            raBlock.Children.Add(rtBlocktext);

            _printDocument.Body.Children.Add(raBlock);

            // add empty space
            _printDocument.Body.Children.Add(new RenderEmpty("70mm"));

            // add text
            var raText = new RenderArea();
            raText.Style.FontItalic = true;
            raText.Style.TextColor = Colors.DarkBlue;
            raText.Style.TextAlignHorz = AlignHorzEnum.Center;
            raText.SplitVertBehavior = SplitBehaviorEnum.Never;
            raText.Width = "80%";

            // text is horizontally centered
            raText.X = "parent.x + (parent.width - width)/2";

            AddTextBlocks(raText, "Tour the Gastronomic World with Northwind Traders!");

            AddTextBlocks(raText, "When Northwind Traders buyers set out to search for the Wonders of the Gastronomic World they found a lot more than seven of them. " +
                "And here they are--tastefully presented in our Fall Catalog.");

            AddTextBlocks(raText, "The beverages and confections we're featuring this fall are sure to please even the most discerning palates. For thirst quenchers, " +
                "try exotic Chang, hearty Laughing Lumberjack Lager, robust Rhonbrau Klosterbier, and refreshing Lakkalikoori.");

            AddTextBlocks(raText, "And for a taste of something sweet, try Pavlova, the intriguing meringue dessert from Australia; " +
                "Teatime Chocolate Biscuits from England, tasty Maxilaku from Finland; and the Berlin specialty, NuNuCa Nus-Nougat-Creme.");

            AddTextBlocks(raText, "Our sales representatives are ready to take your orders now. " +
                "For your convenience, we've included details on ordering on the last page of this catalog.");

            _printDocument.Body.Children.Add(raText);

            // add empty space
            _printDocument.Body.Children.Add(new RenderEmpty("60mm"));

            // define data schema
            var dataSource = CreateDemoDataSource();

            var dsProducts = new DataSet(dataSource,
                "SELECT c.CategoryName, c.Description, p.ProductID, p.ProductName, p.QuantityPerUnit, p.UnitPrice " +
                "FROM Products p, Categories c " +
                "WHERE p.CategoryID = c.CategoryID " +
                "ORDER BY c.CategoryName, p.ProductName");

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(dataSource);
            _printDocument.DataSchema.DataSets.Add(dsProducts);

            // create table
            var rt = new RenderTable();

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // category name
            rt.Cells[0, 0].Text = "[Fields!CategoryName.Value]";
            rt.Cells[0, 0].SpanCols = 4;
            rt.Cells[0, 0].Style.Parents = categoryNameStyle;
            rt.Rows[0].Height = "15mm";

            rt.Cells[1, 0].Text = "[Fields!Description.Value]";
            rt.Cells[1, 0].SpanCols = 4;
            rt.Cells[1, 0].Style.Parents = descriptionStyle;

            // header
            rt.Cells[2, 0].Text = "Product Name:";
            rt.Cells[2, 0].Style.Parents = headerStyle;

            rt.Cells[2, 1].Text = "Product ID:";
            rt.Cells[2, 1].Style.Parents = headerStyle;

            rt.Cells[2, 2].Text = "Quantity Per Unit:";
            rt.Cells[2, 2].Style.Parents = headerStyle;

            rt.Cells[2, 3].Text = "Unit Price:";
            rt.Cells[2, 3].Style.Parents = headerStyle;

            // data row
            rt.Cells[3, 0].Text = "[Fields!ProductName.Value]";
            rt.Cells[3, 0].Style.Parents = boldDataStyle;

            rt.Cells[3, 1].Text = "[Fields!ProductID.Value]";
            rt.Cells[3, 1].Style.Parents = normalDataStyle;
            rt.Cells[3, 1].CellStyle.Spacing.Right = "30mm";

            rt.Cells[3, 2].Text = "[Fields!QuantityPerUnit.Value]";
            rt.Cells[3, 2].Style.Parents = normalDataStyle;
            rt.Cells[3, 2].CellStyle.Spacing.Right = "20mm";

            rt.Cells[3, 3].Text = "$[Fields!UnitPrice.Value]";
            rt.Cells[3, 3].Style.Parents = rightAlignedDataStyle;

            // auto-size columns
            rt.Cols[1].SizingMode = TableSizingModeEnum.Auto;
            rt.Cols[2].SizingMode = TableSizingModeEnum.Auto;
            rt.Cols[3].SizingMode = TableSizingModeEnum.Auto;

            // create group by category name
            TableVectorGroup tvg = rt.RowGroups[0, 4];
            tvg.DataBinding.DataSource = dsProducts;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!CategoryName.Value");

            // create group by description
            tvg = rt.RowGroups[0, 4];
            tvg.DataBinding.DataSource = dsProducts;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!Description.Value");

            // add data rows
            tvg = rt.RowGroups[3, 1];
            tvg.DataBinding.DataSource = dsProducts;
            tvg.SplitBehavior = SplitBehaviorEnum.Never;

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task SalesChart()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Verdana";
            _printDocument.Style.FontSize = 8;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // set header 1 style: country
            C1.C1Preview.Style countryHeaderStyle = _printDocument.Style.Children.Add();
            countryHeaderStyle.FontSize = 12;

            // set header 2 style: employee name
            C1.C1Preview.Style nameHeaderStyle = _printDocument.Style.Children.Add();
            nameHeaderStyle.FontSize = 8;

            // set data style
            C1.C1Preview.Style dataStyle = _printDocument.Style.Children.Add();
            dataStyle.FontSize = 7;

            // define data schema
            var dataSource = CreateDemoDataSource();

            var dsSales = new DataSet(dataSource,
                "SELECT o.ShipCountry, e.EmployeeID, e.FirstName, e.LastName, o.ShippedDate, o.Freight " +
                "FROM Employees e, Orders o " +
                "WHERE e.EmployeeID = o.EmployeeID AND o.ShippedDate IS NOT NULL " +
                "ORDER BY o.ShipCountry, e.FirstName, e.LastName, o.ShippedDate");

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(dataSource);
            _printDocument.DataSchema.DataSets.Add(dsSales);

            // add caption

            var rtHeader = new RenderText();

            rtHeader.Text = "NorthWind Sales";
            rtHeader.Style.FontSize = 14;
            rtHeader.Style.Spacing.Bottom = "4mm";

            _printDocument.Body.Children.Add(rtHeader);

            // add summary info
            var rtSummaryHeader = new RenderText();

            rtSummaryHeader.Text = "Total sales amount: $[Sum(\"Fields!Freight.Value\")]";

            // set parameters
            rtSummaryHeader.Style.FontSize = 6;
            rtSummaryHeader.Style.FontItalic = true;
            rtSummaryHeader.Style.Spacing.Bottom = "2mm";
            rtSummaryHeader.Style.TextAlignHorz = AlignHorzEnum.Right;

            _printDocument.Body.Children.Add(rtSummaryHeader);

            // create table
            var rt = new RenderTable();

            // draw table borders
            rt.Style.Borders.All = new LineDef("0.5pt", Colors.LightGray);

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // add header 1: country
            rt.Cells[0, 0].Text = "[Fields!ShipCountry.Value]";
            rt.Cells[0, 0].Style.Parents = countryHeaderStyle;

            rt.Cells[0, 3].Text = "$[Aggregates!SumByCountry.Value]";
            rt.Cells[0, 3].Style.Parents = countryHeaderStyle;
            rt.Cells[0, 3].Style.TextAlignHorz = AlignHorzEnum.Right;

            rt.Rows[0].Style.BackColor = Colors.LightBlue;

            // add header 2: employee name
            rt.Cells[1, 0].Text = "[Fields!FirstName.Value] [Fields!LastName.Value]";
            rt.Cells[1, 0].Style.Parents = nameHeaderStyle;

            rt.Cells[1, 1].Text = "Shipped Date";
            rt.Cells[1, 1].Style.Parents = nameHeaderStyle;

            rt.Cells[1, 2].Text = "Sale Amount";
            rt.Cells[1, 2].Style.Parents = nameHeaderStyle;

            rt.Rows[1].Style.BackColor = Colors.LightGreen;
            rt.Rows[1].CellStyle.Spacing.Left = "3mm";

            // add data

            // create rectangle
            var rr = new RenderRectangle();

            // set rectangle border color
            rr.Style.ShapeLine = new LineDef("0.5pt", Colors.Green);

            // set rectangle fill color
            rr.Style.ShapeFillColor = Colors.Green;

            var rrCell = rt.Cells[2, 0];

            // set rectangle location and size
            rr.X = new Unit(rrCell.Bounds.Left, _printDocument.ResolvedUnit);
            rr.Y = new Unit(rrCell.Bounds.Top, _printDocument.ResolvedUnit);
            rr.Width = new Unit(rrCell.Bounds.Width, _printDocument.ResolvedUnit);
            rr.Height = new Unit(rrCell.Bounds.Height, _printDocument.ResolvedUnit);

            // show all exceptions and warnings for script debug
            _printDocument.ThrowExceptionOnError = true;
            _printDocument.AddWarningsWhenErrorInScript = true;

            // calculate and set rectangle width
            rr.FormatDataBindingInstanceScript = @"
            Dim rr as RenderRectangle = DirectCast(RenderObject, RenderRectangle)
            Dim freight = RenderObject.Original.DataBinding.Parent.Fields!Freight.Value
            Dim sum = Document.DataSchema.Aggregates!SumByEmployee.Value

            Dim f,s as Double

            ' convert objects to double
            If Double.TryParse(freight, f) And Double.TryParse(sum, s) Then

                ' calculate rectangle width                
                Dim w as String = Convert.ToString(Math.Round(f/s, 3), System.Globalization.CultureInfo.InvariantCulture)

                ' Console.WriteLine(""w = "" & w) ' output to console for debug
                
                ' create width expression
                Dim width as String = string.Format(""self.width * {0}"", w)

                rr.Rectangle.Width = width
            End If
            ";

            rrCell.RenderObject = rr;

            rt.Cells[2, 1].Text = "[FormatDateTime(Fields!ShippedDate.Value, DateFormat.ShortDate)]";
            rt.Cells[2, 1].Style.Parents = dataStyle;

            rt.Cells[2, 2].Text = "$[Fields!Freight.Value]";
            rt.Cells[2, 2].Style.Parents = dataStyle;

            rt.Rows[2].CellStyle.Spacing.Left = "3mm";

            // create group by countries
            TableVectorGroup tvg = rt.RowGroups[0, 3];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!ShipCountry.Value");

            // add freight cost sum by countries aggregate
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SumByCountry", "Fields!Freight.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // create group by employee names
            tvg = rt.RowGroups[1, 2];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!EmployeeID.Value");

            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SumByEmployee", "Fields!Freight.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // add data rows
            tvg = rt.RowGroups[2, 1];
            tvg.DataBinding.DataSource = dsSales;

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task ProductsByCategory()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // add tag for products count
            var tag = new Tag("ProductCounter", 0, typeof(int));
            _printDocument.Tags.Add(tag);

            // set default style
            _printDocument.Style.FontName = "Tahoma";
            _printDocument.Style.FontSize = 8;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // set category header style
            C1.C1Preview.Style categoryStyle = _printDocument.Style.Children.Add();
            categoryStyle.FontSize = 12;
            categoryStyle.TextAlignVert = AlignVertEnum.Bottom;

            // define data schema
            var dataSource = CreateDemoDataSource();

            var dsCategories = new DataSet(dataSource,
                "SELECT c.CategoryName, c.Description, p.ProductID, p.ProductName, p.QuantityPerUnit, p.UnitPrice " +
                "FROM Products p, Categories c " +
                "WHERE p.CategoryID = c.CategoryID " +
                "ORDER BY c.CategoryName, p.ProductName");

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(dataSource);
            _printDocument.DataSchema.DataSets.Add(dsCategories);

            // add caption
            var raCaption = new RenderArea();
            raCaption.Style.BackColor = Colors.LightGray;
            raCaption.Style.Padding.All = "2mm";
            raCaption.Style.FontSize = 14;

            var header1 = new RenderText();
            header1.Text = "Products By Category";
            header1.Style.Spacing.Bottom = "2mm";

            var header2 = new RenderText();
            header2.Text = DateTime.Now.ToShortDateString();
            header2.Style.FontSize = 6;
            header2.Style.FontItalic = true;

            raCaption.Children.Add(header1);
            raCaption.Children.Add(header2);

            _printDocument.Body.Children.Add(raCaption);

            // create table
            var rt = new RenderTable();

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // set header
            rt.Cells[0, 0].Text = "[Fields!CategoryName.Value]";
            rt.Cells[0, 0].Style.Parents = categoryStyle;

            rt.Rows[0].Height = "15mm";

            var raProducts = new RenderArea();
            raProducts.Stacking = StackingRulesEnum.InlineLeftToRight;

            _printDocument.ThrowExceptionOnError = true;
            _printDocument.AddWarningsWhenErrorInScript = true;

            var rtt = new RenderText("[RenderObject.DataBinding.RowNumber]) [Fields!ProductName.Value]");
            rtt.DataBinding.DataSource = dsCategories;
            rtt.Width = "30%";
            raProducts.Children.Add(rtt);

            rt.Cells[1, 0].RenderObject = raProducts;

            // create group by category
            var tvg = rt.RowGroups[0, 2];
            tvg.DataBinding.DataSource = dsCategories;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!CategoryName.Value");

            // add aggregate for products
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("ProductCount", "Fields!ProductID.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Count));

            // add data rows
            tvg = rt.RowGroups[1, 1];
            tvg.DataBinding.DataSource = dsCategories;
            tvg.SplitBehavior = SplitBehaviorEnum.Never;

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }
        private async Task EmployeeSalesByCountry()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Tahoma";
            _printDocument.Style.FontSize = 8;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // set category header style
            C1.C1Preview.Style headerCountryStyle = _printDocument.Style.Children.Add();
            headerCountryStyle.FontSize = 12;

            // define data schema
            var dataSource = CreateDemoDataSource();

            // set dates
            var date1 = new DateTime(2015, 9, 7, 0, 0, 0);
            var date2 = new DateTime(2016, 5, 5, 0, 0, 0);

            var dsSales = new DataSet(dataSource, string.Format(
                "SELECT o.ShipCountry, e.EmployeeID, e.FirstName, e.LastName, o.ShippedDate, o.Freight " +
                "FROM Employees e, Orders o " +
                "WHERE e.EmployeeID = o.EmployeeID AND o.ShippedDate IS NOT NULL AND o.ShippedDate BETWEEN #{0} 00:00:00# AND #{1} 00:00:00# " +
                "ORDER BY o.ShipCountry, e.FirstName, e.LastName, o.ShippedDate", date1.ToString(@"MM\/dd\/yyyy"), date2.ToString(@"MM\/dd\/yyyy")));

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(dataSource);
            _printDocument.DataSchema.DataSets.Add(dsSales);

            // add caption
            var raCaption = new RenderArea();
            raCaption.Style.BackColor = Colors.LightGray;
            raCaption.Style.Padding.All = "2mm";
            raCaption.Style.FontSize = 14;

            var header1 = new RenderText();
            header1.Text = "Employee sales by country";
            header1.Style.Spacing.Bottom = "2mm";

            var header2 = new RenderText();
            header2.Text = string.Format("Between {0} and {1}", date1.ToShortDateString(), date2.ToShortDateString());
            header2.Style.FontSize = 6;
            header2.Style.FontItalic = true;

            raCaption.Children.Add(header1);
            raCaption.Children.Add(header2);

            _printDocument.Body.Children.Add(raCaption);

            // create table
            var rt = new RenderTable();

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // header: country
            rt.Cells[0, 0].Text = "[Fields!ShipCountry.Value]";
            rt.Cells[0, 0].Style.Parents = headerCountryStyle;

            rt.Cells[0, 2].Text = "$[Aggregates!SumByCountry.Value]";
            rt.Cells[0, 2].Style.Parents = headerCountryStyle;
            rt.Cells[0, 2].CellStyle.Spacing.Left = "6mm";

            rt.Rows[0].Height = "10mm";
            rt.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;
            rt.Rows[0].Style.Borders.Bottom = new LineDef("1mm", Colors.LightGray);

            // data
            rt.Cells[1, 0].Text = "[Fields!FirstName.Value] [Fields!LastName.Value]";

            rt.Cells[1, 1].Text = "[Math.Round(Aggregates!SumByEmployee.Value / Aggregates!SumByCountry.Value * 100, 1)]%";

            rt.Cells[1, 2].Text = "$[Aggregates!SumByEmployee.Value]";

            rt.Rows[1].CellStyle.Spacing.Left = "6mm";

            // create group by ship country
            TableVectorGroup tvg = rt.RowGroups[0, 2];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!ShipCountry.Value");

            // add total sum by country aggregate
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SumByCountry", "Fields!Freight.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            tvg = rt.RowGroups[1, 1];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!EmployeeID.Value");

            // add total sum by employee aggregate
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SumByEmployee", "Fields!Freight.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // add data rows
            tvg = rt.RowGroups[1, 1];
            tvg.DataBinding.DataSource = dsSales;

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        /// <summary>
        /// Builds a document with a data bound table, with two levels of master-detail relations.
        /// </summary>
        private async Task DataBoundTable()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // Set styles
            _printDocument.Style.FontName = "Verdana";
            _printDocument.Style.FontSize = 10;

            C1.C1Preview.Style boldFontStyle = _printDocument.Style.Children.Add();
            boldFontStyle.FontName = "Verdana";
            boldFontStyle.FontSize = 10;
            boldFontStyle.FontBold = true;

            C1.C1Preview.Style smallFontStyle = _printDocument.Style.Children.Add();
            smallFontStyle.FontName = "Verdana";
            smallFontStyle.FontSize = 7;

            C1.C1Preview.Style detailCaptionStyle = _printDocument.Style.Children.Add();
            detailCaptionStyle.TextAlignHorz = AlignHorzEnum.Center;
            detailCaptionStyle.TextAlignVert = AlignVertEnum.Center;
            detailCaptionStyle.FontName = "Verdana";
            detailCaptionStyle.FontSize = 8;
            detailCaptionStyle.FontUnderline = true;

            C1.C1Preview.Style detailStyle = _printDocument.Style.Children.Add();
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

            // define data schema: orders sorted by customer sorted by customer country
            DataSource ds = CreateDemoDataSource();

            DataSet dsCities = new DataSet(ds,
                "select o.*, c.Country, c.CompanyName, p.ProductName, od.* from orders o, customers c, products p, `order details` od " +
                "where o.CustomerID = c.CustomerID and o.OrderID = od.OrderID and od.ProductID = p.ProductID " +
                "order by c.Country, c.CompanyName, o.OrderDate");

            // add data source and data set to the document - this will preserve the data binding
            // if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(ds);
            _printDocument.DataSchema.DataSets.Add(dsCities);

            // set document caption
            RenderText txt = new RenderText();
            txt.Text = "List of orders grouped by country and customer";
            txt.Style.FontName = "Tahoma";
            txt.Style.FontSize = 16;
            txt.Style.FontBold = true;
            _printDocument.Body.Children.Add(txt);

            // main body of the document is a data bound table, with 3 nested data bound row groups
            RenderTable rt = new RenderTable();

            // the following two lines make table and table columns auto-sized
            rt.Width = Unit.Auto;
            rt.ColumnSizingMode = TableSizingModeEnum.Auto;

            // spread out data a bit
            rt.CellStyle.Padding.All = "0.5mm";

            // turn lines on for structure clarity
            rt.Style.GridLines.All = new LineDef("0.5pt", Colors.Silver);

            // top level master: country
            rt.Cells[0, 0].Text = "Country:";
            rt.Cells[0, 1].SpanCols = 2;
            rt.Cells[0, 1].Text = "[Fields!Country.Value]";
            rt.Cells[0, 1].Style.Parents = boldFontStyle;

            // top level master: orders count for country
            rt.Cells[0, 3].RenderObject = CreateAggregate("Total orders:", "CountryOrderCount", smallFontStyle, boldFontStyle, false);

            // top level master: orders total for country
            rt.Cells[0, 4].RenderObject = CreateAggregate("Country total:", "CountryTotal", smallFontStyle, boldFontStyle, true);

            // country header back color
            rt.Rows[0].Style.BackColor = Colors.LightGoldenrodYellow;

            // second level master: company
            rt.Cells[1, 0].Text = "Company:";
            rt.Cells[1, 1].Text = "[Fields!CompanyName.Value]";
            rt.Cells[1, 1].Style.Parents = boldFontStyle;
            rt.Cells[1, 1].SpanCols = 2;

            // second level master: orders count for company
            rt.Cells[1, 3].RenderObject = CreateAggregate("Total orders:", "CompanyOrderCount", smallFontStyle, boldFontStyle, false);

            // second level master: orders total for company
            rt.Cells[1, 4].RenderObject = CreateAggregate("Company total:", "CompanyTotal", smallFontStyle, boldFontStyle, true);

            // company header back color
            rt.Rows[1].Style.BackColor = Colors.Lavender;

            // detail: column captions
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

            // 1) detail: order date
            rt.Cells[3, 0].Text = "[FormatDateTime(Fields!OrderDate.Value, DateFormat.ShortDate)]";
            rt.Cells[3, 0].Style.Parents = dateStyle;

            // 2) detail: product name
            rt.Cells[3, 1].Text = "[Fields!ProductName.Value]";
            rt.Cells[3, 1].Style.Parents = descriptionStyle;

            // 3) detail: unit price
            rt.Cells[3, 2].Text = "[string.Format(\"{0:C}\",Fields!UnitPrice.Value)]";
            rt.Cells[3, 2].Style.Parents = currencyStyle;

            // 4) detail: quantity
            rt.Cells[3, 3].Text = "[Fields!Quantity.Value]";
            rt.Cells[3, 3].Style.Parents = quantityStyle;

            // 5) detail: order total
            rt.Cells[3, 4].Text = "[string.Format(\"{0:C}\",Fields!UnitPrice.Value * Fields!Quantity.Value)]";
            rt.Cells[3, 4].Style.Parents = currencyStyle;

            // New in 2009 v3 release of C1Report - style expressions
            // Use it to highlight orders worth $1000 or more
            rt.Cells[3, 4].Style.TextColorExpr = "iif(Fields!UnitPrice.Value * Fields!Quantity.Value >= 1000, Colors.Blue, Colors.Black)";

            // top-level master footer: country total (duplicated in cell (0,4))
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

            // grand total for all countries
            rt.Cells[9, 0].SpanCols = 2;
            rt.Cells[9, 0].Text = "Grand Total:";
            rt.Cells[9, 0].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cells[9, 2].SpanCols = 3;
            rt.Cells[9, 2].Text = "[string.Format(\"{0:C}\", Aggregates!GrandTotal.Value)]";
            rt.Cells[9, 2].Style.Parents = currencyStyle;

            // define databinding within table: top level master row group - country - includes all defined rows (0-9)
            TableVectorGroup g = rt.RowGroups[0, 9];
            g.DataBinding.DataSource = dsCities;

            // group by country
            g.DataBinding.Grouping.Expressions.Add("Fields!Country.Value");

            // add outline group header for each contry
            g.DataBinding.OutlineText = "[Fields!Country.Value]";
            g.SplitBehavior = SplitBehaviorEnum.SplitIfNeeded;

            // make sure country header is followed by company and at least one detail row
            rt.RowGroups[0, 1].MinVectorsAfter = 3;

            // add country level aggregates (attached to top level master DataBinding)
            // (because there are several OrderID fields in the 'select' we must qualify it)
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("CountryOrderCount", "Fields(\"o.OrderID\").Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Count));

            _printDocument.DataSchema.Aggregates.Add(new Aggregate("CountryTotal", "Fields!UnitPrice.Value * Fields!Quantity.Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // document level aggregate for the grand total
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("GrandTotal", "Fields!UnitPrice.Value * Fields!Quantity.Value", g.DataBinding, RunningEnum.Document, AggregateFuncEnum.Sum));

            // second level master row group - company - includes rows 1-4
            g = rt.RowGroups[1, 4];
            g.DataBinding.DataSource = dsCities;

            // group by company
            g.DataBinding.Grouping.Expressions.Add("Fields!CompanyName.Value");

            // add outline group header for each company
            g.DataBinding.OutlineText = "[Fields!CompanyName.Value]";
            g.SplitBehavior = SplitBehaviorEnum.SplitIfNeeded;

            // ensure company header is followed by at least one detail row
            rt.RowGroups[1, 2].MinVectorsAfter = 1;
            rt.RowGroups[1, 2].SplitBehavior = SplitBehaviorEnum.SplitIfLarge;

            // add company level aggregates (attached to second level master DataBinding)
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("CompanyTotal", "Fields!UnitPrice.Value * Fields!Quantity.Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // see note about OrderID above; but really, because we are just counting, any field would do here - e.g. CustomerID
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("CompanyOrderCount", "Fields(\"o.OrderID\").Value", g.DataBinding, RunningEnum.Group, AggregateFuncEnum.Count));

            // finally, detail level data binding (just one row)
            g = rt.RowGroups[3, 1];
            g.DataBinding.DataSource = dsCities;

            // add outline entry for each product name
            g.DataBinding.OutlineText = "[Fields!ProductName.Value]";

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // we're done; reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task CrossTabReports()
        {
            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Tahoma";
            _printDocument.Style.FontSize = 8;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            var rtHeader = new RenderText();
            rtHeader.Text = "Cross-tab Reports";
            rtHeader.Style.FontSize = 16;

            _printDocument.Body.Children.Add(rtHeader);

            // add empty space
            _printDocument.Body.Children.Add(new RenderEmpty("3mm"));

            // define data schema
            var dataSource = CreateDemoDataSource();

            var dsSales = new DataSet(dataSource,
                "SELECT Orders.ShipCountry, Categories.CategoryName, Year([OrderDate]) AS OrderYear, " +
                        "Sum([Order Details].[UnitPrice] * [Order Details].[Quantity] - [Discount]) AS SaleAmount, DatePart(\"q\",[OrderDate]) AS OrderQtr " +
                "FROM Categories INNER JOIN(Products INNER JOIN (Orders INNER JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID) " +
                        "ON Products.ProductID = [Order Details].ProductID) ON Categories.CategoryID = Products.CategoryID " +
                "GROUP BY Orders.ShipCountry, Categories.CategoryName, Year([OrderDate]), DatePart(\"q\",[OrderDate]) " +
                "ORDER BY Orders.ShipCountry, Categories.CategoryName, Year([OrderDate])");

            // add data source and data set to the document: this will preserve the data binding if the document is saved as c1d/c1dx
            _printDocument.DataSchema.DataSources.Add(dataSource);
            _printDocument.DataSchema.DataSets.Add(dsSales);

            // create table
            var rt = new RenderTable();

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // header 1: country
            rt.Cells[0, 0].Text = "[Fields!ShipCountry.Value]";

            rt.Rows[0].Height = "15mm";
            rt.Rows[0].Style.FontSize = 16;
            rt.Rows[0].Style.TextColor = Colors.DarkBlue;
            rt.Rows[0].Style.TextAlignVert = AlignVertEnum.Bottom;

            // header 2
            rt.Cells[1, 0].Text = "Product Category";
            rt.Cells[1, 1].Text = "Year";

            rt.Cells[1, 2].Text = "Sales";
            rt.Cells[1, 2].Style.TextAlignHorz = AlignHorzEnum.Right;

            rt.Cells[1, 3].Text = "Q1";
            rt.Cells[1, 3].Style.TextAlignHorz = AlignHorzEnum.Right;

            rt.Cells[1, 4].Text = "Q2";
            rt.Cells[1, 4].Style.TextAlignHorz = AlignHorzEnum.Right;

            rt.Cells[1, 5].Text = "Q3";
            rt.Cells[1, 5].Style.TextAlignHorz = AlignHorzEnum.Right;

            rt.Cells[1, 6].Text = "Q4";
            rt.Cells[1, 6].Style.TextAlignHorz = AlignHorzEnum.Right;

            rt.Rows[1].Style.FontBold = true;
            rt.Rows[1].Style.BackColor = System.Windows.Media.Color.FromArgb(255, 245, 245, 245);

            // data row
            rt.Cells[2, 0].Text = "[Fields!CategoryName.Value]";

            rt.Cells[2, 2].Style.Borders.Right = new LineDef("0.5pt", Colors.Gray);

            rt.Cells[3, 1].Text = "[Fields!OrderYear.Value]";

            rt.Cells[3, 2].Text = "$[Aggregates!SaleAmountTotal.Value]";

            rt.Cells[3, 2].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cells[3, 2].Style.Borders.Right = new LineDef("0.5pt", Colors.Gray);

            // quarter 1
            rt.Cells[3, 3].Text = "$[Aggregates!SaleAmountQ1.Value]";
            rt.Cells[3, 3].Style.TextAlignHorz = AlignHorzEnum.Right;

            // quarter 2
            rt.Cells[3, 4].Text = "$[Aggregates!SaleAmountQ2.Value]";
            rt.Cells[3, 4].Style.TextAlignHorz = AlignHorzEnum.Right;

            // quarter 3
            rt.Cells[3, 5].Text = "$[Aggregates!SaleAmountQ3.Value]";
            rt.Cells[3, 5].Style.TextAlignHorz = AlignHorzEnum.Right;

            // quarter 4
            rt.Cells[3, 6].Text = "$[Aggregates!SaleAmountQ4.Value]";
            rt.Cells[3, 6].Style.TextAlignHorz = AlignHorzEnum.Right;

            // total row
            rt.Cells[4, 0].Text = "Total";
            rt.Cells[4, 2].Text = "$[Aggregates!Total.Value]";
            rt.Cells[4, 2].Style.TextAlignHorz = AlignHorzEnum.Right;

            // total sales in quarter 1 by country
            rt.Cells[4, 3].Text = "$[Aggregates!SaleAmountTotalQ1.Value]";
            rt.Cells[4, 3].Style.TextAlignHorz = AlignHorzEnum.Right;

            // total sales in quarter 2 by country
            rt.Cells[4, 4].Text = "$[Aggregates!SaleAmountTotalQ2.Value]";
            rt.Cells[4, 4].Style.TextAlignHorz = AlignHorzEnum.Right;

            // total sales in quarter 3 by country
            rt.Cells[4, 5].Text = "$[Aggregates!SaleAmountTotalQ3.Value]";
            rt.Cells[4, 5].Style.TextAlignHorz = AlignHorzEnum.Right;

            // total sales in quarter 4 by country
            rt.Cells[4, 6].Text = "$[Aggregates!SaleAmountTotalQ4.Value]";
            rt.Cells[4, 6].Style.TextAlignHorz = AlignHorzEnum.Right;

            // set row style
            rt.Rows[4].Style.BackColor = System.Windows.Media.Color.FromArgb(255, 245, 245, 245);
            rt.Rows[4].Style.TextColor = Colors.DarkViolet;
            rt.Rows[4].Style.FontBold = true;

            // auto-size first column, spread the rest columns
            rt.Cols[0].SizingMode = TableSizingModeEnum.Auto;

            // create group by ship country
            var tvg = rt.RowGroups[0, 5];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!ShipCountry.Value");

            // create group by product name
            tvg = rt.RowGroups[2, 2];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!CategoryName.Value");

            // create group by order year
            tvg = rt.RowGroups[3, 1];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!OrderYear.Value");

            // add aggregate for sale amount by year
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountTotal", "Fields!SaleAmount.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // add aggregates for quarters by year
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountQ1", "IIF(Fields!OrderQtr.Value = 1, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountQ2", "IIF(Fields!OrderQtr.Value = 2, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountQ3", "IIF(Fields!OrderQtr.Value = 3, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountQ4", "IIF(Fields!OrderQtr.Value = 4, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // create group by ship country
            tvg = rt.RowGroups[4, 1];
            tvg.DataBinding.DataSource = dsSales;
            tvg.DataBinding.Grouping.Expressions.Add("Fields!ShipCountry.Value");

            // add aggregates for quarters by country
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountTotalQ1", "IIF(Fields!OrderQtr.Value = 1, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountTotalQ2", "IIF(Fields!OrderQtr.Value = 2, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountTotalQ3", "IIF(Fields!OrderQtr.Value = 3, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("SaleAmountTotalQ4", "IIF(Fields!OrderQtr.Value = 4, Fields!SaleAmount.Value, 0)", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // add aggregate for total sales amount
            _printDocument.DataSchema.Aggregates.Add(new Aggregate("Total", "Fields!SaleAmount.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            // add table to the document
            _printDocument.Body.Children.Add(rt);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task BalanceSheet()
        {
            // create data for left area
            var currentAssets = new List<AssetItem>();

            currentAssets.Add(new AssetItem() { Name = "Current Assets" });
            currentAssets.Add(new AssetItem() { Name = "Cash in Bank", Cost = 45000 });
            currentAssets.Add(new AssetItem() { Name = "Inventory", Cost = 45000 });
            currentAssets.Add(new AssetItem() { Name = "Prepaid Expenses", Cost = 600 });
            currentAssets.Add(new AssetItem() { Name = "Other", Cost = 10000 });

            var fixedAssets = new List<AssetItem>();

            fixedAssets.Add(new AssetItem() { Name = "Fixed Assets" });
            fixedAssets.Add(new AssetItem() { Name = "Machinary & Equipment", Cost = 56200 });
            fixedAssets.Add(new AssetItem() { Name = "Furniture & Fixtures", Cost = 32400 });
            fixedAssets.Add(new AssetItem() { Name = "Leasehold Improvements", Cost = 6300 });
            fixedAssets.Add(new AssetItem() { Name = "Real Estate/Buildings", Cost = 6250000 });
            fixedAssets.Add(new AssetItem() { Name = "Other", Cost = 7000 });

            var otherAssets = new List<AssetItem>();

            otherAssets.Add(new AssetItem() { Name = "Other Assets" });
            otherAssets.Add(new AssetItem() { Name = "Receivable from employee", Cost = 12500 });
            otherAssets.Add(new AssetItem() { Name = "Receivable from clients", Cost = 32600 });
            otherAssets.Add(new AssetItem() { Name = "Intangible assets", Cost = 3200 });

            // create data for right area
            var currentLiabilities = new List<AssetItem>();

            currentLiabilities.Add(new AssetItem() { Name = "Current Liabilities" });
            currentLiabilities.Add(new AssetItem() { Name = "Accounts Payable", Cost = 2585600 });
            currentLiabilities.Add(new AssetItem() { Name = "Taxes Payable", Cost = 56263 });
            currentLiabilities.Add(new AssetItem() { Name = "Notes Payable (due within 12 months)", Cost = 216 });
            currentLiabilities.Add(new AssetItem() { Name = "Current Portion Long-term Debt", Cost = 3800 });
            currentLiabilities.Add(new AssetItem() { Name = "Other Current Liabilities (specify)", Cost = 3000 });

            var longTermLiabilities = new List<AssetItem>();

            longTermLiabilities.Add(new AssetItem() { Name = "Long-Term Liabilities" });
            longTermLiabilities.Add(new AssetItem() { Name = "Bank Loans Payable (greater then 12 months)", Cost = 200 });
            longTermLiabilities.Add(new AssetItem() { Name = "Less: Short-term Portion", Cost = 560 });
            longTermLiabilities.Add(new AssetItem() { Name = "Notes Payable to Stockholders", Cost = 6203 });
            longTermLiabilities.Add(new AssetItem() { Name = "Other Long-term Debts (specify)", Cost = 450 });

            var shareholdersEquity = new List<AssetItem>();

            shareholdersEquity.Add(new AssetItem() { Name = "Shareholders Equity" });
            shareholdersEquity.Add(new AssetItem() { Name = "Common Stock", Cost = 16300 });
            shareholdersEquity.Add(new AssetItem() { Name = "Additional Paid-in Capital", Cost = 8500 });
            shareholdersEquity.Add(new AssetItem() { Name = "Retained Earnings", Cost = 3819708 });

            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Tahoma";
            _printDocument.Style.FontSize = 8;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // add title
            var rtTitle = new RenderText();
            rtTitle.Text = "Balance Sheet";
            rtTitle.Height = "15mm";
            rtTitle.Style.FontSize = 16;
            rtTitle.Style.TextAlignVert = AlignVertEnum.Center;
            rtTitle.Style.TextColor = Colors.DarkMagenta;
            rtTitle.Style.BackColor = System.Windows.Media.Color.FromArgb(255, 250, 250, 250);
            rtTitle.Style.Padding.Left = "10mm";

            _printDocument.Body.Children.Add(rtTitle);

            // add empty space
            _printDocument.Body.Children.Add(new RenderEmpty("5mm"));

            // the add area contains two areas side by side
            var raContainer = new RenderArea();
            raContainer.Stacking = StackingRulesEnum.InlineLeftToRight;

            var raLeft = new RenderArea();
            raLeft.Width = "50%";
            raLeft.Style.Padding.Right = "5mm";
            raContainer.Children.Add(raLeft);

            // calculate assets total cost
            var assetsTotalCost = currentAssets.Sum(x => x.Cost) + fixedAssets.Sum(x => x.Cost) + otherAssets.Sum(x => x.Cost);

            // add data for left area
            raLeft.Children.Add(CreateHeader("Total Assets", assetsTotalCost));
            raLeft.Children.Add(CreateRenderArea(currentAssets, "currentAssetsTotal"));
            raLeft.Children.Add(CreateRenderArea(fixedAssets, "fixedAssetsTotal"));
            raLeft.Children.Add(CreateRenderArea(otherAssets, "otherAssetsTotal"));

            var raRight = new RenderArea();
            raRight.Width = "50%";
            raRight.Style.Padding.Left = "5mm";
            raContainer.Children.Add(raRight);

            // calculate liabilities total sum
            var liabilitiesTotal = currentLiabilities.Sum(x => x.Cost) + longTermLiabilities.Sum(x => x.Cost) + shareholdersEquity.Sum(x => x.Cost);

            // add data for right area
            raRight.Children.Add(CreateHeader("Total Liabilities", liabilitiesTotal));
            raRight.Children.Add(CreateRenderArea(currentLiabilities, "currentLiabilitiesTotal"));
            raRight.Children.Add(CreateRenderArea(longTermLiabilities, "longTermLiabilitiesTotal"));
            raRight.Children.Add(CreateRenderArea(shareholdersEquity, "shareholdersEquityTotal"));

            _printDocument.Body.Children.Add(raContainer);

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        private async Task PriceComparison()
        {
            // create data for products
            var mostPopularTv = new List<ProductItem>();

            mostPopularTv.Add(new ProductItem() { Name = "Vizio 60\" LED HDTV", Price2013 = 688, Price2014 = 799 });
            mostPopularTv.Add(new ProductItem() { Name = "Emerson 50 \" LCD HDTV", Price2013 = 299, Price2014 = 100 });
            mostPopularTv.Add(new ProductItem() { Name = "Funai 64\" LED HDTV", Price2013 = 102, Price2014 = 150 });

            var mostPopularTablet = new List<ProductItem>();

            mostPopularTablet.Add(new ProductItem() { Name = "Apple Ipad Air 16 GB Wi-Fi", Price2013 = 688, Price2014 = 788 });
            mostPopularTablet.Add(new ProductItem() { Name = "Apple Ipad min 16 GB Wi-Fi", Price2013 = 299, Price2014 = 200 });
            mostPopularTablet.Add(new ProductItem() { Name = "Samsung 24\" 16 GB Wi-Fi", Price2013 = 102, Price2014 = 90 });

            var mostPopularOther = new List<ProductItem>();

            mostPopularOther.Add(new ProductItem() { Name = "Samsung Max 24\" 16 GB Wi-Fi", Price2013 = 102, Price2014 = 123 });
            mostPopularOther.Add(new ProductItem() { Name = "HP min 16 GB Wi-Fi", Price2013 = 299, Price2014 = 340 });
            mostPopularOther.Add(new ProductItem() { Name = "Samsung guru 16 GB Wi-Fi", Price2013 = 102, Price2014 = 230 });

            var mostPopularConsole = new List<ProductItem>();

            mostPopularConsole.Add(new ProductItem() { Name = "XBox One with KitNet", Price2013 = 299, Price2014 = 120 });
            mostPopularConsole.Add(new ProductItem() { Name = "Play Station 435", Price2013 = 102, Price2014 = 300 });
            mostPopularConsole.Add(new ProductItem() { Name = "Play Station with games", Price2013 = 299, Price2014 = 250 });

            this.Cursor = Cursors.Wait;

            _printDocument.Clear();
            await Task.Delay(1);

            // set default style
            _printDocument.Style.FontName = "Tahoma";
            _printDocument.Style.FontSize = 8;

            // set margins
            _printDocument.PageLayout.PageSettings.LeftMargin = "12mm";
            _printDocument.PageLayout.PageSettings.RightMargin = "12mm";
            _printDocument.PageLayout.PageSettings.TopMargin = "12mm";
            _printDocument.PageLayout.PageSettings.BottomMargin = "12mm";

            // add title
            var rtTitle = new RenderText();
            rtTitle.Text = "Black`s Friday Most Popular";
            rtTitle.Height = "15mm";
            rtTitle.Style.FontSize = 16;
            rtTitle.Style.TextAlignVert = AlignVertEnum.Center;
            rtTitle.Style.TextColor = Colors.DarkBlue;
            rtTitle.Style.BackColor = System.Windows.Media.Color.FromArgb(255, 230, 230, 230);
            rtTitle.Style.Padding.Left = "10mm";

            _printDocument.Body.Children.Add(rtTitle);

            // add empty space
            _printDocument.Body.Children.Add(new RenderEmpty("5mm"));

            // create tables
            _printDocument.Body.Children.Add(CreateMostPopular(mostPopularTv, "TV", "tv"));
            _printDocument.Body.Children.Add(CreateMostPopular(mostPopularTablet, "Tablet", "tablet"));
            _printDocument.Body.Children.Add(CreateMostPopular(mostPopularOther, "Other", "other"));
            _printDocument.Body.Children.Add(CreateMostPopular(mostPopularConsole, "Console", "console"));

            // generate document
            _printDocument.Generate();

            // reset cursor
            this.Cursor = _defaultCursor;
        }

        #region ** helper methods

        /// <summary>
        /// Creates the demo data source using "c1nwind.mdb" database located in user's home dir, such as:
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
        /// Get connection string for c1nwind.mdb.
        /// </summary>
        private static string GetConnectionString()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ComponentOne Samples\Common";
            string conn = @"provider=microsoft.jet.oledb.4.0;data source={0}\c1nwind.mdb;";
            return string.Format(conn, path);
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
        private RenderObject CreateAggregate(string caption, string aggregateName, C1.C1Preview.Style captionStyle, C1.C1Preview.Style aggregateStyle, bool currency)
        {
            RenderParagraph result = new RenderParagraph();
            ParagraphText pt = new ParagraphText(caption + "\r");
            pt.Style.Parents = captionStyle;
            result.Content.Add(pt);

            if (currency)
            {
                pt = new ParagraphText("[string.Format(\"{0:C}\",Aggregates!" + aggregateName + ".Value)]");
            }
            else
            {
                pt = new ParagraphText("[Aggregates!" + aggregateName + ".Value]");
            }

            pt.Style.Parents = aggregateStyle;
            result.Content.Add(pt);
            return result;
        }

        /// <summary>
        /// Add RenderText to RenderArea
        /// </summary>
        /// <param name="renderArea">The RenderArea container.</param>
        /// <param name="text">The text for added RenderText.</param>
        /// <returns></returns>
        private RenderArea AddTextBlocks(RenderArea renderArea, string text)
        {
            var renderText = new RenderText();
            renderText.Text = text;

            renderArea.Children.Add(renderText);

            // add empty space
            renderArea.Children.Add(new RenderEmpty("1mm"));

            return renderArea;
        }

        private RenderArea CreateRenderArea(List<AssetItem> assetList, string aggregateName)
        {
            var renderArea = new RenderArea();

            var rtTitle = new RenderText();

            rtTitle.Text = assetList[0].Name;

            // set title parameters
            rtTitle.Style.BackColor = System.Windows.Media.Color.FromArgb(255, 245, 245, 245);
            rtTitle.Style.FontSize = 12;
            rtTitle.Height = "7mm";
            rtTitle.Style.TextAlignVert = AlignVertEnum.Center;
            rtTitle.Style.Padding.All = "1mm";

            // add title
            renderArea.Children.Add(rtTitle);

            // add table
            var rt = new RenderTable();

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            rt.Cells[0, 0].Text = "[Fields!Name.Value]";
            rt.Cells[0, 1].Text = "$[Fields!Cost.Value]";

            // cost aligned to right
            rt.Cells[0, 1].Style.TextAlignHorz = AlignHorzEnum.Right;

            // add to row the line at bottom
            rt.Rows[0].Style.Borders.Bottom = new LineDef("0.5pt", Colors.Gray);

            rt.Cells[1, 0].Text = "Total";
            rt.Cells[1, 1].Text = "$[Aggregates!" + aggregateName + ".Value]";

            // total cost aligned to right
            rt.Cells[1, 1].Style.TextAlignHorz = AlignHorzEnum.Right;

            // set row parameters
            rt.Rows[1].Style.BackColor = System.Windows.Media.Color.FromArgb(255, 245, 245, 245);
            rt.Rows[1].Style.TextColor = Colors.DarkMagenta;
            rt.Rows[1].Style.FontSize = 10;

            // auto-size first column, spread the rest columns
            rt.Cols[0].SizingMode = TableSizingModeEnum.Auto;

            // add data rows
            var tvg = rt.RowGroups[0, 1];

            // set list except first element
            tvg.DataBinding.DataSource = assetList.Skip(1);

            // add aggregate for cost
            _printDocument.DataSchema.Aggregates.Add(new Aggregate(aggregateName, "Fields!Cost.Value", tvg.DataBinding, RunningEnum.Group, AggregateFuncEnum.Sum));

            renderArea.Children.Add(rt);

            // add empty space
            renderArea.Children.Add(new RenderEmpty("10mm"));

            return renderArea;
        }

        private RenderArea CreateHeader(string totalText, double cost)
        {
            var renderArea = new RenderArea();

            var rt = new RenderTable();

            rt.Cells[0, 0].SpanCols = 2;
            rt.Rows[0].Height = "3mm";
            rt.Rows[0].Style.BackColor = Colors.LightYellow;

            rt.Cells[1, 0].Text = totalText;
            rt.Cells[1, 0].CellStyle.Padding.Left = "3mm";

            rt.Cells[1, 1].Text = string.Format("${0}", cost);
            rt.Cells[1, 1].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cells[1, 1].CellStyle.Padding.Right = "1mm";

            rt.Rows[1].Style.FontSize = 14;
            rt.Rows[1].Height = "12mm";
            rt.Rows[1].Style.TextAlignVert = AlignVertEnum.Center;
            rt.Rows[1].Style.BackColor = System.Windows.Media.Color.FromArgb(255, 245, 245, 245);
            rt.Rows[1].Style.TextColor = Colors.DarkMagenta;

            // auto-size first column, spread the rest columns
            rt.Cols[0].SizingMode = TableSizingModeEnum.Auto;

            renderArea.Children.Add(rt);

            // add empty space
            renderArea.Children.Add(new RenderEmpty("5mm"));

            return renderArea;
        }

        private RenderArea CreateMostPopular(List<ProductItem> productList, string product, string imageName)
        {
            var raContainer = new RenderArea();

            var raInner = new RenderArea();
            raInner.Stacking = StackingRulesEnum.InlineLeftToRight;

            var riPicture = new RenderImage();
            riPicture.Width = "15%";

            // get image from resources by name
            riPicture.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(imageName, Properties.Resources.Culture);

            var rt = new RenderTable();
            rt.Width = "85%";

            // set cell padding
            rt.CellStyle.Padding.All = "1mm";

            // add header row
            rt.Cells[0, 0].Text = "2013's Most Popular " + product;
            rt.Cells[0, 1].Text = "2013 Price";
            rt.Cells[0, 2].Text = ""; // empty column
            rt.Cells[0, 3].Text = "2014's Most Popular " + product; ;
            rt.Cells[0, 4].Text = "2014 Price";
            rt.Cells[0, 5].Text = "%";
            rt.Cells[0, 6].Text = "Change";

            rt.Cells[0, 5].Style.TextAlignHorz = AlignHorzEnum.Right;

            rt.Rows[0].Height = "10mm";
            rt.Rows[0].Style.FontSize = 9;
            rt.Rows[0].Style.TextColor = Colors.DarkBlue;
            rt.Rows[0].Style.TextAlignVert = AlignVertEnum.Center;
            rt.Rows[0].Style.BackColor = System.Windows.Media.Color.FromArgb(255, 230, 230, 230);

            // add data rows
            rt.Cells[1, 0].Text = "[Fields!Name.Value]";
            rt.Cells[1, 1].Text = "$[Fields!Price2013.Value]";
            rt.Cells[1, 3].Text = "[Fields!Name.Value]";
            rt.Cells[1, 4].Text = "$[Fields!Price2014.Value]";

            // create rectangle
            var rr = new RenderRectangle();

            var rrCell = rt.Cells[1, 5];

            rrCell.Style.BackColor = System.Windows.Media.Color.FromArgb(255, 245, 245, 245);

            // set rectangle location and size
            rr.X = new Unit(rrCell.Bounds.Left, _printDocument.ResolvedUnit);
            rr.Y = new Unit(rrCell.Bounds.Top, _printDocument.ResolvedUnit);
            rr.Width = new Unit(rrCell.Bounds.Width, _printDocument.ResolvedUnit);
            rr.Height = new Unit(rrCell.Bounds.Height, _printDocument.ResolvedUnit);

            // show all exceptions and warnings for script debug
            _printDocument.ThrowExceptionOnError = true;
            _printDocument.AddWarningsWhenErrorInScript = true;

            // calculate and set rectangle parameters
            rr.FormatDataBindingInstanceScript = @"
                Dim rr as RenderRectangle = DirectCast(RenderObject, RenderRectangle)
                Dim price2013 as Double = RenderObject.Original.DataBinding.Parent.Fields!Price2013.Value
                Dim price2014 as Double = RenderObject.Original.DataBinding.Parent.Fields!Price2014.Value
                
                ' calculating the price difference as a percentage                
                Dim p as Double = (price2014 - price2013) / price2014 * 100

                Dim w as Double = Math.Abs(p)

                ' the width limitation
                If w > 100 Then
                    w = 100
                End If

                ' create width expression, half the width is used
                Dim width as String = string.Format(System.Globalization.CultureInfo.InvariantCulture, ""(self.width / 2) * {0:#.###} / 100"", w)

                ' set rectangle width
                rr.Rectangle.Width = width

                If p > 0 Then
                    ' set rectangle X position
                    rr.Rectangle.X = new Unit(""self.x + (self.width / 2)"")
            
                    ' set rectangle border color
                    rr.Style.ShapeLine = new LineDef(""0.5pt"", Colors.Green)

                    ' set rectangle fill color
                    rr.Style.ShapeFillColor = Colors.Green
                Else
                    ' set rectangle X position
                    rr.Rectangle.X = New Unit(""self.x + (self.width / 2) - "" & width)
                    
                    ' set rectangle border color
                    rr.Style.ShapeLine = new LineDef(""0.5pt"", Colors.Red)

                    ' set rectangle fill color
                    rr.Style.ShapeFillColor = Colors.Red
                End If
            ";

            rrCell.RenderObject = rr;

            rt.Cells[1, 6].Text = "[string.Format(\"{0:0}\", (Fields!Price2014.Value - Fields!Price2013.Value) / Fields!Price2014.Value * 100)]%";

            rt.Rows[1].Style.Borders.Bottom = new LineDef("0.5pt", Colors.LightGray);
            rt.Cells[1, 2].Style.Borders.Bottom = LineDef.Empty; ;

            rt.Cols[0].Width = "23%";
            rt.Cols[2].Width = "4mm";
            rt.Cols[3].Width = "23%";
            rt.Cols[5].Width = "15%";

            rt.Cols[1].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cols[4].Style.TextAlignHorz = AlignHorzEnum.Right;
            rt.Cols[6].Style.TextAlignHorz = AlignHorzEnum.Right;

            // add data group
            TableVectorGroup tvg = rt.RowGroups[1, 1];
            tvg.DataBinding.DataSource = productList;

            raInner.Children.Add(riPicture);
            raInner.Children.Add(rt);

            raContainer.Children.Add(raInner);

            // add empty space
            raContainer.Children.Add(new RenderEmpty("5mm"));

            return raContainer;
        }

        private System.Data.DataTable GetDataSource()
        {
            // set up connection string
            var conn = GetConnectionString();

            // set up SQL statement
            var sql = "SELECT c.CategoryName, p.ProductName, p.UnitPrice, p.UnitsInStock " +
                "FROM Products p, Categories c " +
                "WHERE p.CategoryID = c.CategoryID " +
                "ORDER BY c.CategoryName, p.ProductName";

            // retrieve data into DataSet
            var da = new System.Data.OleDb.OleDbDataAdapter(sql, conn);
            var ds = new System.Data.DataSet();

            da.Fill(ds);

            return ds.Tables[0];
        }

        #endregion ** helper methods

        #region ** data elements

        class AssetItem
        {
            public string Name { get; set; }
            public double Cost { get; set; }
        }

        class ProductItem
        {
            public string Name { get; set; }
            public double Price2013 { get; set; }
            public double Price2014 { get; set; }
        }

        #endregion ** data elements	

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.CheckPathExists = true;
            dlg.OverwritePrompt = true;
            dlg.Title = "Export";

            dlg.FileName = "Document";
            dlg.ValidateNames = true;

            StringBuilder sb = new StringBuilder();
            var providers = ExportProviders.RegisteredProviders;
            int filterIndex = 0;
            for (int i = 0; i < providers.Count; i++)
            {
                var p = providers[i];
                if (p is C1mdxExportProvider) // only works for MultiDocument
                {
                    continue;
                }
                if ( p is PdfExportProvider)
                {
                    filterIndex = i;
                }
                if (i > 0)
                {
                    sb.Append('|');
                }
                sb.Append(p.FormatName);
                sb.Append(" (*.");
                sb.Append(p.DefaultExtension);
                sb.Append(")|*.");
                sb.Append(p.DefaultExtension);
            }
            dlg.Filter = sb.ToString();
            dlg.FilterIndex = filterIndex + 1;

            var res = dlg.ShowDialog();
            int index = dlg.FilterIndex - 1;
            if (res.HasValue && res.Value && index >= 0 && index < providers.Count)
            {
                _printDocument.Export(dlg.FileName);
            }
        }
    }
}
