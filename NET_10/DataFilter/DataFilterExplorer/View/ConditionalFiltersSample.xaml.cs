using C1.DataCollection;
using C1.WPF.Accordion;
using C1.WPF.DataFilter;
using DataFilterExplorer.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DataFilterExplorer
{
    /// <summary>
    /// Interaction logic for ConditionalFiltersSample.xaml
    /// </summary>
    public partial class ConditionalFiltersSample : UserControl
    {
        private readonly string _fileName = "ConditionalFilterExpressions.xml";

        private DataTable _carsTable;
        public ConditionalFiltersSample()
        {
            InitializeComponent();
            Tag = AppResources.ConditionalFiltersDescription;
            //Get Cars list
            _carsTable = DataProvider.GetCarTable();
            var data = new C1DataCollection<Car>(DataProvider.GetCarDataCollection(_carsTable));
            c1DataFilter.ItemsSource = data;
            flexGrid.ItemsSource = data;
            if (File.Exists(_fileName))
            {
                c1DataFilter.LoadFilterExpression(_fileName);
            }
        }

        private async void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await c1DataFilter.ApplyFilterAsync();
        }

        private void BtnSaveFilter_Click(object sender, RoutedEventArgs e)
        {
            c1DataFilter.SaveFilterExpression(_fileName);
        }

        private void CbAutoApply_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (c1DataFilter != null)
            {
                c1DataFilter.AutoApply = cbAutoApply.IsChecked == true;
            }
        }

        private void FlexGridOnAutoGeneratingColumn(object sender, C1.WPF.Grid.GridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Binding == nameof(Car.DateProductionLine))
            {
                e.Column.Format = "g";
            }
            else if (e.Column.Binding == nameof(Car.PresentationDate))
            {
                e.Column.Format = "MM/dd/yy H:mm:ss zzz";
            }
            else if (e.Column.Binding == nameof(Car.FuelConsumption))
            {
                e.Column.Format = "hh:mm";
            }
        }
    }
}
