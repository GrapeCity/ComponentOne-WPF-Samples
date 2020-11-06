using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Documents;
using C1.Chart;
using C1.WPF;
using C1.WPF.Chart;

namespace CurrencyComparison
{
    /// <summary>
    /// Interaction logic for CurrencyComparisonDemo.xaml
    /// </summary>
    public partial class CurrencyComparisonDemo : UserControl
    {
        Dictionary<string, bool> _visibilityState;
        Axis y2Main, y2Range;
        MeasureMode _measureMode = MeasureMode.Both;
        List<Data> _dtExchangeRate;
        List<Data> _dtPercentageChange;
        List<Data> _sourceTable;
        DateTime _endPlotDate;
        DateTime _startPlotDate;
        TimeFrame _timeFrame = TimeFrame.SixMonths;
        string _baseCurrency;
        bool _mainChartRendered = false;
        bool _rangeChartRendered = false;
        bool isChangeFromLegend;
        CurrencyComparisonModel _viewModel = new CurrencyComparisonModel();

        public CurrencyComparisonDemo()
        {
            InitializeComponent();
            this.DataContext = _viewModel;
            this.Loaded += HandleLoaded;
        }

        void Init()
        {
            _baseCurrency = "USD";
            _sourceTable = HelperExtensions.ImportData();
            _endPlotDate = _sourceTable[0].Date;
            _sourceTable = _sourceTable.FilterTableByDate(_endPlotDate.AddYears(-10));
            _dtExchangeRate = _sourceTable.ConvertToBase(_baseCurrency);
            _dtPercentageChange = _dtExchangeRate.ConvertToPercentage();
            _startPlotDate = _dtExchangeRate[_dtExchangeRate.Count - 1].Date;

            if (_visibilityState == null)
            {
                _visibilityState = new Dictionary<string, bool>();
            }
        }

        void SetUpMainChart()
        {
            y2Main = new Axis();
            y2Main.Position = Position.Right;
            y2Main.Title = "Percentage Change";
            y2Main.Format = "P0";

            for (int i = 0; i < _viewModel.Currencies.Count; i++)
            {
                var currency = _viewModel.Currencies[i];
                currency.ExchangeRateSeries = new ChartSeries
                {
                    Binding = currency.Symbol,
                    SeriesName = currency.Symbol,
                    Style = new ChartStyle()
                };
                currency.PercentageChangeSeries = new ChartSeries
                {
                    Binding = currency.Symbol,
                    SeriesName = currency.Symbol,
                    IsPercentage = true,
                    AxisY = y2Main,
                    Style = new ChartStyle()
                };
                currency.ExchangeRateSeries.Style.Stroke = new SolidColorBrush() { Color = _viewModel.ChartColors[i % _viewModel.ChartColors.Count] };
                currency.PercentageChangeSeries.Style.Stroke = new SolidColorBrush() { Color = _viewModel.ChartColors[i % _viewModel.ChartColors.Count] };
                currency.PercentageChangeSeries.Style.StrokeDashArray = new DoubleCollection() { 5f, 2f };

                //Set Initial Visibilities
                if (currency.Symbol == "JPY")
                {
                    currency.ExchangeRateSeries.Visibility = SeriesVisibility.Visible;
                    currency.PercentageChangeSeries.Visibility = SeriesVisibility.Plot;

                    _visibilityState.Add(currency.Symbol, true);
                }
                else
                {
                    currency.ExchangeRateSeries.Visibility = SeriesVisibility.Legend;
                    currency.PercentageChangeSeries.Visibility = SeriesVisibility.Hidden;
                    _visibilityState.Add(currency.Symbol, false);
                }
                mainChart.Series.Add(currency.ExchangeRateSeries);
                mainChart.Series.Add(currency.PercentageChangeSeries);
            }
            mainChart.SeriesVisibilityChanged += HandleMainChartSeriesVisibilityChanged;
            var layer = AdornerLayer.GetAdornerLayer(mainChart);
            layer.Add(new WatermarkAdorner(mainChart));
        }

