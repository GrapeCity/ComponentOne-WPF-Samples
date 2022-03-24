using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Input;
using System.Data.Services.Client;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DataViewDashboard.ViewModels
{
    public class CustomersViewModel : DataViewModelBase
    {
        public CustomersViewModel(AdventureWorksService.AdventureWorksEntities client)
            : base(client.Customers.Expand("CustomerAddresses/Address,SalesOrderHeaders/SalesOrderDetails/Product"))
        {
            base.DisplayName = "Customers";
        }
        protected override void OnAsyncQueryCompleted(IAsyncResult result)
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                var customers = new List<AdventureWorksService.Customer>();
                foreach (var item in _query.EndExecute(result))
                {
                    customers.Add((AdventureWorksService.Customer)item);
                }
                ProxyServiceQuery.AddData(_query.ToString(), customers);
                OnCompleteSource(customers);
            });
        }     
    }
}
