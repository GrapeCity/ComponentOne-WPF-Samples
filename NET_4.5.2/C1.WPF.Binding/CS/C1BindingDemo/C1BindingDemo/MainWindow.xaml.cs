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

namespace C1BindingDemo
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
            var list = new List<DataObject>();
            for (int i = 0; i < 100; i++)
            {
                var item = new DataObject();
                item.Name = "Item " + i.ToString();
                item.Amount = 100 + 10 * i;
                item.Active = i % 5 != 0;
                list.Add(item);
            }

            // bind to ListBoxes
            _listBox.ItemsSource = list;
            _listBox2.ItemsSource = list;

            // set DataContext to first item
            DataContext = list[0];
        }
        void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var item = DataContext as DataObject;
            item.Amount = slider1.Value;
            item.Name = string.Format("Item {0}", (int)slider1.Value);
            item.Active = slider1.Value > 50;
        }
    }

    public class DataObject : System.ComponentModel.INotifyPropertyChanged
    {
        public string Name
        {
            get { return (string)GetValue("Name", null); }
            set { SetValue("Name", value); }
        }
        public double Amount
        {
            get { return (double)GetValue("Amount", 0.0); }
            set { SetValue("Amount", value); }
        }
        public bool Active
        {
            get { return (bool)GetValue("Active", false); }
            set { SetValue("Active", value); }
        }

        Dictionary<string, object> _dct = new Dictionary<string, object>();
        object GetValue(string name, object defVal)
        {
            object value;
            return _dct.TryGetValue(name, out value)
                ? value
                : defVal;
        }
        void SetValue(string name, object value)
        {
            if (!object.Equals(GetValue(name, null), value))
            {
                _dct[name] = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
                }
            }
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
