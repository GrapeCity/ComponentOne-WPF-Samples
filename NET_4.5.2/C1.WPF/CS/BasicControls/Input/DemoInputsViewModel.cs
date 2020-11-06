using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace C1_Demo
{
    public class DemoInputsViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public DemoInputsViewModel()
        {
            Color = Color.FromArgb(0xFF, 0x3D, 0x3D, 0x3D);
            TextColor = Color.FromArgb(0xFF, 0x00, 0xB2, 0xDA);
            Phone = "0009999999";
            CreditCard = "555555555";
            LowQuality = 20;
            HighQuality = 50;
        }

        public string Model { get; set; }
        public string Size { get; set; }
        public Color Color { get; set; }//#FF3D3D3D
        public string Image { get; set; }
        public double ImageSize { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; }//#FF00B2DA
        public double TextSize { get; set; }
        public string Place { get; set; }
        public double LowQuality { get; set; }
        public double HighQuality { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }//0009999999
        public DateTime? Delivery { get; set; }
        public int Payment { get; set; }
        public string CreditCard { get; set; }//555555555
        public string Total { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private double _lowQuality = 20;
        private bool _displayErrors = false;

        //public System.Collections.IEnumerable GetErrors(string propertyName)
        //{
        //    if (_displayErrors)
        //        yield return new Exception(propertyName);
        //    else
        //        yield break;
        //}

        public bool HasErrors
        {
            get { return _displayErrors; }
        }

        public string Error
        {
            get { return _displayErrors ? "Error" : null; }
        }

        public string this[string columnName]
        {
            get { return _displayErrors ? "Error" : null; }
        }
    }
}
