using C1.Chart;
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
    /// Interaction logic for BoxWhisker.xaml
    /// </summary>
    public partial class ErrorBar : UserControl
    {
        List<string> _chartTypes;
        List<string> _errorAmounts;

        private Sale[] _data = null;
        private List<string> _directions;
        private List<string> _endStyles;

        public ErrorBar()
        {
            InitializeComponent();
            Tag = AppResources.ErrorBarTag;
        }

        public List<string> ChartTypes
        {
            get
            {
                if (_chartTypes == null)
                {
                    _chartTypes = DataCreator.CreateChartTypesForErrorBar();
                }

                return _chartTypes;
            }
        }

        public List<string> ErrorAmounts
        {
            get
            {
                if (_errorAmounts == null)
                {
                    _errorAmounts = DataCreator.CreateErrorAmounts();
                }
                return _errorAmounts;
            }
        }

        public List<string> Directions
        {
            get
            {
                if(_directions == null)
                {
                    _directions = DataCreator.CreateErrorBarDirections();
                }

                return _directions;
            }
        }

        public List<string> EndStyles
        {
            get
            {
                if(_endStyles == null)
                {
                    _endStyles = DataCreator.CreateErrorBarEndStyles();
                }

                return _endStyles;
            }
        }


        public Sale[] Data
        {
            get
            {
                if (_data == null)
                {
                    _data = DataCreator.CreateSales();
                }

                return _data;
            }
        }

        private void cboErrorAmounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && errorBarSeries != null)
            {
                var value = (ErrorAmount)Enum.Parse(typeof(ErrorAmount), e.AddedItems[0].ToString());
                switch (value)
                {
                    case ErrorAmount.FixedValue:
                        errorBarSeries.ErrorValue = 5;
                        break;
                    case ErrorAmount.Percentage:
                        errorBarSeries.ErrorValue = 0.1;
                        break;
                    case ErrorAmount.StandardDeviation:
                        errorBarSeries.ErrorValue = 1;
                        break;
                    case ErrorAmount.Custom:
                        errorBarSeries.CustomMinusErrorValue = 3;
                        errorBarSeries.CustomPlusErrorValue = 5;
                        break;
                }
            }
        }
    }
}
