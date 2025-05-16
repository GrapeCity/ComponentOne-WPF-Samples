using C1.WPF.Core;
using C1.WPF.Grid;
using C1.WPF.Menu;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FlexGridExplorer
{
    public partial class ColumnOptions : UserControl
    {
        public ColumnOptions()
        {
            InitializeComponent();
            Tag = AppResources.ColumnOptionsDescription;

            grid.ItemsSource = Customer.GetCustomerList(100);
            Loaded += ColumnOptions_Loaded;
        }

        private void ColumnOptions_Loaded(object sender, RoutedEventArgs e)
        {
            if(grid.ColumnOptionsMenuVisibility.HasValue)
                Combo.SelectedItem = grid.ColumnOptionsMenuVisibility.Value.ToString();
        }

        private void OnIdColumnOptionsLoading(object sender, GridColumnOptionsLoadingEventArgs e)
        {
            e.Menu.Items.Clear();//Removes default options.

            var menuItem = new C1MenuItem();
            menuItem.Header = "Column options";
            menuItem.Icon = C1IconTemplate.Gear.CreateIcon();
            menuItem.Click += (s, oe) =>
            {
                //Perform some action

                e.Close(); //This closes the menu.
            };

            e.Menu.Items.Add(menuItem);
        }

        private Color?[] _colors = new Color?[] { null, Colors.AliceBlue, Colors.AntiqueWhite, Colors.Azure, Colors.Beige, Colors.Bisque, Colors.BlanchedAlmond, Colors.BurlyWood, Colors.Cornsilk, Colors.LightGoldenrodYellow, Colors.Linen, Colors.MistyRose, Colors.Moccasin, Colors.PapayaWhip, Colors.SeaShell };

        private void OnColumnOptionsLoading(object sender, GridColumnOptionsLoadingEventArgs e)
        {
            if (e.Menu.Items.Count > 0)
                e.Menu.Items.Add(new C1MenuSeparator());

            var menuItem = new C1MenuItem();
            menuItem.Header = "Column background";
            menuItem.Icon = new Border { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Colors.Black), Background = e.Column.Background };
            foreach (var color in _colors)
            {
                var colorMenuItem = new C1MenuItem();
                colorMenuItem.Tag = color;
                colorMenuItem.Header = color?.ToString() ?? "None";
                colorMenuItem.Icon = new Border { BorderThickness = new Thickness(1), BorderBrush = new SolidColorBrush(Colors.Black), Background = color.HasValue ? new SolidColorBrush(color.Value) : null };
                colorMenuItem.Click += (s, oe) =>
                {
                    var color = (Color?)(s as C1MenuItem).Tag;
                    e.Column.Background = color.HasValue ? new SolidColorBrush(color.Value) : null;

                    e.Close(); //This closes the menu.
                };
                menuItem.Items.Add(colorMenuItem);
            }

            e.Menu.Items.Add(menuItem);

        }

        List<string> _columnOptions;
        public List<string> ColumnOption
        {
            get
            {
                if (_columnOptions == null)
                    _columnOptions = Enum.GetNames(typeof(GridColumnOptionsMenuVisibility)).ToList();
                return _columnOptions;
            }
        }

        private void C1ComboBox_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            if (Combo.SelectedItem == null) return;
            grid.ColumnOptionsMenuVisibility
                = (GridColumnOptionsMenuVisibility)Enum.Parse(typeof(GridColumnOptionsMenuVisibility), Combo.SelectedItem.ToString());
        }
    }
}
