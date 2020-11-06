OData
-------------------------------------------------------------------------------------
Demonstrates how to use OData data sources in WPF applications.

This sample uses the NorthWind OData service available at https://services.odata.org/Northwind/Northwind.svc/. 
When you select a category on the top grid, the corresponding products are shown on the bottom grid.

To use OData sources, you should start by adding a service reference to your project, then
specifying the URL of the OData provider. 

Once the service reference has been added to the project, you can obtain the data by instantiating
a data context.
For example, the code below populates the Categories grid in the sample:

<code>
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
	    Uri _svcRoot = new Uri("https://services.odata.org/Northwind/Northwind.svc/");
        NorthwindEntities _context = new NorthwindEntities(_svcRoot);
</code>

The code below populates the Products grid in the sample with 
products that match the category sent as parameter:

<code>
    IOrderedQueryable<Product> GetProducts(Category ctgry)
        {
            if (ctgry != null)
            {
                var products = from p in _context.Products
                               where p.CategoryID == ctgry.CategoryID
                               orderby p.ProductName
                               select p;
                return products;
            }
            return null;
        }
</code>



