using FlexChartExplorer.Resources;
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

namespace FlexChartExplorer
{
    /// <summary>
    /// Interaction logic for Waterfall.xaml
    /// </summary>
    public partial class Waterfall : UserControl
    {
        List<WaterfallItem> _data;

        public Waterfall()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
            Tag = AppResources.WaterfallTag;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            wf.IntermediateTotalPositions = new List<int> { 3, 6, 9, 12 };
        }

        public List<WaterfallItem> Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateWaterfallData();
                }

                return _data;
            }
        }
    }
}
