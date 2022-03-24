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

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for RowDetails.xaml
    /// </summary>
    public partial class RowDetails : UserControl
    {
        public RowDetails()
        {
            InitializeComponent();
            var data = Customer.GetCustomerList(100);
            var view = new ListCollectionView(data);
            _flex.ItemsSource = view;

        }
    }
}
