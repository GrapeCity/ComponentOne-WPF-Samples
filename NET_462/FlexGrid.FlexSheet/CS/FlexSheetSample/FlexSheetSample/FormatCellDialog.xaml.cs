using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using C1.WPF.FlexGrid;

namespace FlexSheetSample
{
    /// <summary>
    /// Interaction logic for FormatCellDialog.xaml
    /// </summary>
    public partial class FormatCellDialog : Window
    {
        AlignmentViewModel _alignViewModel;
        FontViewModel _fontViewModel;
        BorderViewModel _borderViewModel;
        NumberViewModel _numberViewModel;
        public FormatCellDialog()
        {
            InitializeComponent();
           
        }

        C1FlexSheet _flex;
        IEnumerable<CellRange> _cellRange;
        public FormatCellDialog(C1FlexSheet flex, IEnumerable<CellRange> cellRange)
            : this()
        {
            _flex = flex;
            _cellRange = cellRange;

            //Number
            _numberViewModel = new NumberViewModel(flex, cellRange);
            numberGrid.DataContext = _numberViewModel;

            //Align
            _alignViewModel = new AlignmentViewModel(flex, cellRange);
            alignmentGrid.DataContext = _alignViewModel;

            //Font
            _fontViewModel = new FontViewModel(flex, cellRange);
            fontGrid.DataContext = _fontViewModel;
            lstBoxFonts.SelectedItem = _fontViewModel.SelectedFont;
            cmbUnderlines.SelectedItem = _fontViewModel.SelectedUnderLine;
            lstFontSize.SelectedItem = _fontViewModel.SelectedFontSize;
            lstFontStyle.SelectedItem = _fontViewModel.SelectedFontStyle;

            //Fill
            HandleFill(flex, cellRange);
            colorSelector.SelectionChanged += colorSelector_SelectionChanged;
            btnTransparent.Checked += btnTransparent_Checked;

            //Border
            _borderViewModel = new BorderViewModel(flex, cellRange);
            previewPanel.DataContext = _borderViewModel;
        }

        void btnTransparent_Checked(object sender, RoutedEventArgs e)
        {
            colorSelector.UnSelectAll();
            btnTransparent.IsEnabled = false;
        }

        void colorSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as ColorSelector;
            if (selector != null && selector.CustomColor != null)
            {
                btnTransparent.IsEnabled = true;
                btnTransparent.IsChecked = false;
            }
        }

        private Brush _origionFill;
        private void HandleFill(C1FlexSheet flex,IEnumerable<CellRange> cellRange)
        {
            var cell = cellRange.FirstOrDefault();
            var row = flex.Rows[cell.TopRow] as ExcelRow;
            if (row != null && cell.IsValid)
            {
                var col = flex.Columns[cell.Column];
                var cs = row.GetCellStyle(col);
                if (cs == null || cs.Background == null) _origionFill = Brushes.Transparent;
                else
                    _origionFill = cs.Background;
            }
        }

