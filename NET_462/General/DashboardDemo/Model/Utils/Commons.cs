using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.IO;

#if WINDOWS_UWP
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using Point = Windows.Foundation.Point;
using Image = Windows.UI.Xaml.Media.Imaging.BitmapImage;
#elif WPF
using Point = System.Windows.Point;
using System.Windows.Media.Imaging;
using Image = System.Windows.Media.Imaging.BitmapImage;
#elif WinForms
using Point = System.Windows.Point;
using System.Drawing;
#elif XF
using Xamarin.Forms;
using Image = Xamarin.Forms.ImageSource;
#else
using Point = System.Drawing.Point;
using System.Drawing;
#endif

namespace DashboardModel
{
    public class ProductsPageViewModel
    {
        public List<ProductItem> ProductAllItemSource { get { return DataService.GetService().GetProductItemList(CategoryType.All); } }
        public List<ProductItem> ProductSportsItemSource { get { return DataService.GetService().GetProductItemList(CategoryType.Sports); } }
        public List<ProductItem> ProductCityItemSource { get { return DataService.GetService().GetProductItemList(CategoryType.City); } }
        public List<ProductItem> ProductMultiUtilityItemSource { get { return DataService.GetService().GetProductItemList(CategoryType.MultiUtility); } }
        public List<ProductItem> ProductNewEntryItemSource { get { return DataService.GetService().GetProductItemList(CategoryType.NewEntry); } }
    }

    interface IUpdatablePage
    {
        void UpdataPage();
    }

    public class DateRangeChangedEventArgs : EventArgs
    {
        public DateRange NewValue { get; private set; }

        public DateRangeChangedEventArgs(DateRange dateRange) : base()
        {
            NewValue = dateRange;
        }
    }

    public delegate void DateRangeChangedEvent(object sender, DateRangeChangedEventArgs e);

    public enum CategoryType
    {
        All,
        City,
        MultiUtility,
        Sports,
        NewEntry,
    }

    public enum CampainTaskType
    {
        All,
        InProgress,
        Completed,
        Deferred,
        Urgent
    }

    public enum OpportunityLevel
    {
        High,
        Medium,
        Low,
        Unlikely
    }

    public class DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public class SaleGoalItem
    {
        public string Name { get; set; }
        public double Sales { get; set; }
        public double Goal { get; set; }
        public double Profit { get; set; }
        public double CompletePrecent { get { return Sales / Goal; } }
            
    }
    public class SaleItem
    {
        public string Name { get; set; }
        public double Sales { get; set; }
        public double Profit { get; set; }
        public double Id { get; set; }

        public DateTime Date { get; set; }

        public string Month { get { return Date.ToString("MM/yyyy"); } }
    }
    public class RegionSaleItem : SaleItem
    {
        public Point Locat { get; set; }
    }
    public class ProductItem
    {

        public string Label { get; set; }

        public Image ImageSource { get; set; }

        public ProductItem(Product product)
        {
            Label = string.Format(" {0} | {1} | {2:C} ", product.ToString(), product.Category.ToString(), product.Cost.ToString());
#if WINDOWS_UWP || WPF
            ImageSource = new Image();
#endif

#if WINDOWS_UWP
            LoadImageAsync(product);
#elif XF
            ImageSource = ImageSource.FromStream(() => new MemoryStream(product.Image));
#else
            using (var stream = new MemoryStream(product.Image))
            {
#if WPF
                ImageSource.BeginInit();
                ImageSource.CacheOption = BitmapCacheOption.OnLoad;
                ImageSource.StreamSource = stream;
                ImageSource.EndInit();
#else
                ImageSource = Image.FromStream(stream);
#endif
            }
#endif
        }

#if WINDOWS_UWP
        async void LoadImageAsync(Product product)
        {
            ImageSource = new Image();
            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(product.Image.AsBuffer());
                stream.Seek(0);
                await ImageSource.SetSourceAsync(stream);
            }
        }
#endif
    }
    public class CampainTaskItem
    {
        public string Subject { get; set; }
        public string AssignedTo { get; set; }
        public string ItemName { get; set; }
        public DateTime DueDate { get; set; }
        public double Complete { get; set; }

        Sale saleData;

        internal bool Deferred
        {
            get { return saleData.Date > DueDate; }
        }
        internal bool Urgent
        {
            get { return Math.Abs((saleData.Date - saleData.Campaign.Start).Days) < 3; }
        }

        public CampainTaskItem(Sale sale)
        {
            saleData = sale;
            Subject = sale.Campaign.Name;
            AssignedTo = sale.Customer.Name;
            ItemName = sale.Product.Name;
            DueDate = sale.Campaign.Finish;

            DateTime baseTime = new DateTime(2017, 10, 1);

            double total = (sale.Campaign.Finish - sale.Campaign.Start).Days;
            double current = (baseTime - sale.Campaign.Start).Days;
            Complete = Math.Round(Math.Min(1, Math.Max(0, current / total)), 2);
        }
    }
    public class CurrentPriorBudgetItem
    {
        public double Sales { get; set; }
        public double Profit { get; set; }
        public double ProirProfit { get; set; }
        public double ProirSales { get; set; }

        public DateTime Date { get; set; }
    }
    public class ProductsWiseSaleItem
    {
        public int ID { get; set; }

        public double Sale { get; set; }
        public double Quantity { get; set; }

        public string Category { get; set; }
        public string Product { get; set; }
        public string Region { get; set; }

        public DateTime Date { get; set; }
    }
    public class Opportunity
    {
        public OpportunityLevel Level { get; set; }
        public double Sales { get; set; }
    }
}