using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StockAnalysis.Command
{
    public class SettingCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool canExecute = true;
        public bool CanExecute(object parameter)
        {
            return canExecute;
        }

        public void Execute(object parameter)
        {
            var key = parameter==null ? null : parameter.ToString();
            if (!string.IsNullOrEmpty(key))
            {
                Partial.Settings wndSettings = new Partial.Settings(key);
                wndSettings.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;

                wndSettings.ShowDialog(App.Current.MainWindow);
            }
        }
    }
}
