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
using C1.C1Preview;

namespace DemoTables
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            C1PrintDocument doc = null;
            switch( ((ComboBoxItem)cmbSel.SelectedItem)?.Tag)
            {
                case "btnBasicTable":
                    doc = MakeDocHelper.BasicTable();
                    break;
                case "btnStylesInTables":
                    doc = MakeDocHelper.StylesInTables();
                    break;
                case "btnWideTables":
                    doc = MakeDocHelper.WideTable();
                    break;
                case "btnTextStyles":
                    doc = MakeDocHelper.TextStyles();
                    break;
                case "btnLargeTable":
                    doc = MakeDocHelper.LargeTable(800, 4);
                    break;
                case "btnTableBorders":
                    doc = MakeDocHelper.TableBorders();
                    break;
            }

            c1DocumentViewer1.FitToWidth();
            c1DocumentViewer1.Document = doc.FixedDocumentSequence;
        }
    }
}
