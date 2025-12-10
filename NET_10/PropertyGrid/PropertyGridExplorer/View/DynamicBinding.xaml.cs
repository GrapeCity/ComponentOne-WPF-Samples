using C1.WPF.PropertyGrid;
using PropertyGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Controls;

namespace PropertyGridExplorer
{
    public partial class DynamicBinding : UserControl
    {
        dynamic person = new ExpandoObject();
        Random rand = new Random();

        public DynamicBinding()
        {
            InitializeComponent();
            Tag = AppResources.DynamicBindingDesc;

            person.Name = "Paul";
            person.Age = 22;
            person.Instrument = "Guitar";
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute { MemberName = "[Name]" });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute { MemberName = "[Age]" });
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute { MemberName = "[Instrument]" });
            propertyGrid.SelectedObject = person;
            propertyGrid.AvailableEditors.Clear();
            propertyGrid.AvailableEditors.Add(new StringEditor());

        }

        private void OnAddNewProperty(object sender, System.Windows.RoutedEventArgs e)
        {
            string newPropertyName = $"property {rand.Next(1000)}";
            ((IDictionary<string, object?>)person)[newPropertyName] = rand.Next(1000);
            propertyGrid.PropertyAttributes.Add(new PropertyAttribute { MemberName = $"[{newPropertyName}]" });
        }
    }
}
