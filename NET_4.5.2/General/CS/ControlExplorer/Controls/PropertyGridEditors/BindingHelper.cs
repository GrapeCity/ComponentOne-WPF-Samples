using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using C1.WPF.Extended;

namespace ControlExplorer
{
	public static class BindingHelper
	{
		public static void BindEditor(FrameworkElement editor, DependencyProperty dp, PropertyAttribute pa, IValueConverter converter)
		{
			Binding editorBinding = null;
			if (pa.PropertyBinding != null)
			{
				editorBinding = pa.PropertyBinding;
				if (pa.PropertyBinding.Source == null)
				{
					editor.DataContext = pa.SelectedObject;
				}
			}
			else
			{
				editorBinding = new Binding(pa.MemberName);
				editorBinding.Mode = IsReadOnlySource(pa) ? BindingMode.OneWay : BindingMode.TwoWay;
				editorBinding.NotifyOnValidationError = true;
				editorBinding.ValidatesOnExceptions = true;
                editorBinding.ValidatesOnDataErrors = true;
				editorBinding.Converter = converter;
				editorBinding.Source = pa.SelectedObject;
			}
			editor.SetBinding(dp, editorBinding);
		}

		private static bool IsReadOnlySource(PropertyAttribute pa)
		{
			PropertyInfo propInfo = pa.PropertyInfo;
			if (!propInfo.CanWrite)
				return true;
			Type targetType = pa.SelectedObject != null ? pa.SelectedObject.GetType() : propInfo.DeclaringType;
			DependencyPropertyDescriptor dpd = DependencyPropertyDescriptor.FromName(
				propInfo.Name, targetType, targetType);
			if (dpd != null)
				return dpd.IsReadOnly;
			else
				return false;
		}
	}
}
