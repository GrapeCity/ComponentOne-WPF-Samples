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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ImageColumnTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // create some data items
            var list = new List<Status>();
            for (int i = 0; i < 50; i++)
            {
                list.Add(new Status() { Alert = i % 3, Message = "item " + i.ToString() });
            }

            // bind data to grid
            _flex.ItemsSource = list;

            // limit edit options in the Alert column to 0, 1, and 2
            _flex.Columns[0].ValueConverter = new C1.WPF.FlexGrid.ColumnValueConverter(new int[] { 0, 1, 2 }, true);
        }
    }

    /// <summary>
    /// Custom cell factory that creates cells with images based on the 
    /// value of the 'Alert' property in the underlying data item.
    /// </summary>
    public class AlertImageConverter : IValueConverter
    {
        // load static images to show on the grid from application resources
        static BitmapImage _bmpRed, _bmpYellow, _bmpGreen;
        static AlertImageConverter()
        {
            _bmpRed = new BitmapImage(new Uri("/Resources/redBell.png", UriKind.Relative));
            _bmpYellow = new BitmapImage(new Uri("/Resources/yellowBell.png", UriKind.Relative));
            _bmpGreen = new BitmapImage(new Uri("/Resources/greenBell.png", UriKind.Relative));
        }

        // convert 'Alert' int value into corresponding image
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1: return _bmpRed;
                case 2: return _bmpYellow;
            }
            return _bmpGreen;
        }

        // one-way conversion only (ConvertBack is not used)
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Our data item.
    /// </summary>
    public class Status : INotifyPropertyChanged
    {
        int _alert;
        string _msg;

        [Display(Name = "Alert")]
        public int Alert
        {
            get { return _alert; }
            set { _alert = value; OnPropertyChanged("Alert"); }
        }

        [Display(Name = "Message")]
        public string Message
        {
            get { return _msg; }
            set { _msg = value; OnPropertyChanged("Message"); }
        }
        void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
