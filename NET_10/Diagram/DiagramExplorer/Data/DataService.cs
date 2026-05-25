using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#pragma warning disable 1591
namespace DiagramExplorer.Data
{
    // Simple hierarchical organization node
    public class OrgNode
    {
        BitmapImage? image = null;
        bool imageLoaded = false;

        public string Name { get; set; } = "";

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public string Department { get; set; } = "";

        public string JobTitle { get; set; } = "";
        public List<OrgNode> Childs { get; set; } = new List<OrgNode>();

        public BitmapImage? Image
        {
            get
            {
                if (!imageLoaded)
                {
                    image = new BitmapImage(new Uri("Resources\\" + Name.Replace(' ', '_') + ".png", UriKind.Relative));
                    imageLoaded = true;
                }

                return image;
            }
        }
    }

    public class Country
    {
        public required string World { get; set; }
        public required string Continent { get; set; }
        public required string Region { get; set; }

        public required string Name { get; set; }
        public required string Code { get; set; }

        BitmapImage? flag;

        public BitmapImage? Flag
        {
            get
            {
                if (flag == null)
                    flag = new BitmapImage(new Uri($"Resources\\flags\\{Code}.png", UriKind.Relative));

                return flag;
            }
        }
    }

    public class Animal
    {
        BitmapImage? image = null;
        bool imageLoaded = false;

        public string Name { get; set; } = "";
        public string NameScientific { get; set; } = "";

        public string Phylum { get; set; } = "";

        public string Class { get; set; } = "";

        public string Order { get; set; } = "";

        public BitmapImage? Image
        {
            get
            {
                if (!imageLoaded && Class != "Class")
                {
                    image = new BitmapImage(new Uri("Resources\\" + Name.Replace(' ', '_') + ".png", UriKind.Relative));
                    image.Freeze();
                    imageLoaded = true;
                }

                return image;
            }
        }

        public Uri ImageUri
        {
            get
            {
                return new Uri("\\Resources\\" + Name.Replace(' ', '_') + ".png", UriKind.Relative);
            }
        }

        public override string ToString() => Name;
    }


    public class DataService
    {
        static DataService? dataService;
        public static DataService GetService()
        {
            if (dataService == null)
                dataService = new DataService();
            return dataService;
        }

