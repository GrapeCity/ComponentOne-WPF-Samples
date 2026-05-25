using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Input;
using System.Data.Services.Client;
using System.Windows;

namespace DataViewDashboard.ViewModels
{
    public class OrdersViewModel : DataViewModelBase
    {
        public OrdersViewModel(AdventureWorksService.AdventureWorksEntities client)
            : base(client.SalesOrderHeaders.Expand("SalesOrderDetails/Product,Customer"))
        {
            base.DisplayName = "Orders";
        }
        protected override void OnAsyncQueryCompleted(IAsyncResult result)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                var orders = new List<AdventureWorksService.SalesOrderHeader>();
                foreach (var item in _query.EndExecute(result))
                {
                    orders.Add((AdventureWorksService.SalesOrderHeader)item);
                }
                ProxyServiceQuery.AddData(_query.ToString(), orders);
                OnCompleteSource(orders);
            });
        }
    }
}
