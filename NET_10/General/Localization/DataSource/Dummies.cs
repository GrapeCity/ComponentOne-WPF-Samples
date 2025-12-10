using Localization.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Localization.DataSource
{
    public class PersonFactory
    {
        private Random Randomizer = new Random();

        /// <summary>
        /// Generates a list of randomly populated <see cref="PersonModel"/> objects.
        /// </summary>
        /// <remarks>This method creates a specified number of <see cref="PersonModel"/> instances with
        /// randomly generated values for properties such as name, address, age, residence, country of birth, and birth
        /// date. The generated data is intended for testing or demonstration purposes and does not represent real
        /// individuals.</remarks>
        /// <param name="qty">The number of <see cref="PersonModel"/> objects to generate. Must be a non-negative integer.</param>
        /// <returns>A list of <see cref="PersonModel"/> objects, each populated with random data.</returns>
        public List<PersonModel> Generate(int qty)
        {
            var items = new List<PersonModel>();
            for (int i = 0; i < qty; i++)
            {
                var name = Names[Randomizer.Next(Names.Length)] + " " + Surnames[Randomizer.Next(Surnames.Length)];
                var residence = Countries[Randomizer.Next(Countries.Length)];

                items.Add(new PersonModel()
                {
                    Name = name,
                    Address = Streets[Randomizer.Next(Streets.Length)] + " " + (1000 + Randomizer.Next(8999)).ToString(),
                    Age = 10 + Randomizer.Next(40),
                    Gender = Randomizer.Next(2) == 0 ? "Male" : "Female",
                    ContactNumber = 1000000 + Randomizer.Next(5000000),
                    Residence = residence,
                    BloodType = new[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" }[Randomizer.Next(8)],
                    Education = new[] { "High School", "Diploma", "Bachelor's", "Master's", "PhD" }[Randomizer.Next(5)],
                    MaritalStatus = new[] { "Single", "Married", "Divorced", "Widowed" }[Randomizer.Next(4)],
                    NationalID = "CID" + Randomizer.Next(100000000, 999999999),
                    Nationality = residence,
                    Born = Countries[Randomizer.Next(Countries.Length)],
                    BirthDate = GetBirthDate(Randomizer),
                    FathersName = Names[Randomizer.Next(Names.Length)] + " " + Surnames[Randomizer.Next(Surnames.Length)],
                    MothersName = Names[Randomizer.Next(Names.Length)] + " " + Surnames[Randomizer.Next(Surnames.Length)],
                    EmailAddress = GenerateRandomEmail(Names, Surnames, Countries, Randomizer)
                });
            }
            return items;
        }
        private static string GenerateRandomEmail(string[] names, string[] surnames, string[] countries, Random randomizer)
        {
            var name = $"{names[randomizer.Next(names.Length)]}.{surnames[randomizer.Next(surnames.Length)]}".ToLower();
            var domain = countries[randomizer.Next(countries.Length)].ToLower();
            return $"{name}@{domain}.com";
        }

        private static DateTime GetBirthDate(Random r)
        {
            long lowerDateTicks = new DateTime(1920, 1, 1).Ticks;
            long upperDateTicks = DateTime.Now.Ticks;
            long newDateTicks = lowerDateTicks + Convert.ToInt64(Convert.ToDouble(upperDateTicks - lowerDateTicks) * r.NextDouble());
            return new DateTime(newDateTicks);
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
            "Kinley",
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
            "Penjor",
            "Smith",
            "Gates",
            "Meredith",
            "Drexler",
            "Beckham",
            "Jobs",
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
            "Bhutan",
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
