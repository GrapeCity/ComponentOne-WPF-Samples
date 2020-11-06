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
using System.IO;
using Microsoft.Win32;

namespace CombineSheets
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
            _book.Sheets.Clear();

            // load from resources
            Assembly a = Assembly.GetExecutingAssembly();
            foreach (string res in a.GetManifestResourceNames())
            {
                if (!res.ToLower().EndsWith(".xlsx")) continue;

                using (var stream = a.GetManifestResourceStream(res))
                {
                    // load Excel file
                    var book = new C1XLBook();
                    book.Load(stream, FileFormat.OpenXml);

                    // clone and rename first sheet (sheet names must be unique)
                    var ss = res.Split('.');
                    Debug.Assert(ss.Length >= 3);
                    XLSheet clone = book.Sheets[0].Clone();
                    clone.Name = ss[ss.Length - 2];

                    // add cloned sheet to main book
                    _book.Sheets.Add(clone);
                }
            }

            // allow save the file
            _lblStatus.Text = "You can save workbook";
            _btnSave.IsEnabled = true;
        }

        public static Stream GetManifestResource(string resource)
        {
            resource = resource.ToLower();
            Assembly a = Assembly.GetExecutingAssembly();
            foreach (string res in a.GetManifestResourceNames())
            {
                if (res.ToLower().EndsWith(resource))
                    return a.GetManifestResourceStream(res);
            }
            return null;
        }
    }
}
