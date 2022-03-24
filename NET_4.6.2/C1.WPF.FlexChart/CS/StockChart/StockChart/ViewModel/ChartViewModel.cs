using C1.Chart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace StockChart.ViewModel
{
    public class ChartViewModel: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private ChartViewModel()
        {
        }


        private static ChartViewModel _Instance;
        public static ChartViewModel Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ChartViewModel();
                }
                return _Instance;
            }
        }

        #region
        private string _mainSymbol = "MSFT";
        public string MainSymbol
        {
            get { return _mainSymbol; }
            set {
                if (_mainSymbol != value)
                {
                    _mainSymbol = value;
                    //DataSource = DataService.Instance.GetSymbolData(_mainSymbol);
                    if (this.MainChart != null)
                    {
                        DataService.Instance.GetSymbolDataAsync(_mainSymbol, new Action<QuoteData>((data) =>
                        {
                            this.MainChart.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                DataSource = data;
                                this.MainSeries.SeriesName = MainSymbol;
                            }));
                        }));
                    }
                }
            }
        }

        private Dictionary<string, string> _symbolNames;
        public Dictionary<string, string> SymbolNames
        {
            get
            {
                if (_symbolNames == null)
                {
                    _symbolNames = DataService.Instance.SymbolNames;
                }
                return _symbolNames;
            }
        }

        public string SymbolName
        {
            get
            {
                return string.Format("{0}({1})", DataService.Instance.SymbolNames[_mainSymbol], _mainSymbol.ToUpper());
            }
        }
        
        public double Price
        {
            get
            {
                if (ViewModel.ChartViewModel.Instance.DataSource != null)
                {
                    return ViewModel.ChartViewModel.Instance.DataSource.Last().close;
                }
                return 0;
            }
        }
        
        public string Chg
        {
            get
            {
                if (ViewModel.ChartViewModel.Instance.DataSource != null)
                {
                    var quoteData = ViewModel.ChartViewModel.Instance.DataSource;
                    var value = quoteData[Math.Max(quoteData.Count - 1, 0)].close - quoteData[Math.Max(quoteData.Count - 2, 0)].close;

                    return string.Format((value >= 0 ? "+{0:0.00}" : "{0:0.00}"), value);
                }
                return "0.00";
                
            }
        }

        #endregion


        #region ViewModel DataSource
        private Dictionary<string, ChartType> _chartTypes;
        public Dictionary<string, ChartType> ChartTypes
        {
            get
            {
                if (_chartTypes == null)
                {
                    var types = new ChartType[] { C1.Chart.ChartType.Line, C1.Chart.ChartType.Area, C1.Chart.ChartType.Candlestick, C1.Chart.ChartType.HighLowOpenClose };
                    _chartTypes = new Dictionary<string, ChartType>();
                    foreach (var t in types)
                    {
                        _chartTypes.Add("Chart Type: " + t.ToString(), t);
                    }
                }
                return _chartTypes;
            }
        }

        string[] _types = new string[] { "JPG", "PNG", "SVG" };
        private Dictionary<string, string> _exportTypes;
        public Dictionary<string, string> ExportTypes
        {
            get
            {
                if (_exportTypes == null)
                {
                    _exportTypes = new Dictionary<string, string>();
                    foreach (var t in _types)
                    {
                        _exportTypes.Add(t.ToString(), t);
                    }

                }
                return _exportTypes;
            }
        }

        private Dictionary<string, C1.Chart.MovingAverageType> _movingAverageTypes;
        public Dictionary<string, C1.Chart.MovingAverageType> MovingAverageTypes
        {
            get
            {
                if (_movingAverageTypes == null)
                {
                    _movingAverageTypes = new Dictionary<string, C1.Chart.MovingAverageType>();
                    foreach (C1.Chart.MovingAverageType t in Enum.GetValues(typeof(C1.Chart.MovingAverageType)))
                    {
                        _movingAverageTypes.Add("Type: " + t.ToString(), t);
                    }
                }
                return _movingAverageTypes;
            }
        }
        #endregion


        #region 

        private QuoteData _indexDataSource;
        public QuoteData IndexDataSource
        {
            get
            {
                if (_indexDataSource == null)
                {
                    // INX is the symbol AlphaVantage uses for S&P 500 index.
                    _indexDataSource = DataService.Instance.GetSymbolData("SP");
                }
                return _indexDataSource;
            }
        }


        private QuoteData _datasource;
        public QuoteData DataSource
        {
            get
            {
                if (_datasource == null)
                {
                    _datasource = DataService.Instance.GetSymbolData(_mainSymbol);
                }
                return _datasource;
            }
            set
            {
                if (_datasource != value)
                {
                    _datasource = value;
                    OnPropertyChanged("DataSource");
                    OnPropertyChanged("SymbolName");
                    OnPropertyChanged("Price");
                    OnPropertyChanged("Chg");
                    UpdateDataRange();
                    if (IsShowNews)
                    {
                        SetupAnnotation();
                    }
                }
            }
        }

        private string _binding = "close";
        public string Binding
        {
            get
            {
                return _binding;
            }
            set
            {
                _binding = value;
                if (MainSeries != null)
                {
                    SetSeriesAfterBindingChanged(MainSeries);
                }
                if (MainMovingAverage != null)
                {
                    SetSeriesAfterBindingChanged(MainMovingAverage);
                }
            }
        }

        private void SetSeriesAfterBindingChanged(C1.WPF.Chart.Series series)
        {
            series.Dispatcher.BeginInvoke(new Action(() =>
            {
                series.Binding = _binding;
                series.Dispose();
            }));
        }

        public static System.Windows.Media.Color[] _Colors = new System.Windows.Media.Color[]
        {
            System.Windows.Media.Color.FromArgb(255,16,150,24),
            System.Windows.Media.Color.FromArgb(255,153,0,153),
            System.Windows.Media.Color.FromArgb(255,34,40,141),
            System.Windows.Media.Color.FromArgb(255,0,153,198),
        };

        private ObservableCollection<Symbol> _symbolCollection;

        public ObservableCollection<Symbol> SymbolCollection
        {
            get
            {
                if (_symbolCollection == null)
                {
                    _symbolCollection = new ObservableCollection<Symbol>();

                    _symbolCollection.CollectionChanged += (o, e) =>
                    {
                        if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                        {
                            if (MainChart != null)
                            {
                                foreach (Symbol item in e.OldItems)
                                {
                                    if (MainChart.Series.Contains(item.Series))
                                    {
                                        MainChart.Series.Remove(item.Series);
                                    }
                                    if (MainChart.Series.Contains(item.MovingAverage))
                                    {
                                        MainChart.Series.Remove(item.MovingAverage);
                                    }
                                }
                            }
                        }
                        else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                        {
                            foreach (Symbol s in e.NewItems)
                            {
                                if (s != null)
                                {
                                    s.PropertyChanged += (o1, e1) =>
                                    {
                                        if (e1.PropertyName == "Visibility")
                                        {
                                            RefrushComparisonMode();
                                        }
                                    };
                                }
                            }

                        }

                        if (ResetCommand != null)
                        {
                            ResetCommand.RaiseCanExecuteChanged();
                        }

                        OnPropertyChanged("SymbolCollection");
                        RefrushComparisonMode();
                    };
                }
                return _symbolCollection;
            }
        }



        private IEnumerable<QuoteInfo> _comparisonQuoteInfo;
        public IEnumerable<QuoteInfo> ComparisonQuoteInfo
        {
            get
            {

                return _comparisonQuoteInfo;
            }
            set
            {
                if (_comparisonQuoteInfo != value)
                {
                    _comparisonQuoteInfo = value;
                    OnPropertyChanged("ComparisonQuoteInfo");
                }
            }
        }

        public string AddingSymbolCode
        {
            get;
            set;
        }

        private int _rangeSelectIndex = 3;
        public int RangeSelectIndex
        {
            get
            {
                return _rangeSelectIndex;
            }
            set
            {
                _rangeSelectIndex = value;
                UpdateRangeSelect(_rangeSelectIndex);
            }
        }

        private string _dateRangeText;
        public string DateRangeText
        {
            get { return _dateRangeText; }
            set
            {
                if (_dateRangeText != value)
                {
                    _dateRangeText = value;
                    OnPropertyChanged("DateRangeText");
                }
            }
        }

        private bool _isShowVolume = true;
        public bool IsShowVolume
        {
            get
            {
                return _isShowVolume;
            }
            set
            {
                _isShowVolume = value;
                if (VolumeSeries != null)
                {
                    VolumeSeries.Visibility = _isShowVolume ? SeriesVisibility.Visible : SeriesVisibility.Hidden;
                }
            }
        }

        private ChartType _chartType;
        public ChartType ChartType
        {
            get
            {
                return _chartType;
            }
            set
            {
                _chartType = value;
                OnPropertyChanged("ChartType");

                if (MainSeries != null)
                {
                    MainSeries.ChartType = _chartType;
                    ChangeMode();
                }
            }
        }

        private bool _isShowMovingAverage = false;
        public bool IsShowMovingAverage
        {
            get { return _isShowMovingAverage; }
            set
            {
                _isShowMovingAverage = value;
                OnPropertyChanged("IsShowMovingAverage");
                ChangeMovingAverageVisibility();
            }
        }

        private C1.Chart.MovingAverageType _movingAverageType = C1.Chart.MovingAverageType.Simple;
        public C1.Chart.MovingAverageType MovingAverageType
        {
            get { return _movingAverageType; }
            set { _movingAverageType = value; ChangeMovingAverageType(); }
        }
        
        private int _movingAveragePeriod = 10;
        public int MovingAveragePeriod
        {
            get { return _movingAveragePeriod; }
            set { _movingAveragePeriod = value; ChangeMovingAveragePeriod(); }
        }

        private bool _isShowLineMarker = false;
        public bool IsShowLineMarker
        {
            get
            {
                return _isShowLineMarker;
            }
            set
            {
                _isShowLineMarker = value;
                OnPropertyChanged("IsShowLineMarker");
            }
        }

        private bool _isShowNews = false;
        public bool IsShowNews
        {
            get
            {
                return (!IsComparisonMode && _isShowNews);
            }
            set
            {
                _isShowNews = value;
                ChangeNewsVisibility();
            }
        }

        private void RefrushComparisonMode()
        {
            IsComparisonMode = this.SymbolCollection != null && (from p in this.SymbolCollection where p.Visibility == SeriesVisibility.Visible select p).Any();
        }

        private bool _isComparisonMode = false;
        public bool IsComparisonMode
        {
            get
            {
                return _isComparisonMode; // 
            }

            set
            {
                if (_isComparisonMode != value)
                {
                    _isComparisonMode = value;
                    OnPropertyChanged("IsComparisonMode");
                    if (_isComparisonMode)
                    {
                        ChartType = ChartType.Line;
                    }
                    ChangeMode();
                }
                else
                    UpdateDataRange();

            }
        }

        private void ChangeMode()
        {
            if (IsComparisonMode)
            {
                this.Binding = "percentage";
                MainChart.AxisY.Format = "P0";
            }
            else
            {
                if (ChartType == ChartType.Candlestick || ChartType == ChartType.HighLowOpenClose)
                {
                    this.Binding = "high,low,open,close";
                }
                else
                {
                    this.Binding = "close";
                }
                MainChart.AxisY.Format = null;
            }
            UpdateDataRange();
        }


        #endregion

        #region Command

        private ChangeSymbolCommand _changeSymbolCommand;
        public ChangeSymbolCommand ChangeSymbolCommand
        {
            get
            {
                if (_changeSymbolCommand == null)
                {
                    _changeSymbolCommand = new ChangeSymbolCommand(this);
                }
                return _changeSymbolCommand;
            }
        }
        private string _changeSymbolCommandParamter;
        public string ChangeSymbolCommandParamter
        {
            get
            {
                return _changeSymbolCommandParamter;
            }
            set
            {
                _changeSymbolCommandParamter = value;
                OnPropertyChanged("ChangeSymbolCommandParamter");
                if (ChangeSymbolCommand != null)
                {
                    ChangeSymbolCommand.RaiseCanExecuteChanged();
                }
            }
        }



        private AddCommand _addCommand;
        public AddCommand AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new AddCommand(this);
                }
                return _addCommand;
            }
        }
        private string _addCommandParamter;
        public string AddCommandParamter
        {
            get
            {
                return _addCommandParamter;
            }
            set
            {
                _addCommandParamter = value;
                OnPropertyChanged("AddCommandParamter");
                if (AddCommand != null)
                {
                    AddCommand.RaiseCanExecuteChanged();
                }
            }
        }

        private ResetCommand _resetCommand;
        public ResetCommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new ResetCommand(this);
                }
                return _resetCommand;
            }
        }


        private ExportCommand _exportCommand;
        public ExportCommand ExportCommand
        {
            get
            {
                if (_exportCommand == null)
                {
                    _exportCommand = new ExportCommand(this);
                }
                return _exportCommand;
            }
        }


        #endregion

        #region  UI property

        public C1.WPF.Chart.Finance.C1FinancialChart MainChart
        {
            get;
            set;
        }

        private C1.WPF.Chart.Series _mainSeries;
        public C1.WPF.Chart.Series MainSeries
        {
            get { return _mainSeries; }
            set
            {
                _mainSeries = value;
                _mainSeries.Binding = this.Binding;
            }
        }
        public C1.WPF.Chart.Finance.MovingAverage _mainMovingAverage;
        public C1.WPF.Chart.Finance.MovingAverage MainMovingAverage
        {
            get { return _mainMovingAverage; }
            set
            {
                _mainMovingAverage = value;
                _mainMovingAverage.Binding = this.Binding;
            }
        }
        public C1.WPF.Chart.Series VolumeSeries
        {
            get;
            set;
        }

        public C1.WPF.Chart.Interaction.C1RangeSelector RangeSelector
        {
            get;
            set;
        }

        public C1.WPF.Chart.Annotation.AnnotationLayer AnnotationLayer
        {
            get;
            set;
        }

        private C1.WPF.Chart.Interaction.C1LineMarker _lineMarker;
        public C1.WPF.Chart.Interaction.C1LineMarker LineMarker
        {
            get { return _lineMarker; }
            set
            {
                if (_lineMarker!= null)
                {
                    _lineMarker.PositionChanged -= LineMarker_PositionChanged;
                }
                _lineMarker = value;
                _lineMarker.PositionChanged += LineMarker_PositionChanged;
            }
        }

        private double _lowValue = DateTime.Parse(string.Format("01-01-{0}", DateTime.Now.Year)).ToOADate();
        public double LowerValue
        {
            get
            {
                return _lowValue;
            }
            set
            {
                _lowValue = value;
                RangeSelectorChanged();
            }
        }

        private double _upperValue = DateTime.Now.ToOADate();

        public double UpperValue
        {
            get
            {
                return _upperValue;
            }
            set
            {
                _upperValue = value;
                RangeSelectorChanged();
            }
        }


        #endregion

        #region Mothod


        private void UpdateRangeSelect(int index)
        {
            if (RangeSelector != null)
            {
                DateTime lowValue = DateTime.Now;
                switch (index)
                {
                    case 0:
                        lowValue = DateTime.Now.AddMonths(-1);
                        break;
                    case 1:
                        lowValue = DateTime.Now.AddMonths(-3);
                        break;
                    case 2:
                        lowValue = DateTime.Now.AddMonths(-6);
                        break;
                    case 3:
                        lowValue = DateTime.Parse(string.Format("01-01-{0}", DateTime.Now.Year));
                        break;
                    case 4:
                        lowValue = DateTime.Now.AddYears(-1);
                        break;
                    case 5:
                        lowValue = DateTime.Now.AddYears(-3);
                        break;
                    case 6:
                        lowValue = DateTime.Now.AddYears(-6);
                        break;
                    case 7:
                        lowValue = ViewModel.ChartViewModel.Instance.IndexDataSource.Min(p => p.date);
                        break;
                    default:
                        break;
                }
                RangeSelector.LowerValue = lowValue.ToOADate();
                RangeSelector.UpperValue = DateTime.Now.ToOADate();
            }
        }

        internal IEnumerable<QuoteRange> GetYRange(double low, double high)
        {
            IEnumerable<QuoteRange> ranges = (new QuoteRange[] { DataService.Instance.GetSymbolYRange(DataSource, low, high, this.Binding) });

            if (SymbolCollection != null && SymbolCollection.Any())
            {
                var cssRange = from cs in SymbolCollection where cs.Visibility == SeriesVisibility.Visible select DataService.Instance.GetSymbolYRange(cs.DataSource, low, high, this.Binding);
                ranges = ranges.Union(cssRange);
            }
            return ranges;
        }



        internal void RangeSelectorChanged()
        {
            if (MainChart != null)
            {
                MainChart.BeginUpdate();
                UpdateDataRange();

                IEnumerable<QuoteRange> ranges = ViewModel.ChartViewModel.Instance.GetYRange(this.LowerValue, this.UpperValue);
                if (ranges != null && ranges.Any())
                {
                    if (MainChart != null)
                    {
                        MainChart.AxisY.Min = ranges.Min(p => { return p == null ? int.MaxValue : p.PriceMin; });
                        MainChart.AxisY.Max = ranges.Max(p => { return p == null ? int.MinValue : p.PriceMax; });
                    }
                    var quote = ranges.First();
                    if (quote != null && VolumeSeries != null)
                    {
                        VolumeSeries.AxisY.Min = quote.VolumeMin;
                        VolumeSeries.AxisY.Max = quote.VolumeMax * 12;
                    }
                }
                MainChart.EndUpdate();

                if (this.RangeSelector != null)
                {
                    DateRangeText = DateTime.FromOADate(RangeSelector.LowerValue).ToString("MMM dd, yyyy", System.Threading.Thread.CurrentThread.CurrentCulture)
                   + " - " + DateTime.FromOADate(RangeSelector.UpperValue).ToString("MMM dd, yyyy", System.Threading.Thread.CurrentThread.CurrentCulture);
                }
            }
        }


        internal void UpdateDataRange()
        {
            if (MainChart != null)
            {
                DateTime ds = DateTime.FromOADate(this.LowerValue);
                Quote quote = ViewModel.ChartViewModel.Instance.DataSource.FirstOrDefault(p => p.date >= ds);
                if (quote != null)
                {
                    ViewModel.ChartViewModel.Instance.DataSource.ReferenceValue.Value = quote.close;
                }
                
                var thisType = this.MovingAverageType;
                var otherType = MovingAverageType.Simple;
                foreach (MovingAverageType t in Enum.GetValues(typeof(MovingAverageType)))
                {
                    if (t != thisType)
                    {
                        otherType = t;
                        break;
                    }
                }


                if (ViewModel.ChartViewModel.Instance.SymbolCollection != null && ViewModel.ChartViewModel.Instance.SymbolCollection.Any())
                {
                    foreach (var symbol in ViewModel.ChartViewModel.Instance.SymbolCollection)
                    {
                        quote = symbol.DataSource.FirstOrDefault(p => p.date >= ds);
                        if (quote != null)
                        {
                            symbol.DataSource.ReferenceValue.Value = quote.close;
                            if (symbol.Series != null)
                            {
                                symbol.Series.Dispose();
                            }
                            if (symbol.MovingAverage != null)
                            {
                                symbol.MovingAverage.Dispose();
                                symbol.MovingAverage.Type = otherType;
                                symbol.MovingAverage.Type = thisType;
                            }
                        }
                    }
                }
                if (MainSeries != null)
                    MainSeries.Dispose();
                if (MainMovingAverage != null)
                { 
                    MainMovingAverage.Dispose();
                    MainMovingAverage.Type = otherType;
                    MainMovingAverage.Type = thisType;
                }
                UpdateYRange();
            }
        }

        internal void UpdateYRange()
        {
            if (MainChart != null)
            {
                IEnumerable<QuoteRange> ranges = (new QuoteRange[] { DataService.Instance.GetSymbolYRange(DataSource, LowerValue, UpperValue, Binding) });

                if (SymbolCollection != null)
                {
                    var cssRange = from cs in SymbolCollection where cs.Visibility == SeriesVisibility.Visible select DataService.Instance.GetSymbolYRange(cs.DataSource, LowerValue, UpperValue, Binding);
                    ranges = ranges.Union(cssRange);
                }
                if (ranges.Any())
                {
                    MainChart.AxisX.Min = LowerValue;
                    MainChart.AxisX.Max = UpperValue;
                    MainChart.AxisY.Min = ranges.Min(p => { return p == null ? int.MaxValue : p.PriceMin; });
                    MainChart.AxisY.Max = ranges.Max(p => { return p == null ? int.MinValue : p.PriceMax; });
                    if (VolumeSeries != null && ranges.FirstOrDefault() != null)
                    {
                        VolumeSeries.AxisY.Min = ranges.First().VolumeMin;
                        VolumeSeries.AxisY.Max = ranges.First().VolumeMax * 10;
                    }
                }
            }
        }


        void ChangeMovingAverageVisibility()
        {
            if (MainMovingAverage != null)
            {
                MainMovingAverage.Visibility = this.IsShowMovingAverage ? SeriesVisibility.Visible : SeriesVisibility.Hidden;
            }

            foreach (var symbol in SymbolCollection)
            {
                if (symbol != null && symbol.Visibility == SeriesVisibility.Visible && symbol.MovingAverage != null)
                {
                    symbol.MovingAverage.Visibility = this.IsShowMovingAverage ? SeriesVisibility.Visible : SeriesVisibility.Hidden;
                }
            }
        }

        void ChangeMovingAverageType()
        {
            if (MainMovingAverage != null)
            {
                MainMovingAverage.Type = this.MovingAverageType;
            }

            foreach (var symbol in SymbolCollection)
            {
                if (symbol != null && symbol.MovingAverage != null)
                {
                    symbol.MovingAverage.Type = this.MovingAverageType;
                }
            }
        }

        void ChangeMovingAveragePeriod()
        {
            int period = Math.Max(this.MovingAveragePeriod, 2);
            if (MainMovingAverage != null)
            {
                MainMovingAverage.Period = period;
            }

            foreach (var symbol in SymbolCollection)
            {
                if (symbol != null && symbol.MovingAverage != null)
                {
                    symbol.MovingAverage.Period = period;
                }
            }
        }


        void ChangeLineMarkerVisibility()
        {
            if (LineMarker != null)
            {
                LineMarker.Visibility = IsShowLineMarker ? Visibility.Visible : Visibility.Hidden;
            }

        }

        void ChangeNewsVisibility()
        {
            if (AnnotationLayer != null)
            {
                AnnotationLayer.Annotations.Clear();

                if (IsShowNews)
                {
                    SetupAnnotation();
                }
            }
           
        }
        
        private void LineMarker_PositionChanged(object sender, C1.WPF.Chart.Interaction.PositionChangedArgs e)
        {
            var chartPoint = new Point(LineMarker.X, LineMarker.Y);
            var _hitTestInfo = MainChart.HitTest(chartPoint);

            var hitTestInfo = MainChart.HitTest(chartPoint);
            
            
        }

        internal void SetupAnnotation()
        {
            if (AnnotationLayer != null && DataSource != null && DataSource.Any())
            {

                var cache = new Queue<C1.WPF.Chart.Annotation.Square>();


                foreach (var d in DataSource)
                {
                    if (!string.IsNullOrEmpty(d.events))
                    {
                        var dSquare = new C1.WPF.Chart.Annotation.Square("N", 20)
                        {
                            SeriesIndex = 0,
                            Location = new Point((int)d.date.ToOADate(), (int)d.GetValue("close")),
                            Attachment = C1.Chart.Annotation.AnnotationAttachment.DataCoordinate,
                            TooltipText = d.events,
                            
                        };
                        dSquare.ContentStyle = new C1.WPF.Chart.ChartStyle();
                        dSquare.ContentStyle.Stroke = new SolidColorBrush(Colors.Black);
                        dSquare.Style.Fill = new SolidColorBrush(Color.FromArgb(255, 136, 136, 136));
                        dSquare.Style.Stroke = new SolidColorBrush(Colors.Black);
                        dSquare.Style.StrokeThickness = 1;
                        AnnotationLayer.Annotations.Add(dSquare);
                      

                        cache.Enqueue(dSquare);
                    }
                }
            }
        }


        #endregion

    }
}
