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

namespace DataGridSamples
{
    /// <summary>
    /// Interaction logic for SelectionPage.xaml
    /// </summary>
    public partial class Selection : UserControl
    {
        public Selection()
        {
            InitializeComponent();

            DataContext = Person.Generate(300);
        }

        public C1.WPF.DataGrid.DataGridSelectionMode SelectionMode
        {
            get
            {
                return grid.SelectionMode;
            }
            set
            {
                grid.SelectionMode = value;
            }
        }

        public C1.WPF.DataGrid.DataGridColumnHeaderClickAction ColumnHeaderClickAction
        {
            get
            {
                return grid.ColumnHeaderClickAction;
            }
            set
            {
                grid.ColumnHeaderClickAction = value;
            }
        }
    }
}
