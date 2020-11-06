using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace WealthHealth
{
    [DataContract]
    public class Country
    {
        private double _yearIncome;
        private double _yearLifeExpectancy;
        private double _yearPopulation;

        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "region")]
        public string Region { get; set; }
        [DataMember(Name = "income")]
        public List<object> Income { get; set; }
        [DataMember(Name = "population")]
        public List<object> Population { get; set; }
        [DataMember(Name= "lifeExpectancy")]
        public List<object> LifeExpectancy { get; set; }

        public double YearIncome
        {
            get
            {
                return _yearIncome;
            }
            set
            {
                if (_yearIncome != value)
                {
                    _yearIncome = value;
                }
            }
        }
        public double YearLifeExpectancy
        {
            get
            {
                return _yearLifeExpectancy;
            }
            set
            {
                if (_yearLifeExpectancy != value)
                {
                    _yearLifeExpectancy = value;
                }
            }
        }

        public double YearPopulation
        {
            get
            {
                return _yearPopulation;
            }
            set
            {
                if (_yearPopulation != value)
                {
                    _yearPopulation = value;
                }
            }
        }
    }

    public class DataItem
    {
        public double Year { get; set; }
        public double Value { get; set; }
    }
}