        void SetUpRangeChart()
        {
            y2Range = new Axis()
            {
                AxisLine = false,
                Title = string.Empty,
                Labels = false,
                MajorTickMarks = TickMark.None,
                MinorTickMarks = TickMark.None,
            };

            foreach (var currency in _viewModel.Currencies)
            {
                ChartSeries s = new ChartSeries
                {
                    Binding = currency.Symbol,
                    SeriesName = currency.Symbol,
                    Visibility = currency.ExchangeRateSeries.Visibility,
                    Style = new ChartStyle(),
                    ItemsSource = _dtExchangeRate
                };
                ChartSeries p = new ChartSeries
                {
                    Binding = currency.Symbol,
                    SeriesName = currency.Symbol,
                    IsPercentage = true,
                    AxisY = y2Range,
                    Visibility = currency.PercentageChangeSeries.Visibility,
                    Style = new ChartStyle(),
                    ItemsSource = _dtPercentageChange
                };
                s.Style.Stroke = currency.ExchangeRateSeries.Style.Stroke;
                p.Style.Stroke = currency.PercentageChangeSeries.Style.Stroke;
                p.Style.StrokeDashArray = currency.PercentageChangeSeries.Style.StrokeDashArray;

                rangeChart.Series.Add(s);
                rangeChart.Series.Add(p);
            }
        }

        void UpdateGridLines()
        {
            mainChart.AxisY.MajorGrid = _measureMode != MeasureMode.PercentageChange;
            y2Main.MajorGrid = _measureMode == MeasureMode.PercentageChange;
        }

        void UpdateChartView()
        {
            switch (_measureMode)
            {
                case MeasureMode.Both:
                    double max = 0;
                    foreach (var currency in _viewModel.Currencies)
                    {
                        if (Utils.IsVisible(currency.PercentageChangeSeries))
                        {
                            var seriesMax = currency.PercentageChangeSeries.GetValues(0).Max();
                            max = Math.Max(max, seriesMax);
                        }
                    }
                    y2Main.Max = max > 1 ? max + 0.5 : max + 0.2;
                    y2Range.Max = max > 1 ? max + 0.5 : max + 0.2;
                    break;
                case MeasureMode.PercentageChange:
                    y2Main.Max = double.NaN;
                    y2Range.Max = double.NaN;
                    break;
                case MeasureMode.ExchangeRate:
                    y2Main.Max = 0;
                    y2Range.Max = 0;
                    break;
            }
            UpdateGridLines();
        }

        void UpdateChart(TimeFrame timeFrame)
        {
            mainChart.BeginUpdate();
            rangeChart.BeginUpdate();
            _dtPercentageChange = _dtExchangeRate.ConvertToPercentage();
            foreach (var currency in _viewModel.Currencies)
            {
                currency.ExchangeRateSeries.ItemsSource = _dtExchangeRate;
                currency.PercentageChangeSeries.ItemsSource = _dtPercentageChange;
                rangeChart.Series.First(s => !((ChartSeries)s).IsPercentage && s.SeriesName.Equals(currency.Symbol)).ItemsSource = _dtExchangeRate;
                rangeChart.Series.First(s => ((ChartSeries)s).IsPercentage && s.SeriesName.Equals(currency.Symbol)).ItemsSource = _dtPercentageChange;
            }
            rangeChart.EndUpdate();
            mainChart.EndUpdate();
            switch (timeFrame)
            {
                case TimeFrame.FiveDays:
                    mainChart.AxisX.Min = _endPlotDate.AddBusinessDays(-5).ToOADate();
                    mainChart.AxisX.Format = "MMM dd yyyy";
                    break;
                case TimeFrame.TenDays:
                    mainChart.AxisX.Min = _endPlotDate.AddBusinessDays(-10).ToOADate();
                    mainChart.AxisX.Format = "MMM dd yyyy";
                    break;
                case TimeFrame.OneMonth:
                    mainChart.AxisX.Format = "MMM dd yyyy";
                    mainChart.AxisX.Min = _endPlotDate.AddMonths(-1).ToOADate();
                    break;
                case TimeFrame.SixMonths:
                    mainChart.AxisX.Format = "MMM dd yyyy";
                    mainChart.AxisX.Min = _endPlotDate.AddMonths(-6).ToOADate();
                    break;
                case TimeFrame.OneYear:
                    mainChart.AxisX.Format = "MMM yy";
                    mainChart.AxisX.Min = _endPlotDate.AddYears(-1).ToOADate();
                    break;
                case TimeFrame.FiveYears:
                    mainChart.AxisX.Format = "yyyy";
                    mainChart.AxisX.Min = _endPlotDate.AddYears(-5).ToOADate();
                    break;
                case TimeFrame.TenYears:
                    mainChart.AxisX.Format = "yyyy";
                    mainChart.AxisX.Min = _startPlotDate.ToOADate();
                    break;
            }
            mainChart.AxisX.Max = _endPlotDate.ToOADate();
        }

