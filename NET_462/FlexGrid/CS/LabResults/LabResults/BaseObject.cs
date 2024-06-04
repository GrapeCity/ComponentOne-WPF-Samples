using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LabResults
{
    /// <summary>
    /// Base object class implements INotifyPropertyChanged and OnPropertyChanged.
    /// </summary>
    public class BaseObject : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propName));
        }
    }
}