        private void horizontalCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb != null)
            {
                _alignViewModel.SelectedHorAlignment = (HorizontalAlignment)cmb.SelectedItem;
            }
        }

        private void verticalCmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmb = sender as ComboBox;
            if (cmb != null)
            {
                _alignViewModel.SelectedVerAlignment = (VerticalAlignment)cmb.SelectedItem;
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            //Align
            _alignViewModel.Commit();

            //Font
            _fontViewModel.SelectedFont = lstBoxFonts.SelectedItem as FontFamily;
            _fontViewModel.SelectedUnderLine = (FlexSheetSample.FontViewModel.UnderLines)cmbUnderlines.SelectedItem;
            _fontViewModel.SelectedFontSize = (double)lstFontSize.SelectedItem;
            _fontViewModel.SelectedFontStyle = (FlexSheetSample.FontViewModel.FontStyleFormat)lstFontStyle.SelectedItem;
            _fontViewModel.SelectedColor = colorPicker.SelectedColor;
            _fontViewModel.Commit();

            //Fill
            var brush = _origionFill as SolidColorBrush;
            if (brush != null && brush.Color != colorSelector.CustomColor)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.Background, new SolidColorBrush(colorSelector.CustomColor));
            }

            //Bordeer
            _borderViewModel.CellBorderColor = borderColorPicker.SelectedColor;
            _borderViewModel.Commit();

            _numberViewModel.Commit();
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void presets_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == btnNone)
            {
                _borderViewModel.HideAll();
            }
            else if (e.OriginalSource == btnOutline)
            {
                _borderViewModel.ShowAll();
            }
        }

        private void borderSet_Click(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == btnBdrTop)
            {
                _borderViewModel.ShowBorderTop = !_borderViewModel.ShowBorderTop;
            }
            else if (e.OriginalSource == btnBdrLeft)
            {
                _borderViewModel.ShowBorderLeft = !_borderViewModel.ShowBorderLeft;
            }
            else if (e.OriginalSource == btnBdrRight)
            {
                _borderViewModel.ShowBorderRight = !_borderViewModel.ShowBorderRight;
            }
            else if (e.OriginalSource == btnBdrBottom)
            {
                _borderViewModel.ShowBorderBottom = !_borderViewModel.ShowBorderBottom;
            }
        }

        private void formatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _numberViewModel.InvalidateText();
            if (_numberViewModel.NoDecimalPlacesList.Contains(_numberViewModel.SelectedNumberFormat))
            {
                placesPanel.Visibility = System.Windows.Visibility.Hidden;
            }
            else
                placesPanel.Visibility = System.Windows.Visibility.Visible;
        }
    }

    public class AlignmentViewModel : INotifyPropertyChanged
    {
        #region OriginValue
        HorizontalAlignment _originHorAlign;
        VerticalAlignment _originVerAlign;
        double _originIndent;
        bool _originWrapText;
        double _originDegree;
        #endregion

        #region Properties
        public ObservableCollection<HorizontalAlignment> Horizontal
        { get; set; }

        private HorizontalAlignment _selectedHorAlign = HorizontalAlignment.Stretch;
        public HorizontalAlignment SelectedHorAlignment
        {
            get
            {
                return _selectedHorAlign;
            }
            set
            {
                _selectedHorAlign = value;
                OnPropertyChanged("SelectedHorAlignment");
            }
        }

        public ObservableCollection<VerticalAlignment> Vertical { get; set; }

        private VerticalAlignment _selectedVerAlign = VerticalAlignment.Stretch;
        public VerticalAlignment SelectedVerAlignment
        {
            get
            {
                return _selectedVerAlign;
            }
            set
            {
                _selectedVerAlign = value;
            }
        }

        private double _indent = 0;
        public double Indent
        {
            get
            {
                return _indent;
            }
            set
            {
                _indent = value;
                OnPropertyChanged("Indent");
            }
        }

        private bool? _wrapText = false;
        public bool? WrapText
        {
            get
            {
                return _wrapText;
            }
            set
            {
                _wrapText = value;
                OnPropertyChanged("WrapText");
            }
        }

        private double _degree = 0;
        public double Degree
        {
            get
            {
                return _degree;
            }
            set
            {
                _degree = value;
                OnPropertyChanged("Degree");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
        #endregion

        public const string LEFT = "Left", RIGHT = "Right", CENTER = "Center", STRENCH = "General";
        public const string TOP = "Top", MIDDLE = "Middle", BOTTOM = "Bottom";
        private C1FlexSheet _flex;
        public AlignmentViewModel(C1FlexSheet flex, IEnumerable<CellRange> cellRange)
        {
            _flex = flex;
            UpdateContent(cellRange);
            Horizontal = new ObservableCollection<HorizontalAlignment>()
            {
                HorizontalAlignment.Stretch,
                HorizontalAlignment.Left,
                HorizontalAlignment.Center,
                HorizontalAlignment.Right
            };
            Vertical = new ObservableCollection<VerticalAlignment>() 
            {
                VerticalAlignment.Stretch,
                VerticalAlignment.Top,
                VerticalAlignment.Center,
                VerticalAlignment.Bottom,
            };

        }

        private IEnumerable<CellRange> _cellRange;
        public void UpdateContent(IEnumerable<CellRange> cellRange)
        {
            _cellRange = cellRange;
            var cell = cellRange.FirstOrDefault();
            if (cell.IsValid)
            {
                var row = _flex.Rows[cell.Row] as ExcelRow;
                if (row != null)
                {
                    var col = _flex.Columns[cell.Column];
                    var cs = row.GetCellStyle(col);
                    cs = cs ?? new CellStyle();
                    _originHorAlign = cs.HorizontalAlignment ?? HorizontalAlignment.Stretch;
                    _originVerAlign = cs.VerticalAlignment ?? VerticalAlignment.Stretch;
                    _originDegree = row.GetCellAngle(col);
                    _originIndent = row.GetCellIndent(col);
                    _originWrapText = cs.TextWrapping ?? false;
                }
                Degree = _originDegree;
                Indent = _originIndent;
                WrapText = _originWrapText;
                SelectedHorAlignment = _originHorAlign;
                SelectedVerAlignment= _originVerAlign;
                //switch(_originHorAlign)
                //    case HorizontalAlignment.Stretch:
            }
        }

        public void Commit()
        {
            if (_originHorAlign != SelectedHorAlignment)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.HorizontalAlignment, SelectedHorAlignment);
            }
            if (_originVerAlign != SelectedVerAlignment)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.VerticalAlignment, SelectedVerAlignment);
            }
            if (_originDegree != Degree)
            {
                _flex.SetCellAngle(_cellRange, Degree, 8);
            }
            if (_originIndent != Indent)
            {
                _flex.SetCellIndent(_cellRange, Indent);
            }
            if (_originWrapText != WrapText)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.TextWrapping, WrapText);
            }
        }
    }

    public class FontViewModel : INotifyPropertyChanged
    {
        #region field

        private FontFamily _origionFont;
        private FontStyleFormat _origionFontStyle;
        private double _origionFontSize;
        private UnderLines _origionUnderline;
        private Color _origionColor;

        #endregion

        #region Properties
        public FontFamily SelectedFont { get; set; }

        public FontStyleFormat SelectedFontStyle { get; set; }

        public double SelectedFontSize { get; set; }

        public UnderLines SelectedUnderLine { get; set; }

        public Color SelectedColor { get; set; }

        public ObservableCollection<FontFamily> FontsCollection
        { get; set; }

        public ObservableCollection<FontStyleFormat> FontsStyles
        { get; set; }


        public ObservableCollection<double> FontSize { get; set; }

        public ObservableCollection<UnderLines> Underlines
        { get; set; }

        public ObservableCollection<Color> FontColors
        { get; set; }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private C1FlexSheet _flex;
        private IEnumerable<CellRange> _cellRange;
        public FontViewModel(C1FlexSheet flex, IEnumerable<CellRange> cellRange)
        {
            _flex = flex;
            _cellRange = cellRange;
            foreach (var font in Fonts.SystemFontFamilies)
            {
                if (FontsCollection == null)
                    FontsCollection = new ObservableCollection<FontFamily>();

                FontsCollection.Add(font);
            }

            if (FontsStyles == null)
                FontsStyles = new ObservableCollection<FontStyleFormat>() 
                { FontStyleFormat.Normal, FontStyleFormat.Italic, FontStyleFormat.Bold};

            if (FontSize == null)
                FontSize = new ObservableCollection<double>();
            int from = 8; int to = 72;
            for (; from <= to;)
            {
                FontSize.Add(from);
                if (from < 12)
                    from++;
                else
                    from = from + 2;
            }

            if (Underlines == null)
                Underlines = new ObservableCollection<UnderLines>() { UnderLines.None, UnderLines.Underline };

            var cell = cellRange.FirstOrDefault();
            var row = _flex.Rows[cell.TopRow] as ExcelRow;
            if (row != null && cell.IsValid)
            {
                var col = _flex.Columns[cell.Column];
                var cs = row.GetCellStyle(col);
                cs = cs ?? new CellStyle();

                _origionFont = SelectedFont = cs.FontFamily ?? new FontFamily("Arial");

                _origionFontSize = SelectedFontSize = cs.FontSize ?? 11;

                SelectedFontStyle = FontStyleConvertFrom(cs.FontStyle, cs.FontWeight);
                _origionFontStyle = SelectedFontStyle;

                if (cs.TextDecorations == null || cs.TextDecorations.Count == 0)
                    SelectedUnderLine = UnderLines.None;
                else
                    SelectedUnderLine = UnderLinesConvertFrom(cs.TextDecorations);
                _origionUnderline = SelectedUnderLine;

                var brush = (SolidColorBrush)cs.Foreground;
                _origionColor = SelectedColor = brush == null ? Colors.Transparent : brush.Color;
            }
        }

        private UnderLines UnderLinesConvertFrom(TextDecorationCollection source)
        {
            if (source == TextDecorations.Underline)
                return UnderLines.Underline;
            else if (source == TextDecorations.Baseline)
                return UnderLines.BaseLine;
            else if (source == TextDecorations.OverLine)
                return UnderLines.OverLine;
            else if (source == TextDecorations.Strikethrough)
                return UnderLines.Strikethrough;
            else
                return UnderLines.None;
        }

        private TextDecorationCollection UnderLinesConverterTo(UnderLines source)
        {
            if (source == UnderLines.BaseLine)
                return TextDecorations.Baseline;
            else if (source == UnderLines.OverLine)
                return TextDecorations.OverLine;
            else if (source == UnderLines.Strikethrough)
                return TextDecorations.Strikethrough;
            else if (source == UnderLines.Underline)
                return TextDecorations.Underline;
            else
                return new TextDecorationCollection();
        }

        private FontStyleFormat FontStyleConvertFrom(FontStyle? styleSource, FontWeight? weightSource)
        {
            if (styleSource == null) return FontStyleFormat.Normal;
            else if (weightSource == FontWeights.Bold)
            {
                if (styleSource == FontStyles.Italic)
                    return FontStyleFormat.BoldAndItalic;
                return FontStyleFormat.Bold;
            }
            else if (styleSource == FontStyles.Italic)
                return FontStyleFormat.Italic;
            else
                return FontStyleFormat.Normal;

        }

        private FontStyle FontStyleConvertTo(FontStyleFormat source)
        {
            if (source == FontStyleFormat.Normal)
                return FontStyles.Normal;
            else if (source == FontStyleFormat.Italic)
                return FontStyles.Italic;

            return FontStyles.Normal;
        }

        public void Commit()
        {
            if (_origionColor != SelectedColor)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.Foreground, new SolidColorBrush(SelectedColor));
            }
            if (_origionFont != SelectedFont)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.FontFamily, SelectedFont);
            }
            if (_origionFontSize != SelectedFontSize)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.FontSize, SelectedFontSize);
            }
            if (_origionUnderline != SelectedUnderLine)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.TextDecorations, UnderLinesConverterTo(SelectedUnderLine));
            }
            if (_origionFontStyle != SelectedFontStyle)
            {
                _flex.SetCellFormat(_cellRange, CellFormat.FontStyle, FontStyleConvertTo(SelectedFontStyle));
                if (SelectedFontStyle == FontStyleFormat.Bold || SelectedFontStyle == FontStyleFormat.BoldAndItalic)
                {
                    _flex.SetCellFormat(_cellRange, CellFormat.FontWeight, FontWeights.Bold);
                }
            }
        }

        public enum UnderLines
        {
            None,
            BaseLine,
            Underline,
            OverLine,
            Strikethrough,
        }

        public enum FontStyleFormat
        {
            Normal,
            Italic,
            Bold, 
            BoldAndItalic
        }
    }

    public class BorderViewModel : INotifyPropertyChanged
    {
        private C1FlexSheet _flex;
        private IEnumerable<CellRange> _cellRange;
        public BorderViewModel(C1FlexSheet flex, IEnumerable<CellRange> cellRange)
        {
            _flex = flex;
            _cellRange = cellRange;

            var cell = cellRange.FirstOrDefault();
            var row = _flex.Rows[cell.TopRow] as ExcelRow;
            if (row != null && cell.IsValid)
            {
                var col = _flex.Columns[cell.Column];
                var cs = row.GetCellStyle(col) as ExcelCellStyle;
                if (cs != null)
                {
                    if (cs.CellBorderThickness != new Thickness(0))
                        CellBorderThickness = cs.CellBorderThickness;
                    if (cs.CellBorderBrushTop != null)
                    {
                        ShowBorderTop = true;
                        var brush = cs.CellBorderBrushTop as SolidColorBrush;
                        if (brush != null)
                            CellBorderColor = brush.Color;
                    }
                    if (cs.CellBorderBrushLeft != null)
                        ShowBorderLeft = true;
                    if (cs.CellBorderBrushRight != null)
                        ShowBorderRight = true;
                    if (cs.CellBorderBrushBottom != null)
                        ShowBorderBottom = true;                    
                }
            }
            BorderStyle = BorderStyles.Solid;
        }

        private Thickness _bdrThickness = new Thickness(0);
        public Thickness CellBorderThickness
        {
            get { return _bdrThickness; }
            set
            {
                if (value != _bdrThickness)
                {
                    _bdrThickness = value;
                    OnPropertyChanged("BorderThickness");
                }
            }
        }


        private bool _showLeft;
        public bool ShowBorderLeft
        {
            get { return _showLeft; }
            set
            {
                if (value != _showLeft)
                {
                    _showLeft = value;
                    OnPropertyChanged("ShowBorderLeft");
                }
            }
        }

        private bool _showTop;
        public bool ShowBorderTop
        {
            get { return _showTop; }
            set
            {
                if (value != _showTop)
                {
                    _showTop = value;
                    OnPropertyChanged("ShowBorderTop");
                }
            }
        }

        private bool _showRight;
        public bool ShowBorderRight
        {
            get { return _showRight; }
            set
            {
                if (value != _showRight)
                {
                    _showRight = value;
                    OnPropertyChanged("ShowBorderRight");
                }
            }
        }

        private bool _showBottom = false;
        public bool ShowBorderBottom
        {
            get { return _showBottom; }
            set
            {
                if (value != _showBottom)
                {
                    _showBottom = value;
                    OnPropertyChanged("ShowBorderBottom");
                }
            }
        }

        private Brush _bdrLeft;
        public Brush CellBorderBrushLeft
        {
            get { return _bdrLeft; }
        }

        private Brush _bdrTop;
        public Brush CellBorderBrushTop
        {
            get { return _bdrTop; }
        }

        private Brush _bdrRight;
        public Brush CellBorderBrushRight
        {
            get { return _bdrRight; }
        }

        private Brush _bdrBottom;
        public Brush CellBorderBrushBottom
        {
            get { return _bdrBottom; }
        }

        private Color _bdrColor;
        public Color CellBorderColor
        {
            get { return _bdrColor; }
            set
            {
                if (value != _bdrColor)
                {
                    _bdrColor = value;
                    OnPropertyChanged("BorderColor");
                }
            }
        }

        public Brush GetBorderBrush(BorderStyles style)
        {
            switch(style)
            {
                case BorderStyles.None:
                    return Brushes.Transparent;
                case BorderStyles.Solid:
                    return new SolidColorBrush(CellBorderColor);
            }
            return Brushes.Transparent;
        }

        public void SetBorderBrush(BorderAlign align)
        {
            var brush = GetBorderBrush(BorderStyle);
            switch (align)
            {
                case BorderAlign.Left:
                    _bdrLeft = brush;
                    break;
                case BorderAlign.Top:
                    _bdrTop = brush;
                    break;
                case BorderAlign.Right:
                    _bdrRight = brush;
                    break;
                case BorderAlign.Bottom:
                    _bdrBottom = brush;
                    break;
            }
        }

        private BorderStyles _bdrStyle;
        public BorderStyles BorderStyle
        {
            get 
            {
                return _bdrStyle; 
            }
            set 
            {
                if (_bdrStyle != value)
                {
                    _bdrStyle = value;
                    OnPropertyChanged("BorderStyle");
                }
            }
        }

        public void HideAll()
        {
            ShowBorderTop = false;
            ShowBorderLeft = false;
            ShowBorderRight = false;
            ShowBorderBottom = false;
        }

        public void ShowAll()
        {
            ShowBorderTop = true;
            ShowBorderLeft = true;
            ShowBorderRight = true;
            ShowBorderBottom = true;
        }

        public void Commit()
        {
            var cell = _cellRange.FirstOrDefault();
            var row = _flex.Rows[cell.TopRow] as ExcelRow;
            ExcelCellStyle excelCellStyle = new ExcelCellStyle();
            if (row != null && cell.IsValid)
            {
                var col = _flex.Columns[cell.Column];
                var cs = row.GetCellStyle(col) as ExcelCellStyle;
                if (cs != null)
                    excelCellStyle = cs;
            }
                
            if (ShowBorderTop)
            {
                SetBorderBrush(BorderAlign.Top);
                excelCellStyle.CellBorderBrushTop = CellBorderBrushTop;
            }
            if (ShowBorderLeft)
            {
                SetBorderBrush(BorderAlign.Left);
                excelCellStyle.CellBorderBrushLeft = CellBorderBrushLeft;
            }
            if (ShowBorderRight)
            {
                SetBorderBrush(BorderAlign.Right);
                excelCellStyle.CellBorderBrushRight = CellBorderBrushRight;
            }
            if (ShowBorderBottom)
            {
                SetBorderBrush(BorderAlign.Bottom);
                excelCellStyle.CellBorderBrushBottom = CellBorderBrushBottom;
            }
            if (ShowBorderTop || ShowBorderLeft || ShowBorderRight || ShowBorderBottom)
            {
                CellBorderThickness = new Thickness(1);
                excelCellStyle.BorderThickness = excelCellStyle.CellBorderThickness = CellBorderThickness;               
                row.SetCellStyle( _flex.Columns[cell.Column], excelCellStyle);
            }
        }

        public enum BorderStyles
        {
            None,
            Solid
        }

        public enum BorderAlign
        {
            Left,
            Top,
            Right,
            Bottom
        }

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
        #endregion
    }

    public class NumberViewModel:INotifyPropertyChanged
    {
        ObservableCollection<FormatBase> _numberFormats;
        private C1FlexSheet _flex;
        private IEnumerable<CellRange> _cellRange;
        private object _cellValue;
        public NumberViewModel(C1FlexSheet flex, IEnumerable<CellRange> cellRange)
        {
            _flex = flex;
            _cellRange = cellRange;
            var cell = cellRange.FirstOrDefault();
            if (cell.IsValid)
            {
                _cellValue = _flex[cell.Row, cell.Column];
            }
           _numberFormats = new ObservableCollection<FormatBase>();
           if (NoDecimalPlacesList == null)
               NoDecimalPlacesList = new List<FormatBase>();
           InitialNumberFormats();
        }

        private void InitialNumberFormats()
        {
            NumberFormat generalFormat = new NumberFormat();
            generalFormat.Name = "General";
            generalFormat.Format = "G";
            generalFormat.PropertyChanged += generalFormat_PropertyChanged;
            NoDecimalPlacesList.Add(generalFormat);
            NumberFormats.Add(generalFormat);

            NumberFormat numberFormat = new NumberFormat();
            numberFormat.Name = "Number";
            numberFormat.Format = "N";
            numberFormat.PropertyChanged += generalFormat_PropertyChanged;
            NumberFormats.Add(numberFormat);

            NumberFormat currencyFormat = new NumberFormat();
            currencyFormat.Name = "Currency";
            currencyFormat.Format = "C";
            currencyFormat.PropertyChanged += generalFormat_PropertyChanged;
            NumberFormats.Add(currencyFormat);

            NumberFormat percentFormat = new NumberFormat();
            percentFormat.Name = "Percentage";
            percentFormat.Format = "P";
            percentFormat.PropertyChanged += generalFormat_PropertyChanged;
            NumberFormats.Add(percentFormat);

            NumberFormat exponentialFormat = new NumberFormat();
            exponentialFormat.Name = "Scientific";
            exponentialFormat.Format = "E";
            exponentialFormat.PropertyChanged += generalFormat_PropertyChanged;
            NumberFormats.Add(exponentialFormat);

            DateFormat dateFormat = new DateFormat();
            dateFormat.Name = "Date";
            dateFormat.Format = "f";
            dateFormat.PropertyChanged += generalFormat_PropertyChanged;
            NumberFormats.Add(dateFormat);
            NoDecimalPlacesList.Add(dateFormat);


            SelectedNumberFormat = generalFormat;

            var cell = _cellRange.FirstOrDefault();
            var row = _flex.Rows[cell.Row] as ExcelRow;
            var col = _flex.Columns[cell.Column];

            FormatBase defaultFormat = generalFormat;
            if (row != null)
            {
                var cs = row.GetCellStyle(col) as ExcelCellStyle;
                var cv = row.GetValue(col);
                defaultFormat = GetNumberFormat(cs != null ? cs.Format : null, cv);             
            }
            SelectedNumberFormat = defaultFormat;
            SelectedNumberFormat.PropertyChanged += SelectedNumberFormat_PropertyChanged;

            InvalidateText();
        }

        private FormatBase GetNumberFormat(string format, object value)
        {
            FormatBase expectedFormat = NumberFormats.Where(f => f.Format == "G").ToList().FirstOrDefault();
            if (!string.IsNullOrEmpty(format))
            {
                string leader = format[0].ToString();
                NumberFormat n = NumberFormats.Where(f => f.Format == leader).ToList().FirstOrDefault() as NumberFormat;
                if (n != null)
                {
                    string decimalPlaces = format.Substring(1);
                    if (!string.IsNullOrEmpty(decimalPlaces))
                    {
                        int num;
                        if (Int32.TryParse(decimalPlaces, out num))
                            n.DecimalPlaces = num;
                    }
                    expectedFormat = n;
                }
                else
                {
                    DateFormat d = NumberFormats.Where(f => f.Name == "Date").FirstOrDefault() as DateFormat;
                    if ( d.IsDateFormat(format, value))
                    {
                        expectedFormat = d;
                    }
                }
            }
            return expectedFormat;
        }

        internal List<FormatBase> NoDecimalPlacesList { get; set; }        

        void generalFormat_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateText();
        }

        public void InvalidateText()
        {
            var numFormat = SelectedNumberFormat as NumberFormat;
            if (numFormat != null)
            {
                Text = string.Format("{0:" + numFormat.Format + numFormat.DecimalPlaces + "}", _cellValue);
            }
            else
            {
                Text = string.Format("{0:" + SelectedNumberFormat.Format + "}", _cellValue);
            }
        }

        void SelectedNumberFormat_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvalidateText();
        }

        public ObservableCollection<FormatBase> NumberFormats
        {
            get
            {
                return _numberFormats;
            }
            set
            {
                _numberFormats = value;
            }
        }

        private FormatBase _selectedNumberFormat;
        public FormatBase SelectedNumberFormat
        {
            get
            {
                return _selectedNumberFormat;
            }
            set
            {
                if (_selectedNumberFormat != value)
                {
                    _selectedNumberFormat = value;
                    OnPropertyChanged("SelectedNumberFormat");
                }
                
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged("Text");
                }
            }
        }

        public void Commit()
        {
            foreach (var rng in _cellRange)
            {
                if (rng.IsValid)
                {
                    // get cell style
                    var row = _flex.Rows[rng.Row] as ExcelRow;
                    var col = _flex.Columns[rng.Column];
                    ExcelCellStyle excelCellStyle = new ExcelCellStyle();
                    if (row != null)
                    {
                        var cs = row.GetCellStyle(col) as ExcelCellStyle;
                        if (cs != null)
                            excelCellStyle = cs;
                    }
                    excelCellStyle.Format = NoDecimalPlacesList.Contains(SelectedNumberFormat) 
                        ? SelectedNumberFormat.Format 
                        : SelectedNumberFormat.Format + ((NumberFormat)SelectedNumberFormat).DecimalPlaces;

                    row.SetCellStyle(_flex.Columns[rng.Column], excelCellStyle);
                    _flex.Invalidate(rng);
                }
            }
        }

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
        #endregion
    }

    public class FormatBase : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _format;
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                if (_format != value)
                {
                    _format = value;
                    OnPropertyChanged("Format");
                }
            }
        }

        #region INotify
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

        }
        #endregion
    }

    public class NumberFormat : FormatBase
    {
        private int _decimalPlaces = 2;
        public int DecimalPlaces
        {
            get { return _decimalPlaces; }
            set 
            {
                if (_decimalPlaces != value)
                {
                    _decimalPlaces = value;
                    OnPropertyChanged("DecimalPlaces");
                }
            }
        }
    }

    public class DateFormat : FormatBase
    {
        public readonly List<string> SupportedFormats = new List<string>() {"f", "g", "F", "G", "d", "D", "M", "m", "Y" };

        public bool IsDateFormat(string format, object value)
        {
            if ( value is DateTime)
            {
                return true;
            }
            try
            {
                var test = DateTime.Now.ToString(format);
                return true;
            }
            catch
            {

            }
            return false;
        }
    }
}