        void UpdateVisibilityState(Currency currency)
        {
            _visibilityState[currency.Symbol] = Utils.IsVisible(currency.ExchangeRateSeries) || Utils.IsVisible(currency.PercentageChangeSeries);
        }

        void UpdateToFromDates()
        {
            var from = DateTime.FromOADate(mainChart.AxisX.GetMin()).ToShortDateString();
            var to = DateTime.FromOADate(mainChart.AxisX.GetMax()).ToShortDateString();
            tbPeriod.Text = string.Format("Period:{0} to {1}", from, to);
        }

        #region Event handler

        void HandleLoaded(object sender, RoutedEventArgs e)
        {
            Init();
            SetUpMainChart();
            SetUpRangeChart();
            cbCurrencies.SelectedIndex = 0;
            cbMeasureModes.SelectedIndex = 2;
        }

        void HandleMainChartSeriesVisibilityChanged(object sender, SeriesEventArgs e)
        {
            var series = e.Series as ChartSeries;
            var currency = _viewModel.Currencies.First(c => c.Symbol.Equals(series.SeriesName));
            if (isChangeFromLegend)
            {
                if (!series.IsPercentage)
                {
                    if (_measureMode == MeasureMode.Both)
                    {
                        if (Utils.IsVisible(currency.ExchangeRateSeries))
                            currency.PercentageChangeSeries.Visibility = SeriesVisibility.Plot;
                        else
                            currency.PercentageChangeSeries.Visibility = SeriesVisibility.Hidden;
                    }
                }
                UpdateVisibilityState(currency);
            }
            var rangeSeries = rangeChart.Series.First(s => s.SeriesName.Equals(series.SeriesName) && ((ChartSeries)s).IsPercentage == series.IsPercentage);
            rangeSeries.Visibility = series.Visibility;
        }

        void HandleCurrenciesSelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (cbCurrencies.SelectedValue == null)
                return;

            isChangeFromLegend = false;
            var oldBaseCurrency = _baseCurrency;
            _baseCurrency = cbCurrencies.SelectedValue.ToString();
            _dtExchangeRate = _sourceTable.ConvertToBase(_baseCurrency);
            UpdateChart(_timeFrame);
            var newBaseCurrencySeries = _viewModel.Currencies.First(c => c.Symbol.Equals(_baseCurrency));
            var oldBaseCurrencySeries = _viewModel.Currencies.First(c => c.Symbol.Equals(oldBaseCurrency));
            UpdateVisibilityState(newBaseCurrencySeries);
            switch (_measureMode)
            {
                case MeasureMode.ExchangeRate:
                    newBaseCurrencySeries.ExchangeRateSeries.Visibility = SeriesVisibility.Legend;
                    oldBaseCurrencySeries.ExchangeRateSeries.Visibility = _visibilityState[_baseCurrency] ? SeriesVisibility.Visible : SeriesVisibility.Legend;
                    break;
                case MeasureMode.PercentageChange:
                    newBaseCurrencySeries.PercentageChangeSeries.Visibility = SeriesVisibility.Legend;
                    oldBaseCurrencySeries.PercentageChangeSeries.Visibility = _visibilityState[_baseCurrency] ? SeriesVisibility.Visible : SeriesVisibility.Legend;
                    break;
                case MeasureMode.Both:
                    newBaseCurrencySeries.ExchangeRateSeries.Visibility = SeriesVisibility.Legend;
                    newBaseCurrencySeries.PercentageChangeSeries.Visibility = SeriesVisibility.Hidden;
                    oldBaseCurrencySeries.ExchangeRateSeries.Visibility = _visibilityState[_baseCurrency] ? SeriesVisibility.Visible : SeriesVisibility.Legend;
                    oldBaseCurrencySeries.PercentageChangeSeries.Visibility = _visibilityState[_baseCurrency] ? SeriesVisibility.Plot : SeriesVisibility.Hidden;
                    break;
            }
            UpdateChartView();
            isChangeFromLegend = true;
            cbCurrencies.Text = cbCurrencies.SelectedValue.ToString();
        }

