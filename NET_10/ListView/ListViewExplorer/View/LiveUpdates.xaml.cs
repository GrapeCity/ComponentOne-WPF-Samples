using ListViewExplorer.Resources;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class LiveUpdates : UserControl
    {
        private Random _rand = new Random();
        private ObservableCollection<Person> _customers;

        public LiveUpdates()
        {
            InitializeComponent();

            Tag = AppResources.LiveUpdatesDescription;
            _customers = new ObservableCollection<Person>(Person.Generate(10));
            listView.DisplayMemberPath = nameof(Person.Name);
            listView.ItemsSource = _customers;

            SimulateChanges();
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
                    _customers.Insert(_rand.Next(_customers.Count + 1), new Person(_rand.Next()));
                    break;
                case 1:
                    _customers[_rand.Next(_customers.Count)] = new Person(_rand.Next());
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
