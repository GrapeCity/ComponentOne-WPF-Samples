using System.Windows.Controls;

namespace ThemeExplorer
{
    /// <summary>
    /// Interaction logic for InputAndEditing.xaml
    /// </summary>
    public partial class InputAndEditing : UserControl
    {
        public InputAndEditing()
        {
            InitializeComponent();
            var persons = Person.Generate(80); 

            TagEditor.InsertTag("John");
            TagEditor.InsertTag("Jemmy Harden");
            TagEditor1.InsertTag("John");
            TagEditor1.InsertTag("Jemmy Harden");

            MultiSelect.ItemsSource = persons;
            MultiSelect.SelectedItem = persons[0];
            CheckList.ItemsSource = persons;
            CheckList.Items[0].IsSelected = true;
            MultiSelect1.ItemsSource = persons;
            MultiSelect1.SelectedItem = persons[0];
            CheckList1.ItemsSource = persons;
            CheckList1.Items[0].IsSelected = true;

        }

    }
}
