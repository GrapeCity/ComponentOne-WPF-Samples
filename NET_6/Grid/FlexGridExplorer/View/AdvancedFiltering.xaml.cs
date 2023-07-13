using C1.DataCollection;
using C1.WPF.TabControl;
using FlexGridExplorer.Resources;
using System.Linq;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for AdvancedFiltering.xaml
    /// </summary>
    public partial class AdvancedFiltering : UserControl
    {
        private IDataCollection<object> _collection;

        public AdvancedFiltering()
        {
            InitializeComponent();
            Tag = AppResources.AdvancedFilteringDesc;
            _collection = new C1DataCollection<TaskItem>(TaskItem.GetRandomList(100).ToList());
            grid.ItemsSource = _collection;
            dataFilter.ItemsSource = _collection;
        }

        private void OnTabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_collection == null) return;

            var tabControl = sender as C1TabControl;
            FilterExpression completedExpression = null, deferredExpression = null, urgentExpression = null;
            switch (tabControl.SelectedIndex)
            {
                case 1://InProgress
                    completedExpression = new FilterOperationExpression(nameof(TaskItem.Complete), FilterOperation.LessThan, 1.0);
                    break;
                case 2://Completed
                    completedExpression = new FilterOperationExpression(nameof(TaskItem.Complete), FilterOperation.Equal, 1.0);
                    break;
                case 3://Deferred
                    deferredExpression = new FilterOperationExpression(nameof(TaskItem.Deferred), FilterOperation.Equal, true);
                    break;
                case 4://Urgent
                    urgentExpression = new FilterOperationExpression(nameof(TaskItem.Urgent), FilterOperation.Equal, true);
                    break;
            }
            var newFilterExpression = _collection.GetFilterExpression();
            newFilterExpression = newFilterExpression.ReplaceExpressionInScope(nameof(TaskItem.Complete), completedExpression);
            newFilterExpression = newFilterExpression.ReplaceExpressionInScope(nameof(TaskItem.Deferred), deferredExpression);
            newFilterExpression = newFilterExpression.ReplaceExpressionInScope(nameof(TaskItem.Urgent), urgentExpression);
            _collection.FilterAsync(newFilterExpression);
        }

        private void OnFilterAutoGenerating(object sender, C1.WPF.DataFilter.FilterAutoGeneratingEventArgs e)
        {
            if(e.Property.Name == nameof(TaskItem.AssignTo) || e.Property.Name == nameof(TaskItem.OwnedBy))
            {
                e.Filter = new C1.WPF.DataFilter.FullTextFilter() { PropertyName = e.Property.Name };
            }
        }
    }
}
