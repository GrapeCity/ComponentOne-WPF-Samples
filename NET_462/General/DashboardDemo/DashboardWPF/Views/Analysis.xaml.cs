using C1.WPF.Maps;
using DashboardModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DashboardWPF
{
    /// <summary>
    /// Interaction logic for Analysis.xaml
    /// </summary>
    public partial class Analysis : Page
    {
        C1VectorLayer vectorLayer;
        bool isScale;

        public Analysis()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            vectorLayer = new C1VectorLayer();
            Map.Source = new VirtualEarthRoadSource();
            Map.Layers.Add(vectorLayer);
            var engine = OlapPanel.OlapEngine;
            engine.DataSource = DataService.GetService().ProductWiseSaleCollection;
            OlapGrid.DataSource = OlapPanel;
            engine.RowFields.Add("Category");
            engine.RowFields.Add("Product");
            engine.Fields["Product"].Width = 200;
            engine.ColumnFields.Add("Region");
            engine.ValueFields.Add("Sale");
            engine.ValueField.Format = "C";
        }

        private void OnDateRangeChanged(object sender, DateRangeChangedEventArgs e)
        {
            DataService.GetService().DateRange = e.NewValue;
            FunelChart.ItemsSource = DataService.GetService().OpportunityItemList;
            C1MapHelper.UpdataMapMark(vectorLayer);
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            isScale = !isScale;
            ScaleColumn.Width = isScale ? new GridLength(0) : new GridLength(3, GridUnitType.Star);
            ScaleRow.Height = isScale ? new GridLength(0) : new GridLength(3, GridUnitType.Star);
            Scale.Visibility = isScale ? Visibility.Collapsed : Visibility.Visible;
            Close.Visibility = isScale ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
