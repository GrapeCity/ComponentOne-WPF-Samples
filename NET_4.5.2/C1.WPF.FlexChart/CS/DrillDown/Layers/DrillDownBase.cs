using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DrillDown
{
  
    public abstract class DrillDownBase : Bindable
    {
        #region fields
        private IEnumerable<object> _sourceCollection;
        private List<string> _groupNames;
        private string _drillDownPath;
        private int _drillDownLevel = 0;
        private string _aggregateOn = null;
        private AggregateType _aggregateType = AggregateType.Sum;

        protected string _currentPath;

        #endregion

        #region Object Model
        public IEnumerable<object> SourceCollection
        {
            get { return _sourceCollection; }
            protected set
            {
                SetProperty(ref _sourceCollection, value, "SourceCollection");
                Refresh();
            }
        }

        public string DrillDownPath
        {
            get { return _drillDownPath; }
            set
            {
                SetProperty(ref _drillDownPath, value, "DrillDownPath");
                Refresh();
            }
        }

        public int DrillDownLevel
        {
            get { return _drillDownLevel; }
            protected set
            {
                SetProperty(ref _drillDownLevel, value, "DrillDownLevel");
            }
        }

        public string AggregateOn
        {
            get { return _aggregateOn; }
            set
            {
                SetProperty(ref _aggregateOn, value, "AggregateOn");
            }
        }

        public AggregateType AggregateType
        {
            get { return _aggregateType; }
            set
            {
                SetProperty(ref _aggregateType, value, "AggregateType");
                UpdateAggregate();
            }
        }

        public Stack<DrillDownHistory> History
        { get; set; }

        public List<string> GroupNames
        {
            get { return _groupNames; }
            protected set
            {
                SetProperty(ref _groupNames, value, "GroupNames");
            }
        }

        #endregion

        #region Events
        public event EventHandler<DrillDownEventArgs> BeforeDrill;
        public event EventHandler<DrillDownEventArgs> AfterDrill;

        #endregion

        #region Public Methods

        public DrillDownBase(IEnumerable<object> itemsSource, string drillDownPath)
        {
            _sourceCollection = itemsSource;
            _drillDownPath = drillDownPath;
        }

        public void DrillDown(int index)
        {
            var beforeDrill = OnBeforeDrill(_currentPath, DrillDownLevel, true);
            if (beforeDrill)
            {
                PerformDrillDown(index);
                OnAfterDrill(_currentPath, DrillDownLevel, true);
            }
        }

        public void DrillDown(IEnumerable<object> itemsSource)
        {
            var beforeDrill = OnBeforeDrill(_currentPath, DrillDownLevel, true);
            if (beforeDrill)
            {
                PerformDrillDown(itemsSource);
                OnAfterDrill(_currentPath, DrillDownLevel, true);
            }
        }

        public void DrillUp()
        {
            var beforeDrill = OnBeforeDrill(_currentPath, DrillDownLevel, false);
            if (beforeDrill)
            {
                PerformDrillUp();
                OnAfterDrill(_currentPath, DrillDownLevel, false);
            }
        }

        public void DrillUp(int level)
        {
            var beforeDrill = OnBeforeDrill(_currentPath, DrillDownLevel, false);
            if (beforeDrill)
            {
                while (DrillDownLevel != level)
                {
                    PerformDrillUp();
                }
                OnAfterDrill(_currentPath, DrillDownLevel, false);
            }
        }

        public void DrillUp(string path)
        {
            var beforeDrill = OnBeforeDrill(_currentPath, DrillDownLevel, false);
            if (beforeDrill)
            {
                while (_currentPath != path)
                {
                    PerformDrillUp();
                }
            }
            OnAfterDrill(_currentPath, DrillDownLevel, false);
        }

        public abstract void Refresh();
        #endregion

        #region Methods

        protected abstract void UpdateAggregate();
  
        protected abstract void PerformDrillDown(int index);
        protected abstract void PerformDrillDown(IEnumerable<object> itemsSource);
        protected abstract void PerformDrillUp();
        protected void OnAfterDrill(string path, int level, bool isDrillDown)
        {
            if (AfterDrill != null)
            {
                var args = new DrillDownEventArgs(path, level, isDrillDown);
                AfterDrill(this, args);
            }
        }
        protected bool OnBeforeDrill(string path, int level, bool isDrillDown)
        {
            if (BeforeDrill != null)
            {
                var args = new DrillDownEventArgs(path, level, isDrillDown);
                BeforeDrill(this, args);
                return !args.Cancel;
            }
            else
                return true;
        }

        #endregion
    }

    public class DrillDownHistory
    {
        public string[] DrillDownPath { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public object View { get; set; }
        public object Parent { get; set; }

    }

    public class DrillDownEventArgs : CancelEventArgs
    {
        public string DrillDownPath { get; private set; }

        public int DrillDownLevel { get; private set; }

        public bool IsDrillDown { get; private set; }

        internal DrillDownEventArgs(string path, int level, bool isDrillDown)
        {
            DrillDownPath = path;
            DrillDownLevel = level;
            IsDrillDown = isDrillDown;
        }

    }
}
