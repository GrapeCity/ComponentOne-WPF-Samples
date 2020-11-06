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

using C1.WPF.C1Chart;

namespace ChartSamples
{
  /// <summary>
  /// Interaction logic for Selection.xaml
  /// </summary>
  public partial class Selection : UserControl
  {
    public Selection()
    {
      InitializeComponent();

      var source = Product.GetProducts(8);

      chart.View.AxisX.AnnoAngle = 60;

      var style = new Style(typeof(PlotElement));
      //style.Setters.Add(new Setter(PlotElement.FillProperty, Brushes.Red));
      style.Setters.Add(new Setter(PlotElement.StrokeThicknessProperty, 4.0));
      style.Setters.Add(new Setter(PlotElement.StrokeProperty, new SolidColorBrush(Color.FromArgb(255, 224, 64, 64))));

      var data = new ChartData() { ItemNameBinding = new Binding("Name") };
      data.Children.Add(new DataSeries()
      {
        // ItemsSource = source,
        SelectedItemLabelTemplate = (DataTemplate)Resources["lbl"],
        Label = "Price",
        ValueBinding = new Binding("Price"),
        SelectedItemStyle = style
      });
      data.Children.Add(new DataSeries()
      {
        // ItemsSource = source, 
        SelectedItemLabelTemplate = (DataTemplate)Resources["lbl"],
        Label = "Cost",
        ValueBinding = new Binding("Cost"),
        SelectedItemStyle = style
      });

      chart.Data = data;

      data.ItemsSource = source;

      grid.ItemsSource = source;
      grid.AutoGenerateColumns = false;
      grid.Columns.Add(new C1.WPF.FlexGrid.Column() { ColumnName = "Name", Binding = new Binding("Name") });
      grid.Columns.Add(new C1.WPF.FlexGrid.Column() { ColumnName = "Price", Binding = new Binding("Price") });
      grid.Columns.Add(new C1.WPF.FlexGrid.Column() { ColumnName = "Cost", Binding = new Binding("Cost") });

      grid.SelectionMode = C1.WPF.FlexGrid.SelectionMode.Row;
      grid.SelectionForeground = new SolidColorBrush(Colors.Red);

      cbSelectionMode.ItemsSource = Utils.GetEnumValues<SelectionAction>();
      cbSelectionMode.SelectionChanged += (s, e) => data.SelectionAction = (SelectionAction)cbSelectionMode.SelectedItem;
      cbSelectionMode.SelectedIndex = 1;

      cbChartType.ItemsSource = new ChartType[] { ChartType.Column, ChartType.Bar, ChartType.LineSymbols, ChartType.Pie };
      cbChartType.SelectedIndex = 0;
      cbChartType.SelectionChanged += (s, e) => chart.ChartType = (ChartType)cbChartType.SelectedItem;

    }
  }
}
