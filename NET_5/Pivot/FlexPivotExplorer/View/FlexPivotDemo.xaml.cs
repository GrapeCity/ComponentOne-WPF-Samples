using C1.WPF.Core;
using C1.WPF.Ribbon;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for FlexPivotDemo.xaml
    /// </summary>
    public partial class FlexPivotDemo : UserControl
    {
        bool isLoaded = false;

        public FlexPivotDemo()
        {
            InitializeComponent();
            Tag = Properties.Resources.Pivot;

            flexPivotPage.Loaded += (s, ea) =>
            {
                if (!flexPivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                var fpEngine = flexPivotPage.FlexPivotPanel.FlexPivotEngine;
                // apply update to view
                fpEngine.BeginUpdate();
                fpEngine.DataSource = Utils.PivotDataSet.Tables[0].DefaultView;
                // set value field limit to 4 (**default is 1)
                fpEngine.ValueFields.MaxItems = 4;

                //// create conditional formatting
                //fpEngine.Fields["ExtendedPrice"].StyleHigh.ForeColor = System.Drawing.Color.Green;
                //fpEngine.Fields["ExtendedPrice"].StyleHigh.ConditionType = C1.FlexPivot.ConditionType.Percentage;
                //fpEngine.Fields["ExtendedPrice"].StyleHigh.Value = 0.8;
                //fpEngine.Fields["ExtendedPrice"].StyleLow.ForeColor = System.Drawing.Color.Red;
                //fpEngine.Fields["ExtendedPrice"].StyleLow.ConditionType = C1.FlexPivot.ConditionType.Percentage;
                //fpEngine.Fields["ExtendedPrice"].StyleLow.Value = 0.1;

                fpEngine.Fields["ExtendedPrice"].Caption = "Sales";
                fpEngine.RowFields.Add("Country");
                fpEngine.ColumnFields.Add("Salesperson");
                fpEngine.ValueFields.Add("ExtendedPrice");
                fpEngine.Fields["ExtendedPrice"].Format = "c0";
                fpEngine.EndUpdate();

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
                flexPivotPage.MainMenu.Items.Insert(6, menuViews);
            };
        }

        // apply a predefined view
        void mi_Click(object sender, RoutedEventArgs e)
        {
            var mi = sender as C1ButtonTool;
            var viewDef = mi.Tag as string;
            txtStatus.Text = mi.Label.ToString();
            flexPivotPage.ViewDefinition = viewDef;
        }

        // utility to load an image from a URI
        static System.Windows.Controls.Image GetImage(string name)
        {
            var uri = new Uri(name, UriKind.Relative);
            var img = new System.Windows.Controls.Image();
            img.Source = new BitmapImage(uri);
            img.Stretch = Stretch.None;
            img.VerticalAlignment = VerticalAlignment.Center;
            img.HorizontalAlignment = HorizontalAlignment.Center;
            return img;
        }
    }
}
