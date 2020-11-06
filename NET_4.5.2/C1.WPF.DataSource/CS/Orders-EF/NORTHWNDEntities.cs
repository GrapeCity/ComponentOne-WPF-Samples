using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.Data.Entities;

namespace Orders
{
    public partial class NORTHWNDEntities
    {
        // Returns all customer cities.
        public IEnumerable<string> Cities
        {
            get { return (from c in Customers select c.City).Distinct().ToList(); }
        }

        // Returns all customer countries.
        public IEnumerable<string> Countries
        {
            get { return (from c in Customers select c.Country).Distinct().ToList(); }
        }
    }
}
