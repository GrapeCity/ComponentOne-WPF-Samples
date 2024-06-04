using C1.WPF.ExpressionEditor;
using C1.WPF.FlexGrid;
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

namespace C1ExpressionEditorSample
{
    /// <summary>
    /// Interaction logic for C1FlexGridFilter.xaml
    /// </summary>
    public partial class C1FlexGridFilter : UserControl
    {
        C1ExpressionEditor _editor = new C1ExpressionEditor();

        public C1FlexGridFilter()
        {
            InitializeComponent();
            flexGrid.ItemsSource = Product.GetData(200);

            editor.DataSource = flexGrid.CollectionView.CurrentItem;
            editor.Expression = "[Price] > 1000";
            editor.ExpressionChanged += ExpressionEditor_ExpressionChanged;

            flexGrid.CollectionView.Filter = new Predicate<object>(Contains);
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

        private void ExpressionEditor_ExpressionChanged(object sender, EventArgs e)
        {
            _editor.Expression = editor.Expression;
            _editor.DataSource = editor.DataSource;
            if (_editor.IsValid)
            {
                flexGrid.CollectionView.Filter = new Predicate<object>(Contains);
            }
        }

        private bool Contains(object obj)
        {
            _editor.Expression = editor.Expression;
            _editor.DataSource = obj as Product;
            var value = _editor.Evaluate();
            var ret = true;
            if (value != null)
                Boolean.TryParse(value.ToString(), out ret);
            return ret;
        }
    }
}
