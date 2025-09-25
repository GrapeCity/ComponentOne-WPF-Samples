using C1.Zip;
using C1.DataCollection;
using C1.DataFilter;
using C1.WPF;
using C1.WPF.C1Chart;
using C1.WPF.Carousel;
using C1.WPF.DataFilter;
using C1.WPF.ExpressionEditor;
using C1.WPF.FlexGrid;
using C1.WPF.FlexReport;
using C1.WPF.OutlookBar;
using C1.WPF.RichTextBox;
using C1.WPF.Sparkline;
using C1.WPF.Theming;
using C1.WPF.Theming.Material;
using C1.WPF.Theming.MaterialDark;
using C1.WPF.Theming.ShinyBlue;
using C1.WPF.Toolbar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using static Themes.Person;

namespace Themes
{
    public partial class Gallery : UserControl
    {
        Popup _window = new Popup();
        Border _border = new Border() { BorderThickness = new Thickness(2), BorderBrush = new SolidColorBrush(Colors.LightGray) };
		C1FlexReport _flexReport;
        Stream _reportStream;
        string _asmName = string.Empty;
        SampleData sampleData = new SampleData();
        DataProvider _dataProvider = new DataProvider();
        C1DataCollection<Car> cars;
        bool _isRTBInitialized = false;

        public Gallery()
        {
            InitializeComponent();

            // generate sample data
            var persons = Person.Generate(80);
            _asmName = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name;

            #region FlexGrid
            flexGrid.ItemsSource = persons;
            foreach (Column c in flexGrid.Columns)
            {
                if (c.ColumnName == "Occupation")
                {
                    var dct = new Dictionary<int, string>();
                    foreach (var occ in Enum.GetValues(typeof(OccupationEnum)))
                    {
                        dct[dct.Count] = occ.ToString();
                    }
                    c.ValueConverter = new ColumnValueConverter(dct);
                }
            }
            #endregion
            grid.ItemsSource = persons;
            listBox.ItemsSource = persons;

            propGrid1.SelectedObject = persons[0];
            propGrid2.SelectedObject = persons[1];
            MultiSelect.ItemsSource = Person.Generate(80);
            MultiSelect.SelectedItem = persons[0];
            InputPanel.ItemsSource = persons;
            CheckList.ItemsSource = persons;
            TagEditor.InsertTag("John");
            TagEditor.InsertTag("Jemmy Harden");
            ViewType.ItemsSource = Enum.GetValues(typeof(C1.WPF.Schedule.ViewType));

            #region OrgChart
            // create data object
            var league = League.GetLeague();
            // show it in C1OrgChart
            _orgChart.Header = league;
            #endregion

            #region OutlookBar
            transformerBorder.MouseEnter += new MouseEventHandler(transformerBorder_MouseEnter);
            _window.Placement = PlacementMode.Relative;
            _window.PlacementTarget = c1OutlookBar1;
            _window.Child = _border;
            _border.MouseLeave += new MouseEventHandler(window_MouseLeave);
            c1OutlookBar1.SelectedItemChanged += new EventHandler<PropertyChangedEventArgs<object>>(c1OutlookBar1_SelectedItemChanged);
            c1OutlookBar1.SizeChanged += new SizeChangedEventHandler(c1OutlookBar1_SizeChanged);
            #endregion

            #region Sparkline
            sparklineType.ItemsSource = Enum.GetValues(typeof(SparklineType));
            sparklineType.SelectedItem = sparkline.SparklineType;
            sparkline.Data = sampleData.DefaultData;

            sparkline.ShowMarkers = true;
            sparkline.DisplayXAxis = true;
            #endregion

            #region Carousel
            for (int i = 101; i <= 125; ++i)
            {
                carouselListBox.Items.Add(new BitmapImage(new Uri("Resources/covers/cover" + i + ".jpg", UriKind.Relative)));
            }
            #endregion

            //Need to init FlexSheet to avoid exception if theme is empty on start up
            InitFlexSheet();
        }

