using System.Windows.Controls;

namespace InputExplorer
{
    /// <summary>
    /// Interaction logic for C1ComboBox.xaml
    /// </summary>
    public partial class C1ComboBox : UserControl
    {
        public C1ComboBox()
        {
            InitializeComponent();
            Tag = Properties.Resources.C1ComboBoxDes;

            ComboBox1.ItemsSource = Employee.GenerateData(7);
            ComboBox2.ItemsSource = Employee.GenerateData(7);
        }
    }
}
