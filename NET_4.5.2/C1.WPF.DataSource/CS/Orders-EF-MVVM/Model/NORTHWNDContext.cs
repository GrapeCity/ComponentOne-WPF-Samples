using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C1.Data.Entities;
using C1.LiveLinq.LiveViews;
using C1.Data;

namespace Orders.Model
{
    // Northwind-specific client scope
    public class NORTHWNDContext : EntityClientScope
    {
        // Returns the object context.
        public NORTHWNDEntities Entities
        {
            get
            {
                return (NORTHWNDEntities)((EntityClientCache)ClientCache).DbContext;
            }
        }

        public NORTHWNDContext(EntityClientCache clientCache)
            : base(clientCache)
        {

        }

        // Returns a ClientView of all Orders.
        public ClientView<Order> Orders
        {
            get { return GetItems<Order>(); }
        }

        // Returns a ClientView of all Products
        public ClientView<Product> Products
        {
            get { return GetItems<Product>(); }
        }

        // Returns all cities.
        public IEnumerable<string> Cities
        {
            get { return Entities.Cities; }
        }
    }
}
