using System;
using System.Windows.Input;

namespace WealthHealth
{
    public class PlayCommand : ICommand
    {
        private Action _execute;

        public PlayCommand(Action execute)
        {
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _execute();
        }
    }
}
