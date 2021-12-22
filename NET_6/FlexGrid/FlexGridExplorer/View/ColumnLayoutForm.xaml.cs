using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for ColumnLayoutForm.xaml
    /// </summary>
    public partial class ColumnLayoutForm : Window
    {
        FlexGrid _grid;
        ObservableCollection<ColumnLayoutItemViewModel> _columns = new ObservableCollection<ColumnLayoutItemViewModel>();
        public ColumnLayoutForm(FlexGrid grid)
        {
            InitializeComponent();
            _grid = grid;
            btnOK.Content = AppResources.OK;
            btnCancel.Content = AppResources.Cancel;

            foreach (var column in _grid.Columns)
            {
                _columns.Add(new ColumnLayoutItemViewModel(_columns, column));
            }
            DataContext = _columns;
            UpdateColumns();
        }

        public void UpdateColumns()
        {
            try
            {
                for (int i = 0; i < _columns.Count; i++)
                {
                    var cvm = _columns[i];
                    var currentIndex = _grid.Columns.IndexOf(cvm.Column);
                    if (currentIndex != i)
                    {
                        _grid.Columns.Move(currentIndex, i);
                    }
                    if (cvm.IsVisible != cvm.Column.IsVisible)
                    {
                        cvm.Column.IsVisible = cvm.IsVisible;
                    }
                }
            }
            catch { }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            grid.FinishEditing();
            UpdateColumns();
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class ColumnLayoutItemViewModel
    {
        public ColumnLayoutItemViewModel(ObservableCollection<ColumnLayoutItemViewModel> columns, GridColumn column)
        {
            Columns = columns;
            Column = column;
            Title = column.ColumnName;
            IsVisible = column.IsVisible;
            UpCommand = new MoveCommand(param => this.MoveUp(), param => this.CanMoveUp());
            DownCommand = new MoveCommand(param => this.MoveDown(), param => this.CanMoveDown());
        }

        public ObservableCollection<ColumnLayoutItemViewModel> Columns { get; set; }
        public GridColumn Column { get; set; }
        public string Title { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand DownCommand { get; set; }
        public bool IsVisible { get; set; }

        private bool CanMoveUp()
        {
            return Columns.IndexOf(this) > 0;
        }

        private void MoveUp()
        {
            var currentIndex = Columns.IndexOf(this);
            Columns.Move(currentIndex, currentIndex - 1);
            //Columns.RemoveAt(currentIndex);
            //Columns.Insert(currentIndex - 1, this);
            //UpdateCommands();
        }

        private bool CanMoveDown()
        {
            return Columns.IndexOf(this) < Columns.Count - 1;
        }

        private void MoveDown()
        {
            var currentIndex = Columns.IndexOf(this);
            Columns.Move(currentIndex, currentIndex + 1);
            //Columns.RemoveAt(currentIndex);
            //Columns.Insert(currentIndex + 1, this);
            //UpdateCommands();
        }

        //private void UpdateCommands()
        //{
        //    foreach (var column in Columns)
        //    {
        //        column.UpCommand.CanExecute()
        //        column.DownCommand.ChangeCanExecute();
        //    }
        //}
    }

    public class MoveCommand : ICommand
    {
        #region Fields

        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;

        #endregion // Fields

        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public MoveCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public MoveCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameters)
        {
            return _canExecute == null ? true : _canExecute(parameters);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameters)
        {
            _execute(parameters);
        }

        #endregion // ICommand Members
    }
}
