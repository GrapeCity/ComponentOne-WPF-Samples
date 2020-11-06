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
using System.Reflection;
using System.Diagnostics;

using C1.WPF.Excel;
using System.Globalization;
using Microsoft.Win32;

namespace AutoSizeColumns
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

            // add some styles
            XLStyle s1 = new XLStyle(_book);
            XLStyle s2 = new XLStyle(_book);
            XLStyle s3 = new XLStyle(_book);
            s1.Format = "#,##0.00000";
            s2.Format = "#,##0.00000";
            s2.Font = new XLFont("Courier New", 14);
            s3.Format = "dd-MMM-yy";

            // populate sheet with some random values
            XLSheet sheet = _book.Sheets[0];
            Random r = new Random();
            for (int i = 0; i < 100; i++)
            {
                sheet[i, 0].Value = r.NextDouble() * 100000;
                sheet[i, 0].Style = (i % 13 == 0) ? s2 : s1;
            }
            for (int i = 0; i < 100; i++)
            {
                sheet[i, 1].Value = new DateTime(2005, r.Next(1, 12), r.Next(1, 28));
                sheet[i, 1].Style = s3;
            }

            // automatic sizing
            AutoSizeColumns(sheet);

            // allow save the file
            _lblStatus.Text = "You can save workbook";
            _btnSave.IsEnabled = true;
        }

        private void AutoSizeColumns(XLSheet sheet)
        {
            for (int c = 0; c < sheet.Columns.Count; c++)
            {
                int colWidth = -1;
                for (int r = 0; r < sheet.Rows.Count; r++)
                {
                    object value = sheet[r, c].Value;
                    if (value != null)
                    {
                        // get value (unformatted at this point)
                        string text = value.ToString();

                        // format value if cell has a style with format set
                        var s = sheet[r, c].Style;
                        if (s != null && s.Format.Length > 0 && value is IFormattable)
                        {
                            string fmt = XLStyle.FormatXLToDotNet(s.Format);
                            text = ((IFormattable)value).ToString(fmt, CultureInfo.CurrentCulture);
                        }

                        // get font (default or style)
                        var font = this._book.DefaultFont;
                        if (s != null && s.Font != null)
                        {
                            font = s.Font;
                        }

                        // measure string (add a little tolerance)
                        _tblMeasure.FontFamily = new FontFamily(font.FontName);
                        _tblMeasure.FontSize = 3 * font.FontSize / 2;
                        _tblMeasure.FontWeight = font.Bold ? FontWeights.Bold : FontWeights.Normal;
                        _tblMeasure.FontStyle = font.Italic ? FontStyles.Italic : FontStyles.Normal;
                        _tblMeasure.Text = text;

                        // keep widest so far
                        int w = (int)(_tblMeasure.ActualWidth);
                        if (w > colWidth)
                            colWidth = w;
                    }
                }

                // done measuring, set column width
                if (colWidth > -1)
                    sheet.Columns[c].Width = C1XLBook.PixelsToTwips(colWidth);
            }
        }
    }
}
