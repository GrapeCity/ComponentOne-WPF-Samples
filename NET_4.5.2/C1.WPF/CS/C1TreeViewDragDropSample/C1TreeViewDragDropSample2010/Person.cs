using System;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace C1TreeViewDragDropSample2010
{
    public class Person
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Address { get; set; }

        public string Residence; //{ get; set; }

        public string Born; //{ get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}, {2}", Name, Age, Address);
        }

        #region Data Generator

        public static ObservableCollection<Person> Generate(int qty)
        {
            Random Randomizer = new Random();

            var items = new ObservableCollection<Person>();
            for (int i = 0; i < qty; i++)
            {
                items.Add(new Person()
                {
                    Name = Names[Randomizer.Next(Names.Length)] + " " + Surnames[Randomizer.Next(Surnames.Length)],
                    Address = Streets[Randomizer.Next(Streets.Length)] + " " + (1000 + Randomizer.Next(8999)).ToString(),
                    Age = 30 + Randomizer.Next(10),
                    Residence = Countries[Randomizer.Next(Countries.Length)],
                    Born = Countries[Randomizer.Next(Countries.Length)],
                });
            }
            return items;
        }

        public static string[] Streets = new string[] 
        {
            "5th Avenue",
            "10th Avenue",
            "13th Street",
            "Solano García",
            "Fray Bentos",
            "Lois Albert Herrera",
            "Brasil Avenue",
            "Kenedy Avenue",
            "Peter Campbell",
            "Missisipi"
        };

        public static string[] Names = new string[] 
        {
            "Michael",
            "John",
            "Bernardo",
            "Alvaro",
            "Max",
            "Leonard",
            "Noela",
            "Gabriel",
            "Bruno",
            "Rodrigo",
            "Sheela",
            "Chris",
            "Martin",
            "Ben",
            "Alex",
            "Irina",
            "Dave",
            "Patric",
        };

        public static string[] Surnames = new string[] 
        {
            "Johnson",
            "Alvarez",
            "Maxtor",
            "Johansen",
            "Vera",
            "García",
            "Days",
            "Castle",
            "Varela",
            "Smith",
            "Gates",
            "Meredith",
            "Drexler",
            "Beckham",
            "Jobs",
        };

        public static string[] Countries = new string[] 
        {
            "Afghanistan", 
            "Albania",
            "Algeria",
            "Andorra",
            "Angola",
            "Argentina",
            "Armenia",
            "Australia",
            "Austria",
            "Azerbaijan",
            "Bahrain",
            "Bangladesh",
            "Barbados",
            "Belarus",
            "Belgium",
            "Belize",
            "Benin",
            "Bhutan",
            "Bolivia",
            "Botswana",
            "Brazil",
            "Brunei",
            "Bulgaria",
            "Burkina Faso",
            "Burundi",
            "Côte d'Ivoire",
            "Cambodia",
            "Cameroon",
            "Canada",
            "Cape Verde",
            "Chad",
            "Chile",
            "China",
            "Colombia",
            "Comoros",
            "Congo",
            "Costa Rica",
            "Croatia",
            "Cuba",
            "Cyprus",
            "Czech Republic",
            "Denmark",
            "Djibouti",
            "Dominica",
            "Dominican Republic",
            "Ecuador",
            "Egypt",
            "El Salvador",
            "Equatorial Guinea",
            "Eritrea",
            "Estonia",
            "Ethiopia",
            "Fiji",
            "Finland",
            "France",
            "Gabon",
            "Georgia",
            "Germany",
            "Ghana",
            "Greece",
            "Grenada",
            "Guatemala",
            "Guinea",
            "Guinea-Bissau",
            "Guyana",
            "Haiti",
            "Honduras",
            "Hungary",
            "Iceland",
            "India",
            "Indonesia",
            "Iran",
            "Iraq",
            "Ireland",
            "Israel",
            "Italy",
            "Jamaica",
            "Japan",
            "Jordan",
            "Kazakhstan",
            "Kenya",
            "Kiribati",
            "Kuwait",
            "Kyrgyzstan",
            "Laos",
            "Latvia",
            "Lebanon",
            "Lesotho",
            "Liberia",
            "Libya",
            "Liechtenstein",
            "Lithuania",
            "Luxembourg ",
            "Macau -- ",
            "Madagascar",
            "Malawi",
            "Malaysia",
            "Maldives",
            "Mali",
            "Malta",
            "Marshall Islands",
            "Mauritania",
            "Mauritius",
            "Mexico",
            "Micronesia",
            "Moldova",
            "Monaco",
            "Mongolia",
            "Morocco",
            "Mozambique",
            "Myanmar",
            "Namibia",
            "Nauru",
            "Nepal",
            "Netherlands",
            "New Zealand",
            "Nicaragua",
            "Niger",
            "Nigeria",
            "North Korea",
            "Norway",
            "Oman",
            "Pakistan",
            "Palau",
            "Panama",
            "Papua New Guinea",
            "Paraguay",
            "Peru",
            "Philippines",
            "Poland",
            "Portugal",
            "Qatar",
            "Romania",
            "Russia",
            "Rwanda",
            "São Tomé and Príncipe",
            "Saint Kitts and Nevis",
            "Saint Lucia",
            "Samoa",
            "San Marino",
            "Saudi Arabia",
            "Senegal",
            "Seychelles",
            "Sierra Leone",
            "Singapore",
            "Slovakia",
            "Slovenia",
            "Solomon Islands",
            "Somalia",
            "South Africa",
            "South Korea",
            "Spain",
            "Sri Lanka",
            "Sudan",
            "Suriname",
            "Swaziland",
            "Sweden",
            "Switzerland",
            "Syria",
            "Taiwan",
            "Tajikistan",
            "Tanzania",
            "Thailand",
            "The Bahamas",
            "The Gambia",
            "Togo",
            "Tonga",
            "Trinidad and Tobago",
            "Tunisia",
            "Turkey",
            "Turkmenistan",
            "Tuvalu",
            "Uganda",
            "Ukraine",
            "United Arab Emirates",
            "United Kingdom",
            "United States",
            "Uruguay",
            "Uzbekistan",
            "Vanuatu",
            "Vatican City",
            "Venezuela",
            "Vietnam",
            "Western Sahara",
            "Yemen",
            "Yugoslavia",
            "Zambia",
            "Zimbabwe"
        };
    }

        #endregion
}
