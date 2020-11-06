using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Data;

namespace GapAnalysis
{
    /// <summary>
    /// ViewModel for "Traditional" binding tab.
    /// </summary>
    public class Traditional
    {
        ICollectionView _products;

        public Traditional()
        {
            var cvs = new CollectionViewSource();
            cvs.Source = Product.GetProducts(2000);
            _products = cvs.View;

            // add grouping
            _products.GroupDescriptions.Add(new PropertyGroupDescription("Line"));
            _products.GroupDescriptions.Add(new PropertyGroupDescription("Rating"));

            // set some invalid values
            _products.MoveCurrentToPosition(6);
            ((Product)_products.CurrentItem).Price = -123;
            _products.MoveCurrentToPosition(9);
            ((Product)_products.CurrentItem).Cost = -123;
            _products.MoveCurrentToFirst();
        }
        public ICollectionView Products
        {
            get { return _products; }
        }
    }

    // ** ValueConverters

   
    public class LineConverter : C1.WPF.FlexGrid.ColumnValueConverter
    {
        public LineConverter()
        {
            SetSource(Product.GetLines(), false);
        }
    }
    public class ColorConverter : C1.WPF.FlexGrid.ColumnValueConverter
    {
        public ColorConverter()
        {
            SetSource(Product.GetColors(), true);
        }
    }
    public class RatingConverter : C1.WPF.FlexGrid.ColumnValueConverter
    {
        public RatingConverter()
        {
            var dct = new System.Collections.Generic.Dictionary<int, string>();
            dct[0] = "Terrible";
            dct[1] = "Bad";
            dct[2] = "Average";
            dct[3] = "Good";
            dct[4] = "Great";
            dct[5] = "Epic";
            SetSource(dct);
        }
    }
}