        public List<OrgNode> GetOrgChartData()
        {
            var company = new OrgNode
            {
                Name = "Acme Corporation",
                Department = "CEO",
                Childs = new List<OrgNode>
                {
                    new OrgNode
                    {
                        FirstName = "Sarah",
                        LastName = "Mitchell",
                        Department = "CEO",
                        Name = "Sarah Mitchell",
                        JobTitle = "CEO",
                        Childs = new List<OrgNode>
                        {
                            new OrgNode
                            {
                                FirstName = "David",
                                LastName = "Chen",
                                Name = "David Chen",
                                Department = "Engineering",
                                JobTitle = "VP Engineering",
                                Childs = new List<OrgNode>
                                {
                                    new OrgNode
                                    {
                                        FirstName = "Amanda",
                                        LastName = "Rodriguez",
                                        Department = "Engineering",
                                        Name = "Amanda Rodriguez",
                                        JobTitle = "Engineering Manager",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "John",
                                                LastName = "Smith",
                                                Department = "Engineering",
                                                Name = "John Smith",
                                                JobTitle = "Senior Software Engineer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Lisa",
                                                LastName = "Wang",
                                                Department = "Engineering",
                                                Name = "Lisa Wang",
                                                JobTitle = "Software Engineer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Michael",
                                                LastName = "Brown",
                                                Department = "Engineering",
                                                Name = "Michael Brown",
                                                JobTitle = "Software Engineer"
                                            }
                                        }
                                    },
                                    new OrgNode
                                    {
                                        FirstName = "Robert",
                                        LastName = "Taylor",
                                        Department = "Engineering",
                                        Name = "Robert Taylor",
                                        JobTitle = "QA Manager",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Emma",
                                                LastName = "Wilson",
                                                Department = "Engineering",
                                                Name = "Emma Wilson",
                                                JobTitle = "QA Engineer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Chris",
                                                LastName = "Anderson",
                                                Department = "Engineering",
                                                Name = "Chris Anderson",
                                                JobTitle = "QA Engineer"
                                            }
                                        }
                                    }
                                }
                            },
                            new OrgNode
                            {
                                FirstName = "Jennifer",
                                LastName = "Williams",
                                Name = "Jennifer Williams",
                                JobTitle = "VP Product",
                                Department = "Product",
                                Childs = new List<OrgNode>
                                {
                                    new OrgNode
                                    {
                                        FirstName = "Andrew",
                                        LastName = "Miller",
                                        Department = "Product",
                                        Name = "Andrew Miller",
                                        JobTitle = "Senior Product Manager",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Nicole",
                                                LastName = "Harris",
                                                Department = "Product",
                                                Name = "Nicole Harris",
                                                JobTitle = "Product Manager"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Brian",
                                                LastName = "Clark",
                                                Department = "Product",
                                                Name = "Brian Clark",
                                                JobTitle = "Product Manager"
                                            }
                                        }
                                    },
                                    new OrgNode
                                    {
                                        FirstName = "Sophia",
                                        LastName = "Lewis",
                                        Department = "Product",
                                        Name = "Sophia Lewis",
                                        JobTitle = "Design Lead",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Mark",
                                                LastName = "Robinson",
                                                Department = "Product",
                                                Name = "Mark Robinson",
                                                JobTitle = "UX Designer"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Laura",
                                                LastName = "Walker",
                                                Department = "Product",
                                                Name = "Laura Walker",
                                                JobTitle = "UI Designer"
                                            }
                                        }
                                    }
                                }
                            },
                            new OrgNode
                            {
                                FirstName = "Thomas",
                                LastName = "Anderson",
                                Name = "Thomas Anderson",
                                JobTitle = "VP Sales & Marketing",
                                Department = "Sales",
                                Childs = new List<OrgNode>
                                {
                                    new OrgNode
                                    {
                                        FirstName = "Karen",
                                        LastName = "White",
                                        Name = "Karen White",
                                        Department = "Sales",
                                        JobTitle = "Sales Director",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Paul",
                                                LastName = "Martinez",
                                                Department = "Sales",
                                                Name = "Paul Martinez",
                                                JobTitle = "Account Executive"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Rebecca",
                                                LastName = "Young",
                                                Department = "Sales",
                                                Name = "Rebecca Young",
                                                JobTitle = "Account Executive"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Frank",
                                                LastName = "Adams",
                                                Department = "Sales",
                                                Name = "Frank Adams",
                                                JobTitle = "Sales Engineer"
                                            }
                                        }
                                    },
                                    new OrgNode
                                    {
                                        FirstName = "Linda",
                                        LastName = "Campbell",
                                        Department = "Sales",
                                        Name = "Linda Campbell",
                                        JobTitle = "Marketing Director",
                                        Childs = new List<OrgNode>
                                        {
                                            new OrgNode
                                            {
                                                FirstName = "Eric",
                                                LastName = "Baker",
                                                Department = "Sales",
                                                Name = "Eric Baker",
                                                JobTitle = "Content Manager"
                                            },
                                            new OrgNode
                                            {
                                                FirstName = "Hannah",
                                                LastName = "Collins",
                                                Department = "Sales",
                                                Name = "Hannah Collins",
                                                JobTitle = "Marketing Specialist"
                                            }
                                        }
                                    }
                                }
                            },
                        }
                    }
                }
            };

            return new List<OrgNode> { company };
        }

        readonly List<Country> countryData = [];

        public List<Country> GetCountryData()
        {
            if (countryData.Count == 0)
            {
                var stream = GetResourceStream("countries.csv");

                var text = "";
                if (stream != null)
                {
                    using var sr = new StreamReader(stream);
                    text = sr.ReadToEnd();
                }

                var ss = text.Split([Environment.NewLine], StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ss.Length; i++)
                {
                    var values = ss[i].Split(',');
                    countryData.Add(new Country { World = "World", Continent = values[0], Region = values[1], Name = values[2], Code = values[3] });
                }
            }

            return countryData;
        }

        readonly List<Animal> animalData = [];

        public List<Animal> GetAnimalData()
        {
            if (animalData.Count == 0)
            {
                var stream = GetResourceStream("animals10.csv");

                var text = "";
                if (stream != null)
                {
                    using (var sr = new StreamReader(stream))
                        text = sr.ReadToEnd();
                }

                var ss = text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < ss.Length; i++)
                {
                    var values = ss[i].Split(',');
                    animalData.Add(new Animal { Name = values[0], NameScientific = values[1], Phylum = values[2], Class = values[3], Order = values[4] });
                }
            }

            return animalData;
        }

        static Stream? GetResourceStream(string name)
        {
            var asm = Assembly.GetExecutingAssembly();
            return asm.GetManifestResourceStream($"{asm.GetName().Name}.Resources.{name}");
        }
    }
}
