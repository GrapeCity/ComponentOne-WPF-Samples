using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class SummaryRow : UserControl
    {
        Random _rand = new Random();
        public SummaryRow()
        {
            InitializeComponent();
            Tag = AppResources.SummaryRowDescription;

            var list = new List<Dictionary<string, double>>();
            for (int i = 0; i < 100; i++)
            {
                var dictionary = new Dictionary<string, double>();
                for (int j = 1; j <= 4; j++)
                    dictionary[$"Col{j}"] = _rand.NextDouble();
                list.Add(dictionary);
            }
            grid.ItemsSource = list;

        }
    }

    /// <summary>
    /// Returns the number of rows whose value for the column is between <see cref="Minimum"/> and <see cref="Maximum"/>.
    /// </summary>
    public class CountBetweenFunction : GridAggregateFunction
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public new double Minimum { get; set; } = double.MinValue;

        /// <summary>
        /// The maximum value.
        /// </summary>
        public new double Maximum { get; set; } = double.MaxValue;

        ///<inheritdoc/>
        public override double GetValue(GridColumn column, IEnumerable<GridRow> rows)
        {
            var count = 0;
            var grid = column.Grid;
            foreach (var row in rows)
            {
                // get raw value
                var val = grid[row, column];
                if (val is double dVal)
                    if (dVal >= Minimum && dVal <= Maximum)
                        count++;
            }

            return count;
        }
    }
}
