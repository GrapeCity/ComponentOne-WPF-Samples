using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StockAnalysis.ViewModel
{
    public struct IndicatorPalettes
    {

        public static Color DefaultBorder = Color.FromArgb(255, 96, 125, 139);

        public static Color Black = Color.FromArgb(255, 0, 0, 0);

        public static Color LightGreen = Color.FromArgb(255, 95, 219, 107);
        public static Color DarkGreen = Color.FromArgb(255, 57, 159, 91);

        public static Color Blue = Color.FromArgb(255, 53, 130, 214);

        public static Color LightYellow = Color.FromArgb(255, 253, 255, 39);
        public static Color Orange = Color.FromArgb(255, 249, 180, 12);

        public static Color DarkRed = Color.FromArgb(255, 196, 0, 0);

        ///00B050   ff1d25
        public static Color StockGreen = Color.FromArgb(255, 00, 176, 80);
        public static Color StockRed = Color.FromArgb(255, 255, 29, 37);


        public static Color OverlayGrey = Color.FromArgb(255, 96, 125, 139);

        public static Color StockBlue = Color.FromArgb(128, 66, 133, 244);
        public static Color StockStrokeBlue = Color.FromArgb(255, 51, 104, 214);

    }

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private ViewModel()
        {
            Data.DataService.GetQuotesAsync(Symbols,
                (o, e) =>
                {
                    Quotes = e.Quotes;
                }
                );
        }

        private static ViewModel instance;
        public static ViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ViewModel();
                }
                return instance;
            }
        }

        private C1.WPF.Chart.RenderMode renderMode = C1.WPF.Chart.RenderMode.Direct2D;
        public C1.WPF.Chart.RenderMode RenderMode
        {
            get
            {
                return renderMode;
            }
        }

        private Dictionary<string, string> symbols;
        public Dictionary<string, string> Symbols
        {
            get
            {
                if (symbols == null)
                {
                    symbols = new Dictionary<string, string>();
                    symbols.Add("AAPL", "Apple");
                    symbols.Add("MSFT", "Microsoft");
                    symbols.Add("INTC", "Intel");
                    symbols.Add("CSCO", "Cisco");
                    symbols.Add("GOOG", "Google");
                    symbols.Add("AMZN", "Amazon");
                    symbols.Add("EBAY", "eBay");
                    symbols.Add("DVMT", "Dell");
                    symbols.Add("ORCL", "Oracle");
                    symbols.Add("VOD", "Vodafone");
                }
                return symbols;
            }
        }


        private IEnumerable<Object.Quote> quotes;
        public IEnumerable<Object.Quote> Quotes
        {
            get
            {
                return quotes;
            }
            private set
            {
                quotes = value;
                OnPropertyChanged("Quotes");
            }
        }

        private bool isLoaded = false;
        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
            internal set
            {
                isLoaded = value;
                OnPropertyChanged("IsLoaded");
            }
        }

        private Object.Quote _currectQuote;
        public Object.Quote CurrectQuote
        {
            get
            {
                return _currectQuote;
            }
            set
            {
                _currectQuote = value;
                OnPropertyChanged("CurrectQuote");
            }

        }

        private double? _lowValue = null;
        public double? LowerValue
        {
            get
            {
                return _lowValue;
            }
            set
            {
                _lowValue = value;
                OnPropertyChanged("LowerValue");
            }
        }

        private double? _upperValue = null;
        public double? UpperValue
        {
            get
            {
                return _upperValue;
            }
            set
            {
                _upperValue = value;
                OnPropertyChanged("UpperValue");
            }
        }

        private IEnumerable<C1.Chart.Finance.FinancialChartType> category_01 = new C1.Chart.Finance.FinancialChartType[]
        {
            C1.Chart.Finance.FinancialChartType.Candlestick,
            C1.Chart.Finance.FinancialChartType.ArmsCandleVolume,
            C1.Chart.Finance.FinancialChartType.CandleVolume,
            C1.Chart.Finance.FinancialChartType.HeikinAshi,
            C1.Chart.Finance.FinancialChartType.HighLowOpenClose,
            C1.Chart.Finance.FinancialChartType.Line,
            C1.Chart.Finance.FinancialChartType.LineSymbols,
        };
        private IEnumerable<C1.Chart.Finance.FinancialChartType> category_02 = new C1.Chart.Finance.FinancialChartType[]
        {
            C1.Chart.Finance.FinancialChartType.Kagi,
            C1.Chart.Finance.FinancialChartType.Renko,
        };

        private C1.Chart.Finance.FinancialChartType chartType = C1.Chart.Finance.FinancialChartType.Candlestick;
        public C1.Chart.Finance.FinancialChartType ChartType
        {
            get { return this.chartType; }
            set
            {
                if (this.chartType != value)
                {
                    if (category_01.Contains(this.chartType) ^ category_01.Contains(value))
                    {
                        OnPropertyChanged("ChartTypeCategory");
                    }
                    this.chartType = value;
                    OnPropertyChanged("ChartType");
                }
            }
        }

        private System.Collections.ObjectModel.ObservableCollection<Type> indicatorCharts;
        public System.Collections.ObjectModel.ObservableCollection<Type> IndicatorCharts
        {
            get
            {
                if (indicatorCharts == null)
                {
                    indicatorCharts = new System.Collections.ObjectModel.ObservableCollection<Type>();
                    //indicatorCharts.CollectionChanged += (o, e)=>
                    //{
                    //    OnPropertyChanged("IndicatorCharts");
                    //};
                }
                return this.indicatorCharts;
            }
        }

        private Dictionary<string, IEnumerable<Data.SettingParam>> settings;
        public Dictionary<string, IEnumerable<Data.SettingParam>> Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new Dictionary<string, IEnumerable<Data.SettingParam>>();
                    InitSettings(settings);
                }
                return settings;
            }
        }


        readonly static uint _defalutPeriod = 14;
        readonly static uint _defalutMminimumPeriod = 2;
        private void InitSettings(Dictionary<string, IEnumerable<Data.SettingParam>> settings)
        {
            settings
                .Add("Average True Range (ATR)",
                new Data.SettingParam[]
                {
                    new Data.SettingParam("Period", typeof(uint), _defalutPeriod, _defalutMminimumPeriod),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "Color"),
                }
                );
            settings
                .Add("Relative Strength Index (RSI)",
                new Data.SettingParam[]
                {
                    new Data.SettingParam("Period", typeof(uint), _defalutPeriod, _defalutMminimumPeriod),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "Color"),

                    new Data.SettingParam("ZonesVisibility", typeof(bool), true, "Show Zones"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(80, 0, new SolidColorBrush(IndicatorPalettes.StockGreen)), "OverBought", "OverBought"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(20, 0, new SolidColorBrush(IndicatorPalettes.StockRed)), "OverSold", "OverSold"),
                }
                );
            settings
                .Add("Commodity Channel Index (CCI)",
                new Data.SettingParam[]
                {
                    new Data.SettingParam("Period", typeof(uint), _defalutPeriod, _defalutMminimumPeriod),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "Color"),

                    new Data.SettingParam("ZonesVisibility", typeof(bool), true, "Show Zones"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(100, 0, new SolidColorBrush(IndicatorPalettes.StockGreen)), "OverBought", "OverBought"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(-100, new SolidColorBrush(IndicatorPalettes.StockRed)), "OverSold", "OverSold"),
                }
                );
            settings
                .Add("Williams% R",
                new Data.SettingParam[]
                {
                    new Data.SettingParam("Period", typeof(uint), _defalutPeriod, _defalutMminimumPeriod),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "Color"),

                    new Data.SettingParam("ZonesVisibility", typeof(bool), true, "Show Zones"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(-20, new SolidColorBrush(IndicatorPalettes.StockGreen)), "OverBought", "OverBought"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(-80, new SolidColorBrush(IndicatorPalettes.StockRed)), "OverSold", "OverSold"),
                }
                );
            settings
                .Add("MACD",
                new Data.SettingParam[]
                {
                    new Data.SettingParam("FastPeriod", typeof(uint), 12, "Fast MA period"),
                    new Data.SettingParam("SlowPeriod", typeof(uint), 26, "Slow MA period"),
                    new Data.SettingParam("SmoothingPeriod", typeof(uint), 9, "Signal smoothing period"),
                    new Data.SettingParam("MacdLineStyle.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "Series", "MACD"),
                    new Data.SettingParam("SignalLineStyle.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DarkGreen), "Series", "Signal"),

                    new Data.SettingParam("IncreasingBar", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.StockGreen), "Increasing Bar"),
                    new Data.SettingParam("DecreasingBar", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.StockRed), "Decreasing Bar"),

                }
                );
            settings
                .Add("Stochastic",
                new Data.SettingParam[]
                {
                    new Data.SettingParam("StochasticType", typeof(Partial.CustomControls.IndicatorSeries.StochasticPresetType), Partial.CustomControls.IndicatorSeries.StochasticPresetType.Manual, "Stochastic Type",
                        new object[]{
                            Partial.CustomControls.IndicatorSeries.StochasticPresetType.Manual,
                            Partial.CustomControls.IndicatorSeries.StochasticPresetType.Fast,
                            Partial.CustomControls.IndicatorSeries.StochasticPresetType.Slow,
                            Partial.CustomControls.IndicatorSeries.StochasticPresetType.Full,
                        }
                    ),

                    new Data.SettingParam("DPeriod", typeof(uint), 3, "%D period"),
                    new Data.SettingParam("KPeriod", typeof(uint), 14, "%K period"),
                    new Data.SettingParam("SmoothingPeriod", typeof(uint), 1, "Signal smoothing period"),
                    new Data.SettingParam("DLineStyle.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "%D Color"),
                    new Data.SettingParam("KLineStyle.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.Orange), "%K Color"),

                    new Data.SettingParam("ZonesVisibility", typeof(bool), true, "Show Zones:"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(80, new SolidColorBrush(IndicatorPalettes.StockGreen)), "OverBought", "OverBought:"),
                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(20, new SolidColorBrush(IndicatorPalettes.StockRed)), "OverSold", "OverSold:"),
                }
                );
            settings
                .Add("Volume",
                new Data.SettingParam[]
                {
                    new Data.SettingParam("UpVolume", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.StockGreen), "UpVolume"),
                    new Data.SettingParam("DownVolume", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.StockRed), "DownVolume"),
                }
                );
            settings
               .Add("Average Directional Index (ADX)",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("Period", typeof(uint), _defalutPeriod, _defalutMminimumPeriod),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.StockGreen), "D+", "UpTrend"),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.StockRed), "D-", "DownTrend"),
                    //new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "ADX", "ADX Color"),
               }
               );
            settings
               .Add("Mass Index",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("Period", typeof(uint), 25, _defalutMminimumPeriod),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DefaultBorder), "Color"),

                    new Data.SettingParam("Threshold", typeof(Object.Threshold), new Object.Threshold(29, 0), "Threshold", "Threshold"),
               }
               );


            settings
               .Add("Kagi",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("RangeMode", typeof(C1.Chart.Finance.RangeMode), C1.Chart.Finance.RangeMode.Fixed, "Range Mode",
                        new object[]{ C1.Chart.Finance.RangeMode.ATR, C1.Chart.Finance.RangeMode.Fixed, C1.Chart.Finance.RangeMode.Percentage }
                    ),
                    new Data.SettingParam("ReversalAmount", typeof(uint), 2, "Reversal Amount"),
                    new Data.SettingParam("DataFields", typeof(C1.Chart.Finance.DataFields), C1.Chart.Finance.DataFields.Close, "Data Fields",
                        new object[]{ C1.Chart.Finance.DataFields.High, C1.Chart.Finance.DataFields.Low, C1.Chart.Finance.DataFields.Open, C1.Chart.Finance.DataFields.Close, C1.Chart.Finance.DataFields.HighLow, C1.Chart.Finance.DataFields.HL2, C1.Chart.Finance.DataFields.HLC3, C1.Chart.Finance.DataFields.HLOC4 }
                    )
               }
               );
            settings
               .Add("Renko",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("RangeMode", typeof(C1.Chart.Finance.RangeMode), C1.Chart.Finance.RangeMode.Fixed, "Range Mode",
                        new object[]{ C1.Chart.Finance.RangeMode.ATR, C1.Chart.Finance.RangeMode.Fixed }
                    ),
                    new Data.SettingParam("ReversalAmount", typeof(uint), 2, "Reversal Amount"),
                    new Data.SettingParam("DataFields", typeof(C1.Chart.Finance.DataFields), C1.Chart.Finance.DataFields.Close, "Data Fields",
                        new object[]{ C1.Chart.Finance.DataFields.High, C1.Chart.Finance.DataFields.Low, C1.Chart.Finance.DataFields.Open, C1.Chart.Finance.DataFields.Close, C1.Chart.Finance.DataFields.HighLow, C1.Chart.Finance.DataFields.HL2, C1.Chart.Finance.DataFields.HLC3, C1.Chart.Finance.DataFields.HLOC4 }
                    )
               }
               );

            settings
               .Add("PointAndFigure",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("DataFields", typeof(C1.Chart.Finance.DataFields), C1.Chart.Finance.DataFields.Close, "Data Fields",
                        new object[]{ C1.Chart.Finance.DataFields.High, C1.Chart.Finance.DataFields.Low, C1.Chart.Finance.DataFields.Open, C1.Chart.Finance.DataFields.Close, C1.Chart.Finance.DataFields.HighLow, C1.Chart.Finance.DataFields.HL2, C1.Chart.Finance.DataFields.HLC3, C1.Chart.Finance.DataFields.HLOC4 }
                    ),
                    new Data.SettingParam("ReversalAmount", typeof(uint), 2, "Reversal"),
                    new Data.SettingParam("Scaling", typeof(C1.Chart.Finance.PointAndFigureScaling), C1.Chart.Finance.PointAndFigureScaling.Traditional, "Scaling",
                        new object[]{ C1.Chart.Finance.PointAndFigureScaling.Traditional, C1.Chart.Finance.PointAndFigureScaling.Fixed, C1.Chart.Finance.PointAndFigureScaling.Dynamic }
                    ),
                    new Data.SettingParam("BoxSize", typeof(uint), 2, "Box Size"),
                    new Data.SettingParam("Period", typeof(uint), 14, "ATR Period"),
                    new Data.SettingParam("SquareGrid", typeof(bool), false),
               }
               );

            settings.Add("IchimokuCloud", new Data.SettingParam[]
         {
                new Data.SettingParam("ConversionPeriod", typeof(uint), 9),
                new Data.SettingParam("BasePeriod", typeof(uint), 26),
                new Data.SettingParam("LeadingPeriod", typeof(uint), 52),
                new Data.SettingParam("LaggingPeriod", typeof(uint), 26),
                new Data.SettingParam("ConversionLineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.Orange), "Conversion Color"),
                new Data.SettingParam("BaseLineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.Black), "Base Color"),
                new Data.SettingParam("LeadingSpanALineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.StockGreen), "Leading A Color"),
                new Data.SettingParam("LeadingSpanBLineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.StockRed), "Leading B Color"),
                new Data.SettingParam("LaggingLineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.StockStrokeBlue), "Lagging Color"),
                new Data.SettingParam("BearishCloudColor", typeof(Brush), new SolidColorBrush(IndicatorPalettes.StockRed), "Bearish Color"),
                new Data.SettingParam("BullishCloudColor", typeof(Brush), new SolidColorBrush(IndicatorPalettes.StockGreen), "Bullish Color")
         });
            settings.Add("Alligator", new Data.SettingParam[]
            {
                new Data.SettingParam("JawPeriod", typeof(uint), 13, "Jaw Period"),
                new Data.SettingParam("TeethPeriod", typeof(uint), 8, "Teeth Period"),
                new Data.SettingParam("LipsPeriod", typeof(uint), 5, "Lips Period"),
                new Data.SettingParam("JawLineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.StockRed), "Jaw Color"),
                new Data.SettingParam("TeethLineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.Black), "Teeth Color"),
                new Data.SettingParam("LipsLineStyle.Stroke", typeof(Brush), new SolidColorBrush(IndicatorPalettes.StockGreen), "Lips Color")
            });

            settings
               .Add("ZigZag",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("Distance", typeof(uint), 5, "Distance"),                   
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.StockGreen), "Color"),
               }
               );

            settings
               .Add("Bollinger Bands",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("Period", typeof(uint), _defalutPeriod, _defalutMminimumPeriod),
                    new Data.SettingParam("Multiplier", typeof(uint), 2),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.OverlayGrey), "Color"),
               }
               );
            settings
               .Add("Envelopes",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("Period", typeof(uint), _defalutPeriod, _defalutMminimumPeriod),
                    new Data.SettingParam("Size", typeof(double), 0.03),
                    new Data.SettingParam("Type", typeof(C1.Chart.Finance.MovingAverageType), C1.Chart.Finance.MovingAverageType.Simple, "Type",
                        new object[]{ C1.Chart.Finance.MovingAverageType.Simple, C1.Chart.Finance.MovingAverageType.Exponential}
                    ),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.OverlayGrey), "Color"),
               }
               );
            settings
               .Add("Fibonacci Retracements",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("Uptrend", typeof(bool), true),
                    new Data.SettingParam("LabelPosition", typeof(C1.Chart.LabelPosition), C1.Chart.LabelPosition.Left, "Label Position",
                        new object[]{ C1.Chart.LabelPosition.Left, C1.Chart.LabelPosition.Center, C1.Chart.LabelPosition.Right }
                    ),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.OverlayGrey), "Color"),
               }
               );
            settings
               .Add("Fibonacci Arcs",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("StartX", typeof(uint), 0),
                    new Data.SettingParam("EndX", typeof(uint), 0),
                    new Data.SettingParam("StartY", typeof(uint), 0),
                    new Data.SettingParam("EndY", typeof(uint), 0),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.OverlayGrey), "Color"),
               }
               );
            settings
               .Add("Fibonacci Fans",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("StartX", typeof(uint), 0),
                    new Data.SettingParam("EndX", typeof(uint), 0),
                    new Data.SettingParam("StartY", typeof(uint), 0),
                    new Data.SettingParam("EndY", typeof(uint), 0),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.OverlayGrey), "Color"),
               }
               );
            settings
               .Add("Fibonacci Time Zones",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("StartX", typeof(uint), 0, 0),
                    new Data.SettingParam("EndX", typeof(uint), 3),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.OverlayGrey), "Color"),
               }
               );
            settings
               .Add("Pivot Point",
               new Data.SettingParam[]
               {
                    new Data.SettingParam("PivotType", typeof(Partial.CustomControls.CustomIndicator.PivotPointType), Partial.CustomControls.CustomIndicator.PivotPointType.Standard, "Pivot Type",
                        new object[]{ Partial.CustomControls.CustomIndicator.PivotPointType.Standard, Partial.CustomControls.CustomIndicator.PivotPointType.Fibonacci }
                    ),

                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.Black), "Pivot", "Pivot"),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.LightGreen), "R1", "Resistance 1"),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DarkGreen), "R2", "Resistance 2"),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.Blue), "R3", "Resistance 3"),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.LightYellow), "S1", "Support 1"),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.Orange), "S2", "Support 2"),
                    new Data.SettingParam("Style.Stroke", typeof(System.Windows.Media.Brush), new SolidColorBrush(IndicatorPalettes.DarkRed), "S3", "Support 3"),
               }
               );
        }

        private System.Collections.ObjectModel.ObservableCollection<Data.OverlayType> overlayTypes;
        public System.Collections.ObjectModel.ObservableCollection<Data.OverlayType> OverlayTypes
        {
            get
            {
                if (overlayTypes == null)
                {
                    overlayTypes = new System.Collections.ObjectModel.ObservableCollection<Data.OverlayType>();
                    overlayTypes.CollectionChanged += (o, e) =>
                    {
                        OnPropertyChanged("OverlayTypes");
                    };
                }
                return overlayTypes;
            }
        }


        private Data.NewAnnotationType newAnnotationType = Data.NewAnnotationType.None;
        public Data.NewAnnotationType NewAnnotationType
        {
            get
            {
                return newAnnotationType;
            }
            set
            {
                if (newAnnotationType != value)
                {
                    newAnnotationType = value;
                    OnPropertyChanged("NewAnnotationType");
                    OnPropertyChanged("AnnotationStyle");
                }
            }
        }

        //private Dictionary<Data.NewAnnotationType, Data.AnnotationStyle> _annotationStyles = new Dictionary<Data.NewAnnotationType, Data.AnnotationStyle>();

        //public Data.AnnotationStyle AnnotationStyle
        //{
        //    get
        //    {
        //        if (!_annotationStyles.Keys.Contains(this.NewAnnotationType))
        //        {
        //            _annotationStyles[this.NewAnnotationType] = new Data.AnnotationStyle();
        //            _annotationStyles[this.NewAnnotationType].PropertyChanged += (o, e) =>
        //            {
        //                OnPropertyChanged("AnnotationStyle");
        //            };
        //        }
        //        return _annotationStyles[this.NewAnnotationType];
        //    }
        //}

        private Data.AnnotationStyle _annotationStyles = null;

        public Data.AnnotationStyle AnnotationStyle
        {
            get
            {
                if (_annotationStyles == null)
                {
                    _annotationStyles = new Data.AnnotationStyle();
                    _annotationStyles.PropertyChanged += (o, e) =>
                    {
                        OnPropertyChanged("AnnotationStyle");
                    };
                    OnPropertyChanged("AnnotationStyle");
                }
                return _annotationStyles;
            }
        }

        internal Data.AnnotationStyle GetAnnotationStyle(Data.NewAnnotationType type)
        {
            if (_annotationStyles == null)
            {
                _annotationStyles = new Data.AnnotationStyle();
                _annotationStyles.PropertyChanged += (o, e) =>
                {
                    OnPropertyChanged("AnnotationStyle");
                };
            }
            return _annotationStyles;
        }


        private Data.AnnotationStyle editingAnnotationStyle;
        public Data.AnnotationStyle EditingAnnotationStyle
        {
            get
            {
                if (editingAnnotationStyle == null)
                {
                    editingAnnotationStyle = new Data.AnnotationStyle();
                    editingAnnotationStyle.PropertyChanged += (o, e) =>
                    {
                        OnPropertyChanged("EditingAnnotationStyle");
                    };
                }
                return editingAnnotationStyle;
            }
        }

    }


}
