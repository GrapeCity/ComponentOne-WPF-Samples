﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;

namespace ThemeExplorer
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
		
		[Category("Current Location")]
		public string Address { get; set; }
		
		[Category("Current Location")]
        public string Residence { get; set; }
		
        public string Born { get; set; }

        public DateTime DOB { get; set; }
        public bool Male { get; set; }

        public string Occupation { get; set; }

        private static Random Randomizer = new Random();

        public static List<Person> Generate(int qty)
        {
            var items = new List<Person>();
            for (int i = 0; i < qty; i++)
            {
                items.Add(new Person()
                {
                    Name = Names[Randomizer.Next(Names.Length)] + " " + Surnames[Randomizer.Next(Surnames.Length)],
                    Address = Streets[Randomizer.Next(Streets.Length)] + " " + (1000 + Randomizer.Next(8999)).ToString(),
                    Age = 17 + Randomizer.Next(40),
                    Residence = Countries[Randomizer.Next(Countries.Length)],
                    Born = Countries[Randomizer.Next(Countries.Length)],
                    DOB = DateTime.Now.AddDays(Randomizer.Next(-45, 45)),
                    Male = Randomizer.Next(1, 99) % 2 == 0,
                    //FlagColor = Colors.Gold,
                    Occupation = Occupations[Randomizer.Next(Occupations.Length)]
                });
            }
            return items;
        }

        private static string[] Streets = new string[] 
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

        private static string[] Names = new string[] 
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

        private static string[] Surnames = new string[] 
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

        public static string[] Occupations = new string[]
        {
            "Teacher",
            "Engineer",
            "Doctor",
            "Nurse",
            "Postman",
            "Singer",
            "Gardener",
            "Plumber",
            "Mechanic"
        };

        private static string[] Countries = new string[] 
        {
            "Afghanistan", 
            "Albania",
            "Algeria",
            "Andorra",
            "Angola",
            "Antigua and Barbuda",
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
            "Bosnia and Herzegovina",
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
            "Central African Republic",
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
            "Democratic Republic of the Congo",
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
            "Former Yugoslav Republic of Macedonia",
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
            "Luxembourg",
            "Macau",
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
            "Saint Vincent and the Grenadines",
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
}
