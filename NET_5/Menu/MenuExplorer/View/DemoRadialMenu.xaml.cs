using System;
using System.Windows.Controls;
using System.Windows.Input;
using C1.WPF.Core;
using C1.WPF.Menu;

namespace MenuExplorer
{
    /// <summary>
    /// Interaction logic for DemoRadialMenu.xaml
    /// </summary>
    public partial class DemoRadialMenu : UserControl
    {
        public DemoRadialMenu()
        {
            InitializeComponent();
            DataContext = this;
            this.Tag = Properties.Resources.DemoRadialMenuDesc;
            UndoCommand = new DelegateCommand<object>(ExecuteUndoCommand, GetCanExecuteUndoCommand);
            RedoCommand = new DelegateCommand<object>(ExecuteRedoCommand, GetCanExecuteRedoCommand);
            ClearFormatCommand = new DelegateCommand<object>(ExecuteClearFormatCommand, GetCanExecuteClearFormatCommand);
        }

        private void contextMenu_ItemClick(object sender, SourcedEventArgs e)
        {
            C1RadialMenuItem item = e.Source as C1RadialMenuItem;
            if (item is C1RadialColorItem)
            {
                this.text.Foreground = ((C1RadialColorItem)item).Brush;
                txt.Text = "Item Clicked: " + ((C1RadialColorItem)item).Color.ToString();
            }
            else if (item is C1RadialNumericItem)
            {
                txt.FontSize = ((C1RadialNumericItem)item).Value;
                txt.Text = "Item Clicked: " + ((C1RadialNumericItem)item).Value.ToString();
            }
            else
            {
                txt.Text = "Item Clicked: " + (item.Header ?? item.Name).ToString();
            }
        }

        private void contextMenu_ItemOpened(object sender, SourcedEventArgs e)
        {
            C1RadialMenuItem item = e.Source as C1RadialMenuItem;
            txt.Text = "Item Opened: " + (item.Header ?? item.Name).ToString();
        }

        #region ** Commands
        // define some commands and attach them to menu items in xaml
        public DelegateCommand<object> UndoCommand { get; set; }

        private void ExecuteUndoCommand(object parameter)
        {
            txt.Text = "Undo Command executed";
            _canExecuteUndo = false;
            _canExecuteRedo = true;
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private bool _canExecuteUndo = false;
        private bool GetCanExecuteUndoCommand(object parameter)
        {
            return _canExecuteUndo;
        }

        public DelegateCommand<object> RedoCommand { get; set; }

        private void ExecuteRedoCommand(object parameter)
        {
            txt.Text = "Redo Command executed";
            _canExecuteUndo = true;
            _canExecuteRedo = false;
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private bool _canExecuteRedo = false;
        private bool GetCanExecuteRedoCommand(object parameter)
        {
            return _canExecuteRedo;
        }

        public DelegateCommand<object> ClearFormatCommand { get; set; }

        private void ExecuteClearFormatCommand(object parameter)
        {
            txt.Text = "Clear Format Command executed";
            _canExecuteUndo = true;
            _canExecuteRedo = false;
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }

        private bool GetCanExecuteClearFormatCommand(object parameter)
        {
            return true;
        }
        #endregion

        private void contextMenu_Opened(object sender, EventArgs e)
        {
            // expand menu immediately, as in this sample we don't have underlying editable controls
            contextMenu.Expand();
        }
    }


    public class DelegateCommand<T> : ICommand
    {
        private static readonly Action<T> EmptyExecute = (T) => { };
        private static readonly Func<T, bool> EmptyCanExecute = (T) => true;


        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, null)
        {
        }

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public void Execute(T parameter)
        {
            _execute(parameter);
        }


        public bool CanExecute(T parameter)
        {
            return _canExecute(parameter);
        }


        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute((T)parameter);
        }


        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged()
        {
            var h = this.CanExecuteChanged;
            if (h != null)
            {
                h(this, EventArgs.Empty);
            }
        }


        void ICommand.Execute(object parameter)
        {
            this.Execute((T)parameter);
        }
    }
}