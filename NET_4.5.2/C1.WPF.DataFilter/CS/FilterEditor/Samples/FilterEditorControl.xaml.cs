using C1.DataCollection;
using C1.DataFilter;
using C1.WPF;
using C1.WPF.Theming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Serialization;

namespace FilterEditor
{
    /// <summary>
    /// Interaction logic for FilterSummarySample.xaml
    /// </summary>
    public partial class FilterEditorSample : UserControl
    {
        C1DataCollection<Car> data = new C1DataCollection<Car>(GetCars());
        string pathToXmlFile = Directory.GetCurrentDirectory() + "\\FilterExpression.xml";

        public FilterEditorSample()
        {
            InitializeComponent();

            filterEditor.ItemsSource = data;
            flexGrid.ItemsSource = data;
            InitExpressionAndApply();
            filterEditor.FilterChanged += filterEditor_FilterChanged;
            cmbTheme.ItemsSource = typeof(C1AvailableThemes).GetEnumValues<C1AvailableThemes>();
            cmbTheme.SelectedItem = C1AvailableThemes.Office2016White;
        }

        void cmbTheme_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            var theme = C1ThemeFactory.GetTheme((C1AvailableThemes)cmbTheme.SelectedItem);
            C1Theme.ApplyTheme(LayoutRoot, theme);

            var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
            if (adornerLayer != null)
            {
                C1Theme.ApplyTheme(adornerLayer, theme);
            }
        }

        public static IEnumerable<Car> GetCars()
        {
            var carsTable = GetDataTableCars();
            foreach (DataRow row in carsTable.Rows)
            {
                yield return new Car
                {
                    Brand = row.Field<string>("Brand"),
                    Category = row.Field<string>("Category"),
                    Description = row.Field<string>("Description"),
                    Liter = row.Field<double>("Liter"),
                    Model = row.Field<string>("Model"),
                    Picture = row.Field<byte[]>("Picture"),
                    Price = row.Field<double>("Price"),
                    TransmissAutomatic = row.Field<string>("TransmissAutomatic"),
                    ID = row.Field<int>("ID")
                };
            }
        }

        static DataTable GetDataTableCars()
        {
            string rs = "select * from Cars;";
            string cn = GetConnectionString();
            OleDbDataAdapter da = new OleDbDataAdapter(rs, cn);
            DataTable dt = new DataTable("Cars");
            da.Fill(dt);
            return dt;
        }

        static string GetConnectionString()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + @"\ComponentOne Samples\Common";
            string conn = @"provider=microsoft.jet.oledb.4.0;data source={0}\c1nwind.mdb;";
            return string.Format(conn, path);
        }

        async void InitExpressionAndApply()
        {
            CombinationExpression filterExpression = null;

            if (File.Exists(pathToXmlFile))
            {
                filterExpression = LoadFilterFromFile(pathToXmlFile);
                filterEditor.Expression = filterExpression;
            }

            await filterEditor.ApplyFilterAsync();
        }

        CombinationExpression LoadFilterFromFile(string filePath)
        {
            CombinationExpression filterExpression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Open(filePath, FileMode.Open))
            {
                filterExpression = xmlSerializer.Deserialize(fs) as CombinationExpression;
            }

            return filterExpression;
        }

        private void filterEditor_FilterChanged(object sender, EventArgs e)
        {
            SaveFilterToFile(pathToXmlFile);
        }

        private void SaveFilterToFile(string filePath)
        {
            var expression = filterEditor.Expression;
            var xmlSerializer = new XmlSerializer(typeof(CombinationExpression));
            using (var fs = File.Create(filePath))
            {
                xmlSerializer.Serialize(fs, expression);
            }
        }
    }

    public class Car
    {
        [Browsable(false)]
        public int ID { get; set; }

        public string Brand { get; set; }

        public string Model { get; set; }

        [Browsable(false)]
        public double Liter { get; set; }

        [Browsable(false)]
        public string TransmissAutomatic { get; set; }

        public string Category { get; set; }

        [Browsable(false)]
        public string Description { get; set; }

        [Browsable(false)]
        public byte[] Picture { get; set; }

        public double Price { get; set; }
    }
}
