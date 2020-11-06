using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;

namespace WealthHealth
{
    public class DataService
    {
        List<Country> _countries;
        CountryCompare _compare = new CountryCompare();

        public List<Country> CreateData()
        {
            if (_countries == null)
            {
                string path = string.Format("WealthHealth.{0}.json", "nations");
                Assembly asm = this.GetType().Assembly;
                using (var stream = asm.GetManifestResourceStream(path))
                {
                    var ser = new DataContractJsonSerializer(typeof(List<Country>));
                    _countries = (List<Country>)ser.ReadObject(stream);
                }
            }
            for (var i = _countries.Count - 1; i >=0; i--)
            {
                var country = _countries[i];
                if (country.Population.Count <= 1 || country.Income.Count <= 1 || country.LifeExpectancy.Count <= 1)
                    _countries.Remove(country);
            }
            _countries.Sort(0, _countries.Count, _compare);
            return _countries;
        }

        public List<Country> UpdateData(double year)
        {
            var newCountries = new List<Country>();
            _countries.ForEach(country => 
            {
                country.YearIncome = Interpolate(country.Income, year);
                country.YearLifeExpectancy = Interpolate(country.LifeExpectancy, year);
                var pop = Interpolate(country.Population, year);
                if (pop > 1000000)
                    pop = Math.Round(pop / 1000000) * 1000000;
                country.YearPopulation = pop;
                newCountries.Add(country);
            });
            newCountries.Sort(0, newCountries.Count, _compare);
            _countries = newCountries;

            return newCountries;
        }

        public double Interpolate(List<object> list, double year)
        {
            int min = 0, max = list.Count - 1, cur;
            while (min <= max)
            {
                cur = (min + max) >> 1;
                var item = (object[])list[cur];
                if (ToDouble(item[0]) > year)
                {
                    max = cur - 1;
                }
                else if (ToDouble(item[0]) < year)
                {
                    min = cur + 1;
                }
                else
                {
                    return ToDouble(item[1]);
                }
            }

            if (min == 0)
                return ToDouble(((object[])list[min])[1]);
            if (min == list.Count)
                return ToDouble(((object[])list[max])[1]);

            var pct = (year - ToDouble(((object[])list[max])[0])) / (ToDouble(((object[])list[min])[0]) - ToDouble(((object[])list[max])[0]));
            return ToDouble(((object[])list[max])[1]) + pct * (ToDouble(((object[])list[min])[1]) - ToDouble(((object[])list[max])[1]));
        }

        private double ToDouble(object obj)
        {
            return double.Parse(obj.ToString());
        }
    }

    public class CountryCompare : IComparer<Country>
    {
        public int Compare(Country x, Country y)
        {
            if (x.YearPopulation > y.YearPopulation)
                return -1;
            else if (x.YearPopulation == y.YearPopulation)
                return 0;
            else
                return 1;
        }
    }
}
