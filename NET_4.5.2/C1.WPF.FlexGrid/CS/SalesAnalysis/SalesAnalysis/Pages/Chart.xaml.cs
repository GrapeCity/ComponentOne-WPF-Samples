using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using C1.WPF.C1Chart;
namespace SalesAnalysis
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        private DataServiceRef.NORTHWNDEntities dataServiceClient;
        private System.Data.Services.Client.DataServiceQuery<DataServiceRef.Invoice> nwndQuery;
       
        public Chart()
        {
            InitializeComponent();
            _chartByCountry.ChartType = ChartType.Line;
            _chartByProduct.ChartType = ChartType.Line;
            dataServiceClient = new DataServiceRef.NORTHWNDEntities(new Uri("http://localhost:50297/NorthWindDataService.svc/"));
            nwndQuery = dataServiceClient.Invoices;
            #region DataForCountry-Sales Chart
            var cntryslschrtdt = GetSalesByCountry(nwndQuery.ToArray().ToList());
            DataSeries amtsers = new DataSeries();
            amtsers.ValuesSource = cntryslschrtdt.ConvertAll(e => e.Amount).ToArray();
            _chartByCountry.Data.Children.Add(amtsers);
            var xvals = cntryslschrtdt.ConvertAll(e => e.Item).Distinct().ToArray();
            _chartByCountry.Data.ItemNames = xvals;
            _chartByCountry.View.AxisX.Title = CreateTextBlock("Countries");
            _chartByCountry.View.AxisX.AnnoAngle = 45;
            _chartByCountry.View.AxisY.Title = CreateTextBlock("Sales");  
            #endregion

            #region DataForProduct-Sales Chart
            var pdctslchrtdt = GetSalesByProduct(nwndQuery.ToArray().ToList());
            DataSeries amtserspd = new DataSeries();
            amtserspd.ValuesSource = pdctslchrtdt.ConvertAll(e => e.Amount).ToArray();
            _chartByProduct.Data.Children.Add(amtserspd);
            var xvalspd = pdctslchrtdt.ConvertAll(e => e.Item).Distinct().ToArray();
            _chartByProduct.Data.ItemNames = xvalspd;
            _chartByProduct.View.AxisX.Title = CreateTextBlock("Product");
            _chartByProduct.View.AxisX.AnnoAngle = 45;
            _chartByProduct.View.AxisY.Title = CreateTextBlock("Sales");   
            #endregion
          
        }
        // get sales by country
        // the data is grouped, aggregated, and sorted and first 15 records are returned
        // the final result is all that is sent to the client (only 15 records)
        public List<ChartDataPoint> GetSalesByCountry(List<DataServiceRef.Invoice> listinv)
        {

           var grpd=from inv in listinv
                    group inv by inv.Country into g 
                    let amount = g.Sum(inv => inv.ExtendedPrice)
                    orderby amount descending
                    select new ChartDataPoint() { Item = g.Key + " ", Amount = amount };
           return grpd.Take(15).ToList();
        }

        public List<ChartDataPoint> GetSalesByProduct(List<DataServiceRef.Invoice> listinv)
        {
            var grpd = from inv in listinv
                       group inv by inv.ProductName into g
                       let amount = g.Sum(inv => inv.ExtendedPrice)
                       orderby amount descending
                       select new ChartDataPoint() { Item = g.Key + " ", Amount = amount };
            return grpd.Take(15).ToList();
        }
        /// <summary>
        /// Method to create the Textblock
        /// That will contain the axes title
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        internal TextBlock CreateTextBlock(string txt)
        {
            TextBlock txtblk = new TextBlock
            {
                Text = txt,
                TextAlignment=TextAlignment.Center,
                FontSize = 20,
                FontWeight = FontWeights.Bold
                
            };

            return txtblk;
        }
    }
}
