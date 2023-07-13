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
            Tag = Properties.Resources.PropertyGridAutoPropsDesc;
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
    }
}
