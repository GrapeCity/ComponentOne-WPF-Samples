using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

using C1.LiveLinq.LiveViews;
using C1.WPF.LiveLinq;
using C1.Data.Entities;

namespace Orders.ViewModel
{
    using Orders.Model;

    // View model for ProductsView.
    public class ProductsViewModel : WorkspaceViewModel
    {
        public IList Products { get; private set; }

        public ProductsViewModel(NORTHWNDContext data)
        {
            base.DisplayName = "Edit Products";

            Products = data.Products;
        }
    }
}