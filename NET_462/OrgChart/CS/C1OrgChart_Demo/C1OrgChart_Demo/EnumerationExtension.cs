using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Reflection;
using System.Collections.Generic;

namespace Util
{
    /// <summary>
    /// MarkupExtension used to bind ComboBox controls to enumeration values.
    /// </summary>
    /// <remarks>
    /// To use this class, bind the ComboBox.SelectedValue property as usual and
    /// use the EnumerationExtension to populate the ComboBox.ItemsSource property.
    /// For example:
    /// <code>
    /// &lt;ComboBox 
    ///   SelectedValue="{Binding ElementName=_orgChart, Path=Orientation, Mode=TwoWay}" 
    ///   ItemsSource="{Binding Source={ext:EnumerationExtension EnumType=Orientation}}" /&lt;
    /// </code>
    /// </remarks>
    public class EnumerationExtension : MarkupExtension
    {
        public Type EnumType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            // this is OK in WPF only
            //return Enum.GetValues(EnumType);

            // this works in WPF and in Silverlight
            var list = new List<object>();
            foreach (var value in EnumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                list.Add(value.GetValue(null));
            }
            return list;
        }
    }
}
