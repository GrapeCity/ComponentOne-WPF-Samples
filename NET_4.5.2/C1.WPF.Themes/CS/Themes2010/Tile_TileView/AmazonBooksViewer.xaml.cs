using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Themes
{
    /// <summary>
    /// Interaction logic for AmazonBooksViewer.xaml
    /// </summary>
    public partial class AmazonBooksViewer : UserControl
    {
        private bool _isContentLoaded = false;
        List<Book> books;

        public AmazonBooksViewer()
        {
            InitializeComponent();
            Loaded += AmazonBooksWindow_Loaded;
        }

        void AmazonBooksWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_isContentLoaded && this.IsVisible)
            {
                books = LoadBooks();
                c1TileView1.ItemsSource = books;
                _isContentLoaded = true;
            }
        }

        /// <summary>
        /// Load books from Amazon.xaml.
        /// </summary>
        List<Book> LoadBooks()
        {
            List<Book> list = new List<Book>();
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Tile_TileView/Amazon.xml", UriKind.Relative));
            if (resource != null)
            {
                using (var stream = resource.Stream)
                {
                    XDocument doc = XDocument.Load(stream);
                    return (from reader in doc.Descendants("book")
                                select new Book
                                {
                                    Title = reader.Attribute("title").Value,
                                    CoverUri = reader.Attribute("coverUri").Value,
                                    Id = reader.Attribute("id").Value,
                                    Price = reader.Attribute("price").Value,
                                    Author = reader.Attribute("author").Value,
                                    Description = reader.Attribute("description").Value,
                                    StockAmount = int.Parse(reader.Attribute("stockAmount").Value)
                                }).ToList();
                }
            }
            return list;
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string CoverUri { get; set; }
        public string Id { get; set; }
        public string Price { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int StockAmount { get; set; }
    }
}
