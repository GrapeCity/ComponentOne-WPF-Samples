using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Markup;

namespace Ext
{
    public class EnumerationExtension : MarkupExtension
    {
        public Type EnumType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var list = new List<object>();
            foreach (var value in EnumType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                list.Add(value.GetValue(null));
            }
            return list;
        }
    }
}
