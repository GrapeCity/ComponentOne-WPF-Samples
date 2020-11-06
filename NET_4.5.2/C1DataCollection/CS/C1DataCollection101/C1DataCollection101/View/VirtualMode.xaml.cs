using C1DataCollection101.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C1.DataCollection;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using C1.WPF.DataCollection;

namespace C1DataCollection101
{
    public partial class VirtualMode : Page
    {
        VirtualModeCollectionView _virtualCollection;

        public VirtualMode()
        {
            InitializeComponent();
            Title = AppResources.SortingTitle;
            var task = UpdateData();

        }

        private async Task UpdateData()
        {
            try
            {
                message.Visibility = System.Windows.Visibility.Collapsed;
                activityIndicator.Visibility = System.Windows.Visibility.Visible;
                _virtualCollection = new VirtualModeCollectionView() { Mode = VirtualDataCollectionMode.Manual };
                grid.ItemsSource = new C1CollectionView(_virtualCollection);
                grid.ScrollPositionChanged += OnScrollPositionChanged;
                _ = _virtualCollection.LoadAsync(grid.ViewRange.Row, grid.ViewRange.Row2);
            }
            catch
            {
                message.Text = AppResources.InternetConnectionError;
                message.Visibility = System.Windows.Visibility.Visible;
            }
            finally
            {
                activityIndicator.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void OnScrollPositionChanged(object sender, EventArgs e)
        {
            _ = _virtualCollection.LoadAsync(grid.ViewRange.Row, grid.ViewRange.Row2);
        }
    }

    public class VirtualModeCollectionView : C1VirtualDataCollection<Customer>
    {
        public int TotalCount { get; set; } = 1_000;

        protected override async Task<Tuple<int, IReadOnlyList<Customer>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Delay(500, cancellationToken);//Simulates network traffic.
            return new Tuple<int, IReadOnlyList<Customer>>(TotalCount, Enumerable.Range(startingIndex, count).Select(i => new Customer(i)).ToList());
        }
    }
}
