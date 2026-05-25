using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace DataViewDashboard.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        AdventureWorksService.AdventureWorksEntities dataServiceClient;
        
        public MainViewModel()
        {
            if (!App.IsInDesignMode)
            {

                dataServiceClient = new AdventureWorksService.AdventureWorksEntities(new Uri("http://demos.componentone.com/WPF/AdventureWorksService/AdventureWorksService.svc"));
                this.SelectedDataView = new CustomersViewModel(dataServiceClient);
                dataServiceClient.MergeOption = System.Data.Services.Client.MergeOption.OverwriteChanges;
                this.SelectedView = new DataGridViewTypeViewModel();
            }
        }

        private DataViewModelBase _selectedDataView;
        public DataViewModelBase SelectedDataView
        {
            get
            {
                return _selectedDataView;
            }
            set
            {
                _selectedDataView = value;
                OnPropertyChanged("SelectedDataView");
            }
        }

        private ViewTypeViewModelBase _selectedView;
        public ViewTypeViewModelBase SelectedView
        {
            get
            {
                return _selectedView;
            }
            set
            {
                _selectedView = value;
                OnPropertyChanged("SelectedView");
            }
        }

        #region Commands

        // ClearFilter Command
        private RelayCommand clearFilterCommand;
        public ICommand ClearFilterCommand
        {
            get
            {
                if (clearFilterCommand == null)
                {
                    clearFilterCommand = new RelayCommand(param => this.ClearFilter(param), param => this.CanClearFilter());
                }
                return clearFilterCommand;
            }
        }
        private void ClearFilter(object parameter)
        {
            if (this.SelectedDataView.Source != null && this.SelectedDataView.Source.View != null)
            {
                var editableView = this.SelectedDataView.Source.View as IEditableCollectionView;
                if (editableView != null)
                {
                    if (editableView.IsAddingNew)
                    {
                        editableView.CommitNew();
                    }
                    else if (editableView.IsEditingItem)
                    {
                        editableView.CommitEdit();
                    }
                }
                this.SelectedDataView.Source.View.Filter = null;
            }
        }
        private bool CanClearFilter()
        {         
            return true;
        }

        // Refresh Command
        private RelayCommand refreshCommand;
        public ICommand RefreshCommand
        {
            get
            {
                if (refreshCommand == null)
                {
                    refreshCommand = new RelayCommand(param => this.RefreshData(), param => true);
                }
                return refreshCommand;
            }
        }
        private void RefreshData()
        {
            _selectedDataView.RefreshData();
        }

        // ChangeView Command
        private RelayCommand changeViewCommand;
        public ICommand ChangeViewCommand
        {
            get
            {
                if (changeViewCommand == null)
                {
                    changeViewCommand = new RelayCommand(param => this.ChangeView(param), param => this.CanChangeView());
                }
                return changeViewCommand;
            }
        }

        private void ChangeView(object parameter)
        {
            string viewType = parameter.ToString();
            if (viewType.Equals("DataGrid"))
            {
                this.SelectedView = new DataGridViewTypeViewModel();
            }
            else if (viewType.Equals("PropertyGrid"))
            {
                this.SelectedView = new PropertyGridViewTypeViewModel();
            }
            else if (viewType.Equals("TileView"))
            {
                this.SelectedView = new TileViewTypeViewModel();
            }
            else if (viewType.Equals("Carousel"))
            {
                this.SelectedView = new CarouselViewTypeViewModel();
            }
            else if (viewType.Equals("Book"))
            {
                this.SelectedView = new BookViewTypeViewModel();
            }
        }

        private bool CanChangeView()
        {
            if (this.SelectedDataView != null)
                return true;
            else
                return false;
        }

        // ChangeData Command
        private RelayCommand changeDataCommand;
        public ICommand ChangeDataCommand
        {
            get
            {
                if (changeDataCommand == null)
                {
                    changeDataCommand = new RelayCommand(param => this.ChangeData(param), param => this.CanChangeData());
                }
                return changeDataCommand;
            }
        }

        private void ChangeData(object parameter)
        {
            string dataSet = parameter.ToString();
            if (dataSet.Equals("Customers"))
            {
                this.SelectedDataView = new CustomersViewModel(dataServiceClient);
            }
            else if (dataSet.Equals("Products"))
            {
                this.SelectedDataView = new ProductsViewModel(dataServiceClient);
            }
            else if (dataSet.Equals("Orders"))
            {
                this.SelectedDataView = new OrdersViewModel(dataServiceClient);
            }
        }

        private bool CanChangeData()
        {
            return true;
        }

        #endregion
    }

}
