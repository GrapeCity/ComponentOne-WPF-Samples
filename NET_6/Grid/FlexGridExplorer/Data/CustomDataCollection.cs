using C1.DataCollection;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace FlexGridExplorer
{
    public class CustomDataCollection<T> : C1DataCollection<T>
        where T : class
    {
        public CustomDataCollection(IEnumerable source)
            : base(source)
        {
        }

        public override async Task<int> InsertAsync(int index, object item, CancellationToken cancellationToken=default)
        {
            await Task.Delay(1000, cancellationToken); //simulates a network operation
            return await base.InsertAsync(index, item, cancellationToken);
        }

        public override async Task RemoveAsync(int index, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000, cancellationToken); //simulates a network operation
            await base.RemoveAsync(index, cancellationToken);
        }

        public override async Task RemoveRangeAsync(int index, int count, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000, cancellationToken); //simulates a network operation
            await base.RemoveRangeAsync(index, count, cancellationToken);
        }

        public override async Task ReplaceAsync(int index, object item, CancellationToken cancellationToken = default)
        {
            await Task.Delay(1000, cancellationToken); //simulates a network operation
            await base.ReplaceAsync(index, item, cancellationToken);
        }
    }
}
