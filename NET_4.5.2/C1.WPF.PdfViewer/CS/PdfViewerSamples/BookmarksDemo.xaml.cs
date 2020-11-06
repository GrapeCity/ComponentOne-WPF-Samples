using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using C1.WPF;
using C1.WPF.PdfViewer;

namespace PdfViewerSamples
{
    /// <summary>
    /// Interaction logic for BookmarksDemo.xaml
    /// </summary>
    public partial class BookmarksDemo : UserControl
    {
        public BookmarksDemo()
        {
            InitializeComponent();

            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Bookmarks.pdf", UriKind.Relative));
            pdfviewer.LoadDocument(resource.Stream);

        }

        void C1TreeView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0) return;
            var treeViewItem = e.AddedItems[0] as C1TreeViewItem;
            if (treeViewItem == null) return;
            var bookmark = treeViewItem.Header as Bookmark;
            if (bookmark == null) return;
            List<string> titles = new List<string>();
            while (bookmark != null)
            {
                titles.Insert(0, bookmark.Title);
                if (treeViewItem.Parent == null)
                    break;
                treeViewItem = treeViewItem.Parent as C1TreeViewItem;
                if (treeViewItem == null) break; ;
                bookmark = treeViewItem.Header as Bookmark;
                if (bookmark == null) break;
            }

            pdfviewer.GoToBookmark(titles.ToArray());
        }
    }
}
