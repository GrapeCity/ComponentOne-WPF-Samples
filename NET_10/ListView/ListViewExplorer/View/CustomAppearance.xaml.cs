using ListViewExplorer.Resources;
using System.Windows.Controls;

namespace ListViewExplorer
{
    public partial class CustomAppearance : UserControl
    {
        public CustomAppearance()
        {
            InitializeComponent();
            Tag = AppResources.CustomAppearanceDescription;
            listView.ItemsSource = Person.Generate(100);
        }
    }
}
