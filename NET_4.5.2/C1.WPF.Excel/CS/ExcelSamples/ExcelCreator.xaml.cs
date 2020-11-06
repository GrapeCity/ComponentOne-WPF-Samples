using C1.WPF.Excel;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExcelSamples
{
    /// <summary>
    /// Interaction logic for ExcelCreator.xaml
    /// </summary>
    public partial class ExcelCreator : UserControl
    {
        public ExcelCreator()
        {
            InitializeComponent();
        }

        private void btnHelloWorld_Click(object sender, RoutedEventArgs e)
        {
            SaveBook(book =>
            {
                book.Sheets[0][0, 0].Value = "Hello World";
            });
        }

        private void btnStyles_Click(object sender, RoutedEventArgs e)
        {
            SaveBook(book =>
            {
                // get the sheet that was created by default, give it a name
                var sheet = book.Sheets[0];

                // create styles for odd and even values
                var styleOdd = new XLStyle(book);
                styleOdd.Font = new XLFont("Tahoma", 9, false, true);
                styleOdd.ForeColor = Colors.Blue;

                var styleEven = new XLStyle(book);
                styleEven.Font = new XLFont("Tahoma", 9, true, false);
                styleEven.ForeColor = Colors.Red;
                styleEven.BackColor = Colors.Yellow;

                // step 3: write content and format into some cells
                for (int i = 0; i < 30; i++)
                {
                    XLCell cell = sheet[i, 0];
                    cell.Value = i + 1;
                    cell.Style = ((i + 1) % 2 == 0) ? styleEven : styleOdd;
                }
            });
        }

        private void btnFormulas_Click(object sender, RoutedEventArgs e)
        {
            SaveBook(book =>
            {
                // first sheet
                var sheet = book.Sheets[0];

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

                var style = new XLStyle(book);
                var dtfi = CultureInfo.CurrentCulture.DateTimeFormat;
                style.Format = XLStyle.FormatDotNetToXL(dtfi.ShortDatePattern + " " + dtfi.ShortTimePattern);
                sheet[12, 1].Style = style;
            });
        }

        private void SaveBook(Action<C1XLBook> action)
        {
            if (!IsPermissionGranted(new FileIOPermission(PermissionState.Unrestricted)))
            {
                string msg = "There is no permission to save file.";
                if (Application.Current == null)
                {
                    MessageBox.Show(msg, "C1.WPF.Excel", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    MessageBox.Show(msg, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                InternalSave(action);
            }
        }

        private static void InternalSave(Action<C1XLBook> action)
        {

            var dlg = new SaveFileDialog();
            dlg.Filter = "Excel Files (*.xlsx)|*.xlsx";
            if (dlg.ShowDialog() == true)
            {
                try
                {
                    var book = new C1XLBook();
                    if (action != null)
                    {
                        action(book);
                    }
                    using (var stream = dlg.OpenFile())
                    {
                        book.Save(stream);
                    }
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        internal static bool IsPermissionGranted(CodeAccessPermission requestedPermission)
        {
            try
            {
                // Try and get this permission
                requestedPermission.Demand();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
