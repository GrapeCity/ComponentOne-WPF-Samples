using System;
using System.IO;
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

namespace ReportViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter =
                "All report files (*.pdf;*.htm;*.html)|*.pdf;*.htm;*.html|" +
                "PDF files (*.pdf)|*.pdf|" +
                "Html files (*.htm;*.html)|*.htm;*.html";
            if (dlg.ShowDialog().Value)
            {
                using (var s = new FileStream(dlg.FileName, FileMode.Open))
                {
                    _c1RptViewer.LoadDocument(s);
                }
            }
        }
    }
}
