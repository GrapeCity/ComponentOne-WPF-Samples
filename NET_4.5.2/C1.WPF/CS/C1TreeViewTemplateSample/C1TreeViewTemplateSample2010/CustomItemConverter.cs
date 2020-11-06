using System;
using System.ComponentModel;
using System.Globalization;
using C1.WPF;

namespace C1TreeViewTemplateSample2010
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
                var group = itemValue.Header as WorldCupGroup;
                var team = itemValue.Header as WorldCupTeam;
                
                if (group != null)
                    return group.GroupName;

                if (team != null)
                    return team.CountryName;
            }

            return value.ToString();
        }
    }
}
