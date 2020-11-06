using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace DataViewDashboard.Converters
{
    public class ProductDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            AdventureWorksService.ProductModel model = value as AdventureWorksService.ProductModel;
            if (model != null)
            {
                if (model.ProductModelProductDescriptions != null & model.ProductModelProductDescriptions.Count > 0)
                {
                    if (model.ProductModelProductDescriptions.FirstOrDefault<AdventureWorksService.ProductModelProductDescription>().ProductDescription != null)
                    {
                        return model.ProductModelProductDescriptions.FirstOrDefault<AdventureWorksService.ProductModelProductDescription>().ProductDescription.Description;
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
