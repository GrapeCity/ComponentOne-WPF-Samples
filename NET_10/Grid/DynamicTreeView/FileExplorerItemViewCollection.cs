using C1.DataCollection;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using SortDescription = C1.DataCollection.SortDescription;

namespace DynamicTreeView
{
    public class FileExplorerItemViewCollection : C1SelectDataCollection<FileExplorerItem, FileExplorerItemView>
    {
        public FileExplorerItemViewCollection() : this(null, OnSelect)
        {
            Source = new FileExplorerViewModel().Root;
        }

        public FileExplorerItemViewCollection(IEnumerable source) : this(source, OnSelect, OnSelectBack)
        {
        }

        private static FileExplorerItemView OnSelect(IReadOnlyList<FileExplorerItem> list, int index)
        {
            return new FileExplorerItemView(list[index]);
        }

        private static FileExplorerItem OnSelectBack(FileExplorerItemView view)
        {
            return view.Item;
        }

        public FileExplorerItemViewCollection(IEnumerable source, Func<IReadOnlyList<FileExplorerItem>, int, FileExplorerItemView> select, Func<FileExplorerItemView, FileExplorerItem>? selectBack = null) : base(source, select, selectBack)
        {
        }

        public override Type GetItemType()
        {
            return typeof(FileExplorerItemView);
        }

        public override IReadOnlyList<SortDescription> SortDescriptions
        {
            get
            {
                var newSortDescriptions = new List<SortDescription>();
                foreach (var sortDescription in base.SortDescriptions)
                {
                    newSortDescriptions.Add(new SortDescription("Item." + sortDescription.SortPath, sortDescription.Direction));
                }
                return newSortDescriptions;
            }
        }

        public override bool CanSort(params SortDescription[] sortDescriptions)
        {
            var newSortDescriptions = new List<SortDescription>();
            foreach (var sortDescription in sortDescriptions)
            {
                if (sortDescription.SortPath.StartsWith("Item."))
                    newSortDescriptions.Add(new SortDescription(sortDescription.SortPath.Substring("Item.".Length), sortDescription.Direction));
            }
            return base.CanSort(newSortDescriptions.ToArray());
        }

        public override Task SortAsync(SortDescription[] sortDescriptions, CancellationToken cancellationToken = default)
        {
            var newSortDescriptions = new List<SortDescription>();
            foreach (var sortDescription in sortDescriptions)
            {
                if (sortDescription.SortPath.StartsWith("Item."))
                    newSortDescriptions.Add(new SortDescription(sortDescription.SortPath.Substring("Item.".Length), sortDescription.Direction));
            }
            return base.SortAsync(newSortDescriptions.ToArray(), cancellationToken);
        }
    }

    public class FileExplorerItemView
    {
        ImageSource _icon;
        string _type;
        FileExplorerItemViewCollection _items;

        public FileExplorerItemView(FileExplorerItem item)
        {
            Item = item;
        }

        public FileExplorerItem Item { get; }

        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    try
                    {

                        var bitmap = Win32.GetFileIcon(Item.Path).ToBitmap();
                        using (MemoryStream memory = new MemoryStream())
                        {
                            bitmap.Save(memory, ImageFormat.Png);
                            memory.Position = 0;
                            BitmapImage bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memory;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();
                            _icon = bitmapImage;
                        }
                    }
                    catch { }
                }
                return _icon;
            }
        }

        public string Type
        {
            get
            {
                if (_type == null)
                {
                    _type = Win32.GetFileTypeDescription(Path.GetExtension(Item.Path));
                }
                return _type;
            }
        }

        public FileExplorerItemViewCollection Items
        {
            get
            {
                if (_items == null && Item.Items != null)
                {
                    _items = new FileExplorerItemViewCollection(Item.Items);
                }
                return _items;
            }
        }

        #region windows api's

        public class Win32
        {

            [StructLayout(LayoutKind.Sequential)]
            public struct SHFILEINFO
            {
                public IntPtr hIcon;
                public int iIcon;
                public uint dwAttributes;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public string szDisplayName;
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
                public string szTypeName;
            }


            [DllImport("shell32.dll")]
            public static extern IntPtr SHGetFileInfo(string pszPath,
                                        uint dwFileAttributes,
                                        ref SHFILEINFO psfi,
                                        uint cbSizeFileInfo,
                                        uint uFlags);
            public static Icon GetFileIcon(string filePath)
            {
                var shinfo = new SHFILEINFO();
                var hImgSmall = Win32.SHGetFileInfo(filePath, 0, ref shinfo,
                      (uint)Marshal.SizeOf(shinfo),
                       SHGFI_ICON |
                       SHGFI_SMALLICON);

                return System.Drawing.Icon.FromHandle(shinfo.hIcon);
            }
            public static string GetFileTypeDescription(string fileNameOrExtension)
            {
                var shinfo = new SHFILEINFO();
                if (IntPtr.Zero != SHGetFileInfo(
                                    fileNameOrExtension,
                                    FILE_ATTRIBUTE_NORMAL,
                                    ref shinfo,
                                    (uint)Marshal.SizeOf(typeof(SHFILEINFO)),
                                    SHGFI_USEFILEATTRIBUTES | SHGFI_TYPENAME))
                {
                    return shinfo.szTypeName;
                }
                return null;
            }


            private const uint FILE_ATTRIBUTE_READONLY = 0x00000001;
            private const uint FILE_ATTRIBUTE_HIDDEN = 0x00000002;
            private const uint FILE_ATTRIBUTE_SYSTEM = 0x00000004;
            private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
            private const uint FILE_ATTRIBUTE_ARCHIVE = 0x00000020;
            private const uint FILE_ATTRIBUTE_DEVICE = 0x00000040;
            private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
            private const uint FILE_ATTRIBUTE_TEMPORARY = 0x00000100;
            private const uint FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200;
            private const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400;
            private const uint FILE_ATTRIBUTE_COMPRESSED = 0x00000800;
            private const uint FILE_ATTRIBUTE_OFFLINE = 0x00001000;
            private const uint FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000;
            private const uint FILE_ATTRIBUTE_ENCRYPTED = 0x00004000;
            private const uint FILE_ATTRIBUTE_VIRTUAL = 0x00010000;

            private const uint SHGFI_ICON = 0x000000100;     // get icon
            private const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
            private const uint SHGFI_TYPENAME = 0x000000400;     // get type name
            private const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
            private const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
            private const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
            private const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
            private const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
            private const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
            private const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
            private const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
            private const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
            private const uint SHGFI_OPENICON = 0x000000002;     // get open icon
            private const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
            private const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
            private const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
        }

        #endregion
    }

}