        void HandleMeasureModesSelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            isChangeFromLegend = false;
            if (_viewModel.Currencies != null)
            {
                _measureMode = (MeasureMode)cbMeasureModes.SelectedValue;
                foreach (var currency in _viewModel.Currencies)
                {
                    switch (_measureMode)
                    {
                        case MeasureMode.ExchangeRate:
                            if (Utils.IsVisible(currency.PercentageChangeSeries))
                                currency.ExchangeRateSeries.Visibility = SeriesVisibility.Visible;
                            else
                                currency.ExchangeRateSeries.Visibility = SeriesVisibility.Legend;
                            currency.PercentageChangeSeries.Visibility = SeriesVisibility.Hidden;
                            break;

                        case MeasureMode.PercentageChange:
                            if (Utils.IsVisible(currency.ExchangeRateSeries))
                                currency.PercentageChangeSeries.Visibility = SeriesVisibility.Visible;
                            else
                                currency.PercentageChangeSeries.Visibility = SeriesVisibility.Legend;
                            currency.ExchangeRateSeries.Visibility = SeriesVisibility.Hidden;
                            break;

                        case MeasureMode.Both:
                            if (Utils.IsVisible(currency.ExchangeRateSeries) || Utils.IsVisible(currency.PercentageChangeSeries))
                            {
                                currency.ExchangeRateSeries.Visibility = SeriesVisibility.Visible;
                                currency.PercentageChangeSeries.Visibility = SeriesVisibility.Plot;
                            }
                            else
                            {
                                currency.PercentageChangeSeries.Visibility = SeriesVisibility.Hidden;
                                currency.ExchangeRateSeries.Visibility = SeriesVisibility.Legend;
                            }
                            break;
                    }
                }
                UpdateChartView();
            }
            isChangeFromLegend = true;
        }

        void HandleRangeSelectorValueChanged(object sender, EventArgs e)
        {
            if (_rangeChartRendered && _mainChartRendered)
            {
                mainChart.AxisX.Min = rangeSelector.LowerValue;
                mainChart.AxisX.Max = rangeSelector.UpperValue;
                mainChart.AxisX.Format = string.Empty;

                UpdateToFromDates();
            }
        }

        void HandleMainChartRendered(object sender, RenderEventArgs e)
        {
            var min = mainChart.AxisX.GetMin();
            if (!double.IsNaN(min))
            {
                _mainChartRendered = true;
                rangeSelector.LowerValue = mainChart.AxisX.GetMin();
                mainChart.Rendered -= HandleMainChartRendered;
            }
        }

        void HandleRangeChartRendered(object sender, RenderEventArgs e)
        {
            _rangeChartRendered = true;
        }

        void HandleMainChartMouseMove(object sender, MouseEventArgs e)
        {
            var hitTestInfo = mainChart.HitTest(e.GetPosition(mainChart));
            if (hitTestInfo != null && hitTestInfo.Series != null)
            {
                var series = (ChartSeries)hitTestInfo.Series;
                if (series.IsPercentage)
                    mainChart.ToolTipContent = "Currency: {seriesName}\nDate: {x:MM-dd-yyyy}\nValue: {value:p4}";
                else
                    mainChart.ToolTipContent = "Currency: {seriesName}\nDate: {x:MM-dd-yyyy}\nValue: {value}";
            }
        }

        void HandleRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var btnSelectedTimeFrame = sender as RadioButton;
            if (btnSelectedTimeFrame.Tag == null)
                return;

            _timeFrame = _viewModel.DictTimeFrame[btnSelectedTimeFrame.Tag.ToString()];
            UpdateChart(_timeFrame);
            if (rangeSelector != null)
            {
                rangeSelector.ValueChanged -= HandleRangeSelectorValueChanged;
                rangeSelector.UpperValue = _endPlotDate.ToOADate();
                rangeSelector.LowerValue = mainChart.AxisX.Min;
                rangeSelector.ValueChanged += HandleRangeSelectorValueChanged;
            }
            UpdateToFromDates();
            UpdateChartView();
        }

        #endregion
    }
}
