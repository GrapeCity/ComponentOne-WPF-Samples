using C1.WPF.Chart.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlexChartEditableAnnotations
{
    /// <summary>
    /// Interaction logic for ContentEditor.xaml
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
