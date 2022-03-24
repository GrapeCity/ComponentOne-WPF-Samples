using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using System.Windows.Data;
using System.Globalization;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Legend : UserControl
    {
        List<FruitsDataItem> _fruits;
        List<string> _legendPostion;
        List<string> _legendOrientation;
        List<string> _legendTextWrapping;
        List<LegendMaxWidthItem> _legendMaxWidth;

        public Legend()
        {
            this.InitializeComponent();
        }

        public List<FruitsDataItem> Data
        {
            get
            {
                if (_fruits == null)
                {
                    _fruits = DataCreator.CreateFruits();
                }

                return _fruits;
            }
        }

        public List<string> LegendPosition
        {
            get
            {

                if (_legendPostion == null)
                {
                    _legendPostion = Enum.GetNames(typeof(Position)).ToList();

                }
                return _legendPostion;
            }
        }

        public List<string> LegendOrientation
        {
            get
            {
                if (_legendOrientation == null)
                {
                    _legendOrientation = Enum.GetNames(typeof(C1.Chart.Orientation)).ToList();    
                }

                return _legendOrientation;
            }
        }

        public List<string> LegendTextWrapping
        {
            get
            {
                if (_legendTextWrapping == null)
                {
                    _legendTextWrapping = Enum.GetNames(typeof(TextWrapping)).ToList();
                }

                return _legendTextWrapping;
            }
        }

        public List<LegendMaxWidthItem> LegendMaxWidth
        {
            get
            {
                if (_legendMaxWidth == null)
                {
                    _legendMaxWidth = new List<LegendMaxWidthItem>();
                    _legendMaxWidth.Add(new LegendMaxWidthItem() { Name = "Narrow", Value = 80 });
                    _legendMaxWidth.Add(new LegendMaxWidthItem() { Name = "Middle", Value = 180 });
                    _legendMaxWidth.Add(new LegendMaxWidthItem() { Name = "Wide", Value = 360 });
                }

                return _legendMaxWidth;
            }
        }

        private void cbCheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (cbCheckBox.IsChecked.HasValue && flexChart != null)
            {
                foreach (ISeries ser in flexChart.Series)
                {
                    int start = ser.Name.IndexOf("The quick ") + "The quick ".Length;
                    int end = ser.Name.IndexOf(" jumps over");
                    string groupName = ser.Name.Substring(start, end - start);

                    ser.LegendGroup = cbCheckBox.IsChecked == false ? null : groupName;
                }
            }
        }

    }

    public class LegendMaxWidthItem
    {
        public string Name
        {
            get;
            set;
        }
        public int Value
        {
            get;
            set;
        }
    }


    public class MaxWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as LegendMaxWidthItem;

            return item == null? 0: item.Value;
        }
    }
}
