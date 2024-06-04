using System;
using System.Reflection;
using System.ComponentModel;
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

namespace MainTestApplication
{
    public partial class SearchBox : UserControl
    {
        List<PropertyInfo> _propertyInfo = new List<PropertyInfo>();
        C1.Util.Timer _timer = new C1.Util.Timer();

        public SearchBox()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromMilliseconds(800);
            _timer.Tick += _timer_Tick;
        }
        public ICollectionView View { get; set; }
        public List<PropertyInfo> FilterProperties
        {
            get { return _propertyInfo; }
        }
        void _txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_imgClear != null)
            {
                // update clear button visibility
                _imgClear.Visibility = string.IsNullOrEmpty(_txtSearch.Text)
                    ? Visibility.Collapsed
                    : Visibility.Visible;

                // start ticking
                _timer.Stop();
                _timer.Start();
            }
        }
        void _imgClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _txtSearch.Text = string.Empty;
        }
        void _timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            if (View != null && _propertyInfo.Count > 0)
            {
                View.Filter = null;
                View.Filter = (object item) =>
                {
                    // get search text
                    var srch = _txtSearch.Text;

                    // no text? show all items
                    if (string.IsNullOrEmpty(srch))
                    {
                        return true;
                    }

                    // show items that contain the text in any of the specified properties
                    foreach (PropertyInfo pi in _propertyInfo)
                    {
                        var value = pi.GetValue(item, null) as string;
                        if (value != null && value.IndexOf(srch, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            return true;
                        }
                    }

                    // exclude this item...
                    return false;
                };
            }
        }
    }
}
