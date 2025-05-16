using C1.DataCollection;
using PropertyGridExplorer.Resources;
using System.Windows.Controls;

namespace PropertyGridExplorer
{
    /// <summary>
    /// Interaction logic for AutoProperties.xaml
    /// </summary>
    public partial class AutoProperties : UserControl
    {
        public AutoProperties()
        {
            InitializeComponent();
            Tag = AppResources.PropertyGridAutoPropsDesc;
        }

        private void CheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var chkBox = sender as CheckBox;
            if (chkBox.IsChecked == true)
            {
                propertyGrid.SelectedObject = txtbox;
            }
            else
            {
                propertyGrid.SelectedObject = null;
            }    
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            propertyGrid.PropertyGroups.FilterAsync(filter.Text);
        }
    }
}
