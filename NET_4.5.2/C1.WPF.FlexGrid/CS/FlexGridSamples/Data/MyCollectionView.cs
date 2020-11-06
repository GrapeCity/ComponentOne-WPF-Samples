using System;
using System.Collections.Specialized;
using System.Windows.Data;

namespace MainTestApplication
{
    // ICollectionView class that exposes the CollectionChanged event (doh!)
    class MyCollectionView : ListCollectionView
    {
        public MyCollectionView(System.Collections.IList list)
            : base(list)
        {
        }
        new public NotifyCollectionChangedEventHandler CollectionChanged;
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            if (CollectionChanged != null)
            {
                CollectionChanged(this, e);
            }
        }
    }

    // converter used to group countries by their first initial
    class CountryInitialConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((string)value)[0].ToString().ToUpper();
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
