using C1.WPF.Chart;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using C1.Chart;

namespace FlexChartCustomization
{
    /// <summary>
    /// Interaction logic for LegendItems.xaml
    /// </summary>
    public partial class LegendItems : UserControl
    {
        Series defaultSeries, customSeries;
        public LegendItems()
        {
            InitializeComponent();
            customSeries = new SeriesWithPointLegendItems()
            {
                ParentControl = this,
                ShowLegendNames = chbLegendNames.IsChecked.Value,
                ShowCustomIcons = chbLegendCustomIcons.IsChecked.Value,
                ShowLegendGroups = chbLegendGroups.IsChecked.Value,
            };
            customSeries.SeriesName = "Shipments";
            customSeries.SymbolRendering += VendorSeries_SymbolRendering;
            flexChart.Series.Add(customSeries);
        }

        private void VendorSeries_SymbolRendering(object sender, RenderSymbolEventArgs e)
        {
            e.Engine.SetFill(new SolidColorBrush(ViewModel.SmartPhoneVendors.ElementAt(e.Index).Color));
            e.Engine.SetStroke(new SolidColorBrush(ViewModel.SmartPhoneVendors.ElementAt(e.Index).Color));
        }


        // get and store the available size of this control as it is arranged.
        public Size ArrangeBounds { get; set; }
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            ArrangeBounds = arrangeBounds;
            ArrangeBounds = base.ArrangeOverride(arrangeBounds);
            return ArrangeBounds;
        }

        public class SeriesWithPointLegendItems : Series, ISeries
        {
            public LegendItems ParentControl { get; set; }
            public bool ShowLegendGroups { get; set; }
            public bool ShowCustomIcons { get; set; }
            public bool ShowLegendNames { get; set; }

            private ImageSource makeLegendImageSource(ImageSource imgsrc, Brush brush, _Size size)
            {
                if (imgsrc is BitmapImage)
                {
                    BitmapImage bmp = imgsrc as BitmapImage;
                    Canvas cv = new Canvas()
                    {
                        Width = size.Width,
                        Height = size.Height,
                        Background = brush,
                    };

                    var img = new Image() { Source = imgsrc };

                    Canvas.SetLeft(img, (size.Width - bmp.PixelWidth) / 2);
                    Canvas.SetTop(img, (size.Height - bmp.PixelHeight) / 2);
                    cv.Children.Add(img);
                    cv.Arrange(new Rect(new Size(size.Width, size.Height)));

                    var rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
                    rtb.Render(cv);

                    return rtb;
                }
                else
                    return imgsrc;
            }

            /// <summary>
            /// Gets the name of legend.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            string ISeries.GetLegendItemName(int index)
            {
                return ShowLegendNames ? ViewModel.SmartPhoneVendors.ElementAt(index).Name : null;
            }

            /// <summary>
            /// Gets the style of legend.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            _Style ISeries.GetLegendItemStyle(int index)
            {
                return new _Style { Fill = new SolidColorBrush(ViewModel.SmartPhoneVendors.ElementAt(index).Color) };
            }

            /// <summary>
            /// Get the number of series items in the legend.
            /// </summary>
            int ISeries.GetLegendItemLength() { return ViewModel.SmartPhoneVendors.Count; }

            /// <summary>
            /// Get the legend group name for the series item.
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            string ISeries.GetLegendItemGroup(int index)
            {
                if (ShowLegendGroups)
                    return ViewModel.SmartPhoneVendors.ElementAt(index).Country;
                else
                    return null;
            }

            object ISeries.GetLegendItemImageSource(int index, ref C1.Chart._Size imageSize)
            {
                if (ShowCustomIcons)
                {
                    imageSize.Width = 60;
                    imageSize.Height = 60;

                    SmartPhoneVendor vendor = ViewModel.SmartPhoneVendors.ElementAt(index);
                    if (vendor.ImageSource is BitmapImage)
                    {
                        // replace the BitmapImage with a new ImageSource wrapped in the legend item color
                        SolidColorBrush brush = new SolidColorBrush(vendor.Color);
                        vendor.ImageSource = makeLegendImageSource(vendor.ImageSource, brush, imageSize);
                    }

                    // Try and keep the original size of the logo bitmaps, but reduce their size if the chart window
                    // is too small to display the bitmaps properly.
                    double boundsHeight = ParentControl.ArrangeBounds.Height - 50;
                    double divadj = (boundsHeight > 800) ? 13 : 25;
                    double fracHeight = boundsHeight / divadj;
                    if (fracHeight < imageSize.Height)
                        imageSize.Width = imageSize.Height = fracHeight;

                    return vendor.ImageSource;
                }
                else
                    return null;
            }
        }

        private void setCustomOptionCheckBoxes(bool customOptionVisibility)
        {
            System.Windows.Visibility visibility = customOptionVisibility
                ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            chbLegendGroups.Visibility = visibility;
            chbLegendNames.Visibility = visibility;
            chbLegendCustomIcons.Visibility = visibility;
        }

        private void ChbPointsLegends_Changed(object sender, System.Windows.RoutedEventArgs e)
        {
            if (flexChart == null) return;
            if (chbPointsLegends.IsChecked.HasValue && chbPointsLegends.IsChecked.Value)
            {
                flexChart.Series.Clear();
                flexChart.Series.Add(customSeries);
                setCustomOptionCheckBoxes(true);
            }
            else
            {
                if (defaultSeries == null)
                {
                    defaultSeries = new Series();
                    defaultSeries.SeriesName = "Shipments";
                }
                flexChart.Series.Clear();
                flexChart.Series.Add(defaultSeries);
                setCustomOptionCheckBoxes(false);
            }
        }

        private void chbLegendGroups_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (flexChart == null) return;
            if (flexChart.Series[0] == customSeries)
            {
                (customSeries as SeriesWithPointLegendItems).ShowLegendGroups = chbLegendGroups.IsChecked.Value;
                flexChart.Invalidate();
            }
        }

        private void chbLegendCustomIcons_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (flexChart == null) return;
            if (flexChart.Series[0] == customSeries)
            {
                (customSeries as SeriesWithPointLegendItems).ShowCustomIcons = chbLegendCustomIcons.IsChecked.Value;
                flexChart.Invalidate();
            }
        }

        private void chbLegendNames_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (flexChart == null) return;
            if (flexChart.Series[0] == customSeries)
            {
                (customSeries as SeriesWithPointLegendItems).ShowLegendNames = chbLegendNames.IsChecked.Value;
                flexChart.Invalidate();
            }
        }
    }
}
