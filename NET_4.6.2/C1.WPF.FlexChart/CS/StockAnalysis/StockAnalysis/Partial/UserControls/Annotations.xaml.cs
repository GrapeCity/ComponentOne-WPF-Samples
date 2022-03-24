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
using StockAnalysis.Command;

namespace StockAnalysis.Partial.UserControls
{
    /// <summary>
    /// Interaction logic for Annotitions.xaml
    /// </summary>
    public partial class Annotations : UserControl
    {
        public Annotations()
        {
            InitializeComponent();
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var src = e.AddedItems[0] as Data.ListDataItem;
                if (src != null)
                {
                    ViewModel.ViewModel.Instance.NewAnnotationType = (Data.NewAnnotationType)src.Tag;
                }

                CloseDropdownCommand closeCMD = new CloseDropdownCommand();
                closeCMD.Execute(this);
            }
        }
    }
}
