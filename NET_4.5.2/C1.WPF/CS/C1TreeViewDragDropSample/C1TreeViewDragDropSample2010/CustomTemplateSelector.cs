using System.Windows;
using C1.WPF;

namespace C1TreeViewDragDropSample2010
{
    /// <summary>
    /// Selects a data template depending on the business object.
    /// If the business object represents a Department it gets the DepartmentTemplate.
    /// If the business object represents an Employee, then it selects the MaleEmployeeTemplate
    /// or the FemaleEmployeeTemplate depending on the Gender property of each employee.
    /// </summary>
    public class CustomTemplateSelector : C1DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Department)
            {
                return Resources["DepartmentTemplate"] as DataTemplate;
            }
            else
            {
                Employee employee = (Employee)item;
                string templateKey = (employee.Gender == 'M') ? "MaleEmployeeTemplate" : "FemaleEmployeeTemplate";
                return Resources[templateKey] as DataTemplate;
            }
        }
    }
}