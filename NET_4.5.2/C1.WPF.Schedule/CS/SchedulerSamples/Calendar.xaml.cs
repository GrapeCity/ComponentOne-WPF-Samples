using System;
using System.Windows.Controls;

namespace SchedulerSamples
{
    public partial class Calendar : UserControl
    {
        public Calendar()
        {
            InitializeComponent();

            selModeCombo.ItemsSource = Enum.GetValues(typeof(SelectionMode));
            selModeCombo.SelectedIndex = 2;
            selModeCombo.SelectionChanged += ((s, e) =>
            {
                this.calendar1.SelectionMode = (SelectionMode)selModeCombo.SelectedItem;
            });

        }
    }
}
