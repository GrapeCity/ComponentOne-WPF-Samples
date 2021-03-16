using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for MouseHover.xaml
    /// </summary>
    public partial class MouseHover : UserControl
    {
        public MouseHover()
        {
            InitializeComponent();
            Tag = AppResources.MouseHoverDescription;

            grid.ItemsSource = Customer.GetCustomerList(1000);
            //grid.Columns.RemoveAt(1);
            Dictionary<int, string> dct = new Dictionary<int, string>();
            foreach (var country in Customer.GetCountries())
            {
                dct[dct.Count] = country.Value;
            }
            grid.Columns["CountryID"].DataMap = new GridDataMap { ItemsSource = dct, SelectedValuePath = "Key", DisplayMemberPath = "Value" };
            grid.MinColumnWidth = 85;
        }
    }
}
