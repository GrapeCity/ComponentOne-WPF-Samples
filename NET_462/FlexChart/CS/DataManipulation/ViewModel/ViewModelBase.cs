using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DataManipulation
{
    public class ViewModelBase<T> : INotifyPropertyChanged where T: FilterBase
    {
        public virtual T Filter
        {
            get;
            set;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase()
        {
            Filter = typeof(T).Assembly.CreateInstance(typeof(T).ToString()) as T;

            this.Filter.PropertyChanged += (o, e) =>
            {
                this.OnPropertyChanged(e.PropertyName);
            };
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string[] Bindings
        {
            get
            {
                return Filter.Bindings;
            }
            set
            {
                Filter.Bindings = value;
            }
        }

        public string BindingX
        {
            get { return Filter.BindingX; }
            set { Filter.BindingX = value; }
        }

        public AggregateType AggregateType
        {
            get { return Filter.AggregateType; }
            set { Filter.AggregateType = value; }
        }

        public SortType SortType
        {
            get
            {
                return Filter.SortType;
            }
            set
            {
                Filter.SortType = value;
            }
        }

        public bool SortOrder
        {
            get
            {
                return Filter.SortOrder;
            }
            set
            {
                Filter.SortOrder = value;
            }
        }
        

        private C1.Chart.ChartType _chartType = C1.Chart.ChartType.Column;
        public C1.Chart.ChartType ChartType
        {
            get
            {
                return _chartType;
            }
            set
            {
                _chartType = value;
            }
        }

        public object Source
        {
            get { return Filter.Source; }
            set { Filter.Source = value; }
        }

        public object DataSource
        {
            get
            {
                return Filter.DataSource;
            }
        }



    }
}
