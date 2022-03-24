using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TreeViewExample
{
    public class Product
    {
        private string _name;
        private ObservableCollection<Product> _childProducts;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public ObservableCollection<Product> ChildProducts
        {
            get
            {
                return _childProducts;
            }
            set
            {
                _childProducts = value;
            }
        }

        public Product()
        {
            ChildProducts = new ObservableCollection<Product>();
        }

        public Product(string name)
        {
            ChildProducts = new ObservableCollection<Product>();
            Name = name;
        }
    }
}
