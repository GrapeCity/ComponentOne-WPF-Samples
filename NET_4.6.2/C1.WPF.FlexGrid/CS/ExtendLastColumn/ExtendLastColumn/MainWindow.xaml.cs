using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace ExtendLastColumn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // create some data
            List<Customer> list = new List<Customer>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(new Customer());
            }

            // show data in both grids
            _flex1.ItemsSource = list;
            _flex2.ItemsSource = list;

            // star sizing with MinWidth
            _flex1.Columns[2].MinWidth = 180;
            _flex1.Columns[2].Width = new GridLength(1, GridUnitType.Star);

            // setting MinWidth = auto-width
            _flex2.AutoSizeColumns(2, 2, 0, true, true);
            _flex2.Columns[2].MinWidth = (int)(_flex2.Columns[2].ActualWidth + .5);
            _flex2.Columns[2].Width = new GridLength(1, GridUnitType.Star);
            _flex2.CellEditEnded += _flex2_CellEditEnded;
        }

        // update MinWidth after edits
        void _flex2_CellEditEnded(object sender, C1.WPF.FlexGrid.CellEditEventArgs e)
        {
            var c = e.Column;
            var col = _flex2.Columns[c];
            if (col.MinWidth > 0)
            {
                col.Width = new GridLength(1);
                col.MinWidth = 0;
                _flex2.AutoSizeColumns(c, c, 0, true, true);
                col.MinWidth = (int)(col.ActualWidth + .5);
                col.Width = new GridLength(1, GridUnitType.Star);
            }
        }
    }

    public class Customer
    {
        static Random _rnd = new Random(0);
        string[] _first = "Joe|Paul|Steve|Mark".Split('|');
        string[] _last = "Stevens|Doe|Smith|Cameron|Paulson|Richards".Split('|');
        string[] _streets = "Main|Broadway|53rd Ave|Embarcadero|El Camino|Van Ness|Shattuck".Split('|');
        public Customer()
        {
            FirstName = _first[_rnd.Next() % _first.Length];
            LastName = _last[_rnd.Next() % _last.Length];
            Address = string.Format("{0}, {1}", _rnd.Next(10, 1000), _streets[_rnd.Next() % _streets.Length]);
        }

        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}
