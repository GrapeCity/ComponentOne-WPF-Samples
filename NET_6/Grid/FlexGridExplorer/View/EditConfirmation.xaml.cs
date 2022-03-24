using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Windows;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for EditConfirmation.xaml
    /// </summary>
    public partial class EditConfirmation : UserControl
    {
        public EditConfirmation()
        {
            InitializeComponent();
            Tag = AppResources.EditConfirmationDescription;
            var data = Customer.GetCustomerList(100);
            grid.ItemsSource = data;
            grid.MinColumnWidth = 85;
        }

        private object _originalValue;

        private void OnBeginningEdit(object sender, GridCellEditEventArgs e)
        {
            _originalValue = grid[e.CellRange.Row, e.CellRange.Column];
        }

        private void OnCellEditEnded(object sender, GridCellEditEventArgs e)
        {
            var originalValue = _originalValue;
            var currentValue = grid[e.CellRange.Row, e.CellRange.Column];
            if (!e.CancelEdits && (originalValue == null && currentValue != null || !originalValue.Equals(currentValue)))
            {
                if (MessageBox.Show(AppResources.EditConfirmationQuestion, AppResources.EditConfirmationQuestionTitle, MessageBoxButton.YesNo) == MessageBoxResult.No)
                {
                    grid[e.CellRange.Row, e.CellRange.Column] = originalValue;
                }
            }
        }
    }
}
