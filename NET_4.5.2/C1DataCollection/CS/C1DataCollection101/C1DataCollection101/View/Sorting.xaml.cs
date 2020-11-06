using C1DataCollection101.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C1.DataCollection;
using System.Windows.Controls;
using System.Windows.Media;

namespace C1DataCollection101
{
    public partial class Sorting : Page
    {
        C1DataCollection<Customer> _dataCollection;

        public Sorting()
        {
            InitializeComponent();
            Title = AppResources.SortingTitle;
            var task = UpdateData();

        }

        private async Task UpdateData()
        {
            try
            {
                message.Visibility = System.Windows.Visibility.Collapsed;
                activityIndicator.Visibility = System.Windows.Visibility.Visible;
                _dataCollection = new C1DataCollection<Customer>(Customer.GetCustomerList(100));
                grid.ItemsSource = _dataCollection;
                _dataCollection.SortChanged += OnSortChanged;
                UpdateSortButton();
            }
            catch
            {
                message.Text = AppResources.InternetConnectionError;
                message.Visibility = System.Windows.Visibility.Visible;
            }
            finally
            {
                activityIndicator.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        void OnSortChanged(object sender, EventArgs e)
        {
            UpdateSortButton();
        }

        async void OnSortClicked(object sender, EventArgs e)
        {
            if (_dataCollection != null)
            {
                var direction = GetCurrentSortDirection();
                await _dataCollection.SortAsync(x => x.FirstName, direction == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending);
            }
        }

        private void UpdateSortButton()
        {
            var direction = GetCurrentSortDirection();
            if (direction == SortDirection.Ascending)
            {
                btnImage.Source = Sample.GetImageSource("sort-ascending.png");
            }
            else
            {
                btnImage.Source = Sample.GetImageSource("sort-descending.png");
            }
        }

        private SortDirection GetCurrentSortDirection()
        {
            var sort = _dataCollection.SortDescriptions.FirstOrDefault(sd => sd.SortPath == "FirstName");
            return sort != null ? sort.Direction : SortDirection.Descending;
        }        
    }
}