        // prepare data at first load
        private void DataManagement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;
            var item = e.AddedItems[0] as C1TabItem;
            #region Filter Editor
            if (item.Header.Equals("Filter Editor") && filterEditor.ItemsSource == null)
            {
                // initialize filter editor
                cars = new C1DataCollection<Car>(_dataProvider.GetCars());

                filterEditor.ItemsSource = cars;
                filterEditorFlexGrid.ItemsSource = cars;
                InitExpressionAndApply();
                filterEditor.FilterChanged += filterEditor_FilterChanged;
            }
            #endregion
            #region Olap
            if (item.Header.Equals("OLAP") && olapPage.DataSource == null)
            {
                var persons = Person.Generate(80);
                olapPage.DataSource = persons;
                olapPage.Loaded += olapPage_Loaded;
            }
            #endregion
            #region DataFilter
            if (item.Header.Equals("Data Filter") && c1DataFilter1.ItemsSource == null)
            {
                var data = new C1DataCollection<Employee>(_dataProvider.GetEmployees());
                c1DataFilter1.ItemsSource = data;
                dataFilterFlexGrid.ItemsSource = data;

                foreach (ChecklistFilter filter in c1DataFilter1.Filters.Where(f => f is ChecklistFilter))
                {
                    filter.SelectAll();
                    filter.DisplayedItems = 2;
                }
            }
            #endregion
        }

