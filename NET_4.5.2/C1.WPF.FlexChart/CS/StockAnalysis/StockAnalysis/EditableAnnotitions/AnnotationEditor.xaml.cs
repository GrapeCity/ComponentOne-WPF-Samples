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

namespace StockAnalysis.EditableAnnotitions
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
    
        public Data.AnnotationStyle EditingAnnotationStyle
        {
            get { return ViewModel.ViewModel.Instance.EditingAnnotationStyle; }
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var popup = this.Parent as Popup;
            if (popup != null)
            {
                popup.IsOpen = false;

                var wnd = Application.Current.MainWindow;
                if (wnd != null)
                {
                    wnd.Deactivated += (o, e) =>
                    {
                        popup.IsOpen = false;
                    };
                }

                Partial.CustomControls.DropdownControl.OnDropdownOpened += (o, e) =>
                {
                    popup.IsOpen = false;
                };

                System.Threading.ThreadPool.QueueUserWorkItem((obj) =>
                {
                    System.Threading.Thread.Sleep(100);
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        popup.IsOpen = true;
                    }));
                });
            }
            
        }
        

        public override void UpdateEditorContent()
        {
            txtAnnotationContent.Text = GetAnnotationContent();
            if (this.Annotation is Text)
            {
                annotationShapeSettings.Visibility = Visibility.Collapsed;
            }
            else
            {
                annotationShapeSettings.Visibility = Visibility.Visible;
            }
            if (this.Annotation is C1.WPF.Chart.Annotation.Line)
            {
                annotationShapeSettings.FillGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                annotationShapeSettings.FillGrid.Visibility = Visibility.Visible;
            }
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
                    //RejectChanges();
                    (this.Parent as Popup).IsOpen = false;
                    break;
            }

        }


    }
}
