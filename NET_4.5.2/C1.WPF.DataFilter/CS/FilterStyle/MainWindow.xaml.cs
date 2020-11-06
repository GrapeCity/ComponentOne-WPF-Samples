using C1.DataCollection;
using C1.WPF.DataFilter;
using System;
using System.Linq;
using System.Windows;

namespace FilterStyle
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataProvider _dataProvider = new DataProvider();

        public MainWindow()
        {
            InitializeComponent();
            var data = new C1DataCollection<Employee>(_dataProvider.GetEmployees());
            c1DataFilter1.ItemsSource = data;
            flexGrid.ItemsSource = data;

            //// Color filter
            //var cf = new ColorFilter
            //{
            //    HeaderText = "Color",
            //    PropertyName = "Color"
            //};
            //cf.SetColors(_dataProvider.Colors.Select(c => c.ToString()));
            //c1DataFilter1.Filters.Add(cf);

            foreach (ChecklistFilter filter in c1DataFilter1.Filters.Where(f => f is ChecklistFilter))
                filter.SelectAll();
        }
    }
}
