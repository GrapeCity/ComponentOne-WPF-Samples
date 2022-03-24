using System;

namespace FlexChartMvvmDemo.Model
{
    public class OrderModel
    {
        public int ID
        {
            get; set;
        }

        public DateTime Date
        {
            get; set;
        }

        public string Category
        {
            get;
            set;
        }

        public string Country
        {
            get;
            set;
        }

        public string Year
        {
            get;
            set;
        }

        public double Amount
        {
            get;
            set;
        }
    }
}
