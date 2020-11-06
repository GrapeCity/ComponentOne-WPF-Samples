using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using C1.Chart;
using C1.WPF.Chart;

namespace AnimationDemo.Views
{
    /// <summary>
    /// Interaction logic for FlexChartAnimation.xaml
    /// </summary>
    public partial class FlexPieAnimation : UserControl
    {
        public FlexPieAnimation()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            NewData();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            chart.BeginUpdate();

            var data = chart.ItemsSource as ObservableCollection<DataItem>;
            if (data != null)
            {
                var npts = data.Count;

                for (var i = 0; i < npts; i++)
                    data[i].Value = rnd.Next(1, 10);
            }

            chart.EndUpdate();
        }

        Random rnd = new Random();

        void NewData(int npts = 6)
        {
            var data = new ObservableCollection<DataItem>();

            for (var i = 0; i < npts; i++)
                data.Add( new DataItem { Value = rnd.Next(1, 10), Name = "Item " + i.ToString() });

            chart.BeginUpdate();
            chart.Binding = "Value";
            chart.BindingName = "Name";

            chart.ItemsSource = data;
            chart.EndUpdate();
        }

        private void chart_Loaded(object sender, RoutedEventArgs e)
        {
            cbAnimation.ItemsSource = new AnimationSettings[]
                    { AnimationSettings.None, AnimationSettings.Load, AnimationSettings.Update, AnimationSettings.All };
            cbAnimation.SelectedValue = AnimationSettings.All;

            NewData();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var data = chart.ItemsSource as ObservableCollection<DataItem>;
            if (data != null)
                data.Add(new DataItem { Value = rnd.Next(1, 10), Name = "Item " + data.Count.ToString() });
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            var data = chart.ItemsSource as ObservableCollection<DataItem>;
            if (data != null && data.Count>0)
                data.RemoveAt(data.Count-1);
        }

        private void cbLabels_Checked(object sender, RoutedEventArgs e)
        {
            chart.DataLabel.Position = cbLabels.IsChecked == true ? PieLabelPosition.Center : PieLabelPosition.None;
        }

        private void cbAnimation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pg1.IsEnabled = (chart.AnimationSettings & AnimationSettings.Load) != 0;
            pg2.IsEnabled = (chart.AnimationSettings & AnimationSettings.Update) != 0;
        }
    }

    public class DataItem : INotifyPropertyChanged
    {
        string name;
        double value;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                if (this.value != value)
                {
                    this.value = value;
                    OnPropertyChanged("Value");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
                PropertyChanged( this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
