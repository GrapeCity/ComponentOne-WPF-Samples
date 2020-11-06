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
using C1.WPF.Pdf;

namespace PdfAcroform
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1PdfDocument _pdf;

        public MainWindow()
        {
            InitializeComponent();

            _pdf = new C1PdfDocument(PaperKind.Letter);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            PdfUtil.CreateDocument(_pdf);
            c1PdfViewer1.LoadDocument(PdfUtil.SaveToStream(_pdf));
        }
    }
}
