using System.ComponentModel;

namespace CustomEngineSamples
{
    public class StoreCollection: BindingList<Store>
    {
        public static StoreCollection GetData()
        {
            var stores = new StoreCollection
            {
                new Store("Pear Inc.")
            };
            stores[0].ProductsGroups.Add(new ProductsGroup("Mobile phones"));
            stores[0].ProductsGroups[0].Products.Add(new Product("fPhone 34", 999.99, 10));
            stores[0].ProductsGroups[0].Products.Add(new Product("fPhone 34Z", 9999.99, 5));
            stores[0].ProductsGroups[0].Products.Add(new Product("fPhone 34XX", 100000, 0));
            stores[0].ProductsGroups.Add(new ProductsGroup("Notebooks"));
            stores[0].ProductsGroups[1].Products.Add(new Product("DuckBook S", 9999.99, 30));
            stores[0].ProductsGroups[1].Products.Add(new Product("DuckBook Ultra", 14000, 3));
            stores[0].ProductsGroups[1].Products.Add(new Product("DuckBook Pro", 20000, 100));
            stores[0].ProductsGroups.Add(new ProductsGroup("Computers"));
            stores[0].ProductsGroups[2].Products.Add(new Product("DuckPC 3", 10000.99, 4));
            stores[0].ProductsGroups[2].Products.Add(new Product("DuckPro X", 15000, 43));
            stores[0].ProductsGroups[2].Products.Add(new Product("DuckPro Ultra", 19000, 0));

            stores.Add(new Store("Space Inc."));
            stores[1].ProductsGroups.Add(new ProductsGroup("Mobile phones"));
            stores[1].ProductsGroups[0].Products.Add(new Product("Rocket 1A", 900, 66));
            stores[1].ProductsGroups[0].Products.Add(new Product("Rocket 2X", 3999, 55));
            stores[1].ProductsGroups[0].Products.Add(new Product("Rocket 3E", 20000, 44));
            stores[1].ProductsGroups.Add(new ProductsGroup("Notebooks"));
            stores[1].ProductsGroups[1].Products.Add(new Product("Shuttle 1A", 9999.99, 99));
            stores[1].ProductsGroups[1].Products.Add(new Product("Shuttle 1X", 14000, 33));
            stores[1].ProductsGroups[1].Products.Add(new Product("Shuttle Pro", 20000, 100));
            stores[1].ProductsGroups.Add(new ProductsGroup("Computers"));
            stores[1].ProductsGroups[2].Products.Add(new Product("IssPC 2D", 10000.99, 2));
            stores[1].ProductsGroups[2].Products.Add(new Product("IssPro 2X", 15000, 0));
            stores[1].ProductsGroups[2].Products.Add(new Product("IssPro Pro", 19000, 31));

            stores.Add(new Store("Fruit Inc."));
            stores[2].ProductsGroups.Add(new ProductsGroup("Mobile phones"));
            stores[2].ProductsGroups[0].Products.Add(new Product("Pineapple 1", 2900, 29));
            stores[2].ProductsGroups[0].Products.Add(new Product("Mango 1", 3099, 55));
            stores[2].ProductsGroups[0].Products.Add(new Product("Orange 1", 5000, 4));
            stores[2].ProductsGroups.Add(new ProductsGroup("Notebooks"));
            stores[2].ProductsGroups[1].Products.Add(new Product("Mandarin X", 9999.99, 10));
            stores[2].ProductsGroups[1].Products.Add(new Product("Lemon X", 14000, 7));
            stores[2].ProductsGroups[1].Products.Add(new Product("Lemon Pro", 20000, 33));
            stores[2].ProductsGroups.Add(new ProductsGroup("Computers"));
            stores[2].ProductsGroups[2].Products.Add(new Product("Plum X", 10000.99, 13));
            stores[2].ProductsGroups[2].Products.Add(new Product("Plum Z", 15000, 23));
            stores[2].ProductsGroups[2].Products.Add(new Product("Plum Pro", 19000, 0));
        
            return stores;
        }
    }
}
