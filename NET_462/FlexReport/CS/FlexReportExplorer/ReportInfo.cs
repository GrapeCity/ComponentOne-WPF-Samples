
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

namespace C1FlexReportExplorer
{
    #region Categories
    public class Categories : INotifyPropertyChanged
    {
        #region Variables
        public event PropertyChangedEventHandler PropertyChanged;
        string _name = "";
        string _text = "";
        string _imgUrl = "";
        string _expenderImg = "";
        ObservableCollection<Reports> _reports = null;
        #endregion

        #region Ctors
        public Categories()
        { }
        public Categories(string name, string text, string imgUrl, ObservableCollection<Reports> reports)
        {
            _name = name;
            _text = text;
            _imgUrl = imgUrl;
            _reports = reports;
        }
        #endregion

        #region Properties
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
        public string ExpenderImg
        {
            get
            {
                return _expenderImg;
            }
            set
            {
                if (_expenderImg != value)
                {
                    _expenderImg = value;
                    OnPropertyChanged(this, new PropertyChangedEventArgs("ExpenderImg"));
                }
            }
        }
        #endregion

        #region PublicMethods
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(sender, e);
            }
        }
        public ObservableCollection<Reports> ReportsList
        {
            get
            {

                if (_reports == null) _reports = new ObservableCollection<Reports>();
                return _reports;
            }
            set { _reports = value; }
        }

        #endregion

        #region static method
        public static ObservableCollection<Categories> GetAll()
        {

            ObservableCollection<Categories> listToReturn = new ObservableCollection<Categories>();
            Assembly thisExe = Assembly.GetExecutingAssembly();
            string[] resList = thisExe.GetManifestResourceNames();
            Stream file = thisExe.GetManifestResourceStream("C1FlexReportExplorer.Resources.ReportInfos.xml");
            XDocument xdoc = XDocument.Load(file);
            List<XElement> categoriesList = xdoc.Descendants("Category").ToList<XElement>();

            foreach (XElement xelemCategory in categoriesList)
            {
                Categories treeItem = new Categories();
                treeItem.Name = xelemCategory.Attribute("Name").Value;
                treeItem.Text = xelemCategory.Attribute("Text").Value;
                treeItem.ExpenderImg = "Resources/expand.png";
                try
                {
                    treeItem.ImageUrl = @"..\..\Resources\" + xelemCategory.Attribute("Image").Value + ".png";
                }
                catch { treeItem.ImageUrl = null; }
                foreach (XElement xelemReport in xelemCategory.Descendants("Report").ToList<XElement>())
                {
                    string rptName = xelemReport.Descendants("ReportName").FirstOrDefault().Value;
                    string fileName = @"..\..\Reports\" + treeItem.Name + @"\" + xelemReport.Descendants("FileName").FirstOrDefault().Value;
                    XElement eleSelected =  xelemReport.Descendants("IsSelected").FirstOrDefault();
                    bool isSelected =(eleSelected==null)?false: Convert.ToBoolean(eleSelected.Value);
                    BitmapSource imgUrl = ToBitmapSource(@"..\..\Reports\" + treeItem.Name + @"\Images\" + xelemReport.Descendants("ImageName").FirstOrDefault().Value);
                    string rptTitle = xelemReport.Descendants("ReportTitle").FirstOrDefault().Value;
                    treeItem.ReportsList.Add(new Reports(rptName, fileName, rptTitle, imgUrl, isSelected));
                }
                listToReturn.Add(treeItem);
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
    public class Reports
    {
        #region variables
        string _rptName = "";
        string _fileName = "";
        BitmapSource _imgUrl = null;
        string _rptTitle = "";
        bool _isSelected = false;
        #endregion

        #region Ctors
        public Reports()
        {
        }
        public Reports(string rptName, string fileName, string rptTitle, BitmapSource imgUrl, bool isSelected)
        {
            _rptName = rptName;
            _fileName = fileName;
            _rptTitle = rptTitle;
            _imgUrl = imgUrl;
            _isSelected = isSelected;
        }
        #endregion

        #region Properties
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
