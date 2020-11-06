using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockChart.ViewModel
{
    public class ResetCommand : System.Windows.Input.ICommand
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
        public ResetCommand(ChartViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.SymbolCollection != null && _viewModel.SymbolCollection.Any())
            {
                return true;
            }
            return false;
        }

        public void Execute(object parameter)
        {
            C1.WPF.Chart.C1FlexChart chart = _viewModel.MainChart;

            foreach (var sysmbol in _viewModel.SymbolCollection)
            {
                if (chart.Series.Contains(sysmbol.Series))
                {
                    chart.Series.Remove(sysmbol.Series);
                }
                if (chart.Series.Contains(sysmbol.MovingAverage))
                {
                    chart.Series.Remove(sysmbol.MovingAverage);
                }
            }
            _viewModel.SymbolCollection.Clear();
            

        }
    }
}
