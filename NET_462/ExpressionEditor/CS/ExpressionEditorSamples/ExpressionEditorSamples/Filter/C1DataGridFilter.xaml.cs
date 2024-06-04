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
    /// Interaction logic for C1DataGridFilter.xaml
    /// </summary>
    public partial class C1DataGridFilter : UserControl
    {
        C1ExpressionEditor _editor = new C1ExpressionEditor();
        public ICollectionView View;

        public C1DataGridFilter()
        {
            InitializeComponent();
            CollectionViewSource view = new CollectionViewSource() { Source = Product.GetData(200) };
            View = view.View;
            dataGrid.ItemsSource = View;

            editor.DataSource = View.CurrentItem;
            editor.Expression = "[Price] > 1000";
            editor.ExpressionChanged += ExpressionEditor_ExpressionChanged; 

            View.Filter = new Predicate<object>(Contains);
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

        private void ExpressionEditor_ExpressionChanged(object sender, EventArgs e)
        {
            _editor.Expression = editor.Expression;
            _editor.DataSource = editor.DataSource;
            if (_editor.IsValid)
            {
                View.Filter = new Predicate<object>(Contains);
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
