using C1.WPF.DataGrid;
using C1.WPF.ExpressionEditor;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;

namespace C1ExpressionEditorSample
{
    /// <summary>
    /// Interaction logic for C1DataGridIntegration.xaml
    /// </summary>
    public partial class C1DataGridIntegration : UserControl
    {
        public C1DataGridIntegration()
        {
            InitializeComponent();

            List<Product> items = Product.GetData(200);
            CollectionViewSource view = new CollectionViewSource() { Source = items };
            dataGrid.ItemsSource = view.View;
            C1ExpressionEditor c1ExpressionEditor1 = new C1ExpressionEditor();
            C1ExpressionEditor c1ExpressionEditor2 = new C1ExpressionEditor();
            c1ExpressionEditor1.Expression = "[Price]*0.95";
            c1ExpressionEditor2.Expression = "[Price]*0.8";
            dataGrid.ExpressionEditors.Add("CustomField1", c1ExpressionEditor1);
            dataGrid.ExpressionEditors.Add("CustomField2", c1ExpressionEditor2);
            comboBox.SelectedIndex = 0;
            editor.OkClick += Editor_OkClick;
            editor.CancelClick += Editor_CancelClick;
        }

        private void Editor_CancelClick(object sender, EventArgs e)
        {
            var field = ((ComboBoxItem)comboBox.SelectedItem).Tag as string;
            if (dataGrid != null && dataGrid.ExpressionEditors.Contains(field))
            {
                editor.Expression = dataGrid.ExpressionEditors[field].Expression;
            }
        }

        private void Editor_OkClick(object sender, EventArgs e)
        {
            var field = ((ComboBoxItem)comboBox.SelectedItem).Tag as string;
            if (dataGrid != null && dataGrid.ExpressionEditors.Contains(field))
            {
                dataGrid.ExpressionEditors[field].Expression = editor.Expression;
            }
        }

        // Change ExpressionEditor settings to allow edit expression for selected C1DataGrid column.
        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var field = (e.AddedItems[0] as ComboBoxItem).Tag as string;
            if (dataGrid != null && dataGrid.ExpressionEditors.Contains(field))
            {
                editor.DataSource = dataGrid.ExpressionEditors[field].DataSource;
                editor.Expression = dataGrid.ExpressionEditors[field].Expression;
            }
        }
    }

    public class C1DataGridEE : C1DataGrid, ISupportExpressions
    {
        #region Ctor
        public C1DataGridEE() : base()
        {
            ExpressionEditors = new ExpressionEditorCollection(this);

            this.CommittedEdit += C1DataGridEE_CommittedEdit;
            this.RowsAdded += C1DataGridEE_RowsAdded;
        }

        private void C1DataGridEE_RowsAdded(object sender, DataGridRowsAddedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                var row = e.AddedRows[0];
                // evaluate the expression
                var idx = ((ListCollectionView)ItemsSource).IndexOf(row.DataItem);
                if (idx >= 0 && Rows.Count > idx)
                {
                    ExpressionEditors.Evaluate(row.Index);
                }
            }));
        }

        private void C1DataGridEE_CommittedEdit(object sender, DataGridCellEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                var row = e.Cell.Row;
                // this is not a new row. New row is not present in ItemsSource with which ExpressionEditor works
                // ignore expression column changing
                if (!ExpressionEditors.Contains(e.Cell.Column.Name))
                {
                    // evaluate the expression
                    var idx = ((ListCollectionView)ItemsSource).IndexOf(row.DataItem);
                    if (idx >= 0 && Rows.Count > idx)
                    { 
                        ExpressionEditors.Evaluate(idx);
                    }
                }
            }));
        }

        #endregion

        #region ISupportExpressions

        public void SetCellValue(int row, string colName, object value)
        {
            var col = Columns[colName].Index;
            this[row, col].Value = value;     
        }

        public ExpressionEditorCollection ExpressionEditors { get; }
        #endregion

    }
}
