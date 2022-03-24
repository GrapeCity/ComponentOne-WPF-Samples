using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using C1.WPF.DataGrid;

namespace DataGridSamples
{
    /// <summary>
    /// Implements a column composed of multiple columns.
    /// </summary>
    public class CompositeColumn
        : C1.WPF.DataGrid.DataGridColumn
    {
        // Height of each level
        public static int LevelHeaderHeight = 18;

        // Inner Columns
        public ObservableCollection<C1.WPF.DataGrid.DataGridColumn> InnerColumns { get; set; }

        // Global Header
        public object CompositeHeader { get; set; }

        // Nested Levels
        public int NestedLevels { get; private set; }

        public CompositeColumn() 
        {
            InnerColumns = new ObservableCollection<C1.WPF.DataGrid.DataGridColumn>();

            // the following features are not implemented
            CanUserResize = false;
            CanUserSort = false;
            CanUserFilter = false;
        }

        public void Update() 
        {
            double totalWidth = 0;
            int maxNestedLevels = 0;

            // initialize grid
            var panel = new Grid();
            panel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(CompositeColumn.LevelHeaderHeight) });
            panel.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(CompositeColumn.LevelHeaderHeight) });
            foreach (var col in InnerColumns)
            {
                // support nested scenarios
                var cc = col as CompositeColumn;
                if (cc != null)
                {
                    cc.Update();
                    maxNestedLevels = Math.Max(cc.NestedLevels, maxNestedLevels);
                }

                panel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(col.Width.Value) });
                totalWidth += col.Width.Value;
            }

            // add global header
            var globalHeader = new ContentControl() { Content = CompositeHeader };
            Grid.SetColumnSpan(globalHeader, InnerColumns.Count);
            panel.Children.Add(globalHeader);

            // add individual headers
            for (int i = 0; i < InnerColumns.Count; i++)
            {
                var col = InnerColumns[i];

                // add nested headers
                var content = new DataGridColumnHeaderPresenter() {  Content = col.Header, Background = new SolidColorBrush(Colors.Transparent) };
                Grid.SetColumn(content, i);
                Grid.SetRow(content, 1);
                panel.Children.Add(content);
            }

            // update header & global width
            Header = panel;
            Width = new C1.WPF.DataGrid.DataGridLength(totalWidth);
            NestedLevels = 1 + maxNestedLevels;
        }

        #region Cell Content

        public override object GetCellContentRecyclingKey(C1.WPF.DataGrid.DataGridRow row)
        {
            return typeof(CompositeColumn);
        }

        public override FrameworkElement CreateCellContent(C1.WPF.DataGrid.DataGridRow row)
        {
            // initialize grid
            var panel = new Grid();
            foreach (var col in InnerColumns)
            {
                panel.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(col.Width.Value) });
            }

            // add individual content
            for (int i = 0; i < InnerColumns.Count; i++)
            {
                var col = InnerColumns[i];
                var content = col.CreateCellContent(row);
                Grid.SetColumn(content, i);
                panel.Children.Add(content);
            }
            return panel;
        }

        public override void BindCellContent(FrameworkElement cellContent, C1.WPF.DataGrid.DataGridRow row)
        {
            Panel panel = (Panel)cellContent;

            // bind individual cells
            for (int i = 0; i < InnerColumns.Count; i++)
            {
                var col = InnerColumns[i];
                var control = panel.Children[i] as FrameworkElement;
                col.BindCellContent(control, row);
            }
        }

        public override void UnbindCellContent(FrameworkElement cellContent, C1.WPF.DataGrid.DataGridRow row)
        {
            Panel panel = (Panel)cellContent;

            // bind individual cells
            for (int i = 0; i < InnerColumns.Count; i++)
            {
                var col = InnerColumns[i];
                var control = panel.Children[i] as FrameworkElement;
                col.UnbindCellContent(control, row);
            }
        }

        #endregion

        #region Editing

        public override FrameworkElement GetCellEditingContent(C1.WPF.DataGrid.DataGridRow row)
        {
            // initialize grid
            var panel = new Grid();
            foreach (var col in InnerColumns)
            {
                panel.ColumnDefinitions.Add(new ColumnDefinition() {  Width = new GridLength(col.Width.Value) });
            }

            // add individual content
            for (int i = 0; i < InnerColumns.Count; i++)
            {
                var col = InnerColumns[i];
                var content = col.GetCellEditingContent(row);
                Grid.SetColumn(content, i);
                panel.Children.Add(content);
            }
            return panel;
        }

        public override object PrepareCellForEdit(FrameworkElement editingElement)
        {
            // compose all the values into a list of objects
            List<object> values = new List<object>();
            var children = (editingElement as Panel).Children;

            for (int i = 0; i < InnerColumns.Count; i++)
            {
                var value = InnerColumns[i].PrepareCellForEdit(children[i] as FrameworkElement);
                values.Add(value);
            }
            return values;
        }

        public override void CancelCellEdit(FrameworkElement editingElement, object uneditedValue)
        {
            // decompose all the values from the list of objects
            // and invoke each of the cancels
            var values = (List<object>) uneditedValue;
            var children = (editingElement as Panel).Children;

            for (int i = 0; i < InnerColumns.Count; i++)
            {
                InnerColumns[i].CancelCellEdit(children[i] as FrameworkElement, values[i]);
            }            
        }

        public override bool BeginEdit(FrameworkElement editingElement, RoutedEventArgs routedEventArgs)
        {
            // pass input to first column
            if (InnerColumns.Count > 0)
            {
                var children = (editingElement as Panel).Children;
                return InnerColumns[0].BeginEdit(children[0] as Control, routedEventArgs);
            }
            return false;
        }

        public override void EndEdit(FrameworkElement editingElement)
        {
            for (int i = 0; i < InnerColumns.Count; i++)
            {
                if (editingElement is Panel)
                {
                    var children = (editingElement as Panel).Children;
                    if (children.Count > i)
                    {
                        InnerColumns[i].EndEdit(children[i] as Control);
                    }
                }
            }
        }

        #endregion
    }
}
