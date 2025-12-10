using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpressionEditorExplorer
{
    /// <summary>
    /// Interaction logic for ExpressionEditorView.xaml
    /// </summary>
    public partial class ExpressionEditorView : UserControl
    {
        public ExpressionEditorView()
        {
            InitializeComponent();
            this.DataContext = new SampleDataSource();
            Loaded += ExpressionEditorView_Loaded;
        }

        private void ExpressionEditorView_Loaded(object sender, RoutedEventArgs e)
        {
            lbSamples.SelectedItem = lbSamples.Items[0];
        }
    }
}
