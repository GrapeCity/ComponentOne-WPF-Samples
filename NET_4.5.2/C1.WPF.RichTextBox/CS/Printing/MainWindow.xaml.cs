using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.RichTextBox;
using System.IO;

namespace Printing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            using (var stream = Application.GetResourceStream(new Uri("/dickens.htm", UriKind.Relative)).Stream)
            {
                rtb.Html = new StreamReader(stream).ReadToEnd();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new PrintDialog();
            if (dlg.ShowDialog() == true)
            {
                var paginator = new C1RichTextBoxPaginator(
                                    rtb, 
                                    new Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight),
                                    new Thickness(40, 60, 40, 60) /* hardcoded margin */
                                );
                dlg.PrintDocument(paginator, "Test");
            }
        }
    }
}
