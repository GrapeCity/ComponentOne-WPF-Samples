using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using C1.WPF;
using C1.WPF.DataGrid;
using C1.WPF.DataGrid.Filters;

namespace DataGridSamples
{
    [ContentProperty("Items")]
    public partial class DataGridTabFilters : UserControl, IDataGridFilterUnity
    {
        public DataGridTabFilters()
        {
            DataContext = this;

            InitializeComponent();

            Items = new ObservableCollection<IDataGridFilterUnity>();
            Items.CollectionChanged += delegate
            {
                UpdateTabItems();
            };
        }

        public ObservableCollection<IDataGridFilterUnity> Items
        {
            get { return (ObservableCollection<IDataGridFilterUnity>)GetValue(ItemsProperty); }
            private set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<IDataGridFilterUnity>), typeof(DataGridTabFilters), new PropertyMetadata(null, (d, a) =>
                {
                    (d as DataGridTabFilters).UpdateTabItems();
                }));

        public void UpdateTabItems()
        {
            tabs.ItemsSource = Items.Select(f =>
            {
                if (f is FrameworkElement && ((FrameworkElement)f).Parent is ContentControl)
                {
                    ((ContentControl)((FrameworkElement)f).Parent).Content = null;
                }
                var tabItem = new C1TabItem();
                tabItem.Header = f is DataGridNumericFilter || f is DataGridMultiLineNumericFilter ? "Numeric" : f is DataGridTextFilter || f is DataGridMultiLineTextFilter ? "Text" : f is DataGridNumericRangeFilter ? "Range" : "";
                tabItem.Content = f;
                return tabItem;
            });
        }
        private DataGridFilterState _filter = null;

        public DataGridFilterState Filter
        {
            get
            {
                if (tabs != null && tabs.SelectedItem is IDataGridFilterUnity && ((IDataGridFilterUnity)tabs.SelectedItem).Filter != _filter)
                {
                    _filter = ((IDataGridFilterUnity)tabs.SelectedItem).Filter;
                }
                return _filter;
            }
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    if (tabs != null && tabs.SelectedItem is IDataGridFilterUnity && ((IDataGridFilterUnity)tabs.SelectedItem).Filter != _filter)
                    {
                        ((IDataGridFilterUnity)tabs.SelectedItem).Filter = _filter;
                    }
                    RaisePropertyChanged("Filter");
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="e:PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">Name of the property that was changed.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (C1TabItem removedItem in e.RemovedItems)
            {
                var filterUnity = removedItem.Content as IDataGridFilterUnity;
                if (filterUnity != null)
                {
                    filterUnity.PropertyChanged -= new System.ComponentModel.PropertyChangedEventHandler(filterUnity_PropertyChanged);
                }
            }

            foreach (C1TabItem addedItem in e.AddedItems)
            {
                var filterUnity = addedItem.Content as IDataGridFilterUnity;
                if (filterUnity != null)
                {
                    filterUnity.Filter = _filter;
                    filterUnity.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(filterUnity_PropertyChanged);
                }
            }
        }

        void filterUnity_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Filter = (sender as IDataGridFilterUnity).Filter;
        }
    }
}
