using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RibbonThemes
{
    public class ImageHeaderData
    {
        public string ImageSource { get; set; }
        public string Header { get; set; }
    }

    public class ImageHeaderToolTipData : ImageHeaderData
    {
        public string ToolTip { get; set; }
        public string ToolTipDescription { get; set; }
    }

    public class ColorData
    {
        public Brush Fill { get; set; }
        public string ToolTip { get; set; }
    }

    public class CellStyleData : ColorData
    {
        public Brush Foreground { get; set; }
    }

    public class LineData
    {
        public DoubleCollection StrokeDashArray { get; set; }
        public double StrokeThickness { get; set; }
        public object Tag { get; set; }
    }

    public class RecentDocuments : INotifyPropertyChanged
    {
        public int Index { get; set; }
        public string Name { get; set; }

        private bool _isPinned;

        public bool IsPinned
        {
            get { return _isPinned; }
            set
            {
                _isPinned = value;
                OnPropertyChanged("IsPinned");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
