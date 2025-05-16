using PropertyGridExplorer.Resources;
using System.Windows.Controls;

namespace PropertyGridExplorer
{
    public partial class AttachedProperties : UserControl
    {
        public AttachedProperties()
        {
            InitializeComponent();
            Tag = AppResources.AttachedPropertiesDesc;
        }

        private void OnAutoGeneratingPropertyAttribute(object sender, C1.WPF.PropertyGrid.PropertyGridAttributeAutoGeneratingEventArgs e)
        {
            if (e.PropertyAttribute.MemberName != "Canvas.Left" &&
                e.PropertyAttribute.MemberName != "Canvas.Top")
                e.Cancel = true;
        }
    }
}
