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
using System.Windows.Shapes;

namespace PdfView
{
    /// <summary>
    /// Interaction logic for PasswordWindow.xaml
    /// </summary>
    public partial class PasswordWindow : Window
    {
        public PasswordWindow()
        {
            InitializeComponent();
        }

        public string EnterPassword(string fileName)
        {
            lblFileName.Text = String.Format("'{0}' is protected.\r\nPlease enter a Document Open Password.", System.IO.Path.GetFileName(fileName));
            tbPassword.Focus();
            bool? dr = ShowDialog();
            if (dr.HasValue && dr.Value)
                return tbPassword.Password;
            return null;
        }

        public static string DoEnterPassword(string fileName)
        {
            PasswordWindow f = new PasswordWindow();
            return f.EnterPassword(fileName);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
