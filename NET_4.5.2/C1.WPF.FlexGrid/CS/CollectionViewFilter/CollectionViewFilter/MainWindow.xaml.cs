using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CollectionViewFilter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ViewFilter _filter;

        public MainWindow()
        {
            InitializeComponent();

            // create regular view, attach ViewFilter
            var view = new ListCollectionView(Customer.GetCustomerList(200));
            _filter = new ViewFilter(view);

            // show the data
            _flex.ItemsSource = view;
        }

        // apply filter on button click or enter key
        void Button_Click(object sender, RoutedEventArgs e)
        {
            _filter.FilterExpression = _txtFilter.Text;
        }
        void _txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _filter.FilterExpression = _txtFilter.Text;
            }
        }
    }
}
