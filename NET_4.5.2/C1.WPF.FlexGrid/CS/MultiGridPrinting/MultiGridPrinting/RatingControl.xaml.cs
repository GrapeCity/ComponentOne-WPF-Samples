using System;
using System.Collections.Generic;
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

namespace MultiGridPrinting
{
    public partial class RatingControl : UserControl
    {
        public RatingControl()
        {
            InitializeComponent();
            UpdateStars();
        }

        public int Rating
        {
            get { return (int)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }
        void UpdateStars()
        {
            for (int i = 0; i < _sp.Children.Count; i++)
            {
                _sp.Children[i].Opacity = Rating > i ? 1 : 0.1;
            }
        }
        public static readonly DependencyProperty RatingProperty =
            DependencyProperty.Register(
                "Rating",
                typeof(int),
                typeof(RatingControl),
                new PropertyMetadata(0, RatingControl.OnPropertyChanged));

        static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == RatingProperty)
            {
                RatingControl ctl = (RatingControl)d;
                ctl.UpdateStars();
            }
        }
    }
}
