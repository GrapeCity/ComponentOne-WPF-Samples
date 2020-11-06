using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockChart.ViewModel
{
    public class ChangeSymbolCommand : System.Windows.Input.ICommand
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
        public ChangeSymbolCommand(ChartViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            if (_viewModel.ChangeSymbolCommandParamter != null)
            {
                var text = _viewModel.ChangeSymbolCommandParamter.ToString();
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
            if (_viewModel != null && parameter != null)
            {
                _viewModel.MainSymbol = parameter.ToString().ToUpper();

                _viewModel.ChangeSymbolCommandParamter = string.Empty;
            }


        }
    }
}
