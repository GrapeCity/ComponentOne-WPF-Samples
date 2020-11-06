using C1.WPF.ExpressionEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace C1ExpressionEditorSample
{
    /// <summary>
    /// Interaction logic for DataGridIntegration.xaml
    /// </summary>
    public partial class DataGridIntegration : UserControl
    {
        public DataGridIntegration()
        {
            InitializeComponent();

            List<Product> items = Product.GetData(200);
            CollectionViewSource view = new CollectionViewSource() { Source = items };
            grid.ItemsSource = view.View;

            C1ExpressionEditor c1ExpressionEditor1 = new C1ExpressionEditor();
            C1ExpressionEditor c1ExpressionEditor2 = new C1ExpressionEditor();
            c1ExpressionEditor1.Expression = "[Price]*0.95";
            c1ExpressionEditor2.Expression = "[Price]*0.8";
            grid.ExpressionEditors.Add("CustomField1", c1ExpressionEditor1);
            grid.ExpressionEditors.Add("CustomField2", c1ExpressionEditor2);
            comboBox.SelectedIndex = 0;
            editor.OkClick += Editor_OkClick;
            editor.CancelClick += Editor_CancelClick;
            grid.Loaded += Grid_Loaded;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (DataGridColumn column in grid.Columns)
            {
                if (column.Header.Equals("CustomField1"))
                {
                    // don't allow cell edits
                    column.IsReadOnly = true;
                    // use human-friendly header
                    column.Header = "Special Price (New Customers)";

                }
                if (column.Header.Equals("CustomField2"))
                {
                    // don't allow cell edits
                    column.IsReadOnly = true;
                    // use human-friendly header
                    column.Header = "Special Price (Card Holders)";
                }
            }            
        }

        private void Editor_CancelClick(object sender, EventArgs e)
        {
            var field = ((ComboBoxItem)comboBox.SelectedItem).Tag as string;
            if (grid != null && grid.ExpressionEditors.Contains(field))
            {
                editor.Expression = grid.ExpressionEditors[field].Expression;
            }
        }

        private void Editor_OkClick(object sender, EventArgs e)
        {
            var field = ((ComboBoxItem)comboBox.SelectedItem).Tag as string;
            if (grid != null && grid.ExpressionEditors.Contains(field))
            {
                grid.ExpressionEditors[field].Expression = editor.Expression;
            }
        }

        // Change ExpressionEditor settings to allow edit expression for selected DataGrid column.
        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var field = (e.AddedItems[0] as ComboBoxItem).Tag as string;
            if (grid != null && grid.ExpressionEditors.Contains(field))
            {
                editor.DataSource = grid.ExpressionEditors[field].DataSource;
                editor.Expression = grid.ExpressionEditors[field].Expression;
            }
        }
    }

    public class DataGridEE : DataGrid, ISupportExpressions
    {

        #region Ctor
        public DataGridEE() : base()
        {
            ExpressionEditors = new ExpressionEditorCollection(this);
            Loaded += DataGridEE_Loaded;
        }

        private void DataGridEE_Loaded(object sender, RoutedEventArgs e)
        {
            // refresh after loading
            Items.Refresh();
        }
        #endregion

        protected override void OnCellEditEnding(DataGridCellEditEndingEventArgs e)
        {
            base.OnCellEditEnding(e);
            if (!e.Cancel && e.EditAction == DataGridEditAction.Commit)
            {
                // subscribe to the property changed for the evaluation after commit
                ((INotifyPropertyChanged)e.Row.Item).PropertyChanged += DataGridEE_PropertyChanged;
            }
        }

        private void DataGridEE_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ((INotifyPropertyChanged)sender).PropertyChanged -= DataGridEE_PropertyChanged;
            // ignore expression column changing
            if (!ExpressionEditors.Contains(e.PropertyName))
            {
                // evaluate the expression
                ExpressionEditors.Evaluate( Items.IndexOf(sender) );
            }
        }

        #region ISupportExpressions

        public void SetCellValue(int row, string colName, object value)
        {
            var item = Items[row];
            var propertyInfo = item?.GetType()?.GetProperty(colName);
            if (null != propertyInfo)
            {
                var oldValue = propertyInfo.GetValue(item, null);
                if (!oldValue.Equals(value))
                {
                    // set property
                    propertyInfo.SetValue(item, value, null);

                    // try to find TextBlock representing cell and update it
                    var cell = GetCell(row, colName);
                    if (cell != null)
                    {
                        var binding = cell.GetBindingExpression(TextBlock.TextProperty);
                        binding.UpdateTarget();
                    }
                    else if ( IsLoaded) // if we haven't found cell - force full refresh
                    {
                        Items.Refresh();
                    }
                }
            }
        }

        // Get TextBlock from cell presenter for just edited cell
        private TextBlock GetCell(int rowNumber, string columnName)
        {
            DataGridRow row = this.ItemContainerGenerator.ContainerFromIndex(rowNumber) as DataGridRow; // it might be null if row is not loaded yet
            if (row != null)
            {
                int column = -1;
                // need column index, but only have column name
                for (int i = 0; i < Columns.Count; i++)
                {
                    // todo: check whether it is possible to make this in more safe way
                    if ( Columns[i] is DataGridBoundColumn && ((Binding)((DataGridBoundColumn)Columns[i]).Binding).Path.Path == columnName)
                    {
                        column = i;
                    }
                }
                if (column >= 0)
                {
                    DataGridCellsPresenter presenter = C1.WPF.VTreeHelper.GetChildOfType(row, typeof(DataGridCellsPresenter)) as DataGridCellsPresenter;
                    if (presenter != null)
                    {

                        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                        if ( cell != null)
                        {
                            return C1.WPF.VTreeHelper.GetChildOfType(cell, typeof(TextBlock)) as TextBlock;
                        }
                    }
                }
            }
            return null;
        }

        public ExpressionEditorCollection ExpressionEditors { get; }

        #endregion

        protected override void OnAutoGeneratingColumn(DataGridAutoGeneratingColumnEventArgs e)
        {
            if ( e.PropertyType == typeof(double))
            {
                ((DataGridTextColumn)e.Column).Binding.StringFormat = "F2";
            }
            base.OnAutoGeneratingColumn(e);
        }
    }
}
