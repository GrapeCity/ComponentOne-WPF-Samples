using System;
using System.ComponentModel;
using System.Globalization;
using C1.WPF.Core;
using C1.WPF.TreeView;

namespace DataFilterExplorer
{

    /// <summary>
    /// Represents a custom TypeConverter used to convert the item headers from the sample
    /// to a String representation so we can make use of the auto-search and find operations.
    /// </summary>
    public class CustomItemConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            C1TreeViewItem itemValue = value as C1TreeViewItem;
            if ((itemValue != null) && (itemValue.Header != null))
            {
                var group = itemValue.Header as CarStoreGroup;
                var team = itemValue.Header as CountInStore;

                if (group != null)
                    return group.Car.Model;

                if (team != null)
                    return team.Store.City;
            }

            return value.ToString();
        }
    }
}
