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
using System.Windows.Forms;
using C1.WPF.Excel;
using System.IO;
using C1.WPF.FlexGrid;

namespace ExportExcel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IEnumerable<Product> products = Product.GetProducts(250);
            products.ElementAt(0).Price = -10;
            products.ElementAt(1).Price = -10;

            C1FlexGrid1.ItemsSource = products;
          
           
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = "xlsx";
            dlg.Filter = "Excel Workbook (*.xlsx)|*.xlsx|" + "HTML File (*.htm;*.html)|*.htm;*.html|" + "Comma Separated Values (*.csv)|*.csv|" + "Text File (*.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                var ext = System.IO.Path.GetExtension(dlg.SafeFileName).ToLower();
                ext = ext == ".htm" ? "ehtm" : ext == ".html" ? "ehtm" : ext;
                switch (ext)
                {
                    case "ehtm":
                        {
                            C1FlexGrid1.Save(dlg.FileName, C1.WPF.FlexGrid.FileFormat.Html, SaveOptions.Formatted);
                            break;
                        }
                    case ".csv":
                        {
                            C1FlexGrid1.Save(dlg.FileName, C1.WPF.FlexGrid.FileFormat.Csv, SaveOptions.Formatted);
                            break;
                        }
                    case ".txt":
                        {
                            C1FlexGrid1.Save(dlg.FileName, C1.WPF.FlexGrid.FileFormat.Text, SaveOptions.Formatted);
                            break;
                        }
                    default:
                        {
                            Save(dlg.FileName,C1FlexGrid1);
                            break;
                        }
                }
            }
        }

        public void Save(string filename, C1FlexGrid flexgrid)
        {
            // create the book to save
            var book = new C1XLBook();
            book.Sheets.Clear();
            var  xlSheet = book.Sheets.Add("Sheet1");
            ExcelExport.ExcelFilter.Save(flexgrid, xlSheet);


            // save the book
            book.Save(filename, C1.WPF.Excel.FileFormat.OpenXml);
        }
    }
}
