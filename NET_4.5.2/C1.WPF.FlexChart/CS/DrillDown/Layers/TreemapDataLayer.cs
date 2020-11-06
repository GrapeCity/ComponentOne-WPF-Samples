using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DrillDown
{
    public class TreemapDataLayer : Bindable
    {
        private Leaf[] _itemsSource;
        
        private ObservableCollection<KeyValuePair<string, Leaf[]>> _histories = new ObservableCollection<KeyValuePair<string, Leaf[]>>();

        public TreemapDataLayer(Leaf[] dataSource)
        {
            ItemsSource = dataSource;
            _histories.CollectionChanged += _histories_CollectionChanged;
            _histories.Add(new KeyValuePair<string, Leaf[]> ( "All Types", dataSource ));
        }

        private void _histories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Notify("CurrentType");
        }

        public Leaf[] ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                SetProperty(ref _itemsSource, value, "ItemsSource");
            }
        }
    }
}
