using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
#if WPF
using System.Windows;
using System.Windows.Media;
using C1.WPF.Maps;
using System.Windows.Data;
using System.ComponentModel;
#elif WINDOWS_UWP
using C1.Xaml;
using Windows.UI;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using C1.Xaml.Maps;
using ListCollectionView = C1.Xaml.C1CollectionView;
#endif

namespace DashboardModel
{
#if !WinForms
    public class C1MapHelper
    {
        public static EllipseGeometry CreateMarkBySale(double sales, double maxValue)
        {
            double delta = 14 * sales / maxValue;
#if WPF
            return new EllipseGeometry(new Point(0, 0), delta, delta);
#elif WINDOWS_UWP
            return new EllipseGeometry() { Center = new Point(0, 0), RadiusX = delta, RadiusY = delta };
#endif
        }

#if WPF
        public static void UpdataMapMark(C1VectorLayer vectorLayer)
#elif WINDOWS_UWP
        public static void UpdataMapMark(C1VectorLayer vectorLayer, C1VectorLayer scaleLayer,bool isPhone)
#endif
        {
            vectorLayer.Children.Clear();
#if WINDOWS_UWP
            if (!isPhone)
                scaleLayer.Children.Clear();
#endif
            var regionSalesCollection = new ListCollectionView(DataService.GetService().GetRegionWiseSales());
            regionSalesCollection.SortDescriptions.Add(new SortDescription("Sales", ListSortDirection.Descending));
#if WPF
            double maxValue = (regionSalesCollection.GetItemAt(0) as RegionSaleItem).Sales;
#elif WINDOWS_UWP
            double maxValue = (regionSalesCollection.ElementAt(0) as RegionSaleItem).Sales;
#endif
            foreach (RegionSaleItem sales in regionSalesCollection)
            {
                C1VectorPlacemark mark = new C1VectorPlacemark();
                mark.Label = string.Format("{0:C}", sales.Sales);
                mark.LabelPosition = LabelPosition.Center;
                mark.Geometry = CreateMarkBySale(sales.Sales, maxValue);
                mark.Fill = sales.Profit > 0 ? new SolidColorBrush(Colors.Orange) : new SolidColorBrush(Colors.RoyalBlue);
                mark.GeoPoint = sales.Locat;
                vectorLayer.Children.Add(mark);
#if WINDOWS_UWP
                if (!isPhone)
                {
                    C1VectorPlacemark clone = new C1VectorPlacemark();
                    clone.Label = mark.Label;
                    clone.LabelPosition = mark.LabelPosition;
                    clone.Geometry = mark.Geometry;
                    clone.Fill = mark.Fill;
                    clone.GeoPoint = sales.Locat;
                    scaleLayer.Children.Add(clone);
                }
#endif
            }
        }
    }
#endif
}
