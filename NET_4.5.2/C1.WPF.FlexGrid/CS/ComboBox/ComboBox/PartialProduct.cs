using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComboBox.ServiceReference1
{
    /// <summary>
    /// Extends Product class to udpate CategoryID property when Category changes.
    /// </summary>
    public partial class Product
    {
        public Product()
        {
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "Category")
                {
                    if (Category != null)
                    {
                        this.CategoryID = Category.CategoryID;
                    }
                    else
                    {
                        this.CategoryID = null;
                    }
                }
            };
        }
    }
}
