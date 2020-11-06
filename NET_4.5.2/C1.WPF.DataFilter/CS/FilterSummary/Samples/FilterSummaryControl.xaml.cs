using C1.DataCollection;
using C1.DataFilter;
using C1.WPF;
using C1.WPF.DataFilter;
using C1.WPF.Theming;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
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

namespace FilterSummary
{
    /// <summary>
    /// Interaction logic for FilterSummarySample.xaml
    /// </summary>
    public partial class FilterSummarySample : UserControl
    {
        C1DataCollection<Car> data = new C1DataCollection<Car>(GetCars());

        public FilterSummarySample()
        {
            InitializeComponent();

            flexGrid.ItemsSource = data;
            c1DataFilter1.ItemsSource = data;

            foreach (ChecklistFilter filter in c1DataFilter1.Filters.Where(f => f is ChecklistFilter))
                filter.SelectAll();

            cmbTheme.ItemsSource = typeof(C1AvailableThemes).GetEnumValues<C1AvailableThemes>();
            cmbTheme.SelectedItem = C1AvailableThemes.Office2016White;
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

        private void C1DataFilter1_FilterAutoGenerating(object sender, FilterAutoGeneratingEventArgs e)
        {
            foreach (Filter f in c1DataFilter1.Filters)
            {
                if (f.PropertyName == "Brand")
                {
                    var brandFilter = f as ChecklistFilter;
                    brandFilter.FilterSummary.Label = "Models:";
                    brandFilter.FilterSummary.PropertyName = "Brand";
                    brandFilter.FilterSummary.AggregateType = C1.DataFilter.AggregateType.Count;
                }

                if (f.PropertyName == "Model")
                {
                    var modelFilter = f as ChecklistFilter;
                    modelFilter.FilterSummary.AggregateType = C1.DataFilter.AggregateType.Max;
                    modelFilter.FilterSummary.CustomFormat = "C0";
                    modelFilter.FilterSummary.Label = "Max price: ";
                    modelFilter.FilterSummary.PropertyName = "Price";
                }

                if (f.PropertyName == "Price")
                {
                    var modelFilter = f as RangeFilter;
                    modelFilter.Maximum = data.Max(x => (x as Car).Price);
                    modelFilter.Minimum = data.Min(x => (x as Car).Price);
                    modelFilter.Digits = 0;
                }
            }
        }

        private void CbAutoApply_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (c1DataFilter1 != null)
            {
                c1DataFilter1.AutoApply = cbAutoApply.IsChecked == true;
            }
        }

        private async void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await c1DataFilter1.ApplyFilterAsync();
        }

        private void cmbTheme_SelectedItemChanged(object sender, PropertyChangedEventArgs<object> e)
        {
            var theme = C1ThemeFactory.GetTheme((C1AvailableThemes)cmbTheme.SelectedItem);
            C1Theme.ApplyTheme(LayoutRoot, theme);

            var adornerLayer = AdornerLayer.GetAdornerLayer(LayoutRoot);
            if (adornerLayer != null)
            {
                C1Theme.ApplyTheme(adornerLayer, theme);
            }
        }
    }

    public class Car
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }

        [Browsable(false)]
        public int ID { get; set; }
        [Browsable(false)]
        public double Liter { get; set; }
        [Browsable(false)]
        public string TransmissAutomatic { get; set; }
        [Browsable(false)]
        public string Description { get; set; }
        [Browsable(false)]
        public byte[] Picture { get; set; }
    }
}
