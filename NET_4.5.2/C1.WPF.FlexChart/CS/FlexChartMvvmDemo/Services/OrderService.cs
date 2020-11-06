using System;
using System.Collections.Generic;
using FlexChartMvvmDemo.Model;

namespace FlexChartMvvmDemo.Services
{
    public class OrderService : IOrderService
    {
        List<OrderModel> _list;

        public void FetchOrders( IList<OrderModel> list)
        {
            if (_list == null)
            {
                _list = new List<OrderModel>();
                for (var i = 0; i < 200; i++)
                    _list.Add(GenerateOrder(i + 1));
            }
            foreach(var o in _list)
                list.Add(o);
        }

        Random rnd = new Random();

        private OrderModel GenerateOrder(int i)
        {
            var countries = "France,Spain,UK,USA".Split(',');
            var categories = "Electronics,Audio,Video".Split(',');

            return new OrderModel()
            {
                ID = i,
                Date = new DateTime(2014,1,1).AddDays(i*5),
                Country = countries[rnd.Next(countries.Length)],
                Category = categories[rnd.Next(categories.Length)],
                Amount = rnd.Next(1000)
            };
        }
    }
}
