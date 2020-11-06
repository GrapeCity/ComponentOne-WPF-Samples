using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FinancialChartExplorer
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
            var fIndex = formattedText.IndexOf("[b]");
            var lIndex = formattedText.IndexOf("[/b]");
            if (fIndex != -1 && lIndex != -1)
            {
                string title = formattedText.Substring(fIndex + 3, lIndex - fIndex - 3);
                tb.Inlines.Add(new Run() { Text = title, FontWeight = FontWeights.Bold });
                if (len > lIndex + 4)
                {
                    string description = formattedText.Substring(lIndex + 4, len - lIndex - 4);
                    tb.Inlines.Add(new Run() { Text = description });
                }
            }
            else
            {
                tb.Text = formattedText;
            }
        }
    }
}
