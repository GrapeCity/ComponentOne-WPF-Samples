using System.Runtime.Serialization;

namespace FinancialChartExplorer
{
    [DataContract]
    public class Quote
    {
        [DataMember(Name = "date")]
        public string Date { get; set; }

        [DataMember(Name = "high")]
        public double High { get; set; }

        [DataMember(Name = "low")]
        public double Low { get; set; }

        [DataMember(Name = "open")]
        public double Open { get; set; }

        [DataMember(Name = "close")]
        public double Close { get; set; }

        [DataMember(Name = "volume")]
        public double Volume { get; set; }

        public System.DateTime date
        {
            get { return System.DateTime.ParseExact(Date.ToString(), "MM/dd/yy", System.Globalization.CultureInfo.InvariantCulture); }
        }
    }

    public class Company
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
