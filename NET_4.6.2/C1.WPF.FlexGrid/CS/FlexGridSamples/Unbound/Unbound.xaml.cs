using System.Windows;
using System.Windows.Controls;
using C1.WPF.FlexGrid;
using System.Collections.ObjectModel;

namespace MainTestApplication
{
    /// <summary>
    /// Interaction logic for Unbound.xaml
    /// </summary>
    public partial class Unbound : UserControl
    {
        const int ROW_COUNT = 500;

        public Unbound()
        {
            InitializeComponent();
            PopulateUnboundGrid();
        }

        // fill unbound grid with data
        void PopulateUnboundGrid()
        {
            var data = new ObservableCollection<Customer>();
            for (int i = 0; i < ROW_COUNT; i++)
                data.Add(new Customer());
            var view = new MyCollectionView(data);
            // allow merging
            var fg = _flexUnbound;
            fg.AllowMerging = AllowMerging.All;
            // add rows/columns to the unbound grid
            for (int i = 0; i < 10; i++)
            {
                fg.Columns.Add(new Column());
            }
            for (int i = 0; i < 50; i++)
            {
                fg.Rows.Add(new Row());
            }

            // populate the unbound grid with some stuff
            for (int r = 0; r < fg.Rows.Count; r++)
            {
                for (int c = 0; c < fg.Columns.Count; c += 10)
                {
                    fg[r, c + 0] = data[r].Name;
                    fg[r, c + 1] = data[r].Country;
                    fg[r, c + 2] = data[r].CountryID;
                    fg[r, c + 3] = data[r].Active;
                    fg[r, c + 4] = data[r].First;
                    fg[r, c + 5] = data[r].Last;
                    fg[r, c + 6] = data[r].Hired.ToShortDateString();
                    fg[r, c + 7] = data[r].Father;
                    fg[r, c + 8] = data[r].Brother;
                    fg[r, c + 9] = data[r].Cousin;
                }
            }

            // add some group rows above the data
            for (int offset = 0; offset < fg.Rows.Count - 10; offset += 10)
            {
                for (int i = 0; i < 3; i++)
                {
                    fg.Rows.Insert(offset + i, new GroupRow() { Level = i });
                    fg[offset + i, 0] = string.Format("level {0}", i);
                }
            }

            // set unbound column headers
            var ch = fg.ColumnHeaders;
            ch.Rows.Add(new Row());
            for (int c = 0; c < ch.Columns.Count; c++)
            {
                ch[0, c] = "Customer Infomation";
            }
            ch[1, 0] = "Name";
            ch[1, 1] = "Country";
            ch[1, 2] = "CountryID";
            ch[1, 3] = "Active";
            ch[1, 4] = "First";
            ch[1, 5] = "Last";
            ch[1, 6] = "Hired";
            ch[1, 7] = "Father";
            ch[1, 8] = "Brother";
            ch[1, 9] = "Cousin";

            // allow merging the first fixed row
            ch.Rows[0].AllowMerging = true;

            // set unbound row headers
            var rh = fg.RowHeaders;
            //rh.Columns.Add(new Column());
            for (int c = 0; c < rh.Columns.Count; c++)
            {
                rh.Columns[c].Width = new GridLength(60);
                for (int r = 0; r < rh.Rows.Count; r++)
                {
                    if (!(rh.Rows[r] is GroupRow))
                    {
                        int level = (r % 3 > 1) ? 1 : 2;
                        rh[r, c] = "VIP" + level.ToString();
                    }
                }
            }

            // allow merging the first fixed column
            rh.Columns[0].AllowMerging = true;

            // listen to column resized event
            _flexUnbound.ResizedColumn += _flexUnbound_ResizedColumn;
        }

        // make all unbound grid columns the same width after they are resized
        void _flexUnbound_ResizedColumn(object sender, CellRangeEventArgs e)
        {
            var col = _flexUnbound.Columns[e.Column];
            if (col.ActualWidth > 0)
            {
                _flexUnbound.Columns.DefaultSize = col.ActualWidth;
            }
            col.Width = new GridLength(1, GridUnitType.Auto);
        }
    }
}
