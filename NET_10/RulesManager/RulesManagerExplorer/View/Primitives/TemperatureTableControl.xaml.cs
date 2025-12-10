using C1.WPF.RulesManager;
using System;
using System.Windows;
using System.Windows.Controls;

namespace RulesManagerExplorer
{
    public partial class TemperatureTableControl : UserControl
    {
        public TemperatureTableControl()
        {
            InitializeComponent();

            grid.ItemsSource = AverageTemperatureData.GetDemoData();
            Source = new DataGridRulesEngineSource(grid);
        }

        public C1RulesEngine RulesEngine
        {
            get => (C1RulesEngine)GetValue(RulesEngineProperty);
            set => SetValue(RulesEngineProperty, value);
        }
        public DataGridRulesEngineSource Source { get; }

        public static readonly DependencyProperty RulesEngineProperty = DependencyProperty.Register(
            nameof(RulesEngine),
            typeof(C1RulesEngine),
            typeof(TemperatureTableControl),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.None, OnEngineChanged));

        private static void OnEngineChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TemperatureTableControl control)
            {
                control.OnEngineChanged(e.OldValue as C1RulesEngine, e.NewValue as C1RulesEngine);
            }
        }

        private void OnEngineChanged(C1RulesEngine oldValue, C1RulesEngine newValue)
        {
            if (oldValue is not null)
            {
                oldValue.RulesChanged -= OnRulesEngineRulesChanged;
            }
            if (newValue is not null)
            {
                newValue.RulesChanged += OnRulesEngineRulesChanged;
            }
            Refresh();
        }

        private void OnRulesEngineRulesChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Refresh()
        {
            grid.Items.Refresh();
        }

        public RulesEngineStyle GetConditionalFormattingStyle(DataGridCell cell)
        {
            return RulesEngine?.GetStyle(Source, DataGridRulesEngineSource.GetIndex(grid, cell), DataGridRulesEngineSource.GetField(cell)) ?? new RulesEngineStyle();
        }
    }
}