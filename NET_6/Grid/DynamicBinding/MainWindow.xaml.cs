using System.Collections.ObjectModel;
using System.Dynamic;
using System.Windows;

namespace DynamicBinding
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var customers = new ObservableCollection<ExpandoObject>();
            
            dynamic employee1 = new ExpandoObject();
            employee1.Name = "John";
            employee1.Age = 18;
            
            dynamic employee2 = new ExpandoObject();
            employee2.Name = "Paul";
            employee2.Age = 22;
            employee2.Instrument = "Guitar";

            dynamic employee3 = new ExpandoObject();
            employee3.Name = "Ringo";
            employee3.Instrument = "Drums";

            dynamic employee4 = new ExpandoObject();
            employee4.Name = "George";
            employee4.Age = 23;

            customers.Add(employee1);
            customers.Add(employee2);
            customers.Add(employee3);
            customers.Add(employee4);

            grid.ItemsSource = customers;

            Language = System.Windows.Markup.XmlLanguage.GetLanguage(Dispatcher.Thread.CurrentUICulture.Name);
        }

        private void OnFilterLoading(object sender, C1.WPF.Grid.GridColumnFilterLoadingEventArgs e)
        {
            e.DataFilter.Filters.Add(new C1.WPF.DataFilter.FullTextFilter { PropertyName = e.Column.Binding });
        }
    }
}
