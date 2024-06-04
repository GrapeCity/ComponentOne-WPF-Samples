using C1.DataCollection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ListViewExplorer
{
    public class VirtualModeDataCollection : C1VirtualDataCollection<Person>
    {
        private readonly int TotalCount = 1_000_000_000;

        protected override async Task<Tuple<int, IReadOnlyList<Person>>> GetPageAsync(int pageIndex, int startingIndex, int count, IReadOnlyList<SortDescription> sortDescriptions = null, FilterExpression filterExpression = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Delay(100, cancellationToken).ConfigureAwait(false); //Simulates network traffic.
            return new Tuple<int, IReadOnlyList<Person>>(TotalCount, Enumerable.Range(startingIndex, count).Select(i => new Person(i)).ToList());
        }
    }
}
