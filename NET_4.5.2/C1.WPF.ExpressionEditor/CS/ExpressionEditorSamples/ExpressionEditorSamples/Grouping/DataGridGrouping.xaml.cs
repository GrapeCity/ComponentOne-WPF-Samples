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
    /// Interaction logic for DataGridGrouping.xaml
    /// </summary>
    public partial class DataGridGrouping : UserControl
    {
        public ICollectionView View;
        public DataGridGrouping()
        {
            InitializeComponent();

            CollectionViewSource view = new CollectionViewSource() { Source = Product.GetData(200) };
            View = view.View;

            grid.ItemsSource = View;

            editor.OkClick += Editor_OkClick;
            editor.CancelClick += Editor_CancelClick;
            editor.DataSource = View.CurrentItem;
            editor.Expression = Expressions.GetExpression();
            grid.Loaded += Grid_Loaded;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DataGridColumn column in grid.Columns)
            {
                if (column.Header.Equals("CustomField1") || column.Header.Equals("CustomField2"))
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
