using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace C1DataGrid_ComboCols2010
{
    public static class Data
    {
        // generate the data for the data grid
        public static List<UserInfo> GenerateData()
        {
            var rnd = new Random(0);

            // generate countries & regions
            new Country("Argentina", "Buenos Aires", "Mendoza", "Salta");
            new Country("Brazil", "Rio do Janeiro", "Porto Alegre", "Sao Paulo");
            new Country("France", "Paris", "Marselle", "Lyon");
            new Country("Italy", "Genoa", "Milano", "Florencia");
            new Country("USA", "NY", "California", "San Francisco");


            // generate valid userinfo data
            var data = new List<UserInfo>();
            for (int i = 0; i < 30; i++)
            {
                // random country
                var countryId = rnd.Next(Country.AllCountries.Count);

                // select first region from that country
                var regionId = Country.GetCountryById(countryId).Regions[0].Id;

                // create UserInfo
                data.Add(new UserInfo()
                {
                    Name = string.Format("UserInfo{0}", i.ToString()),
                    CountryId = countryId,
                    RegionId = regionId
                });
            }

            return data;
        }
    }

    // object bound to the DataGrid are instances of this class
    public class UserInfo
        : INotifyPropertyChanged
    {
        #region Properties

        private int _countryId;
        public int CountryId
        {
            get { return _countryId; }
            set
            {
                if (value != _countryId)
                {
                    // new country
                    _countryId = value;

                    // regionid is not longer valid for this country
                    // set the first valid region id, instead
                    var country = Country.GetCountryById(value);
                    var regionIdsForThisCountry = from r
                                                  in country.Regions
                                                  select r.Id;
                    if (!regionIdsForThisCountry.Contains(RegionId))
                    {
                        RegionId = country.Regions[0].Id;
                    }

                    // fires property changed
                    OnPropertyChanged("CountryId");
                }
            }
        }

        private int _regionId;
        public int RegionId
        {
            get { return _regionId; }
            set { _regionId = value; OnPropertyChanged("RegionId"); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    // Region class
    public class Region
    {
        #region Static selectors

        // save here all the regions
        public static List<Region> AllRegions = new List<Region>();

        #endregion

        #region Constructor

        public Region()
        {
            Id = AllRegions.Count;
            AllRegions.Add(this);
        }

        public Region(string name)
            : this()
        {
            Name = name;
        }

        #endregion

        public int Id { get; private set; }
        public string Name { get; private set; }
    }

    // Country class
    public class Country
    {
        #region Static selectors

        // store here all the countries
        public static List<Country> AllCountries = new List<Country>();

        public static Country GetCountryById(int countryId)
        {
            return (from c
                    in Country.AllCountries
                    where c.Id == countryId
                    select c).First();
        }

        #endregion

        #region Constructor

        public Country()
        {
            Id = AllCountries.Count;
            AllCountries.Add(this);

            Name = string.Empty;
        }

        public Country(string name, params string[] regionNames)
            : this()
        {
            Name = name;

            _regions = new List<Region>();
            foreach (var regionName in regionNames)
            {
                Regions.Add(new Region(regionName));
            }
        }

        #endregion


        public int Id { get; private set; }
        public string Name { get; private set; }

        public List<Region> _regions = new List<Region>();
        public List<Region> Regions
        {
            get
            {
                return _regions;
            }
        }
    }
}
