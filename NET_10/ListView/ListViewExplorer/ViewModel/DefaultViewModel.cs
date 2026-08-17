using System;
using System.Collections.Generic;
using System.Text;

namespace ListViewExplorer
{
    public class DefaultViewModel
    {
        public DefaultViewModel()
        {
            People = Person.Generate(100);
        }

        public List<Person> People { get; }
    }
}
