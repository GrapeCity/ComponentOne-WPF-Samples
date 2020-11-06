using C1.DataCollection;
using C1.WPF.Grid;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace GridShowCase
{
    public class ViewModel : INotifyPropertyChanged
    {
        static Random _rnd = new Random();
        public ViewModel()
        {
            Countries = Country.ReadAll();

            //Toolbar
            DataSize = new ObservableCollection<DataSize>()
            {
               new DataSize(){ DisplayName = "100 rows", Size=100 },
               new DataSize(){ DisplayName = "1000 rows", Size=1000 },
               new DataSize(){ DisplayName = "10000 rows", Size=10000 },
               new DataSize(){ DisplayName = "50000 rows", Size=50000 },
            };
            SelectedSize = DataSize[0];
        }

        private RelayCommand groupByProduct;
        public ICommand GroupByProduct
        {
            get
            {
                if (groupByProduct == null)
                {
                    groupByProduct = new RelayCommand(param => this.GroupBy(param, "ProductName"), param => true);
                }
                return groupByProduct;
            }
        }
        private RelayCommand groupByCountry;
        public ICommand GroupByCountry
        {
            get
            {
                if (groupByCountry == null)
                {
                    groupByCountry = new RelayCommand(param => this.GroupBy(param, "Country"), param => true);
                }
                return groupByCountry;
            }
        }
        
        private async void GroupBy(object parameter, string ColumnName)
        {
            if ((bool)parameter == true)
            {
                await Products.GroupAsync(ColumnName);
            }
            else
            {
                await Products.GroupAsync(Array.Empty<string>());
            }
        }

        public ObservableCollection<Country> Countries { get; set; }

        public C1DataCollection<Product> Products { get; set; }

        private ObservableCollection<DataSize> _dataSize;

        public ObservableCollection<DataSize> DataSize
        {
            get { return _dataSize; }
            set { _dataSize = value; }
        }
        private DataSize _selectedSize;

        public DataSize SelectedSize
        {
            get { return _selectedSize; }
            set
            {
                _selectedSize = value; OnPropertyChanged(); RefreshDataSize();
            }
        }

        private void RefreshDataSize()
        {
            var products = new ObservableCollection<Product>();

            for (int i = 0; i < SelectedSize.Size; i++)
            {
                var countryID = _rnd.Next(0, Countries.Count - 1);
                products.Add(new Product(i) { Country = Countries[countryID], CountryId = countryID });
            }

            Products = new C1DataCollection<Product>(products);

            OnPropertyChanged("Products");
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class DataSize
    {
        public int Size { get; set; }
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return DisplayName.ToString();
        }
    }

    public class RelayCommand : ICommand
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
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion // ICommand Members
    }
}
