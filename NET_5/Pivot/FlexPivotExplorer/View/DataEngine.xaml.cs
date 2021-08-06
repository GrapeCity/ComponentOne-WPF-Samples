using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FlexPivotExplorer
{
    /// <summary>
    /// Interaction logic for DataEngine.xaml
    /// </summary>
    public partial class DataEngine : UserControl
    {
        bool isLoaded = false;
        bool updating;
        DateTime addTime = DateTime.MinValue;
        DateTime updateTime = DateTime.MinValue;
        string dataPath = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        MemoryStream stream;
        RadioButton sel;
        bool firstLoad = true;

        public DataEngine()
        {
            InitializeComponent();
            Tag = Properties.Resources.DataEngine;

            flexPivotPage.Loaded += (s, ea) =>
            {
                if (!flexPivotPage.IsVisible)
                    return;
                if (isLoaded)
                    return;
                isLoaded = true;

                var fpEngine = flexPivotPage.C1PivotEngine;

                firstLoad = true;

                // where DataEngine data is stored
                flexPivotPage.FlexPivotPanel.Workspace.Init(dataPath);

                // show update log
                flexPivotPage.C1PivotEngine.StartUpdating += FlexPivotEngine_StartUpdating; ;
                flexPivotPage.C1PivotEngine.CancelUpdating += FlexPivotEngine_CancelUpdating;
                flexPivotPage.Updated += _c1FlexPivotPage_Updated;
            };
        }

        private void _c1FlexPivotPage_Updated(object sender, EventArgs e)
        {
            string type = GetDataType();
            if (type == null)
                return;

            string time = DateTime.Now.Subtract(updateTime).TotalSeconds.ToString("n2");

            //// update label
            //Control label = sel.Parent.GetNextControl(sel, true);
            //label.Text = string.Format("{0} sec.", time);

            // update log
            int idx = lstBoxLogs.Items.Count - 1;
            lstBoxLogs.Items[idx] = string.Format("Updated {0}: {1} sec.", type, time);
            ScrollListBox();
            updating = false;
        }

        private void FlexPivotEngine_CancelUpdating(object sender, EventArgs e)
        {
            string time = DateTime.Now.Subtract(updateTime).TotalSeconds.ToString("n2");
            int idx = lstBoxLogs.Items.Count - 1;
            lstBoxLogs.Items[idx] = string.Format("Updating {0} canceled: {1} sec.", GetDataType(), time);
            ScrollListBox();
        }

        private void FlexPivotEngine_StartUpdating(object sender, EventArgs e)
        {
            string type = GetDataType();
            if (type == null)
                return;

            updateTime = DateTime.Now;
            lstBoxLogs.Items.Add(string.Format("Updating {0}...", type));
            ScrollListBox();
        }

        private void ScrollListBox()
        {
            lstBoxLogs.Items.MoveCurrentToLast();
            lstBoxLogs.ScrollIntoView(lstBoxLogs.Items.CurrentItem);
        }

        private void DataTable_Click(object sender, RoutedEventArgs e)
        {
            if (updating)
                return;

            // add data
            int count = StartAddingRows(sender);
            var dt = GetDataTable();
            AddToDataTable(dt, count);
            EndAddingRows(count);

            // set data
            flexPivotPage.FlexPivotPanel.C1PivotEngine.BeginUpdate();
            flexPivotPage.DataSource = dt.DefaultView;
            OnSetData();
            flexPivotPage.FlexPivotPanel.C1PivotEngine.EndUpdate();
        }

        private void DataEngineTable_Click(object sender, RoutedEventArgs e)
        {
            if (updating)
                return;

            // add data to DataEngine
            int count = StartAddingRows(sender);
            DataTable dt = GetDataTable();
            AddToEngineTable(dt, count);
            EndAddingRows(count);

            // connect to DataEngine filled with data
            var fPanel = flexPivotPage.FlexPivotPanel;
            fPanel.C1PivotEngine.BeginUpdate();
            fPanel.ConnectDataEngine(dt.TableName);
            OnSetData();
            fPanel.C1PivotEngine.EndUpdate();
        }

        // initialize or restore pivot view
        void OnSetData()
        {
            if (firstLoad)
            {
                var fpEngine = flexPivotPage.C1PivotEngine;
                // set initial view
                fpEngine.RowFields.Add("Country");
                fpEngine.ColumnFields.Add("Category");
                fpEngine.ValueFields.Add("Sales");
            }
            else
            {
                // restore the view
                using (System.Xml.XmlReader xr = System.Xml.XmlReader.Create(stream))
                {
                    flexPivotPage.ReadXml(xr);
                }
                stream.Dispose();
                stream = null;
            }
            firstLoad = false;
        }

        // get current connected table type
        string GetDataType()
        {
            if (flexPivotPage.DataSource != null)
                return flexPivotPage.DataSource.GetType().FullName;
            else if (flexPivotPage.C1PivotEngine.Fields.Count != 0)
                return "C1.DataEngine.Table";
            else
                return null;
        }

        // start logging adding rows
        int StartAddingRows(object sender)
        {
            updating = true;            

            // save current view
            stream = new MemoryStream();
            using (System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(stream))
            {
                flexPivotPage.C1PivotEngine.WriteXml(xw);
                xw.Flush();
                stream.Position = 0;
            }

            // check selected button
            sel = ((RadioButton)sender);

            // disconnect from data
            var fPanel = flexPivotPage.FlexPivotPanel;
            fPanel.DataSource = null;
            fPanel.ConnectDataEngine(null);

            // start log
            int count = int.Parse(sel.Tag.ToString());
            addTime = DateTime.Now;
            lstBoxLogs.Items.Add(string.Format("Creating {0} rows...", count.ToString("n0")));
            ScrollListBox();
            return count;
        }

        // end logging adding rows
        void EndAddingRows(int count)
        {
            string time = DateTime.Now.Subtract(addTime).TotalSeconds.ToString("n2");
            int idx = lstBoxLogs.Items.Count - 1;
            lstBoxLogs.Items[idx] = string.Format("Created {0} rows: {1} sec.", count.ToString("n0"), time);
            ScrollListBox();
        }

        // create DataTable
        DataTable GetDataTable()
        {
            var dt = new DataTable("NorthWind Sales Data");
            dt.ReadXml("Resources/dataengine.xml");
            return dt;
        }

        // generate new rows in DataTable
        void AddToDataTable(DataTable table, int count)
        {
            // remove column protection
            foreach (DataColumn col in table.Columns)
                col.ReadOnly = false;

            // read data from first rows
            int c = table.Rows.Count;
            int max = Math.Min(c, 5000);
            object[] rd = new object[max];
            for (int i = 0; i < max; i++)
                rd[i] = table.Rows[i].ItemArray;

            // add data to the end of the table
            for (int i = 0; i < count; i += max)
            {
                for (int k = 0; k < max && (k + i) < count; k++)
                {
                    DataRow r = table.NewRow();
                    r.ItemArray = (object[])rd[k];
                    table.Rows.Add(r);
                }
            }
        }

        // generate new rows in the C1 DataEngine table
        void AddToEngineTable(DataTable table, int count)
        {
            // clean previously generated data
            flexPivotPage.FlexPivotPanel.Workspace.Clear();

            AddToDataTable(table, count);
            C1.DataEngine.DbConnector.GetData(flexPivotPage.FlexPivotPanel.Workspace, table.CreateDataReader(), table.TableName);
        }

        // get sample DB connection string
        static string GetConnectionString()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server Local DB\Installed Versions");
            if (key == null)
            {
                throw new Exception("Microsoft SQL Server LocalDB is not installed.");
            }
            var version = "v11.0";
            foreach (var subKeyName in key.GetSubKeyNames())
            {
                double localDBVersion = 0;
                if (double.TryParse(subKeyName, NumberStyles.Number, CultureInfo.InvariantCulture, out localDBVersion) && localDBVersion >= 12)
                {
                    version = "MSSQLLocalDB";
                    break;
                }
            }
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ComponentOne Samples\Common";
            return string.Format(@"Data Source=(LocalDB)\{0};AttachDbFilename={1}\NORTHWND.MDF;Integrated Security=True;Connect Timeout=30", version, path);
        }
    }
}
