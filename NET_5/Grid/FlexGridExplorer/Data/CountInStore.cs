using System.ComponentModel;
using System.Windows.Media;

namespace FlexGridExplorer
{
    public class CountInStore : INotifyPropertyChanged
    {
        private Color _color;
        public Car Car { get; set; }
        public int Count { get; set; }
        public Store Store { get; set; }
        public Color Color
        {
            get => _color;
            set 
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        public SolidColorBrush BrushColor
        {
            get
            {
                if (Color == null)
                {
                    return new SolidColorBrush();
                }

                return new SolidColorBrush(Color);
            }
        }

        #region INotifyPropertyChanged implementation

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
