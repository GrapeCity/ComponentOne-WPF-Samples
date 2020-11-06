using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using C1.WPF.Extended;

namespace ExtendedSamples
{
    /// <summary>
    /// Interaction logic for DemoBook.xaml
    /// </summary>
    public partial class SinglePage : UserControl
    {
        public SinglePage()
        {
            InitializeComponent();
            InitDataSource();
            CheckPermissions();
        }

        private void InitDataSource()
        {
            // load book descriptions from xml
            XDocument doc = XDocument.Load(new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExtendedSamples.C1Book.Amazon.xml")));
            var books = from reader in doc.Descendants("book")
                        select new AmazonBookDescription
                        {
                            Title = reader.Attribute("title").Value,
                            CoverUri = reader.Attribute("coverUri").Value,
                            Author = reader.Attribute("author").Value,
                            Price = reader.Attribute("price").Value,
                        };

            // set the book's item source
            book.ItemsSource = books;
        }

        private void CheckPermissions()
        {
            bool isPermitted = true;
            try
            {
                (new UIPermission(PermissionState.Unrestricted)).Demand();
            }
            catch
            {
                isPermitted = false;
            }
            if (isPermitted)
                book.Visibility = Visibility.Visible;
            else
                notSupportedTb.Visibility = Visibility.Visible;
        }

        #region Object Model

        public bool ShowInnerShadows
        {
            get
            {
                return book.ShowInnerShadows;
            }
            set
            {
                book.ShowInnerShadows = value;
            }
        }

        public bool ShowOuterShadows
        {
            get
            {
                return book.ShowOuterShadows;
            }
            set
            {
                book.ShowOuterShadows = value;
            }
        }

        public Orientation Orientation
        {
            get
            {
                return book.Orientation;
            }
            set
            {
                book.Orientation = value;
            }
        }

        public bool IsFirstPageOnTheRight
        {
            get
            {
                return book.IsFirstPageOnTheRight;
            }
            set
            {
                book.IsFirstPageOnTheRight = value;
            }
        }

        public PageFoldVisibility ShowPageFold
        {
            get
            {
                return book.ShowPageFold;
            }
            set
            {
                book.ShowPageFold = value;
            }
        }

        public PageFoldAction PageFoldAction
        {
            get
            {
                return book.PageFoldAction;
            }
            set
            {
                book.PageFoldAction = value;
            }
        }

        public double FoldSize
        {
            get
            {
                return book.FoldSize;
            }
            set
            {
                book.FoldSize = value;
            }
        }

        public int CurrentPage
        {
            get
            {
                return book.CurrentPage;
            }
            set
            {
                book.CurrentPage = value;
            }
        }

        #endregion

        private void Backward_Click(object sender, RoutedEventArgs e)
        {
            this.book.TurnPage(false);
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            this.book.TurnPage(true);
        }

        private void Vertical_Checked(object sender, RoutedEventArgs e)
        {
            this.book.Orientation = Orientation.Vertical;
            this.book.Width = this.book.RenderSize.Height;
            this.book.Height = this.book.RenderSize.Width;
        }

        private void Vertical_Unchecked(object sender, RoutedEventArgs e)
        {
            this.book.Orientation = Orientation.Horizontal;
            this.book.Width = this.book.RenderSize.Height;
            this.book.Height = this.book.RenderSize.Width;
        }
    }
}
