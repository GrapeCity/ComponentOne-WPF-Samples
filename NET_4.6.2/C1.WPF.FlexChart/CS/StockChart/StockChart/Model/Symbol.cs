using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace StockChart
{
    public  class Symbol : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public Symbol(string code)
        {
            Code = code.ToUpper();
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged("Code"); }
        }

        public System.Windows.Media.Color Color
        {
            get;
            set;
        }


        public C1.Chart.SeriesVisibility Visibility
        {
            get { return MovingAverage == null || Series == null ? C1.Chart.SeriesVisibility.Hidden : MovingAverage.Visibility & Series.Visibility; }
            set
            {
                Series.Visibility = value;
                if (ViewModel.ChartViewModel.Instance.IsShowMovingAverage)
                {
                    MovingAverage.Visibility = value;
                }
                else
                {
                    MovingAverage.Visibility = C1.Chart.SeriesVisibility.Hidden;
                }
                OnPropertyChanged("Visibility");
            }
        }

        internal QuoteData DataSource
        {
            get;
            set;
        }

        internal C1.WPF.Chart.Series Series
        {
            get;
            set;
        }

        internal C1.WPF.Chart.Finance.MovingAverage MovingAverage
        {
            get;
            set;
        }

        public void Dispose()
        {
            this.Series.Dispose();
            this.MovingAverage.Dispose();
            this.DataSource = null;
        }
    }
}
