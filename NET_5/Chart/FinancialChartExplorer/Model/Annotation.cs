using System.Runtime.Serialization;

namespace FinancialChartExplorer
{
    [DataContract]
    public class Annotation
    {
        [DataMember(Name = "title")]
        public string Title
        {
            get;set;
        }

        [DataMember(Name = "description")]
        public string Description
        {
            get; set;
        }

        [DataMember(Name = "publicationDate")]
        public string PublicationDate
        {
            get; set;
        }

        [DataMember(Name = "dataIndex")]
        public int DataIndex
        {
            get;set;
        }
    }

    public class Range
    {
        public double Min { get; set; }
        public double Max { get; set; }
    }
}
