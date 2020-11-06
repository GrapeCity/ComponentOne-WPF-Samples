using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StockAnalysis.Command
{
    public class AnnotationSettingCommand : ICommand
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
                Partial.AnnotationSettings wndSettings = new Partial.AnnotationSettings(key);
                wndSettings.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
                wndSettings.Name = key;

                wndSettings.ShowDialog(App.Current.MainWindow);
            }
        }
    }
}
