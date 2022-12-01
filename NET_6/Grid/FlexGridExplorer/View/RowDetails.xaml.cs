using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for RowDetails.xaml
    /// </summary>
    public partial class RowDetails : UserControl
    {
        private ObservableCollection<Customer> _data;

        public RowDetails()
        {
            InitializeComponent();
            Tag = AppResources.RowDetailsDescription;
            lblMode.Content = AppResources.DetailsVisibiltyMode;

            showItemsPicker.Items.Add(GridDetailVisibilityMode.ExpandMultiple.ToString());
            showItemsPicker.Items.Add(GridDetailVisibilityMode.ExpandSingle.ToString());
            showItemsPicker.Items.Add(GridDetailVisibilityMode.Selection.ToString());
            showItemsPicker.SelectedItemChanged += (s, e) =>
            {
                switch (showItemsPicker.SelectedIndex)
                {
                    case 0:
                        details.DetailVisibilityMode = GridDetailVisibilityMode.ExpandMultiple;
                        break;
                    case 1:
                        details.DetailVisibilityMode = GridDetailVisibilityMode.ExpandSingle;
                        break;
                    case 2:
                        details.DetailVisibilityMode = GridDetailVisibilityMode.Selection;
                        break;
                }
            };

            showItemsPicker.SelectedItem = GridDetailVisibilityMode.ExpandSingle.ToString();

            _data = Customer.GetCustomerList(1000);
            grid.ItemsSource = _data;
            grid.MinColumnWidth = 85;
        }

        private async void OnDetailLoading(object sender, GridDetailLoadingEventArgs<object> e)
        {
            var deferral = e.GetDeferral();
            try
            {
                //Simulates an operation that bring the details data.
                await System.Threading.Tasks.Task.Delay(1000);
            }
            finally
            {
                deferral.Complete();
            }
        }
    }
}
