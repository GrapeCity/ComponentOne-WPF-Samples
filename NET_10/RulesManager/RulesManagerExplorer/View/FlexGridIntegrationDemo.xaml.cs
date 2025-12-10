using C1.WPF.Grid;
using C1.WPF.RulesManager;
using System.Linq;
using System.Windows.Controls;

namespace RulesManagerExplorer
{
    public partial class FlexGridIntegrationDemo : UserControl
    {
        public FlexGridIntegrationDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.FlexGridIntegrationDescription;

            grid.ItemsSource = Customer.GetCustomerList(100);
            grid.Columns[4].DataMap = new GridDataMap() 
            { 
                ItemsSource = Customer.GetCountries(), 
                DisplayMemberPath = "Value", 
                SelectedValuePath = "Key" 
            };
            grid.MinColumnWidth = 85;
        }

        public RulesEngineStyle GetFirstNameStyle(RulesEngineSource source, int index, string field)
        {
            var firstNames = source.GetFieldValues<string>(field);
            double maxLength = firstNames.Max(firstName => firstName.Length);
            var firstName = source.GetValue<string>(index, field);
            return new RulesEngineStyle { DataBarValue = firstName.Length / maxLength };
        }
    }
}
