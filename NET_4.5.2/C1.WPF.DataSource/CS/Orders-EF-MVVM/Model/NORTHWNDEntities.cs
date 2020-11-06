using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.Data.Entities;

using Orders.Model;

namespace Orders
{
    public partial class NORTHWNDEntities
    {
        EntityClientCache _clientCache;
        // Returns the Client Cache for this object context.
        public EntityClientCache ClientCache
        {
            get
            {
                if (_clientCache == null)
                    _clientCache = new EntityClientCache(this);
                return _clientCache;
            }
        }

        // Creates a Northwind-specific client scope
        public NORTHWNDContext CreateContext()
        {
            return new NORTHWNDContext(ClientCache);
        }

        // Returns all cities.
        public IEnumerable<string> Cities
        {
            get { return (from c in Customers select c.City).Distinct().ToList(); }
        }
    }
}
