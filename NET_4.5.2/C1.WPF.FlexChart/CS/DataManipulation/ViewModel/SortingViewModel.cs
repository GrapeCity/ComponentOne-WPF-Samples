using C1.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataManipulation
{
    class SortingViewModel: ViewModelBase<SortingFilter>
    {

        static Random r = new Random();
        public SortingViewModel()
        {
            Filter.Bindings = new string[] { "PredictiveValue" };
            Filter.AggregateType = AggregateType.Sum;
            Queue<SortingItem> sis = new Queue<SortingItem>();
            
            sis.Enqueue(
                   new SortingItem()
                   {
                       Name = "Mechanical",
                       PredictiveValue = 230,
                       ActualValue = 226,
                   });
            sis.Enqueue(
                   new SortingItem()
                   {
                       Name = "3C",
                       PredictiveValue = 960,
                       ActualValue = 870,
                   });
            sis.Enqueue(
                   new SortingItem()
                   {
                       Name = "Medicinal",
                       PredictiveValue = 520,
                       ActualValue = 500,
                   });
            sis.Enqueue(
                   new SortingItem()
                   {
                       Name = "Appliances",
                       PredictiveValue = 370,
                       ActualValue = 300,
                   });
            sis.Enqueue(
                   new SortingItem()
                   {
                       Name = "Software",
                       PredictiveValue = 320,
                       ActualValue = 120,
                   });
            sis.Enqueue(
                   new SortingItem()
                   {
                       Name = "Cosmetic",
                       PredictiveValue = 780,
                       ActualValue = 700,
                   });

            this.Source = sis;
        }

        public override SortingFilter Filter
        {
            get
            {
                return base.Filter;
            }
            set
            {
                base.Filter = value;
            }
        }

        #region Selection Data
        
        
        private Dictionary<string, string> _sortBySource;
        public Dictionary<string, string> SortBySource
        {
            get
            {
                if (_sortBySource == null)
                {
                    _sortBySource = new Dictionary<string, string>();

                    _sortBySource.Add("None", "none");
                    _sortBySource.Add("Descending by PredictiveValue", "dbp");
                    _sortBySource.Add("Ascending by PredictiveValue", "abp");
                    _sortBySource.Add("Descending by ActualValue", "dba");
                    _sortBySource.Add("Ascending by ActualValue", "aba");
                    _sortBySource.Add("Descending by Difference", "dbd");
                    _sortBySource.Add("Ascending by Difference", "abd");
                }
                return _sortBySource;
            }
        }
        

        #endregion Selection Data


        private string _sortBy = "none";
        public string  SortBy
        {
            get
            {
                return _sortBy;
            }
            set
            {
                _sortBy = value;
                
                switch (_sortBy)
                {
                    case "dba":
                        Filter.Bindings = new string[] { "ActualValue" };
                        Filter.AggregateType = AggregateType.Sum;
                        Filter.CustomSortFun = null;
                        Filter.SortType = SortType.Descending;
                        break;
                    case "aba":
                        Filter.Bindings = new string[] { "ActualValue" };
                        Filter.AggregateType = AggregateType.Sum;
                        Filter.CustomSortFun = null;
                        Filter.SortType = SortType.Ascending;
                        break;
                    case "dbp":
                        Filter.Bindings = new string[] { "PredictiveValue" };
                        Filter.AggregateType = AggregateType.Sum;
                        Filter.CustomSortFun = null;
                        Filter.SortType = SortType.Descending;
                        break;
                    case "abp":
                        Filter.Bindings = new string[] { "PredictiveValue" };
                        Filter.AggregateType = AggregateType.Sum;
                        Filter.CustomSortFun = null;
                        Filter.SortType = SortType.Ascending;
                        break;
                    case "dbd":
                        Filter.Bindings = new string[] { "PredictiveValue", "ActualValue" };
                        Filter.CustomSortFun = k => k.First() - k.Last();
                        Filter.SortType = SortType.Descending;
                        break;
                    case "abd":
                        Filter.Bindings = new string[] { "PredictiveValue", "ActualValue" };
                        Filter.CustomSortFun = k => k.First() - k.Last();
                        Filter.SortType = SortType.Ascending;
                        break;
                    case "none":
                    default:
                        Filter.Bindings = new string[] { "PredictiveValue"};
                        Filter.SortType = SortType.None;
                        Filter.CustomSortFun = null;
                        break;
                }



            }
        }
    }
}
