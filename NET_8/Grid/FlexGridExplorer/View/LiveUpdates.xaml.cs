using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for LiveUpdates.xaml
    /// </summary>
    public partial class LiveUpdates : UserControl
    {
        private Random _rand = new Random();
        private ObservableCollection<Customer> _customers;

        public LiveUpdates()
        {
            InitializeComponent();
            Tag = AppResources.LiveUpdatesDescription;
            //Title = AppResources.LiveUpdatesTitle;

            grid.AutoGeneratingColumn += OnAutoGeneratingColumn;
            _customers = Customer.GetCustomerList(10);
            grid.ItemsSource = _customers;

            SimulateChanges();
        }

        private void OnAutoGeneratingColumn(object sender, GridAutoGeneratingColumnEventArgs e)
        {
            var columns = new string[] { "FirstName", "LastName", "Country", "City" };
            if (!columns.Contains(e.Property.Name))
                e.Cancel = true;
            e.Column.Width = new GridLength(1, GridUnitType.Star);
            e.Column.MinWidth = double.NaN;
        }

        private async void SimulateChanges()
        {
            await Task.Delay(TimeSpan.FromMilliseconds(800));
            GenerateRandomChange();
            SimulateChanges();
        }

        private void GenerateRandomChange()
        {
            switch (_rand.Next(_customers.Count == 0 ? 1 : _customers.Count == 1 ? 3 : 4))
            {
                case 0:
                    _customers.Insert(_rand.Next(_customers.Count + 1), new Customer(_rand.Next()));
                    break;
                case 1:
                    _customers[_rand.Next(_customers.Count)] = new Customer(_rand.Next());
                    break;
                case 2:
                    _customers.RemoveAt(_rand.Next(_customers.Count));
                    break;
                case 3:
                    _customers.Move(_rand.Next(_customers.Count), _rand.Next(_customers.Count));
                    break;
            }
        }
    }
}
