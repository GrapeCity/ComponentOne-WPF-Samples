using C1.Chart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DataManipulation
{
    public class TopNViewModel : ViewModelBase<TopNFilter>
    {
        public TopNViewModel()
        {
            Filter.Bindings = new string[] { "Shipments" };
            Filter.AggregateType = AggregateType.Sum;
            Filter.SortType = SortType.Descending;
            Filter.SortOrder = true;
            Queue<SmartPhoneVendor> vendors = new Queue<SmartPhoneVendor>();

            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Alcatel",
                Shipments = 34.1
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Apple",
                Shipments = 215.2
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Huawei",
                Shipments = 139
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Lenovo",
                Shipments = 50.7
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "LG",
                Shipments = 55.1
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Oppo",
                Shipments = 92.5
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Samsung",
                Shipments = 310.3
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Vivo",
                Shipments = 71.7
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "Xiaomi",
                Shipments = 61
            });
            vendors.Enqueue(new SmartPhoneVendor()
            {
                Name = "ZTE",
                Shipments = 61.9
            });
            
            this.Source = vendors;
        }

        public override TopNFilter Filter
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

        private Dictionary<string, int> _topNCounts;
        public Dictionary<string, int> TopNCounts
        {
            get
            {
                if (_topNCounts == null)
                {
                    _topNCounts = new Dictionary<string, int>();
                    _topNCounts.Add("Show all", -1);
                    for (int i = 0; i < 9; i++)
                    {
                        _topNCounts.Add("Top: " + (i + 1), i + 1);
                    }
                }
                return _topNCounts;
            }
        }
        

        #endregion Selection Data
        public int TopNCount
        {
            get { return Filter.TopNCount; }
            set { Filter.TopNCount = value; Filter.Analyse(); }
        }
        public bool ShowOthers
        {
            get { return Filter.ShowOthers; }
            set { Filter.ShowOthers = value; Filter.Analyse(); }
        }



    }
}
