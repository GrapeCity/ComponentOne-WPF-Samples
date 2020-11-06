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

namespace HitTestTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _flex.ItemsSource = Customer.GetCustomerList(100);
        }

        private void _flex_MouseMove(object sender, MouseEventArgs e)
        {
            var ht = _flex.HitTest(e);
            _tb.Text = string.Format("HitTest Result:: cell type: {0}, row: {1}, column: {2}, cell content: '{3}'",
                ht.CellType,
                ht.Row,
                ht.Column,
                ht.CellRange.IsValid ? _flex[ht.Row, ht.Column] : "n/a");
        }
    }
}
