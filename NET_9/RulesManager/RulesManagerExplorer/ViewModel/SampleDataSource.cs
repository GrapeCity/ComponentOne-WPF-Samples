using System.Collections.ObjectModel;

namespace RulesManagerExplorer
{
    public class SampleDataSource
    {
        public SampleDataSource()
        {
            AllItems = new ObservableCollection<SampleItem>
            {
                new SampleItem(
                    Properties.Resources.GettingStartedTitle,
                    Properties.Resources.GettingStartedTitle,
                    new System.Lazy<System.Windows.Controls.UserControl>(() => new GettingStartedDemo())),
                new SampleItem(
                    Properties.Resources.FlexGridIntegrationTitle,
                    Properties.Resources.FlexGridIntegrationTitle,
                    new System.Lazy<System.Windows.Controls.UserControl>(() => new FlexGridIntegrationDemo())),
                new SampleItem(
                    Properties.Resources.DataGridIntegrationTitle,
                    Properties.Resources.DataGridIntegrationTitle,
                    new System.Lazy<System.Windows.Controls.UserControl>(() => new DataGridIntegrationDemo())),
                new SampleItem(
                    Properties.Resources.ListViewIntegrationTitle,
                    Properties.Resources.ListViewIntegrationTitle,
                    new System.Lazy<System.Windows.Controls.UserControl>(() => new ListViewIntegrationDemo())),
            };
        }

        public ObservableCollection<SampleItem> AllItems
        {
            get;
        }
    }
}
