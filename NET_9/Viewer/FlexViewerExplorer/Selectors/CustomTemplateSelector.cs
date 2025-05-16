using System.Windows;
using C1.WPF.Core;

namespace FlexViewerExplorer
{
    public class CustomTemplateSelector : C1DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Group)
            {
                return Resources["GroupTemplate"] as DataTemplate;
            }

            if (item is Report)
            {
                return Resources["ReportTemplate"] as DataTemplate;
            }

            return null;
        }
    }
}