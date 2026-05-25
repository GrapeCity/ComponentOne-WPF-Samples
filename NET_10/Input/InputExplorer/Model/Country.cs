using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace InputExplorer
{
    public class Country
    {
        public override string ToString()
        {
            return Name;
        }

        public string Code { get; set; }
        public string Name { get; set; }

        private static List<Country> _countries;
        public static List<Country> GetCountries()
        {
            if (_countries == null) 
            {
                var assembly = typeof(Country).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream("InputExplorer.Data.countries.json");
                var json = new System.IO.StreamReader(stream).ReadToEnd();
                var countries = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                _countries = countries.Select(item => new Country
                {
                    Code = item.Key,
                    Name = item.Value
                }).OrderBy(c => c.Name).ToList();
            }
            return _countries.ToList();
        }

        public string FlagUrl
        {
            get
            {
                return $"https://raw.githubusercontent.com/GrapeCity/ComponentOne-Blazor-Samples/master/Images/samples/flags/{Code}.png";
            }
        }
    }
}
