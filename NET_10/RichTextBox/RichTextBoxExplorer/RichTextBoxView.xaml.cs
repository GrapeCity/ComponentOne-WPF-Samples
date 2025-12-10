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
            Loaded += RichTextBoxView_Loaded;
        }

        private void RichTextBoxView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
