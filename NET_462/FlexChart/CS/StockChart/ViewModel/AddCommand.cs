using C1.Chart;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;

namespace StockChart.ViewModel
{

    public class AddCommand : System.Windows.Input.ICommand
    {
        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, new EventArgs());
            }
        }

        ChartViewModel _viewModel;
        public AddCommand(ChartViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.AddCommandParamter != null)
            {
                var text = _viewModel.AddCommandParamter;
                if (!string.IsNullOrEmpty(text))
                {
                    if (_viewModel.SymbolNames.Keys.Contains(text))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute(object parameter)
        {
            C1.WPF.Chart.C1FlexChart chart = _viewModel.MainChart;
            
            var code = parameter.ToString();
            if (chart == null || string.IsNullOrEmpty(code))
            {
                return;
            }

            if (code.ToUpper() == _viewModel.MainSymbol.ToUpper())
            {
                return;
            }
            _viewModel.AddCommandParamter = string.Empty;

            var theSymbol = from p in _viewModel.SymbolCollection where p.Code.ToUpper() == code.ToUpper() select p;
            if (theSymbol != null && theSymbol.Any())
            {
                return;
            }
            
            CreateSysmbol(code, (s) =>
            {

                s.PropertyChanged += (o, e) =>
                {
                    if (e.PropertyName == "Visibility")
                    {
                        _viewModel.UpdateYRange();
                    }
                };
                chart.Dispatcher.BeginInvoke(new Action(() =>
                {
                    theSymbol = from p in _viewModel.SymbolCollection where p.Code.ToUpper() == s.Code.ToUpper() select p;
                    if (theSymbol != null && theSymbol.Any())
                    {
                        return;
                    }

                    _viewModel.Binding = "percentage";
                    InitSymbol(s, _viewModel.Binding);
                    _viewModel.SymbolCollection.Add(s);
                    chart.Series.Add(s.Series);
                    chart.Series.Add(s.MovingAverage);
                    s.Series.Dispose();
                    s.MovingAverage.Dispose();

                    if (_viewModel.SymbolCollection.Count > 4)
                    {
                        var symbol = _viewModel.SymbolCollection.FirstOrDefault();
                        _viewModel.SymbolCollection.Remove(symbol);
                    }
                }));
            });
        }

        private void CreateSysmbol(string code, Action<Symbol> callback)
        {
            Symbol s = null;
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback((oo) =>
            {
                try
                {
                    var symbolData = DataService.Instance.GetSymbolData(code);
                    if (symbolData.Any())
                    {
                        s = new Symbol(code) { DataSource = symbolData };
                        callback(s);
                    }
                    else
                    {
                        throw new System.Net.WebException();
                    }
                }
                catch (System.Net.WebException we)
                {
                    System.Diagnostics.Debug.WriteLine(we.Message);
                }
            }));
        }

        static int IndexMemory = 0;
        void InitSymbol(Symbol s, string property)
        {
            s.Color = ChartViewModel._Colors[IndexMemory % ChartViewModel._Colors.Length];
            IndexMemory++;
            HsbColor hsb = ColorEx.RgbToHsb(s.Color);
            
            s.MovingAverage = new C1.WPF.Chart.Finance.MovingAverage()
            {
                Binding = property,
                Type = MovingAverageType.Simple,
                Period = 10,
            };

            s.MovingAverage.Visibility = _viewModel.IsShowMovingAverage ? SeriesVisibility.Visible : SeriesVisibility.Hidden;
            s.MovingAverage.Style = new C1.WPF.Chart.ChartStyle();
            s.MovingAverage.Style.Fill = s.MovingAverage.Style.Stroke
                = new SolidColorBrush(ColorEx.HsbToRgb(new HsbColor() { A = hsb.A, H = hsb.H, S = Math.Max(hsb.B / 2, 0), B = Math.Min(hsb.B * 2, 1) }));
            s.MovingAverage.ItemsSource = s.DataSource;

            s.Series = new C1.WPF.Chart.Series() { Binding = property, SeriesName = s.Code.ToUpper() };
            s.Series.ChartType = ChartType.Line;
            s.Series.Style = new C1.WPF.Chart.ChartStyle();
            s.Series.Style.StrokeThickness = 2;
            s.Series.Style.Fill = new SolidColorBrush(Color.FromArgb(64, s.Color.R, s.Color.G, s.Color.B));
            s.Series.Style.Stroke = new SolidColorBrush(s.Color);
            s.Series.ItemsSource = s.DataSource;
        }
    }
}
