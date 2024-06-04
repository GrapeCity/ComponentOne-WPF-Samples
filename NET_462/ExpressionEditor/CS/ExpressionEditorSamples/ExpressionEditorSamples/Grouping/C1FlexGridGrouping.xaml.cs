using C1.WPF.FlexGrid;
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
    /// Interaction logic for C1FlexGridGrouping.xaml
    /// </summary>
    public partial class C1FlexGridGrouping : UserControl
    {
        public C1FlexGridGrouping()
        {
            InitializeComponent();

            flexGrid.ItemsSource = Product.GetData(200);

            editor.OkClick += Editor_OkClick;
            editor.CancelClick += Editor_CancelClick;
            editor.DataSource = flexGrid.CollectionView.CurrentItem;
            editor.Expression = Expressions.GetExpression();
            flexGrid.Loaded += FlexGrid_Loaded;
        }

        private void FlexGrid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (Column column in flexGrid.Columns)
            {
                if (column.ColumnName.Equals("CustomField1") || column.ColumnName.Equals("CustomField2"))
                    column.Visible = false;
            }
        }

        private void Editor_CancelClick(object sender, EventArgs e)
        {
            flexGrid.CollectionView.GroupDescriptions.Clear();
        }

        private void Editor_OkClick(object sender, EventArgs e)
        {
            if (editor.IsValid)
            {
                ExpressionGroupDescription expression = new ExpressionGroupDescription();
                expression.Expression = editor.Expression;
                flexGrid.CollectionView.GroupDescriptions.Add(expression);
            }
        }
    }
}
