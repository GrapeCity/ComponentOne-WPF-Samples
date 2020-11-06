using C1.WPF.DataGrid;
using C1.WPF.ExpressionEditor;
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

namespace C1ExpressionEditorSample
{
    /// <summary>
    /// Interaction logic for C1DataGridGrouping.xaml
    /// </summary>
    public partial class C1DataGridGrouping : UserControl
    {
        public ICollectionView View;
        public C1DataGridGrouping()
        {
            InitializeComponent();

            CollectionViewSource view = new CollectionViewSource() { Source = Product.GetData(200) };
            View = view.View;

            dataGrid.ItemsSource = View;

            editor.OkClick += Editor_OkClick;
            editor.CancelClick += Editor_CancelClick;
            editor.DataSource = View.CurrentItem;
            editor.Expression = Expressions.GetExpression();
            dataGrid.Loaded += DataGrid_Loaded;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (C1.WPF.DataGrid.DataGridColumn column in dataGrid.Columns)
            {
                if (column.Name.Equals("CustomField1") || column.Name.Equals("CustomField2"))
                    column.Visibility = Visibility.Collapsed;
            }
        }

        private void Editor_CancelClick(object sender, EventArgs e)
        {
            View.GroupDescriptions.Clear();
        }

        private void Editor_OkClick(object sender, EventArgs e)
        {
            if (editor.IsValid)
            {
                C1DataGridExpressionGroupDescription expression = new C1DataGridExpressionGroupDescription();
                expression.Expression = editor.Expression;
                View.GroupDescriptions.Add(expression);
            }
        }
    }
}
