using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace PdfViewerSamples
{
    public class ThumbnailItem : ContentControl
    {
        public ThumbnailItem()
        {
            this.DefaultStyleKey = typeof(ThumbnailItem);
        }

        public double Scale
        {
            get { return (double)GetValue(ScaleProperty); }
            set { SetValue(ScaleProperty, value); }
        }
        public static readonly DependencyProperty ScaleProperty =
            DependencyProperty.Register("Scale", typeof(double), typeof(ThumbnailItem), new PropertyMetadata(1.0));
    }
}
