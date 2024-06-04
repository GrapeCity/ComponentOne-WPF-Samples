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
using C1.WPF.FlexGrid;

namespace CustomMerging
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // create the C1FlexGrid
            var flex = new C1FlexGrid();
            LayoutRoot.Children.Add(flex);

            // change appearance a little
            flex.RowBackground = new SolidColorBrush(Color.FromArgb(0xff, 255, 215, 0));
            flex.AlternatingRowBackground = flex.RowBackground;
            flex.GridLinesVisibility = GridLinesVisibility.All;
            flex.Rows.DefaultSize = 60;
            flex.Columns.DefaultSize = 100;
            flex.RowHeaders.Columns.DefaultSize = 80;

            flex.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            flex.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            flex.ColumnHeaders.Rows[0].HeaderHorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            flex.RowHeaders.Columns[0].HeaderHorizontalAlignment = System.Windows.HorizontalAlignment.Right;

            // activate custom merge manager
            flex.AllowMerging = AllowMerging.All;
            flex.MergeManager = new MyMergeManager();

            // add some unbound rows and columns
            for (int cols = 0; cols < 7; cols++)
            {
                flex.Columns.Add(new Column() { HorizontalAlignment = HorizontalAlignment.Center });
            }
            for (int rows = 0; rows < 5; rows++)
            {
                flex.Rows.Add(new Row() { VerticalAlignment = VerticalAlignment.Center });
            }

            // set column headers
            SetData(flex.ColumnHeaders.Rows[0], "Monday\tTuesday\tWednesday\tThursday\tFriday\tSaturday\tSunday");

            // add data
            SetData(flex, 0, "12:00", "Walker\tMorning Show\tMorning Show\tSport\tWeather\tN/A\tN/A");
            SetData(flex, 1, "13:00", "Today Show\tToday Show\tSesame Street\tFootball\tMarket Watch\tN/A\tN/A");
            SetData(flex, 2, "14:00", "Today Show\tToday Show\tKid Zone\tFootball\tSoap Opera\tN/A\tN/A");
            SetData(flex, 3, "15:00", "News\tNews\tNews\tNews\tNews\tN/A\tN/A");
            SetData(flex, 4, "16:00", "News\tNews\tNews\tNews\tNews\tN/A\tN/A");
        }
        void SetData(Row row, string content)
        {
            var items = content.Split('\t');
            for (int i = 0; i < items.Length; i++)
            {
                row[i] = items[i];
            }
        }
        void SetData(C1FlexGrid flex, int row, string rowHeader, string content)
        {
            flex.RowHeaders[row, 0] = rowHeader;
            SetData(flex.Rows[row], content);
        }
    }
}
