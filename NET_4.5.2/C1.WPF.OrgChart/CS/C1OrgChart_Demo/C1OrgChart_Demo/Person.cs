using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Data
{
    /// <summary>
    /// Our hierarchical data item: A Person has Subordinates of type Person.
    /// </summary>
    public class Person
    {
        ObservableCollection<Person> _list = new ObservableCollection<Person>();

        #region ** object model

        public string Name { get; set; }
        public string Position { get; set; }
        public string Notes { get; set; }
        public IList<Person> Subordinates
        {
            get { return _list; }
        }
        public int TotalCount
        {
            get
            {
                var count = 1;
                foreach (var p in Subordinates)
                {
                    count += p.TotalCount;
                }
                return count;
            }
        }
        public override string ToString()
        {
            return string.Format("{0}:\r\n\t{1}", Name, Position);
        }

        #endregion

        #region ** Person factory

        static Random _rnd = new Random();
        static string[] _positions = "Director|Manager|Designer|Developer|Writer|Assistant".Split('|');
        static string[] _areas = "Development|Marketing|Sales|Support|Accounting".Split('|');
        static string[] _first = "John|Paul|Dan|Dave|Rich|Mark|Greg|Erin|Susan|Sarah|Tim|Trevor|Kevin|Mark|Dewey|Huey|Larry|Moe|Curly|Adam|Albert".Split('|');
        static string[] _last = "Smith|Doe|Williams|Sorensen|Hansen|Mandela|Johnson|Ward|Woodman|Jordan|Mays|Kevorkian|Trudeau|Hendrix|Clinton".Split('|');
        static string[] _verb = "likes|reads|studies|hates|exercises|dreams|plays|writes|argues|sleeps|ignores".Split('|');
        static string[] _adjective = "long|short|important|pompous|hard|complex|advanced|modern|boring|strange|curious|obsolete|bizarre".Split('|');
        static string[] _noun = "products|tasks|goals|campaigns|books|computers|people|meetings|food|jokes|accomplishments|screens|pages".Split('|');

        public static Person CreatePerson(int level)
        {
            var p = CreatePerson();
            if (level > 0)
            {
                level--;
                for (int i = 0; i < _rnd.Next(1, 4); i++)
                {
                    p.Subordinates.Add(CreatePerson(_rnd.Next(level / 2, level)));
                }
            }
            return p;
        }
        public static Person CreatePerson()
        {
            var p = new Person();
            p.Position = string.Format("{0} of {1}", GetItem(_positions), GetItem(_areas));
            p.Name = string.Format("{0} {1}", GetItem(_first), GetItem(_last));
            p.Notes = string.Format("{0} {1} {2} {3}", p.Name, GetItem(_verb), GetItem(_adjective), GetItem(_noun));
            while (_rnd.NextDouble() < .5)
            {
                p.Notes += string.Format(" and {0} {1} {2}", GetItem(_verb), GetItem(_adjective), GetItem(_noun));
            }
            p.Notes += ".";
            return p;
        }
        static string GetItem(string[] list)
        {
            return list[_rnd.Next(0, list.Length)];
        }

        #endregion
    }
}
