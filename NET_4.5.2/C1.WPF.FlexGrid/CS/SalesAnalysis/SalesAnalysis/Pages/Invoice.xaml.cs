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
using System.ComponentModel;
using C1.WPF.FlexGrid;
namespace SalesAnalysis
{
    /// <summary>
    /// Interaction logic for Invoice.xaml
    /// </summary>
    public partial class Invoice : UserControl
    {
        private DataServiceRef.NORTHWNDEntities dataServiceClient;
        private System.Data.Services.Client.DataServiceQuery<DataServiceRef.Invoice> nwndQuery;
        
        public Invoice()
        {
            InitializeComponent();
         
           
            dataServiceClient = new DataServiceRef.NORTHWNDEntities(new Uri("http://localhost:50297/NorthWindDataService.svc/"));
            nwndQuery = dataServiceClient.Invoices;
            _flex.ItemsSource = nwndQuery;
           
          
            
            
        }
        
        private void _imgFilter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetFlexData();
        }

        private void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".csv";
            dlg.Filter =
                "Comma Separated Values (*.csv)|*.csv|" +
                "Plain text (*.txt)|*.txt|" +
                "HTML (*.html)|*.html";
            if (dlg.ShowDialog() == true)
            {
                // select format based on file extension
                var fileFormat = FileFormat.Csv;
                switch (System.IO.Path.GetExtension(dlg.SafeFileName).ToLower())
                {
                    case ".htm":
                    case ".html":
                        fileFormat = FileFormat.Html;
                        break;
                    case ".txt":
                        fileFormat = FileFormat.Text;
                        break;
                }

                // save the file
                using (var stream = dlg.OpenFile())
                {
                    _flex.Save(stream, fileFormat);
                }
            }
          
            }
        

        private void _btnPrint_Click(object sender, RoutedEventArgs e)
        {
            _flex.Print("NorthWind Invoices", ScaleMode.PageWidth, new Thickness(96), 20);
        }

        private void _txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetFlexData();
        }

        void SetFlexData()
        {
            if (!string.IsNullOrEmpty(_txtFilter.Text))
            {
                var fltrddata = nwndQuery.Where(o => o.ProductName.StartsWith(_txtFilter.Text.Trim()));
                _flex.ItemsSource = fltrddata;
            }
            else
            {
                _flex.ItemsSource = nwndQuery;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _txtFilter.Text = "";
            SetFlexData();
        }
    }
}
