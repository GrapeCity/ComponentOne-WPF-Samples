using C1.WPF;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace OlapSamples
{
    /// <summary>
    /// Interaction logic for OlapDemo.xaml
    /// </summary>
    public partial class OlapDemo : UserControl
    {
        public OlapDemo()
        {
            InitializeComponent();
            this.Loaded += OlapDemo_Loaded;
        }

        void OlapDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // load data from embedded zip resource
            var ds = new DataSet();
            var asm = Assembly.GetExecutingAssembly();
            using (var s = asm.GetManifestResourceStream("OlapSamples.Resources.nwind.zip"))
            {
                var zip = new C1.Zip.C1ZipFile(s);
                using (var zr = zip.Entries[0].OpenReader())
                {
                    // load data
                    ds.ReadXml(zr);
                }
            }

            // bind olap page to data
            olapPage.DataSource = ds.Tables[0].DefaultView;

            // view not found in storage, use default
            var olap = olapPage.OlapPanel.OlapEngine;
            olap.DataSource = ds.Tables[0].DefaultView;

            // set value field limit to 4 (**default is 1)
            olap.ValueFields.MaxItems = 4;

            // create conditional formatting
            olap.Fields["ExtendedPrice"].StyleHigh.ForeColor = Colors.Green;
            olap.Fields["ExtendedPrice"].StyleHigh.ConditionType = C1.Olap.ConditionType.Percentage;
            olap.Fields["ExtendedPrice"].StyleHigh.Value = 0.8;
            olap.Fields["ExtendedPrice"].StyleLow.ForeColor = Colors.Red;
            olap.Fields["ExtendedPrice"].StyleLow.ConditionType = C1.Olap.ConditionType.Percentage;
            olap.Fields["ExtendedPrice"].StyleLow.Value = 0.1;

            // apply update to view
            olap.BeginUpdate();
            olap.Fields["ExtendedPrice"].Caption = "Sales";
            olap.RowFields.Add("Country");
            olap.ColumnFields.Add("Salesperson");
            olap.ValueFields.Add("ExtendedPrice");
            olap.Fields["ExtendedPrice"].Format = "c0";
            olap.EndUpdate();

            // get predefined views from XML resource
            var views = new Dictionary<string, string>();
            using (var s = asm.GetManifestResourceStream("OlapSamples.Resources.OlapViews.xml"))
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
            menuViews.Header = "Views";
            menuViews.Icon = GetImage("/Resources/Views.png");
            menuViews.VerticalAlignment = VerticalAlignment.Center;
            ToolTipService.SetToolTip(menuViews, "Select a predefined Olap view.");
            foreach (var id in views.Keys)
            {
                var mi = new C1MenuItem();
                mi.Header = id;
                mi.Padding = new Thickness(5);
                mi.Tag = views[id];
                mi.Click += mi_Click;
                menuViews.Items.Add(mi);
            }

            // add new menu to the page's main menu
            olapPage.MainMenu.Items.Insert(6, menuViews);
        }

        // apply a predefined view
        void mi_Click(object sender, SourcedEventArgs e)
        {
            var mi = sender as C1MenuItem;
            var viewDef = mi.Tag as string;
            txtStatus.Text = mi.Header.ToString();
            olapPage.ViewDefinition = viewDef;
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
