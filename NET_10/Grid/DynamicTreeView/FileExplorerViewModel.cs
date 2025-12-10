using C1.DataCollection;
using System.IO;

namespace DynamicTreeView
{
    public class FileExplorerViewModel
    {
        public FileExplorerViewModel()
        {
            var rootFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Root = new FileExplorerItemsCollection(rootFolderPath);
        }
        public FileExplorerItemsCollection Root { get; }
    }

    public class FileExplorerItemsCollection : C1DataCollectionBase<FileExplorerItem>, ISupportSorting, ISupportFiltering
    {
        public FileExplorerItemsCollection(string fullPath)
        {
            FullPath = fullPath;
            var sequence = new C1SequenceDataCollection<FileExplorerItem>();
            sequence.Collections.Add(new C1SortDataCollection<FileExplorerItem>(new C1FilterDataCollection<FileExplorerItem>(Directory.GetDirectories(fullPath, "*", new EnumerationOptions() { IgnoreInaccessible = true }).Select(entryPath => CreateItem(true, entryPath))) { RunSynchronously = true }) { RunSynchronously = true });
            sequence.Collections.Add(new C1SortDataCollection<FileExplorerItem>(new C1FilterDataCollection<FileExplorerItem>(Directory.GetFiles(fullPath, "*", new EnumerationOptions() { IgnoreInaccessible = true }).Select(entryPath => CreateItem(false, entryPath))) { RunSynchronously = true }) { RunSynchronously = true });
            InternalList = sequence;
        }

        public event EventHandler? SortChanged;
        public event EventHandler? FilterChanged;

        private static FileExplorerItem CreateItem(bool isDirectory, string entryPath)
        {
            return new FileExplorerItem
            {
                Path = entryPath,
                IsDirectory = isDirectory,
                Name = Path.GetFileName(entryPath),
                Size = Directory.Exists(entryPath) ? null : new System.IO.FileInfo(entryPath).Length,
                ModifiedDate = File.GetLastAccessTime(entryPath),
            };
        }

        public string FullPath { get; }

        public IReadOnlyList<SortDescription> SortDescriptions { get; set; }

        public bool CanSort(params SortDescription[] sortDescriptions)
        {
            return true;
        }

        public async Task SortAsync(SortDescription[] sortDescriptions, CancellationToken cancellationToken = default)
        {
            var sequence = InternalList as C1SequenceDataCollection<FileExplorerItem>;
            await sequence.Collections[0].SortAsync(sortDescriptions, cancellationToken);
            await sequence.Collections[1].SortAsync(sortDescriptions, cancellationToken);
            var args = NotifyCollectionChangedAsyncEventArgs.Create(System.Collections.Specialized.NotifyCollectionChangedAction.Reset, cancellationToken);
            OnCollectionChanged(this, args.EventArgs);
            await args.WaitDeferralsAsync();
            SortDescriptions = sortDescriptions;
            SortChanged?.Invoke(this, EventArgs.Empty);
        }

        public FilterExpression? FilterExpression { get; set; }

        public bool CanFilter(FilterExpression? filterExpression)
        {
            return true;
        }

        public async Task FilterAsync(FilterExpression? filterExpression, CancellationToken cancellationToken = default)
        {
            var sequence = InternalList as C1SequenceDataCollection<FileExplorerItem>;
            await sequence.Collections[0].FilterAsync(filterExpression, cancellationToken);
            await sequence.Collections[1].FilterAsync(filterExpression, cancellationToken);
            var args = NotifyCollectionChangedAsyncEventArgs.Create(System.Collections.Specialized.NotifyCollectionChangedAction.Reset, cancellationToken);
            OnCollectionChanged(this, args.EventArgs);
            await args.WaitDeferralsAsync();
            FilterExpression = filterExpression;
            FilterChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class FileExplorerItem
    {
        FileExplorerItemsCollection _items;
        public string Path { get; init; }
        public bool IsDirectory { get; init; }
        public string Name { get; init; }
        public long? Size { get; init; }
        public DateTime ModifiedDate { get; init; }
        public FileExplorerItemsCollection Items
        {
            get
            {
                if (IsDirectory && _items == null)
                {
                    _items = new FileExplorerItemsCollection(Path);
                }
                return _items;
            }
        }
    }
}
