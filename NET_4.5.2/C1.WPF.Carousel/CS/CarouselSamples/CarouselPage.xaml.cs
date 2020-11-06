using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using C1.WPF;
using C1.WPF.Carousel;

namespace CarouselSamples
{
    public partial class CarouselPage : UserControl
    {
        public CarouselPage()
        {
            InitializeComponent();

            InitData();
        }

        private void InitData()
        {
            for (int i = 101; i <= 125; ++i)
            {
                carouselListBox.Items.Add(new BitmapImage(new Uri("Resources/covers/cover" + i + ".jpg", UriKind.Relative)));
            }
        }

        void panelList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (panelList == null || Resources == null || carouselListBox == null)
                return;
            ListBoxItem item = panelList.SelectedItem as ListBoxItem;
            if (item == null)
                return;
            Style style = Resources.Contains(item.Tag) ? Resources[item.Tag] as Style : null;
            if (style != null)
            {
                C1CarouselPanel.ClearCarouselProperties(carouselListBox);
                carouselListBox.Style = style;
            }
        }

    }
}
