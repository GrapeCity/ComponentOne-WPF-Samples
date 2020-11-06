using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FlexChartMvvmDemo.Model;
using FlexChartMvvmDemo.Services;

namespace FlexChartMvvmDemo.ViewModel
{
    public class OrdersViewModel : INotifyPropertyChanged
    {
        private string _group = "Country";
        private string _agg = "Sum";
        private int _selectedIndex = -1;

        public OrdersViewModel()
        {
            Orders = new ObservableCollection<OrderModel>();

            OrderService = new OrderService();
            UpdateOrders();
        }

        public ObservableCollection<OrderModel> Orders
        {
            get;
            set;
        }

        public IOrderService OrderService
        {
            get;
            set;
        }

        public string Group
        {
            get { return _group; }
            set
            {
                _group = value;
                UpdateOrders();
                OnPropertyChanged("Group");
            }
        }

        public string[] Groups
        {
            get
            {
                return "Country,Category,Year".Split(',');
            }
        }

        public string Aggregate
        {
            get { return _agg; }
            set
            {
                _agg = value;
                UpdateOrders();
                OnPropertyChanged("Aggregate");
            }
        }

        public string[] Aggregates
        {
            get
            {
                return "Sum,Count,Average".Split(',');
            }
        }

        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateOrders()
        {
            var list = new List<OrderModel>();
            OrderService.FetchOrders(list);

            var group = list.GroupBy(Selector).Select(
                go => new OrderModel {
                    Country = Group == "Country" ? go.First().Country : null,
                    Category = Group == "Category"? go.First().Category : null,
                    Year = Group == "Year" ? go.First().Date.Year.ToString() : null,
                    Amount = Aggregator(go) 
                });

            Orders.Clear();
            foreach (var o in group)
                Orders.Add(o);
        }

        double Aggregator(IGrouping<string, OrderModel> group)
        {
            var total = 0.0;
            if (Aggregate == "Sum")
                total = group.Sum(o => o.Amount);
            else if (Aggregate == "Count")
                total = group.Count();
            else if (Aggregate == "Average")
                total = group.Average(o => o.Amount);

            return total;
        }

        string Selector(OrderModel order)
        {
            string s = null;
            if (Group == "Category")
                s = order.Category;
            else if (Group == "Country")
                s = order.Country;
            else if (Group == "Year")
                s = order.Date.Year.ToString();
            return s;
        }
    }
}
