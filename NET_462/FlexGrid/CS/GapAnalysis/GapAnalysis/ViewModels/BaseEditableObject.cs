using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GapAnalysis
{
    /// <summary>
    /// Base editable data object class (implements INotifyPropertyChanged).
    /// </summary>
    public class BaseEditableObject : BaseObject, IEditableObject
    {
        object _clone;
        public void BeginEdit()
        {
            _clone = this.MemberwiseClone();
        }
        public void CancelEdit()
        {
            if (_clone != null)
            {
                foreach (var p in this.GetType().GetProperties())
                {
                    var value = p.GetValue(_clone, null);
                    p.SetValue(this, value, null);
                }
            }
        }
        public void EndEdit()
        {
            _clone = null;
        }
    }
    /// <summary>
    /// Base data object class (implements INotifyPropertyChanged).
    /// </summary>
    public class BaseObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        protected bool SetValue<T>(ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
    }
}
