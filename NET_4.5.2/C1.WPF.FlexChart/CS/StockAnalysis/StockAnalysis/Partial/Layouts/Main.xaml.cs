using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace StockAnalysis.Partial.Layouts
{
    /// <summary>
    /// Interaction logic for ucMain.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        public Main()
        {
            InitializeComponent();
            
            this.Loaded += (o, e) =>
             {
                 this.navList.SelectedIndex = 0;
             };
        }
        
        public ViewModel.ViewModel Model
        {
            get
            {
                return ViewModel.ViewModel.Instance;
            }
        }
    }
}
