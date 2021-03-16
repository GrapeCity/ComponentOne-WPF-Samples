using C1.FlexPivot;
using C1.WPF.Core;
using C1.WPF.Menu;
using C1.WPF.Ribbon;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for CustomPage.xaml
    /// </summary>
    public partial class CustomPage : UserControl
    {
        C1ButtonTool collapseAllView;
        bool isLoaded = false;

        public CustomPage()
        {
            InitializeComponent();
            Tag = Properties.Resources.Page;
            pivotPage.Loaded += (s, ea) =>
            {
                if (!pivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;                

                // view not found in storage, use default
                var fpEngine = pivotPage.FlexPivotEngine;
                fpEngine.DataSource = Utils.PivotDataSet.Tables[0].DefaultView;
                fpEngine.BeginUpdate();
                fpEngine.RowFields.Add("Country", "City");
                fpEngine.ColumnFields.Add("Salesperson");
                fpEngine.ValueFields.Add("ExtendedPrice");
                fpEngine.Fields["ExtendedPrice"].Format = "n0";
                fpEngine.ShowTotalsRows = ShowTotals.Subtotals;
                pivotPage.Updated += FlexPivotPage_Updated;

                fpEngine.EndUpdate();

                // get predefined views from XML resource
                var views = Utils.PivotViews;

                // build new menu with predefined views
                var menuViews = new C1MenuTool();
                menuViews.Label = "Views";
                menuViews.IconTemplate = C1IconTemplate.Edit;
                menuViews.VerticalAlignment = VerticalAlignment.Center;
                ToolTipService.SetToolTip(menuViews, "Select a predefined FlexPivot view.");
                foreach (var id in views.Keys)
                {
                    var mi = new C1ButtonTool();
                    mi.Label = id;
                    mi.Padding = new Thickness(5);
                    mi.Tag = views[id];
                    mi.Click += mi_Click;
                    menuViews.Tools.Add(mi);
                }

                // add new menu to the page's main menu
                pivotPage.MainMenu.Items.Insert(6, menuViews);

                // add collapseall menu to page's main menu
                collapseAllView = new C1ButtonTool();
                collapseAllView.Label = "CollapseAll";
                collapseAllView.IconTemplate = C1IconTemplate.ArrowDown;
                collapseAllView.VerticalAlignment = VerticalAlignment.Center;
                ToolTipService.SetToolTip(collapseAllView, "Collapse all the subtotals rows and columns.");
                collapseAllView.Click += collapseAllView_Click;
                pivotPage.MainMenu.Items.Insert(11, collapseAllView);
            };
        }

        void collapseAllView_Click(object sender, RoutedEventArgs e)
        {
            pivotPage.FlexPivotGrid.CollapseAllCols();
            pivotPage.FlexPivotGrid.CollapseAllRows();
        }

        // apply a predefined view
        void mi_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as C1ButtonTool;
            var viewDef = mi.Tag as string;
            pivotPage.ViewDefinition = viewDef;
        }

        void FlexPivotPage_Updated(object sender, EventArgs e)
        {
            // update button status of collapseAllView.
            if (pivotPage.ShowTotalsColumns == ShowTotals.Subtotals || pivotPage.ShowTotalsRows == ShowTotals.Subtotals)
                collapseAllView.IsEnabled = true;
            else
                collapseAllView.IsEnabled = false;
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
