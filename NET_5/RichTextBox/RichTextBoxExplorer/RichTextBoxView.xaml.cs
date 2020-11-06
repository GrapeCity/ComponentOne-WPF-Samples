using System.Windows.Controls;

namespace RichTextBoxExplorer
{
    /// <summary>
    /// Interaction logic for RichTextBoxView.xaml
    /// </summary>
    public partial class RichTextBoxView : UserControl
    {
        public RichTextBoxView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
        }
    }
}
