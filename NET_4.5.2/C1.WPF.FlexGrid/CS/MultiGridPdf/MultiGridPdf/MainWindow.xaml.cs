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
using C1.WPF.FlexGrid;
using System.Windows.Forms;

namespace MultiGridPdf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // load data into first grid
            _flex1.ItemsSource = Product.GetProducts(10);
            _flex1.CollectionView.Filter = FilterComputers;

            // load data into second grid
            _flex2.ItemsSource = Product.GetProducts(20);
            _flex2.CollectionView.Filter = FilterStoves;

            // load data into third grid
            _flex3.ItemsSource = Product.GetProducts(10);
            _flex3.CollectionView.Filter = FilterWashers;

            // load data into last grid
            _flex4.ItemsSource = Product.GetProducts(20);
            _flex4.CollectionView.Filter = FilterComputers;
        }

        bool FilterComputers(object item)
        {
            var p = item as Product;
            return p.Line == "Computers";
        }
        bool FilterStoves(object item)
        {
            var p = item as Product;
            return p.Line == "Stoves";
        }
        bool FilterWashers(object item)
        {
            var p = item as Product;
            return p.Line == "Washers";
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.Filter = "PDF files (*.pdf)|*.pdf";
            //var t = dlg.ShowDialog();
            if (dlg.ShowDialog().Value)
            {
                // create pdf document
                var pdf = new C1PdfDocument();
                pdf.Landscape = true;
                pdf.Compression = CompressionLevel.NoCompression;

                // render all grids into pdf document
                var options = new PdfExportOptions();
                options.ScaleMode = ScaleMode.ActualSize;
                GridExport.RenderGrid(pdf, _flex1, options);
                pdf.NewPage();
                GridExport.RenderGrid(pdf, _flex2, options);
                pdf.NewPage();
                GridExport.RenderGrid(pdf, _flex3, options);
                pdf.NewPage();
                GridExport.RenderGrid(pdf, _flex4, options);

                // save document
                using (var stream = dlg.OpenFile())
                {
                    pdf.Save(stream);
                }
            }
        }
    }
}
