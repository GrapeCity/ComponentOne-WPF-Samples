using System.Collections.Generic;

using FlexChartMvvmDemo.Model;

namespace FlexChartMvvmDemo.Services
{
    public interface IOrderService
    {
        void FetchOrders(IList<OrderModel> list);
    }
}
