using C1.Chart;
using System;
using System.Collections.Generic;
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
using System.Globalization;

namespace FinancialChartExplorer
{
    /// <summary>
    /// Interaction logic for TrendLines.xaml
    /// </summary>
    public partial class TrendLines : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        DataService dataSerivice = DataService.GetService();

        public TrendLines()
        {
            InitializeComponent();
            Tag = "Trend lines are used to visualize trends in data and to help analyze the problems of prediction.";

            Binding bindingMinX = new Binding();
            bindingMinX.Source = this;
            bindingMinX.Path = new PropertyPath("MinX");
            BindingOperations.SetBinding(trendLine, C1.WPF.Chart.TrendLine.MinXProperty, bindingMinX);

            Binding bindingMaxX = new Binding();
            bindingMaxX.Source = this;
            bindingMaxX.Path = new PropertyPath("MaxX");
            bindingMaxX.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(trendLine, C1.WPF.Chart.TrendLine.MaxXProperty, bindingMaxX);
        }
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string pName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(pName));
            }
        }
        
        public List<Quote> Data
        {
            get
            {
                return dataSerivice.GetSymbolData("box");
            }
        }

        private DateTime minDate = DateTime.MinValue;
        public DateTime MinDate
        {
            get
            {
                if (minDate == DateTime.MinValue)
                {
                    minDate = Data.Min(p => p.date);
                }
                return minDate;
            }
        }

        private DateTime maxDate = DateTime.MinValue;
        public DateTime MaxDate
        {
            get
            {
                if (maxDate == DateTime.MinValue)
                {
                    maxDate = Data.Max(p => p.date);
                }
                return maxDate;
            }
        }

        private uint forward = 30;
        public uint Forward
        {
            get { return forward; }
            set { forward = value; MaxX = Convert(MaxDate, (int)Forward); }
        }

        private uint backward = 0;
        public uint Backward 
        {
            get { return backward; }
            set { backward = value; MinX = Convert(MinDate, (int)-Backward);}
        }

        private double minX = double.NaN;
        public double MinX
        {
            get { return minX; }
            set { minX = value; OnPropertyChanged("MinX"); }
        }

        private double maxX = double.NaN;
        public double MaxX
        {
            get { return maxX; }
            set { maxX = value; OnPropertyChanged("MaxX"); }
        }

        private double Convert(DateTime baseValue, int changes)
        {
            var value = double.NaN;
            if (this.chkForecast.IsChecked == true)
            {
                value = baseValue.AddDays(changes).ToOADate();
            }
            return value;
        }

        public List<string> FitType
        {
            get
            {
                return new List<string> { "Linear", "Exponent", "Polynom",
                    "AverageX", "MinX", "MaxX", "AverageY", "MinY", "MaxY"};
            }
        }

        private void chkForecast_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (chkForecast.IsChecked == true)
            {
                financialChart.BeginUpdate();
                financialChart.BindingX = "date";
                financialChart.EndUpdate();
            }
            else
            {
                financialChart.BeginUpdate();
                financialChart.BindingX = "Date";
                financialChart.EndUpdate();
            }
            MinX = Convert(MinDate, (int)-Backward); 
            MaxX = Convert(MaxDate, (int)Forward); 
        }
    }

}
