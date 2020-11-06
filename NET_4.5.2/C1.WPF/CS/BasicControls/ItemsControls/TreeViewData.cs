using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BasicControls
{
    public class Sport
    {
        private List<Sport> _childSport;
        private string _name;
        public Sport()
        {
            ChildSport = new List<Sport>();
        }
        public Sport(string name)
        {
            ChildSport = new List<Sport>();
            Name = name;
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name=value;
            }
        }
        public List<Sport> ChildSport
        {
            get
            {
                return _childSport;
            }
            set
            {
                _childSport = value;
            }
        }
    }
}
