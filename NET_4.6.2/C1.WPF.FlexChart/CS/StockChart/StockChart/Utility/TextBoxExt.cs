using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace StockChart
{
    public class TextBoxExt : DependencyObject
    {
        public static string GetFormattedText(DependencyObject obj)
        {
            return (string)obj.GetValue(FormattedTextProperty);
        }

        public static void SetFormattedText(DependencyObject obj, string value)
        {
            obj.SetValue(FormattedTextProperty, value);
        }

        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.RegisterAttached("FormattedText", typeof(string), typeof(TextBoxExt),
            new PropertyMetadata(string.Empty, OnFormattedTextPropertyChanged));

        private static void OnFormattedTextPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var tb = obj as TextBlock;
            tb.Inlines.Clear();
            var formattedText = tb.GetValue(FormattedTextProperty) as string;
            if (string.IsNullOrEmpty(formattedText))
                return;

            var len = formattedText.Length;

            var titles = formattedText.Split(new string[] {"[/n]" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var title in titles)
            {
                tb.Inlines.Add(new Run() { Text = title });
            }
        }
    }
}
