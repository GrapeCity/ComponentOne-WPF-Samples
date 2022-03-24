using C1.WPF.Chart;
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
    /// Interaction logic for FlexChart.xaml
    /// </summary>
    public partial class FlexChart : UserControl
    {
        public ICollectionView View;

        public FlexChart()
        {
            InitializeComponent();
            View = CollectionViewSource.GetDefaultView(DataCreator.CreateData());

            flexChart.ItemsSource = View;
            flexChart.BindingX = "Country";
            flexChart.Series.Add(new Series() { SeriesName = "Sales", Binding = "Sales" });
            flexChart.Series.Add(new Series() { SeriesName = "Expenses", Binding = "Expenses" });

            editor.DataSource = View.CurrentItem;
            editor.Expression = "[Sales] < 10";

            View.Filter = new Predicate<object>(Contains);
        }

        private bool Contains(object obj)
        {
            editor.DataSource = obj as DataItem;
            var value = editor.Evaluate();
            var ret = true;
            if (value != null)
                Boolean.TryParse(value.ToString(), out ret);
            return ret;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (editor.IsValid)
            {
                View.Filter = new Predicate<object>(Contains);
            }
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            View.Filter = null;
        }

        private void Check_Checked(object sender, RoutedEventArgs e)
        {
            editor.ExpressionChanged += Editor_ExpressionChanged;
            if (editor.IsValid)
            {
                View.Filter = new Predicate<object>(Contains);
            }
        }

        private void Check_Unchecked(object sender, RoutedEventArgs e)
        {
            editor.ExpressionChanged -= Editor_ExpressionChanged;
        }

        private void Editor_ExpressionChanged(object sender, EventArgs e)
        {
            if (editor.IsValid)
            {
                View.Filter = new Predicate<object>(Contains);
            }
        }
    }
}
