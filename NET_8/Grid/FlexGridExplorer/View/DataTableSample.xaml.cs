using FlexGridExplorer.Resources;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class DataTableSample : UserControl
    {
        private int count = 7;
        private DataTable _dataTable = new DataTable();
        private Random _random = new Random();

        public DataTableSample()
        {
            InitializeComponent();
            Tag = AppResources.DataTableSampleDescription;

            _dataTable.Columns.Add("Int", typeof(int));
            _dataTable.Columns.Add("Double", typeof(double));
            _dataTable.Columns.Add("Float", typeof(float));
            _dataTable.Columns.Add("String", typeof(string));
            _dataTable.Columns.Add("DateTime", typeof(DateTime));
            _dataTable.Columns.Add("Boolean", typeof(bool));

            for (int i = 0; i < count; i++)
            {
                _dataTable.Rows.Add(i, _random.NextDouble() * count, (float)_random.NextDouble() * count, "String " + _random.Next(count), DateTime.Now.AddDays(i - _random.Next(count) / 2), _random.Next(count) % 2 == 0);
                ValidateRow(_dataTable.Rows[i]);
            }

            _dataTable.AcceptChanges();
            grid.ItemsSource = _dataTable.DefaultView;

            _dataTable.RowChanged += OnRowChanged;
        }

        private void OnRowChanged(object sender, DataRowChangeEventArgs e)
        {
            ValidateRow(e.Row);
        }

        private void ValidateRow(DataRow row)
        {
            if (!(row["Double"] is double doubleValue) || doubleValue < 3)
            {
                var error = "Column Double should be less than 3";
                row.SetColumnError("Double", error);
                row.RowError = error;
            }
            else
            {
                row.ClearErrors();
            }
        }

        private void OnShowChangesClicked(object sender, System.Windows.RoutedEventArgs e)
        {
            var changes = _dataTable.GetChanges();
            if (changes == null)
            {
                MessageBox.Show(AppResources.NoChangesEditDataTableFirstMessage);
            }
            else
            {
                var added = changes.Rows.Cast<DataRow>().Count(r => r.RowState == DataRowState.Added);
                var modified = changes.Rows.Cast<DataRow>().Count(r => r.RowState == DataRowState.Modified);
                var deleted = changes.Rows.Cast<DataRow>().Count(r => r.RowState == DataRowState.Deleted);
                MessageBox.Show(string.Format(AppResources.ChangesInDataTableMessage, added, modified, deleted));
            }
        }
    }
}
