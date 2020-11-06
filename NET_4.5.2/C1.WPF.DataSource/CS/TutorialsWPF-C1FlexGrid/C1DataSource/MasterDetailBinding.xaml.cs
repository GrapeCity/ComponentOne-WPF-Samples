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

namespace TutorialsWPF
{
    public partial class MasterDetailBinding: Window
    {
        public MasterDetailBinding()
        {
            InitializeComponent();
            dataGrid1.SelectedIndex = 0;
        }

        private void DataGrid1_SelectedItemChanged(object sender, EventArgs e)
        {
            // Update the filter value. It will trigger automatic loading.
            c1DataSource1.ViewSources["Products"].FilterDescriptors[0].Value = ((Category)dataGrid1.SelectedItem).CategoryID;
        }
    }
}
