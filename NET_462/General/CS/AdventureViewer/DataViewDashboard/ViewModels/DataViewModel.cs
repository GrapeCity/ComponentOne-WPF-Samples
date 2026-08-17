using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


using System.Windows.Data;
using System.Data.Services.Client;
using System.Windows;
using System.Threading.Tasks;

namespace DataViewDashboard.ViewModels
{
    public class ProxyServiceQuery
    {
        private static Lazy<Dictionary<string, object>> objectRepository = new Lazy<Dictionary<string, object>>(() => new Dictionary<string, object>());
        public static void ProcessQuery(DataViewModelBase sender, DataServiceQuery query, AsyncCallback callback)
        {
            if (!objectRepository.Value.Keys.Contains(query.ToString()))
            {
                query.BeginExecute(callback, null);
            }
            else
            {
                sender.OnCompleteSource(objectRepository.Value[query.ToString()]);
            }
        }
        public static void AddData(string key, object data)
        {
            if (!objectRepository.Value.Keys.Contains(key))
                objectRepository.Value.Add(key, data);
        }
        public static void ClearData(string key)
        {
            objectRepository.Value.Remove(key);
        }
        public static void UpdateData(string key,object data)
        {
            ClearData(key);
            AddData(key, data);
        }
    }
    public abstract class DataViewModelBase : ViewModelBase
    {
        protected DataServiceQuery _query;
        public DataViewModelBase(DataServiceQuery query)
        {
            _query = query;
            GetData();
        }
        protected abstract void OnAsyncQueryCompleted(IAsyncResult result);

        internal void OnCompleteSource(object data)
        {
            Source = (new CollectionViewSource() { Source = data });

            OnPropertyChanged("Source");
            OnPropertyChanged("SelectedItem");
            OnPropertyChanged("SelectedItemIndex");

            Count = 0; // update count
            IsLoading = false;
        }
        private void GetData()
        {
            this.IsLoading = true;
            ProxyServiceQuery.ProcessQuery(this, _query, OnAsyncQueryCompleted);
        }
        public void RefreshData()
        {
            ProxyServiceQuery.ClearData(_query.ToString());
            GetData();
        }
        public CollectionViewSource Source { get; private set; }

        private bool isLoading = false;
        public bool IsLoading
        {
            get
            {
                return isLoading;
            }
            set
            {
                isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        private int count = 1;
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = this.Source.View.Cast<object>().Count();
                OnPropertyChanged("Count");
            }
        }

        public int SelectedItemIndex
        {
            get
            {
                if (this.Source != null && this.Source.View != null)
                    return this.Source.View.CurrentPosition;
                else
                    return 0;
            }
        }

        public object SelectedItem
        {
            get
            {
                if (this.Source != null)
                    return this.Source.View.CurrentItem;
                else
                    return null;
            }
        }

        #region NextItemCommand

        // NextItemCommand
        private RelayCommand nextItemCommand;
        public ICommand NextItemCommand
        {
            get
            {
                if (nextItemCommand == null)
                {
                    nextItemCommand = new RelayCommand(param => this.GoToNextItem(), param => this.CanGoForward());
                }
                return nextItemCommand;
            }
        }

        private void GoToNextItem()
        {
            this.Source.View.MoveCurrentToNext();
            OnPropertyChanged("SelectedItem");
        }

        private bool CanGoForward()
        {
            if (this.SelectedItemIndex + 1 < this.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region LastItemCommand

        // LastItemCommand
        private RelayCommand lastItemCommand;
        public ICommand LastItemCommand
        {
            get
            {
                if (lastItemCommand == null)
                {
                    lastItemCommand = new RelayCommand(param => this.GoToLastItem(), param => this.CanGoForward());
                }
                return lastItemCommand;
            }
        }

        private void GoToLastItem()
        {
            this.Source.View.MoveCurrentToLast();
            OnPropertyChanged("SelectedItem");
        }

        #endregion

        #region PreviousItemCommand

        // PreviousItemCommand
        private RelayCommand previousItemCommand;
        public ICommand PreviousItemCommand
        {
            get
            {
                if (previousItemCommand == null)
                {
                    previousItemCommand = new RelayCommand(param => this.GoToPreviousItem(), param => this.CanGoBack());
                }
                return previousItemCommand;
            }
        }

        private void GoToPreviousItem()
        {
            this.Source.View.MoveCurrentToPrevious();
            OnPropertyChanged("SelectedItem");
        }

        #endregion

        #region FirstItemCommand

        // FirstItemCommand
        private RelayCommand firstItemCommand;
        public ICommand FirstItemCommand
        {
            get
            {
                if (firstItemCommand == null)
                {
                    firstItemCommand = new RelayCommand(param => this.GoToFirstItem(), param => this.CanGoBack());
                }
                return firstItemCommand;
            }
        }

        private void GoToFirstItem()
        {
            this.Source.View.MoveCurrentToFirst();
            OnPropertyChanged("SelectedItem");
        }

        private bool CanGoBack()
        {
            if (this.SelectedItemIndex > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region GoToItemCommand

        // GoToItemCommand
        private RelayCommand goToItemCommand;
        public ICommand GoToItemCommand
        {
            get
            {
                if (goToItemCommand == null)
                {
                    goToItemCommand = new RelayCommand(param => this.GoToItem(param));
                }
                return goToItemCommand;
            }
        }

        private void GoToItem(object parameter)
        {
            int i;
            if (int.TryParse(parameter.ToString(), out i))
            {
                if (i > this.Count) i = this.Count;
                this.Source.View.MoveCurrentToPosition(i - 1);
                OnPropertyChanged("SelectedItem");

            }

        }
        #endregion

    }

    public class ViewTypeViewModelBase : ViewModelBase
    {
        public ViewTypeViewModelBase()
        {

        }
    }

    public class DataGridViewTypeViewModel : ViewTypeViewModelBase
    {
        public DataGridViewTypeViewModel()
        {

        }
    }

    public class PropertyGridViewTypeViewModel : ViewTypeViewModelBase
    {
        public PropertyGridViewTypeViewModel()
        {

        }
    }

    public class TileViewTypeViewModel : ViewTypeViewModelBase
    {
        public TileViewTypeViewModel()
        {

        }
    }

    public class CarouselViewTypeViewModel : ViewTypeViewModelBase
    {
        public CarouselViewTypeViewModel()
        {

        }
    }

    public class BookViewTypeViewModel : ViewTypeViewModelBase
    {
        public BookViewTypeViewModel()
        {

        }
    }
}
