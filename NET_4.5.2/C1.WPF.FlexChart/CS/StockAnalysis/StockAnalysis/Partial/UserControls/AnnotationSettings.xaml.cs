using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockAnalysis.Partial.UserControls
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class AnnotationSettings : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public AnnotationSettings()
        {
            InitializeComponent();
            this.Loaded += (o, e) =>
              {
                  this.annotationTextSettings.DataContext = Model.AnnotationStyle;
              };

            ViewModel.ViewModel.Instance.PropertyChanged += (o, e)=>
            {
                if (e.PropertyName == "NewAnnotationType")
                {
                    if (ViewModel.ViewModel.Instance.NewAnnotationType == Data.NewAnnotationType.None)
                    {
                        this.fillComboBox.Visibility = Visibility.Collapsed;
                        this.strokeComboBox.Visibility = Visibility.Collapsed;
                        this.fontDropdownControl.Visibility = Visibility.Collapsed;
                    }
                    else if (ViewModel.ViewModel.Instance.NewAnnotationType == Data.NewAnnotationType.Line)
                    {
                        this.fillComboBox.Visibility = Visibility.Collapsed;
                        this.strokeComboBox.Visibility = Visibility.Visible;
                        this.fontDropdownControl.Visibility = Visibility.Visible;
                    }
                    else if (ViewModel.ViewModel.Instance.NewAnnotationType == Data.NewAnnotationType.Text)
                    {
                        this.fillComboBox.Visibility = Visibility.Collapsed;
                        this.strokeComboBox.Visibility = Visibility.Collapsed;
                        this.fontDropdownControl.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        this.fillComboBox.Visibility = Visibility.Visible;
                        this.strokeComboBox.Visibility = Visibility.Visible;
                        this.fontDropdownControl.Visibility = Visibility.Visible;
                    }
                }
            };
        }

        public ViewModel.ViewModel Model
        {
            get { return ViewModel.ViewModel.Instance; }
        }
        
        
    }
}
