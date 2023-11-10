using C1.DataCollection;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    public partial class Paging : UserControl
    {
        public Paging()
        {
            InitializeComponent();
            Tag = AppResources.PagingDescription;
            var pagedCollection = new C1PagedDataCollection<object>(new VirtualModeDataCollection() { PageSize = 10 });
            grid.ItemsSource = pagedCollection;
            pager.Source = pagedCollection;
        }

        internal class VirtualModeDataCollection : C1VirtualDataCollection<Customer>
        {
            protected override async Task<Tuple<int, IReadOnlyList<Customer>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
            {
                await Task.Delay(1000, cancellationToken);//Simulates network traffic.
                return new Tuple<int, IReadOnlyList<Customer>>(2_000_000, Enumerable.Range(startingIndex, count).Select(i => new Customer(i)).ToList());
            }
        }
    }
}
