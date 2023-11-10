using C1.WPF.Grid;
using FlexGridExplorer.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows.Controls;

namespace FlexGridExplorer
{
    /// <summary>
    /// Interaction logic for ColumnLayout.xaml
    /// </summary>
    public partial class ColumnLayout : UserControl
    {
        private string FILENAME = "ColumnLayout.json";

        public ColumnLayout()
        {
            InitializeComponent();
            Tag = AppResources.ColumnLayoutDescription;

            InitializeColumnLayout();
        }

        private void OnEditColumnLayout(object sender, EventArgs e)
        {
            new ColumnLayoutForm(this.grid).ShowDialog();
        }

        private void OnSerializeColumnLayout(object sender, EventArgs e)
        {
            var serializedColumns = SerializeLayout(grid);
            File.WriteAllText(FILENAME, serializedColumns);
        }

        private void InitializeColumnLayout()
        {
            string data = null;
            if (File.Exists(FILENAME))
            {
                data = File.ReadAllText(FILENAME);
            }

            var items = Customer.GetCustomerList(100);
            grid.ItemsSource = items;
            grid.MinColumnWidth = 85;

            if (!string.IsNullOrWhiteSpace(data))
                DeserializeLayout(grid, data);
        }

        private string SerializeLayout(FlexGrid grid)
        {
            var columns = new List<ColumnInfo>();
            foreach (var col in grid.Columns)
            {
                var colInfo = new ColumnInfo { Name = col.ColumnName, IsVisible = col.IsVisible };
                columns.Add(colInfo);
            }
            var serializer = new DataContractJsonSerializer(typeof(ColumnInfo[]));
            var stream = new MemoryStream();
            serializer.WriteObject(stream, columns.ToArray());
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);
            var serializedColumns = new StreamReader(stream).ReadToEnd();
            return serializedColumns;
        }

        private void DeserializeLayout(FlexGrid grid, string layout)
        {
            var serializer = new DataContractJsonSerializer(typeof(ColumnInfo[]));
            var stream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(layout));
            var columns = serializer.ReadObject(stream) as ColumnInfo[];
            for (int i = 0; i < columns.Length; i++)
            {
                var colInfo = columns[i];
                var column = grid.Columns[colInfo.Name];
                var colIndex = grid.Columns.IndexOf(column);
                column.IsVisible = colInfo.IsVisible;
                if (colIndex != i)
                {
                    grid.Columns.Move(colIndex, i);
                }
            }
        }
    }

    [DataContract]
    public class ColumnInfo
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "isVisible")]
        public bool IsVisible { get; set; }
    }
}
