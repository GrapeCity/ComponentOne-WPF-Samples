using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace DrillDown
{
    public class DrillDownManager : DrillDownBase
    {
        #region fields

        private IEnumerable _itemsSource;
        private Queue<string> _drillDownPathQueue;
        private List<object> _chartView;
        private string _bindingX;
        private string _binding;
        private object _current;
        private object _parent;

        #endregion

        #region Object Model

        private List<object> ChartView
        {
            get { return _chartView; }
            set
            {
                _chartView = value;
                UpdateChartItemsSource();
            }
        }

        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            private set
            {
                SetProperty(ref _itemsSource, value, "ItemsSource");
            }
        }

        public RelayCommand BackCommand { get; set; }

        public object Current
        {
            get { return _current; }
            set
            {
                SetProperty(ref _current, value, "Current");
            }
        }

        public object Parent
        {
            get { return _parent; }
            set
            {
                SetProperty(ref _parent, value, "Parent");
            }
        }

        public string BindingX
        {
            get { return _bindingX; }
            set
            {
                SetProperty(ref _bindingX, value, "BindingX");
            }
        }

        public string Binding
        {
            get { return _binding; }
            set
            {
                SetProperty(ref _binding, value, "Binding");
            }
        }

        #endregion

        #region C'tor
        public DrillDownManager(IEnumerable<object> sourceCollection, string aggregateOn, string drillDownPath) :base(sourceCollection,drillDownPath)
        {
            DrillDownLevel = 0;
            AggregateOn = aggregateOn;
            BackCommand = new RelayCommand((p) => Back(p));
            GroupNames = new List<string>();
            //Refresh();
        }

        #endregion

        #region Implementation

        public override void Refresh()
        {
            DrillDownLevel = 0;
            History = new Stack<DrillDownHistory>();
            _drillDownPathQueue = new Queue<string>(DrillDownPath.Split(','));
            GroupNames = _drillDownPathQueue.Count > 1 ? DrillDownPath.Split(',').ToList() : GroupNames;

            var view = CollectionViewSource.GetDefaultView(SourceCollection);
            view.GroupDescriptions.Clear();
            foreach (var path in _drillDownPathQueue)
            {
                view.GroupDescriptions.Add(new PropertyGroupDescription(path));
            }
            _currentPath = _drillDownPathQueue.Dequeue();
            Current = new CollectionViewWrapper { Name = GroupNames[0], View = view };
            ChartView = view.Groups.ToList();
            OnAfterDrill(_currentPath, DrillDownLevel, true);
        }


        protected override void PerformDrillDown(int index)
        {
            if (Current != null && _drillDownPathQueue != null && _drillDownPathQueue.Count > 0)
            {
                var selectedGroup = ChartView[index] as CollectionViewGroup;
                //Push CurrentItems to History and then Perfrom DrillDown
                if (Current != null)
                {
                    var currentItem = Current as CollectionViewGroup;
                    var ddHistory = new DrillDownHistory();
                    ddHistory.DrillDownPath = _drillDownPathQueue.ToArray();
                    ddHistory.Name = currentItem != null ? currentItem.Name.ToString() : GroupNames[0];
                    ddHistory.Path = _currentPath;
                    ddHistory.View = Current;
                    ddHistory.Parent = Parent;
                    History.Push(ddHistory);
                }
                _currentPath = _drillDownPathQueue.Dequeue();
                Parent = Current;
                Current = selectedGroup;
                ChartView = selectedGroup.Items.ToList();
                DrillDownLevel++;
            }
        }

        protected override void PerformDrillDown(IEnumerable<object> itemsSource)
        {
            if (Current != null)
            {
                var currentGroup = Current as CollectionViewGroup;
                History.Push(
                    new DrillDownHistory()
                    {
                        DrillDownPath = _drillDownPathQueue.ToArray(),
                        Name = currentGroup != null ? currentGroup.Name.ToString() : (Current as CollectionViewWrapper).Name,
                        Path = _currentPath,
                        Parent = Parent,
                        View = Current,
                    }
                );
            }
            Parent = Current;
            var view = CollectionViewSource.GetDefaultView(itemsSource);
            view.GroupDescriptions.Add(new PropertyGroupDescription(_currentPath));
            Current = new CollectionViewWrapper { View = view};
            ChartView = view.Groups.ToList(); DrillDownLevel++;
        }

        protected override void PerformDrillUp()
        {
            if (DrillDownLevel == 0)
                return;

            var historicalItem = History.Pop();  //Get Previous View
            _currentPath = historicalItem.Path;
            this.Current = historicalItem.View;
            this.Parent = historicalItem.Parent;

            if (Current is CollectionViewGroup)
                this.ChartView = ((CollectionViewGroup)Current).Items.ToList();
            else
                this.ChartView = ((CollectionViewWrapper)Current).View.Groups.ToList();

            _drillDownPathQueue = new Queue<string>(historicalItem.DrillDownPath);
            DrillDownLevel--;
        }

        protected override void UpdateAggregate()
        {
            UpdateChartItemsSource();
        }

        private void Back(object param)
        {
            if (param == null)
                DrillUp();
            else if (param is int)
                DrillUp((int)param);
            else if (param is string)
                DrillUp(param.ToString());
        }

        private double Aggregate(object item, AggregateType type)
        {
            var group = item as CollectionViewGroup;
            if (group != null)
            {
                var sum = group.Items.Sum(sub => Aggregate(sub, AggregateType.Sum));
                var sum2 = Sum(item);
                var cnt = group.ItemCount;
                var avg = sum / cnt;
                switch (type)
                {
                    case AggregateType.Avg:
                        return Math.Floor(avg);
                    case AggregateType.Max:
                        return group.Items.Max(sub => Aggregate(sub, type));
                    case AggregateType.Min:
                        return group.Items.Min(sub => Aggregate(sub, type));
                    case AggregateType.Rng:
                        return group.Items.Max(sub => Aggregate(sub, AggregateType.Max)) - group.Items.Min(sub => Aggregate(sub, AggregateType.Min));
                    case AggregateType.Cnt:
                        return cnt;
                    case AggregateType.Std:
                        return Math.Floor(Math.Sqrt((sum2 / cnt - avg * avg) * cnt / (cnt - 1)));
                    case AggregateType.Var:
                        return Math.Floor((sum2 / cnt - avg * avg) * cnt / (cnt - 1));
                    case AggregateType.StdPop:
                        return Math.Floor(Math.Sqrt(sum2 / cnt - avg * avg));
                    case AggregateType.VarPop:
                        return Math.Floor(sum2 / cnt - avg * avg);
                    case AggregateType.Sum:
                    default:
                        return sum;
                }
            }
            else
            {
                return (double)GetProperty(item, AggregateOn);
            }
        }

        private double Sum(object item)
        {
            var group = item as CollectionViewGroup;
            if (group != null)
            {
                return group.Items.Sum(sub => Sum(sub));
            }
            else
            {
                var amount = (double)GetProperty(item, AggregateOn);
                return amount * amount;
            }
        }

        private void UpdateChartItemsSource()
        {
            List<object> groups = new List<object>();
            foreach (var item in ChartView)
            {
                dynamic groupItem = new ExpandoObject();
                var dt = groupItem as IDictionary<string, object>;
                dt[_currentPath] = (item as CollectionViewGroup).Name;
                dt["Value"] = Aggregate(item, AggregateType);
                groups.Add(groupItem);
            }
            ItemsSource = groups;
            BindingX = _currentPath;
        }

        private object GetProperty(object obj, string path)
        {
            return obj.GetType().GetProperty(path).GetValue(obj, null);
        }

        #endregion
    }

    public class CollectionViewWrapper
    {
        public string Name
        { get; set; }

        public ICollectionView View
        { get; set; }
    }
}
