using System.ComponentModel;

namespace DrillDown
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class Bindable : INotifyPropertyChanged
    {
        protected void SetProperty<T>(ref T property, T value, string propertyName)
        {
            if (!object.ReferenceEquals(property, value))
            {
                property = value;
                Notify(propertyName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
