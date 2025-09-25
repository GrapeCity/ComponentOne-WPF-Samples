using System.Windows;
using System.Windows.Controls;

namespace InputExplorer
{
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = null;
            if (container is FrameworkElement element)
            {
                Employee employee = null;
                if (item is C1.WPF.ListView.ListViewItem listViewItem)
                {
                    employee = listViewItem.Content as Employee;
                }
                else if (item is Employee e)
                {
                    employee = e;
                }

                if (employee != null)
                {
                    if (employee.Gender == Gender.Male)
                        template = element?.FindResource("Template1") as DataTemplate;
                    else
                        template = element?.FindResource("Template2") as DataTemplate;
                }
            }
            return template;
        }
    }
}
