using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace AmazonBooksSamples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AmazonBooksWindow : Window
    {
        List<Book> books;

        public AmazonBooksWindow()
        {
            InitializeComponent();
            Loaded += AmazonBooksWindow_Loaded;
        }

        void AmazonBooksWindow_Loaded(object sender, RoutedEventArgs e)
        {
            books = LoadBooks();
            c1TileView1.ItemsSource = books;
        }

        /// <summary>
        /// Load books from Amazon.xaml.
        /// </summary>
        List<Book> LoadBooks()
        {
            List<Book> list = new List<Book>();
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/Amazon.xml", UriKind.Relative));
            if (resource != null)
            {
                XDocument doc = XDocument.Load(resource.Stream);
                var books = from reader in doc.Descendants("book")
                            select new Book
                            {
                                Title = reader.Attribute("title").Value,
                                CoverUri = reader.Attribute("coverUri").Value,
                                Id = reader.Attribute("id").Value,
                                Price = reader.Attribute("price").Value,
                                Author = reader.Attribute("author").Value,
                                Description = reader.Attribute("description").Value,
                                StockAmount = int.Parse(reader.Attribute("stockAmount").Value)
                            };

                return books.ToList();
            }

            return list;
        }
    }
}
