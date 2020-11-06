using System.Windows;
using System.Windows.Controls;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for OrgChartSample.xaml
    /// </summary>
    public partial class OrgChartSample : UserControl
    {
        public OrgChartSample()
        {
            InitializeComponent();
            CreateData();
        }

        void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            _orgChart.Orientation = ((CheckBox)sender).IsChecked.Value
                ? Orientation.Horizontal
                : Orientation.Vertical;
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            CreateData();
        }

        void CreateData()
        {
            var p = Data.Person.CreatePerson(10);
            _tbTotal.Text = string.Format(" ({0} items total)", p.TotalCount);
            _orgChart.Header = p;
        }
    }

    /// <summary>
    /// Class used to select the templates for items being created.
    /// </summary>
    public class PersonTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var p = item as Data.Person;
            var e = container as FrameworkElement;
            return p.Position.IndexOf("Director") > -1
                ? e.FindResource("_tplDirector") as DataTemplate
                : e.FindResource("_tplOther") as DataTemplate;
        }
    }
}
