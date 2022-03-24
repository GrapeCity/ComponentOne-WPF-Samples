using System;
using System.Collections.Generic;

namespace C1TreeViewTemplateSample2010
{
    /// <summary>
    /// Represents a World Cup group
    /// </summary>
    public class WorldCupGroup 
    {
        public string GroupName { get; set; }
        public List<WorldCupTeam> Teams { get; set; }


        public static IEnumerable<WorldCupGroup> Groups 
        {
            get 
            {
                // GROUP A
                yield return new WorldCupGroup()
                {
                    GroupName = "Group A",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "South Africa",
                            CountryName = "South Africa",
                            TopScorer = "Kagisho DIKGACOI",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Mexico",
                            CountryName = "Mexico",
                            TopScorer = "Jared BORGETTI",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Uruguay",
                            CountryName = "Uruguay",
                            TopScorer = "Diego FORLAN",
                            WorldCupWon = 2
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "France",
                            CountryName = "France",
                            TopScorer = "Andre-Pierre GIGNAC",
                            WorldCupWon = 1
                        },
                    }
                };

                // GROUP B
                yield return new WorldCupGroup()
                {
                    GroupName = "Group B",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "Argentina",
                            CountryName = "Argentina",
                            TopScorer = "Sergio AGUERO",
                            WorldCupWon = 2
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Nigeria",
                            CountryName = "Nigeria",
                            TopScorer = "Victor OBINNA",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Korea South",
                            CountryName = "Korea Republic",
                            TopScorer = "PARK Ji Sung",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Greece",
                            CountryName = "Greece",
                            TopScorer = "Theofanis GEKAS",
                            WorldCupWon = 0
                        },
                    }
                };

                // GROUP C
                yield return new WorldCupGroup()
                {
                    GroupName = "Group C",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "England",
                            CountryName = "England",
                            TopScorer = "Wayne ROONEY",
                            WorldCupWon = 1
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "United States",
                            CountryName = "USA",
                            TopScorer = "Jozy ALTIDORE",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Algeria",
                            CountryName = "Algeria",
                            TopScorer = "Rafik SAIFI",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Slovenia",
                            CountryName = "Slovenia",
                            TopScorer = "Milivoje NOVAKOVIC",
                            WorldCupWon = 0
                        },
                    }
                };

                // GROUP D
                yield return new WorldCupGroup()
                {
                    GroupName = "Group D",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "Germany",
                            CountryName = "Germany",
                            TopScorer = "Miroslav KLOSE",
                            WorldCupWon = 3
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Australia",
                            CountryName = "Australia",
                            TopScorer = "Tim CAHILL",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Serbia",
                            CountryName = "Serbia",
                            TopScorer = "Milan JOVANOVIC",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Ghana",
                            CountryName = "Ghana",
                            TopScorer = "Matthew AMOAH",
                            WorldCupWon = 0
                        },
                    }
                };

                // GROUP E
                yield return new WorldCupGroup()
                {
                    GroupName = "Group E",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "Netherlands",
                            CountryName = "Netherlands",
                            TopScorer = "Klaas HUNTELAAR",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Denmark",
                            CountryName = "Denmark",
                            TopScorer = "Soren LARSEN",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Japan",
                            CountryName = "Japan",
                            TopScorer = "Shunsuke NAKAMURA",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Cameroon",
                            CountryName = "Cameroon",
                            TopScorer = "Samuel ETOO",
                            WorldCupWon = 0
                        },
                    }
                };


                // GROUP F
                yield return new WorldCupGroup()
                {
                    GroupName = "Group F",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "Italy",
                            CountryName = "Italy",
                            TopScorer = "Alberto GILARDINO",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Paraguay",
                            CountryName = "Paraguay",
                            TopScorer = "Salvador CABANAS",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "NewZealand",
                            CountryName = "New Zealand",
                            TopScorer = "Shane SMELTZ",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Slovak Republic",
                            CountryName = "Slovakia",
                            TopScorer = "Stanislav SESTAK",
                            WorldCupWon = 0
                        },
                    }
                };

                // GROUP G
                yield return new WorldCupGroup()
                {
                    GroupName = "Group G",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "Brazil",
                            CountryName = "Brazil",
                            TopScorer = "LUIS FABIANO",
                            WorldCupWon = 5
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Korea North",
                            CountryName = "Korea DPR",
                            TopScorer = "JONG Chol Min",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Cote Dlvoire",
                            CountryName = "Côte d'Ivoire",
                            TopScorer = "Didier DROGBA",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Portugal",
                            CountryName = "Portugal",
                            TopScorer = "SIMAO",
                            WorldCupWon = 0
                        },
                    }
                };

                // GROUP H
                yield return new WorldCupGroup()
                {
                    GroupName = "Group H",
                    Teams = new List<WorldCupTeam>() 
                    {
                        new WorldCupTeam() 
                        {
                            ShortName = "Spain",
                            CountryName = "Spain",
                            TopScorer = "David VILLA",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Switzerland",
                            CountryName = "Switzerland",
                            TopScorer = "Alexander FREI",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Honduras",
                            CountryName = "Honduras",
                            TopScorer = "Carlos PAVON",
                            WorldCupWon = 0
                        },
                        new WorldCupTeam()
                        {
                            ShortName = "Chile",
                            CountryName = "Chile",
                            TopScorer = "Humberto SUAZO",
                            WorldCupWon = 0
                        },
                    }
                };
            }
        }
    }

    /// <summary>
    /// Represents a team
    /// </summary>
    public class WorldCupTeam
    {
        private string _shortName;

        public string ShortName
        {
            get
            {
                return _shortName;
            }
            set
            {
                _shortName = value;
                FlagUri = new Uri(string.Format("/C1TreeViewTemplateSample2010.4;component/Resources/{0}.png", _shortName), UriKind.RelativeOrAbsolute);
            }
        }
        public string CountryName { get; set; }
        public Uri FlagUri { get; set; }
        public string TopScorer { get; set; }
        public int WorldCupWon { get; set; }
    }
}