using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class Export : UserControl
    {
        public Export()
        {
            InitializeComponent();

            Tag = AppResources.ExportDescription;

            var data = Customer.GetCustomerList(100);
            var actualData = data.Select(data => new ExtendedClass()
            {
                Id = data.Id,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Address = data.Address,
                City = data.City,
                CountryId = data.CountryId,
                Email = data.Email,
                PostalCode = data.PostalCode,
                Active = data.Active,
                LastOrderDate = data.LastOrderDate,
                OrderTotal = data.OrderTotal,
                SampleHyperlink = "https://media-exp1.licdn.com/dms/image/C560BAQE2UbhekqtLAg/company-logo_200_200/0/1519856432286?e=2159024400&v=beta&t=HP2cnfQvlv4mYMh2ouJShTcs4bsKyMQErk2u_kLDjtM",
                SampleHyperlinkContent = $"Hyperlink - {data.Id}",
                SampledImage = "https://media-exp1.licdn.com/dms/image/C560BAQE2UbhekqtLAg/company-logo_200_200/0/1519856432286?e=2159024400&v=beta&t=HP2cnfQvlv4mYMh2ouJShTcs4bsKyMQErk2u_kLDjtM"
            }).ToList();
            grid.ItemsSource = actualData;
            grid.Columns["CountryID"].DataMap = new GridDataMap { ItemsSource = Customer.GetCountries(), SelectedValuePath = "Key", DisplayMemberPath = "Value" };
            grid.Columns["CountryID"].AllowMerging = true;
            grid.MinColumnWidth = 85;
        }

        private void OnExport(object sender, System.Windows.RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = "xlsx";
            dlg.Filter = "HTML File (*.htm;*.html)|*.htm;*.html|" + "Comma Separated Values (*.csv)|*.csv|" + "Text File (*.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                var ext = System.IO.Path.GetExtension(dlg.SafeFileName).ToLower();
                ext = ext == ".htm" ? "ehtm" : ext == ".html" ? "ehtm" : ext;

                var options = GridSaveOptions.None;
                if ((bool)checkFormatted.IsChecked)
                {
                    options |= GridSaveOptions.Formatted;
                }
                if ((bool)checkSaveHeaders.IsChecked)
                {
                    options |= GridSaveOptions.SaveHeaders;
                }
                if ((bool)checkVisibleColumns.IsChecked)
                {
                    options |= GridSaveOptions.VisibleColumns;
                }
                if ((bool)checkVisibleRows.IsChecked)
                {
                    options |= GridSaveOptions.VisibleRows;
                }
                try
                {
                    switch (ext)
                    {
                        case "ehtm":
                            {
                                grid.Save(dlg.FileName, GridFileFormat.Html, options);
                                break;
                            }
                        case ".csv":
                            {
                                grid.Save(dlg.FileName, GridFileFormat.Csv, options);
                                break;
                            }
                        case ".txt":
                            {
                                grid.Save(dlg.FileName, GridFileFormat.Text, options);
                                break;
                            }
                    }
                    Process.Start(new ProcessStartInfo { FileName = dlg.FileName, UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }

    public class ExtendedClass : Customer
    {
        public string SampleHyperlink { get; set; }
        public string SampleHyperlinkContent { get; set; }
        public string SampledImage { get; set; }
    }
}
