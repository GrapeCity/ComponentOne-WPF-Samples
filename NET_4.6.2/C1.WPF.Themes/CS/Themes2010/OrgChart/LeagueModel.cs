using System.Collections.Generic;

namespace Themes
{
    public class League
    {
        public string Name { get; set; }
        public List<League> SubNodes { get; set; }

        public static League GetLeague()
        {
            var league = new League
            {
                Name = "Main League",
                SubNodes = new List<League>()
            };
            foreach (var div in "East,West,North,South".Split(','))
            {
                var d = new League { Name = div, SubNodes = new List<League>() };
                league.SubNodes.Add(d);
                switch (div)
                {
                    case "East":
                        AddNewTeams(d, "Redskins,Eagles,Giants,Cowboys");
                        break;
                    case "West":
                        AddNewTeams(d, "Cardinals,Seahawks,Rams,49ers");
                        break;
                    case "North":
                        AddNewTeams(d, "Vikings, Packers, Lions, Bears");
                        break;
                    case "South":
                        AddNewTeams(d, "Panthers,Falcons,Saints,Buccaneers");
                        break;
                }
            }
            return league;
        }

        static void AddNewTeams(League d, string teamNames)
        {
            foreach (var team in teamNames.Split(','))
            {
                var t = new League { Name = team};
                d.SubNodes.Add(t);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
