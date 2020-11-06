using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StockAnalysis.Command
{
    class CloseDropdownCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            DependencyObject obj = parameter as DependencyObject;
            if (obj != null)
            {
                var pop = Utilities.Helper.FindParent<System.Windows.Controls.Primitives.Popup>(obj);
                if (pop != null)
                {
                    pop.IsOpen = false;
                }
            }
        }
        
    }
}
