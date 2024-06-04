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
using System.Data;
using System.Reflection;
using C1.C1Zip;
using System.Xml;
using C1.WPF;

namespace CustomizePage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string VIEWDEF_KEY = "C1OlapViewDefinition";
        C1MenuItem collapseAllView = new C1MenuItem();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // load data from embedded zip resource
            var ds = new DataSet();
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("CustomizePage.Resources.nwind.zip"))
            {
                var zip = new C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    // load data
                    ds.ReadXml(zr);
                }
            }

            // bind olap page to data
            _c1OlapPage.DataSource = ds.Tables[0].DefaultView;

            // view not found in storage, use default
            var olap = _c1OlapPage.OlapPanel.OlapEngine;
            olap.DataSource = ds.Tables[0].DefaultView;
            olap.BeginUpdate();
            olap.RowFields.Add("Country");
            olap.ColumnFields.Add("Category");
            olap.ValueFields.Add("Sales");
            olap.Fields["Sales"].Format = "n0";
            olap.EndUpdate();

            // get predefined views from XML resource
            var views = new Dictionary<string, string>();
            using (var s = asm.GetManifestResourceStream("CustomizePage.Resources.OlapViews.xml"))
            using (var reader = XmlReader.Create(s))
            {
                // read predefined view definitions
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "C1OlapPage")
                    {
                        var id = reader.GetAttribute("id");
                        var def = reader.ReadOuterXml();
                        views[id] = def;
                    }
                }
            }

            // build new menu with predefined views
            var menuViews = new C1MenuItem();
            menuViews.Header = "View";
            menuViews.Icon = GetImage("Resources/views.png");
            menuViews.VerticalAlignment = VerticalAlignment.Center;
            ToolTipService.SetToolTip(menuViews, "Select a predefined Olap view.");
            foreach (var id in views.Keys)
            {
                var mi = new C1MenuItem();
                mi.Header = id;
                mi.Tag = views[id];
                mi.Click += mi_Click;
                menuViews.Items.Add(mi);
            }

            // add new menu to the page's main menu
            _c1OlapPage.MainMenu.Items.Insert(6, menuViews);

            _c1OlapPage.Updated += c1OlapPage1_Updated;

            // add collapseall menu to page's main menu
            collapseAllView.Header = "CollapseAll";
            collapseAllView.Icon = GetImage("Resources/collapseAll.png");
            collapseAllView.VerticalAlignment = VerticalAlignment.Center;
            ToolTipService.SetToolTip(collapseAllView, "Collapse all the subtotals rows and columns.");
            collapseAllView.Click += collapseAllView_Click;
            _c1OlapPage.MainMenu.Items.Insert(11, collapseAllView);
        }

        void collapseAllView_Click(object sender, SourcedEventArgs e)
        {
            _c1OlapPage.OlapGrid.CollapseAllCols();
            _c1OlapPage.OlapGrid.CollapseAllRows();
        }

        // apply a predefined view
        void mi_Click(object sender, SourcedEventArgs e)
        {
            var mi = sender as C1MenuItem;
            var viewDef = mi.Tag as string;
            _c1OlapPage.ViewDefinition = viewDef;
        }

        void c1OlapPage1_Updated(object sender, EventArgs e)
        {
            // update button status of collapseAllView.
            if (_c1OlapPage.ShowTotalsColumns == C1.Olap.ShowTotals.Subtotals || _c1OlapPage.ShowTotalsRows == C1.Olap.ShowTotals.Subtotals)
                collapseAllView.IsEnabled = true;
            else
                collapseAllView.IsEnabled = false;
        }

        // regenerate the olap view
        void Button_Click(object sender, RoutedEventArgs e)
        {
            _c1OlapPage.OlapPanel.OlapEngine.Update();
        }

        // utility to load an image from a URI
        static Image GetImage(string name)
        {
            var uri = new Uri(name, UriKind.Relative);
            var img = new Image();
            img.Source = new BitmapImage(uri);
            img.Stretch = Stretch.None;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            return img;
        }
    }
}
