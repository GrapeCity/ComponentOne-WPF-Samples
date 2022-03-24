using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using C1.WPF;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Serialization;

namespace TreeViewExample
{
    public class Topic
    {
        public string Title { get; set; }
        private ObservableCollection<Topic> childTopicsValue = new ObservableCollection<Topic>();
        public  ObservableCollection<Topic> ChildTopics
        {
            get
            {
                return childTopicsValue;
            }
            set
            {
                childTopicsValue = value;
            }
        }
        public Topic(string title)
        {
            Title = title;
        }

        public Topic() { }
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isSearched = false;
        private ObservableCollection<Product> Products = new ObservableCollection<Product>();
        static public ObservableCollection<Topic> Topics = new ObservableCollection<Topic>();
        public MainWindow()
        {
            InitializeComponent();
            Products.Add(LoadProduct());
            tree.DataContext = Products;
        }

        private Product LoadProduct()
        {
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/products.xml", UriKind.Relative));
            if (resource != null)
            {
                var serializer = new XmlSerializer(typeof(Product));
                return (Product)serializer.Deserialize(resource.Stream);
            }
            else
            {
                return new Product();
            }   
        }

        /// <summary>
        /// Find the special item from data source by using search buffer.
        /// </summary>
        /// <param name="searchBuffer"></param>
        /// <returns></returns>
        private List<int> FindVirtualizationItem(string searchBuffer)
        {
            List<int> list = new List<int>();
            Product p = LoadProduct();
            FindItemFromDataSource(p, searchBuffer, ref list);
            if (p.Name.StartsWith(searchBuffer, StringComparison.InvariantCultureIgnoreCase))
            {
                list.Clear();
                list.Add(0);

                return list;
            }
            else
            {
                if (list.Count != 0)
                {
                    list.Add(0);
                }
            }
            
            list = Reverse(list);

            return list;
        }

        private void FindItemFromDataSource(Product p, string searchBuffer, ref List<int> list)
        {
            Search(p, searchBuffer, ref list);
        }

        /// <summary>
        /// Search text from items.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="searchBuffer"></param>
        /// <param name="isValidate"></param>
        /// <returns></returns>
        private void Search(Product p, string searchBuffer, ref List<int> searchedIndexes)
        {
            if (p.ChildProducts == null)
            {
                return;
            }

            for (int index = 0; index < p.ChildProducts.Count; index++)
            {
                if (p.ChildProducts[index].Name.StartsWith(searchBuffer, StringComparison.InvariantCultureIgnoreCase))
                {
                    _isSearched = true;
                    searchedIndexes.Add(index);
                    break;
                }
                else
                {
                    Search(p.ChildProducts[index], searchBuffer, ref searchedIndexes);
                    if (_isSearched)
                    {
                        searchedIndexes.Add(index);
                        break;
                    }
                }
            }
        }

        private List<int> Reverse(List<int> list)
        {
            List<int> newList = new List<int>();
            // Add the first level into collection.
            for (int index = list.Count - 1; index >= 0; index--)
            {
                newList.Add(list[index]);
            }

            return newList;
        }

        private void tree_AutoSearchBufferChanged(object sender, AutoSearchBufferChangedEventArgs e)
        {
            _isSearched = false;
            e.Indexes = FindVirtualizationItem(e.SearchBuffer);
        }
    }
}
