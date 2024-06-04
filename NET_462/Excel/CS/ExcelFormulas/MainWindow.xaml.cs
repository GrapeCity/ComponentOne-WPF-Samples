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
using System.Globalization;
using System.Diagnostics;

using C1.WPF.Excel;
using Microsoft.Win32;

namespace ExcelFormulas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        C1XLBook _book;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void _btnSave_Click(object sender, RoutedEventArgs e)
        {
            Debug.Assert(_book != null);
            var dlg = new SaveFileDialog();
            dlg.Filter = "Excel Files (*.xlsx)|*.xlsx";
            if (dlg.ShowDialog().Value)
            {
                try
                {
                    // information
                    _lblStatus.Text = string.Format("Saving {0}...", dlg.SafeFileName);

                    // save workbook
                    using (var stream = dlg.OpenFile())
                    {
                        _book.Save(stream);
                    }
                    _lblStatus.Text = string.Format("Saved {0}", dlg.SafeFileName); ;
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        private void _btCreate_Click(object sender, RoutedEventArgs e)
        {
            // create new workbook
            if (_book == null)
                _book = new C1XLBook();

            // clear the book
            _book.Clear();

            // first sheet
            XLSheet sheet = _book.Sheets[0];

            // column width in twips
            sheet.Columns[0].Width = 2000;
            sheet.Columns[1].Width = 2200;

            // string formulas
            string s = "String:";
            sheet[0, 0].Value = s;
            sheet[1, 0].Value = s;
            sheet[2, 0].Value = s;

            sheet[0, 1].Value = "apples";
            sheet[1, 1].Value = "and";
            sheet[2, 1].Value = "oranges";

            s = "String formula:";
            sheet[4, 0].Value = s;
            sheet[5, 0].Value = s;

            sheet[4, 1].Value = "apples and oranges";
            sheet[5, 1].Value = "apples an";
            sheet[4, 1].Formula = "CONCATENATE(B1,\" \",B2, \" \",B3)";
            sheet[5, 1].Formula = "LEFT(B5,9)";

            // simple formulas
            sheet[7, 0].Value = "Formula: 5!";
            sheet[7, 1].Value = 120;
            sheet[7, 1].Formula = "1*2*3*4*5";

            sheet[8, 0].Value = "Formula: 12/0";
            sheet[8, 1].Value = 0;
            sheet[8, 1].Formula = "12/0";

            sheet[9, 0].Value = "Formula: 1 = 1";
            sheet[9, 1].Value = true;
            sheet[9, 1].Formula = "1=1";

            sheet[10, 0].Value = "Formula: 1 = 2";
            sheet[10, 1].Value = false;
            sheet[10, 1].Formula = "1 = 2";

            // now function
            sheet[12, 0].Value = "Formula: Now()";
            sheet[12, 1].Value = DateTime.Now;
            sheet[12, 1].Formula = "Now()";

            XLStyle style = new XLStyle(_book);
            DateTimeFormatInfo dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
            style.Format = XLStyle.FormatDotNetToXL(dtfi.ShortDatePattern + " " + dtfi.ShortTimePattern);
            sheet[12, 1].Style = style;

            // allow save the file
            _lblStatus.Text = "You can save workbook";
            _btnSave.IsEnabled = true;
        }
    }
}
