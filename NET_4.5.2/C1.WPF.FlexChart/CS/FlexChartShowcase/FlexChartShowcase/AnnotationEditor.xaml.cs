using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace FlexChartShowcase
{
    /// <summary>
    /// Interaction logic for AnnotationEditor.xaml
    /// </summary>
    public partial class AnnotationEditor : TextEditor
    {
        public AnnotationEditor()
        {
            InitializeComponent();
        }

        public override void UpdateEditorContent()
        {
            txtAnnotationContent.Text = GetAnnotationContent();
        }

        private void btnOkCancel_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Tag.ToString())
            {
                case "Ok":
                    AcceptChanges(txtAnnotationContent.Text);
                    (this.Parent as Popup).IsOpen = false;
                    break;
                case "Cancel":
                    RejectChanges();
                    (this.Parent as Popup).IsOpen = false;
                    break;
            }

        }


    }
}
