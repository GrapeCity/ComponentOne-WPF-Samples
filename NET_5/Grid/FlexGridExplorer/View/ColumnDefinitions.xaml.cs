﻿using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System.Collections.Generic;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for ColumnDefinitions.xaml
    /// </summary>
    public partial class ColumnDefinitions : UserControl
    {
        public ColumnDefinitions()
        {
            InitializeComponent();
            Tag = AppResources.ColumnDefinitionDescription;
            var data = Customer.GetCustomerList(1000);
            grid.ItemsSource = data;
            grid.Columns.RemoveAt(1);
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
