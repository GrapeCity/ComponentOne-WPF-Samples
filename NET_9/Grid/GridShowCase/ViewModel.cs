using C1.DataCollection;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace GridShowCase
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string CountryColumnPath = "Country.Name";
        static Random _rnd = new Random();
        public ViewModel()
        {
            Countries = Country.ReadAll();

            //Toolbar
            DataSize = new ObservableCollection<DataSize>()
            {
               new DataSize(){ DisplayName = string.Format(Properties.Resources.XRowsLabel, 100), Size=100 },
               new DataSize(){ DisplayName = string.Format(Properties.Resources.XRowsLabel, 1000), Size=1000 },
               new DataSize(){ DisplayName = string.Format(Properties.Resources.XRowsLabel, 10000), Size=10000 },
               new DataSize(){ DisplayName = string.Format(Properties.Resources.XRowsLabel, 50000), Size=50000 },
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
                    groupByProduct = new RelayCommand(param => GroupBy(param, nameof(Product.ProductName)), param => true);
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
                    groupByCountry = new RelayCommand(param => GroupBy(param, CountryColumnPath), param => true);
                }
                return groupByCountry;
            }
        }

        public bool IsGroupedByCountry
        {
            get
            {
                return Products?.GetGroupDescriptions().Any(gd => gd.GroupPath == CountryColumnPath) ?? false;
            }
        }

        public bool IsGroupedByProduct
        {
            get
            {
                return Products?.GetGroupDescriptions().Any(gd => gd.GroupPath == nameof(Product.ProductName)) ?? false;
            }
        }

        private async void GroupBy(object parameter, string columnName)
        {
            if ((bool)parameter == true)
            {
                await Products.GroupAsync(columnName);
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
                _selectedSize = value;
                OnPropertyChanged();
                RefreshDataSize();
            }
        }

        private void RefreshDataSize()
        {
            if (_selectedSize == null)
                return;
            var products = new ObservableCollection<Product>();

            for (int i = 0; i < SelectedSize.Size; i++)
            {
                var countryID = _rnd.Next(0, Countries.Count - 1);
                products.Add(new Product(i) { Country = Countries[countryID], CountryId = countryID });
            }

            Products?.DetachGroupChanged(OnGroupChanged);
            Products = new C1DataCollection<Product>(products);
            Products.AttachGroupChanged(OnGroupChanged);

            OnPropertyChanged(nameof(Products));
        }

        private void OnGroupChanged(object sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsGroupedByCountry));
            OnPropertyChanged(nameof(IsGroupedByProduct));
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
