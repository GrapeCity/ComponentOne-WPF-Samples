using C1.DataCollection;
using ListViewExplorer.Resources;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class Sorting : UserControl
    {
        SortDirection? sortDirection;

        public Sorting()
        {
            InitializeComponent();
            Tag = AppResources.SortDescription;
            listView.ItemsSource = Person.Generate(100);
            UpdateButtonText();
        }

        private async void btnSort_Click(object sender, RoutedEventArgs e)
        {
            sortDirection = sortDirection != SortDirection.Ascending ? SortDirection.Ascending : SortDirection.Descending;
            UpdateButtonText();
            listView.ScrollToVerticalOffset(0);
            await listView.DataCollection.SortAsync("Name", sortDirection.Value);
        }

        private void UpdateButtonText()
        {
            btnSort.Content = sortDirection != SortDirection.Ascending ? AppResources.SortAscendingly: AppResources.SortDescendingly;
        }
    }
}
