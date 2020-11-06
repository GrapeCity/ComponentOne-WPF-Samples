using C1.WPF.ExpressionEditor;
using C1.WPF.FlexGrid;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace C1ExpressionEditorSample
{
    /// <summary>
    /// Interaction logic for C1FlexGridIntegration.xaml
    /// </summary>
    public partial class C1FlexGridIntegration : UserControl
    {
        public C1FlexGridIntegration()
        {
            InitializeComponent();

            List<Product> items = Product.GetData(200);
            flexGrid.ItemsSource = items;

            C1ExpressionEditor c1ExpressionEditor1 = new C1ExpressionEditor();
            C1ExpressionEditor c1ExpressionEditor2 = new C1ExpressionEditor();
            c1ExpressionEditor1.Expression = "[Price]*0.95";
            c1ExpressionEditor2.Expression = "[Price]*0.8";
            flexGrid.ExpressionEditors.Add("CustomField1", c1ExpressionEditor1);
            flexGrid.ExpressionEditors.Add("CustomField2", c1ExpressionEditor2);
            comboBox.SelectedIndex = 0;
            editor.OkClick += Editor_OkClick;
            editor.CancelClick += Editor_CancelClick;
            flexGrid.Loaded += FlexGrid_Loaded;
        }

        private void FlexGrid_Loaded(object sender, RoutedEventArgs e)
        {
            flexGrid.AutoSizeColumns(0, 1, 2); // to reflect long column headers
        }

        private void Editor_CancelClick(object sender, EventArgs e)
        {
            var field = ((ComboBoxItem)comboBox.SelectedItem).Tag as string;
            if (flexGrid != null && flexGrid.ExpressionEditors.Contains(field))
            {
                editor.Expression = flexGrid.ExpressionEditors[field].Expression;
            }
        }

        private void Editor_OkClick(object sender, EventArgs e)
        {
            var field = ((ComboBoxItem)comboBox.SelectedItem).Tag as string;
            if (flexGrid != null && flexGrid.ExpressionEditors.Contains(field))
            {
                flexGrid.ExpressionEditors[field].Expression = editor.Expression;
            }
        }

        // Change ExpressionEditor settings to allow edit expression for selected C1FlexGrid column.
        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var field = (e.AddedItems[0] as ComboBoxItem).Tag as string;
            if (flexGrid != null && flexGrid.ExpressionEditors.Contains(field))
            {
                editor.DataSource = flexGrid.ExpressionEditors[field].DataSource;
                editor.Expression = flexGrid.ExpressionEditors[field].Expression;
            }
        }
    }

    public class FlexGridEE : C1FlexGrid, ISupportExpressions
    {
        #region Ctor
        public FlexGridEE() : base()
        {
            ExpressionEditors = new ExpressionEditorCollection(this);
        }
        #endregion

        #region Override
        protected override void OnCellEditEnded(CellEditEventArgs e)
        {
            base.OnCellEditEnded(e);
            // ignore expression column changing
            if (!e.Cancel && !ExpressionEditors.Contains(Columns[e.Column].ColumnName))
                ExpressionEditors.Evaluate(GetDataIndex(e.Row));
        }
        #endregion

        #region ISupportExpressions
        public void SetCellValue(int row, string colName, object value)
        {
            this[GetRowIndex(row), colName] = value;
        }

        public ExpressionEditorCollection ExpressionEditors { get; }
        #endregion
    }
}
