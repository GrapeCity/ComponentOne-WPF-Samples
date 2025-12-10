using C1.CalcEngine;
using C1.WPF.ExpressionEditor;
using C1.WPF.Grid;
using ExpressionEditorExplorer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExpressionEditorExplorer
{
    /// <summary>
    /// Interaction logic for ExpressionEditorDataGridDemo.xaml
    /// </summary>
    public partial class ExpressionEditorDataGridDemo : UserControl
    {
        private List<Customer> _data = Customer.GenerateSampleData(50).ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionEditorDataGridDemo"/> class.
        /// </summary>
        public ExpressionEditorDataGridDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.DataGridDemoDescription;

            grid.ItemsSource = _data;
            expressionEditor.Engine.DataSource = _data; 
        }
    }
}
