using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManipulation
{
    public abstract class FilterBase : INotifyPropertyChanged
    {
        private string[] _bindings;
        private string _bindingX;
        private AggregateType _aggregateType = AggregateType.Sum;
        private SortType _sortType = SortType.None;
        private bool _sortOrder = false;
        
        private object _source;

        public string[] Bindings
        {
            get { return this._bindings; }
            set { this._bindings = value; Analyse(); }
        }

        public string BindingX
        {
            get { return this._bindingX; }
            set { this._bindingX = value; Analyse(); }
        }

        public AggregateType AggregateType
        {
            get { return this._aggregateType; }
            set { this._aggregateType = value; Analyse(); }
        }

        public SortType SortType
        {
            get { return this._sortType; }
            set { this._sortType = value; Analyse(); }
        }

        public bool SortOrder
        {
            get { return this._sortOrder; }
            set { this._sortOrder = value; Analyse(); }
        }
        
        public object Source
        {
            get { return this._source; }
            set { this._source = value; Analyse(); }
        }

        public object DataSource
        {
            get;
            protected set;
        }

        public abstract void Analyse();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #region Util Method
        protected static double GetValue(object obj, string binding)
        {
            var v = Convert.ToDouble(obj.GetType().GetProperty(binding).GetValue(obj, null));

            return v;
        }
        protected static void SetValue(object obj, string binding, object value)
        {
            obj.GetType().GetProperty(binding).SetValue(obj, value, null);
        }

        protected static double[] GetValues(object obj, string[] bindings)
        {
            var values = from p in bindings select Convert.ToDouble(obj.GetType().GetProperty(p).GetValue(obj, null));

            return values.ToArray();
        }
        #endregion
    }



    public class DataSourceChangedEventArgs : EventArgs
    {
        public object DataSource
        {
            get;
            set;
        }
    }
}
