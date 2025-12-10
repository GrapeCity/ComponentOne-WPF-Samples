using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using System.IO;
using System.Xml.Linq;
using System.Windows.Media.Imaging;
using System.Windows;
using System.ComponentModel;

namespace FlexViewerExplorer
{
    public class Group
    {
        public Category Category { get; set; }
        public List<Report> Reports { get; set; }
    }

    public abstract class ItemBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, e);
            }
        }
    }

    #region Categories
    public class Category : ItemBase
    {
        #region Variables
        string _name = "";
        string _text = "";
        string _imgUrl = "";
        bool _isExpanded = false;
        ObservableCollection<Report> _reports = null;
        #endregion

        #region Ctors
        public Category()
        { }
        public Category(string name, string text, string imgUrl, ObservableCollection<Report> reports)
        {
            _name = name;
            _text = text;
            _imgUrl = imgUrl;
            _reports = reports;
        }
        #endregion

        #region Properties

        public bool IsExpanded
        {
            get
            {
                return _isExpanded;
            }
            set
            {
                _isExpanded = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs("IsExpanded"));
            }
        }

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
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
        public string ImageUrl
        {
            get
            {
                return _imgUrl;
            }
            set
            {
                _imgUrl = value;
            }
        }
        #endregion

        #region PublicMethods
        public ObservableCollection<Report> ReportsList
        {
            get
            {

                if (_reports == null) _reports = new ObservableCollection<Report>();
                return _reports;
            }
            set { _reports = value; }
        }

        #endregion

        #region static method
        public static ObservableCollection<Category> GetAll()
        {

            ObservableCollection<Category> listToReturn = new ObservableCollection<Category>();
            Assembly thisExe = Assembly.GetExecutingAssembly();
            string[] resList = thisExe.GetManifestResourceNames();
            Stream file = thisExe.GetManifestResourceStream("FlexViewerExplorer.Resources.ReportInfos.xml");
            XDocument xdoc = XDocument.Load(file);
            List<XElement> categoriesList = xdoc.Descendants("Category").ToList<XElement>();

            foreach (XElement xelemCategory in categoriesList)
            {
                Category category = new Category
                {
                    Name = xelemCategory.Attribute("Name").Value,
                    Text = xelemCategory.Attribute("Text").Value,
                };
                try
                {
                    category.ImageUrl = @"..\Resources\" + xelemCategory.Attribute("Image").Value + ".png";
                }
                catch { category.ImageUrl = null; }
                foreach (XElement xelemReport in xelemCategory.Descendants("Report").ToList<XElement>())
                {
                    string rptName = xelemReport.Descendants("ReportName").FirstOrDefault().Value;
                    string fileName = @"..\Reports\" + category.Name + @"\" + xelemReport.Descendants("FileName").FirstOrDefault().Value;
                    XElement eleSelected =  xelemReport.Descendants("IsSelected").FirstOrDefault();
                    bool isSelected =(eleSelected==null)?false: Convert.ToBoolean(eleSelected.Value);
                    BitmapSource imgUrl = ToBitmapSource(@"..\Reports\" + category.Name + @"\Images\" + xelemReport.Descendants("ImageName").FirstOrDefault().Value);
                    string rptTitle = xelemReport.Descendants("ReportTitle").FirstOrDefault().Value;
                    category.ReportsList.Add(new Report(rptName, fileName, rptTitle, imgUrl, isSelected, category));
                }
                listToReturn.Add(category);
            }
            return listToReturn;
        }
        private static BitmapSource ToBitmapSource(string path)
        {
            try
            {
                if (!File.Exists(path))
                    path = path.Substring(path.IndexOf("Reports"));
                if (!File.Exists(path))
                    return null;

                using (System.Drawing.Imaging.Metafile emf = new System.Drawing.Imaging.Metafile(path))
                {
                    using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(emf.Width, emf.Height))
                    {
                        bmp.SetResolution(emf.HorizontalResolution, emf.VerticalResolution);
                        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bmp))
                        {
                            g.DrawImage(emf, 0, 0);
                            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                        }
                    }
                }
            }
            catch (Exception) { return null; }
        }
        #endregion     
    }
    #endregion

    #region Reports
    public class Report : ItemBase
    {
        #region variables
        string _rptName = "";
        string _fileName = "";
        BitmapSource _imgUrl = null;
        string _rptTitle = "";
        bool _isSelected = false;
        #endregion

        #region Ctors
        public Report()
        {
        }
        public Report(string rptName, string fileName, string rptTitle, BitmapSource imgUrl, bool isSelected, Category category)
        {
            _rptName = rptName;
            _fileName = fileName;
            _rptTitle = rptTitle;
            _imgUrl = imgUrl;
            _isSelected = isSelected;
            Category = category;
        }
        #endregion

        #region Properties
        public Category Category { get; set; }
        public string RptName
        {
            get
            {
                return _rptName;
            }
            set
            {
                _rptName = value;
            }
        }
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }
        public string RptTitle
        {
            get
            {
                return _rptTitle;
            }
            set
            {
                _rptTitle = value;
            }
        }
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
            }
        }
        public BitmapSource ImageUrl
        {
            get
            {
                return _imgUrl;
            }
            set
            {
                _imgUrl = value;
            }
        }
        #endregion
    }
    #endregion
}