        private void InputEditing_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;
            var item = e.AddedItems[0] as C1TabItem;
            #region ExpressionEditor
            if (item.Header.Equals("ExpressionEditor") && ExpressionEditor.DataSource == null)
            {
                var persons = Person.Generate(80);
                ExpressionEditor.DataSource = persons;
                ExpressionEditor.ExpressionChanged += ExpressionEditor_ExpressionChanged;
            }
            #endregion
            #region RichTextBox
            if (item.Header.Equals("RichTextBox") && !_isRTBInitialized)
            {
                using (var stream = Application.GetResourceStream(new Uri("/" + _asmName + ";component/RichTextBox/dickens.htm", UriKind.Relative)).Stream)
                {
                    var html = new StreamReader(stream).ReadToEnd();
                    richTB.Html = html;
                    richTB.NavigationMode = NavigationMode.Always;
                }
                _isRTBInitialized = true;
            }
            #endregion
        }

        private void C1TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count < 1)
                return;
            var item = e.AddedItems[0] as C1TabItem;
            if (item.Header.Equals("Reporting & Documents") && fv.DocumentListCombo.Items.Count < 1)
            {
                #region FlexViewer
                _flexReport = new C1FlexReport();
                ExtractC1NWind();
                LoadReportList();
                #endregion
            }
        }

        private void InitFlexSheet()
        {
           
            flexSheet.AddSheet("Sheet1", 50, 10);

            #region Populate Sheet1
            // populate the grid with some formulas (multiplication table)
            for (int r = 0; r < flexSheet.Rows.Count - 2; r++)
            {
                List<double> datas = new List<double>();
                for (int c = 0; c < flexSheet.Columns.Count; c++)
                {
                    flexSheet[r, c] = string.Format("={0}*{1}", r + 1, c + 1);
                    double value = (double)flexSheet[r, c];
                    datas.Add(value);
                }
            }

            // add a totals row to illustrate
            var totRow = flexSheet.Rows.Count - 1;
            for (int c = 0; c < flexSheet.Columns.Count; c++)
            {
                flexSheet[totRow, c] = string.Format("=sum({0}1:{0}{1})",
                    (char)('A' + c), flexSheet.Rows.Count - 3);
            }
            flexSheet.Rows[totRow].FontWeight = FontWeights.Bold;
            #endregion
        }

        private void ExpressionEditor_ExpressionChanged(object sender, EventArgs e)
        {
            C1ExpressionEditor editer = sender as C1ExpressionEditor;
            if (!editer.IsValid)
            {
                Result.Text = "";
                Errors.Text = "";
                editer.GetErrors().ForEach(x => { Errors.Text += x.Message + "\n"; });
            }
            else
            {
                Result.Text = editer.Evaluate().ToString();
                Errors.Text = "";
            }
        }

        void olapPage_Loaded(object sender, RoutedEventArgs e)
        {
            olapPage.OlapEngine.RowFields.Add("Name");
            olapPage.OlapEngine.ColumnFields.Add("Residence");
            olapPage.OlapEngine.ValueFields.Add("Age");
        }

        #region Theme
        public Theme CurrentTheme
        {
            get { return (Theme)GetValue(CurrentThemeProperty); }
            set { SetValue(CurrentThemeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTheme.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentThemeProperty =
            DependencyProperty.Register("CurrentTheme", typeof(Theme), typeof(Gallery), new PropertyMetadata(OnThemeChanged));

        private static void OnThemeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((Gallery)d).OnThemeChanged();
        }

        private void OnThemeChanged()
        {
            simplifiedRibbon.IsCollapsed = true;
            if (CurrentTheme != null)
            {
                if(CurrentTheme.GetType() == typeof(C1ThemeShinyBlue))
                {
                    spParkLine.Background = new SolidColorBrush(Color.FromRgb(2, 101, 189));
                }
                else
                {
                    spParkLine.Background = new SolidColorBrush(Colors.Transparent);
                }
                var theme = CurrentTheme.GetNewResourceDictionary();

                //update theme
                cal1.Theme = theme;
                sched1.Theme = theme;
                ganttPage.Theme = theme;
                flexGrid.Theme = theme;

                // re-init olap control
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.ApplicationIdle, new System.Action(() =>
                {
                    if (olapPage != null && olapPage.OlapPanel != null && olapPage.OlapEngine != null)
                    {
                        olapPage.OlapEngine.RowFields.Add("Name");
                        olapPage.OlapEngine.ColumnFields.Add("Residence");
                        olapPage.OlapEngine.ValueFields.Add("Age");
                    }
                }));

                // Re-init flex sheet data.
                if (flexSheet.Sheets == null || flexSheet.Sheets.Count == 0)
                {
                    InitFlexSheet();
                }
                if (CurrentTheme.GetType() != typeof(C1ThemeMaterialDark) && CurrentTheme.GetType() != typeof(C1ThemeMaterial))
                {
                    button.Height = 28;
                    dateTimePicker.Height = 28;
                    datePicker.Height = 28;
                    calendarGrid.ColumnDefinitions[0].Width = new GridLength(210);
                }
                else
                {
                    calendarGrid.ColumnDefinitions[0].Width = new GridLength(300);
                    cal1.MonthWidth = 290;
                    cal1.MonthHeight = 250;
                }
            }
        }
        #endregion

        #region C1Chart specifics

        private void C1Chart_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            C1Chart chart = (C1Chart)sender;
            chart.Data.Children.Add(new DataSeries() { Label = "Series1", ValuesSource = new double[] { 3, 2, 7, 4, 8 } });

            if (chart.ChartType != ChartType.Pie)
            {
                chart.Data.Children.Add(new DataSeries() { Label = "Series2", ValuesSource = new double[] { 1, 2, 3, 4, 6 } });
                chart.Data.Children.Add(new DataSeries() { Label = "Series3", ValuesSource = new double[] { 0, 1, 6, 2, 3 } });
            }

            chart.Loaded -= new RoutedEventHandler(C1Chart_Loaded);
        }

        #endregion

        #region C1Scheduler specifics
        private void Grouping_Checked(object sender, RoutedEventArgs e)
        {
            sched1.GroupBy = "Category";
        }

        private void Grouping_Unchecked(object sender, RoutedEventArgs e)
        {
            sched1.GroupBy = "";
        }
        #endregion

        private void flexSheet_Loaded(object sender, RoutedEventArgs e)
        {
            formulaBar.FlexSheet = flexSheet;
        }

        #region OutlookBar
        void c1OutlookBar1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_window.IsOpen)
            {
                var item = c1OutlookBar1.SelectedItem as C1OutlookItem;
                FrameworkElement fe = _border.Child as FrameworkElement;
                if (_border.Child != null)
                {
                    _border.Child = null;
                    item.Content = fe;
                }
                _window.IsOpen = false;
                c1OutlookBar1.SelectedIndex = -1;
                c1OutlookBar1.SelectedItem = item;
            }
        }

        void c1OutlookBar1_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (_window.IsOpen)
            {
                var oldItem = e.OldValue as C1OutlookItem;
                var newItem = e.NewValue as C1OutlookItem;
                FrameworkElement fe = _border.Child as FrameworkElement;
                _border.Child = null;
                oldItem.Content = fe;
                fe = newItem.Content as FrameworkElement;
                newItem.Content = null;
                _border.Child = fe;
            }
        }

        void window_MouseLeave(object sender, MouseEventArgs e)
        {
            var item = c1OutlookBar1.SelectedItem as C1OutlookItem;
            FrameworkElement fe = _border.Child as FrameworkElement;
            if (_border.Child != null)
            {
                _border.Child = null;
                item.Content = fe;
            }
            _window.IsOpen = false;
        }

        void transformerBorder_MouseEnter(object sender, RoutedEventArgs e)
        {
            var item = c1OutlookBar1.SelectedItem as C1OutlookItem;
            FrameworkElement fe = item.Content as FrameworkElement;
            var point = transformerBorder.TranslatePoint(new Point(0, 0), c1OutlookBar1);
            if (_border.Child == null)
            {
                item.Content = null;
                _border.Child = fe;
            }
            _border.Height = transformerBorder.ActualWidth;
            _border.Width = c1OutlookBar1.ExpandedWidth;
            _border.Background = item.Background;
            if (SystemParameters.MenuDropAlignment)
            {
                _window.HorizontalOffset = 0;
            }
            else
            {
                _window.HorizontalOffset = c1OutlookBar1.ActualWidth;
            }
            _window.VerticalOffset = point.Y - transformerBorder.ActualWidth;
            _window.IsOpen = true;
        }
        #endregion

        #region FlexViewer
        public void ExtractC1NWind()
        {
            using (Stream stream = Application.GetResourceStream(new Uri("/" + _asmName + ";component/FlexViewer/C1NWind.xml.zip", UriKind.Relative)).Stream)
            {
                C1ZipFile zf = new C1ZipFile();
                zf.Open(stream);
                using (Stream src = zf.Entries[0].OpenReader())
                {
                    if (!File.Exists("C1NWind.xml") || new FileInfo("C1NWind.xml").Length != zf.Entries[0].SizeUncompressed)
                    {
                        using (Stream dst = new FileStream("C1NWind.xml", FileMode.Create))
                        {
                            byte[] buf = new byte[16 * 1024];
                            int len;
                            while ((len = src.Read(buf, 0, buf.Length)) > 0)
                            {
                                dst.Write(buf, 0, len);
                            }
                        }
                    }
                }
            }
        }

        void LoadReportList()
        {
            string[] reports;
            _reportStream = Application.GetResourceStream(new Uri("/" + _asmName + ";component/FlexViewer/FlexCommonTasks_XML.flxr", UriKind.Relative)).Stream;
            reports = C1FlexReport.GetReportList(_reportStream);

            var docListCombo = fv.DocumentListCombo;
            var items = docListCombo.Items;
            for (int i = 0; i < reports.Length; i++)
                items.Add(reports[i]);

            docListCombo.SelectionCommitted += ReportCombo_SelectionCommitted;
            docListCombo.SelectedIndex = 0;
        }

        void ReportCombo_SelectionCommitted(object sender, C1.WPF.PropertyChangedEventArgs<object> e)
        {
            if (_flexReport.IsBusy || _flexReport.IsDisposed)
            {
                return;
            }
            var reportName = fv.DocumentListCombo.SelectedItem as string;
            if (!string.IsNullOrEmpty(reportName))
            {
                fv.DocumentSource = null;
                _reportStream.Seek(0, SeekOrigin.Begin);
                _flexReport.Load(_reportStream, reportName);
                fv.DocumentSource = _flexReport;
                fv.FocusPane();
            }
        }
        #endregion

        #region Sparkline
        private void sparklineType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sparkline.SparklineType = (SparklineType)sparklineType.SelectedItem;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 12;
            double[] vals = new double[cnt];
            Random rnd = new Random();
            for (int i = 0; i < cnt; i++)
            {
                vals[i] = rnd.Next(-50, 50);
            }
            sparkline.Data = vals;
        }
        #endregion

        #region Carousel
        void panelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panelList == null || Resources == null || carouselListBox == null)
                return;
            ListBoxItem item = panelList.SelectedItem as ListBoxItem;
            if (item == null)
                return;
            Style style = LayoutRoot.Resources.Contains(item.Tag) ? LayoutRoot.Resources[item.Tag] as Style : null;
            if (style != null)
            {
                C1CarouselPanel.ClearCarouselProperties(carouselListBox);
                carouselListBox.Style = style;
            }
        }
        #endregion

        #region FilterEditor
        async void InitExpressionAndApply()
        {
            string pathToXmlFile = Directory.GetCurrentDirectory() + "\\FilterExpression.xml";
            if (File.Exists(pathToXmlFile))
            {
                var filterExpression = LoadFilterFromFile(pathToXmlFile);
                filterEditor.Expression = filterExpression;
            }

            await filterEditor.ApplyFilterAsync();
        }

        CombinationExpression LoadFilterFromFile(string filePath)
        {
            CombinationExpression filterExpression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Open(filePath, FileMode.Open))
            {
                filterExpression = xmlSerializer.Deserialize(fs) as CombinationExpression;
            }

            return filterExpression;
        }

        private void filterEditor_FilterChanged(object sender, EventArgs e)
        {
            string pathToXmlFile = Directory.GetCurrentDirectory() + "\\FilterExpression.xml";
            SaveFilterToFile(pathToXmlFile);
        }

        private void SaveFilterToFile(string filePath)
        {
            var expression = filterEditor.Expression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Create(filePath))
            {
                xmlSerializer.Serialize(fs, expression);
            }
        }
        #endregion
    }
}
