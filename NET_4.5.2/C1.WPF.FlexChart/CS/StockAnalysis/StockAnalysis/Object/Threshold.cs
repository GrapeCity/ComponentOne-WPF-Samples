using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace StockAnalysis.Object
{
    public class Threshold: INotifyPropertyChanged
    {
        public Threshold(int value, Brush brush = null)
        {
            this.Value = value;
            if (brush != null)
            {
                this.Brush = brush;
            }
        }
        public Threshold(int value, int minimum, Brush brush = null)
        {
            this.Value = value;
            this.Minimum = minimum;
            if (brush != null)
            {
                this.Brush = brush;
            }
        }

        private int value = 27;
        public int Value
        {
            get { return this.value; }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }
        private int minimum = -999;
        public int Minimum
        {
            get { return this.minimum; }
            set
            {
                this.minimum = value;
                OnPropertyChanged("Minimum");
            }
        }
        private Brush brush = new SolidColorBrush(ViewModel.IndicatorPalettes.StockGreen);
        public Brush Brush
        {
            get { return this.brush; }
            set
            {
                this.brush = value;
                OnPropertyChanged("Brush");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
