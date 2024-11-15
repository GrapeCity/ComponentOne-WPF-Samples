using C1.DataCollection;
using C1.DataCollection.EntityFrameworkCore;
using C1.WPF.DataFilter;
using DataFilterExplorer.Resources;
using Microsoft.Data.Sqlite;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataFilterExplorer
{
    public partial class VirtualSource : UserControl
    {
        static Random _rnd = new Random();
        static string[] _firstNames = "Andy|Ben|Charlie|Dan|Ed|Fred|Gil|Herb|Jack|Karl|Larry|Mark|Noah|Oprah|Paul|Quince|Rich|Steve|Ted|Ulrich|Vic|Xavier|Zeb".Split('|');
        static string[] _lastNames = "Ambers|Bishop|Cole|Danson|Evers|Frommer|Griswold|Heath|Jammers|Krause|Lehman|Myers|Neiman|Orsted|Paulson|Quaid|Richards|Stevens|Trask|Ulam".Split('|');

        public VirtualSource()
        {
            InitializeComponent();
            Tag = AppResources.VirtualSourceDescription;
        }

        private async void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            await c1DataFilter.ApplyFilterAsync();
        }

        private async void OnGenerateSource(object sender, RoutedEventArgs e)
        {
            try
            {
                LayoutRoot.IsEnabled = false;
                var db = new PeopleContext();
                #region ** creates testing data
                //await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();
                var count = db.Person.Count();
                if (count == 0)
                {
                    var total = 100_000;
                    message.Visibility = Visibility.Visible;
                    message.Text = string.Format(AppResources.CreatingRecordsMessage, total);
                    await Task.Run(async () =>
                    {
                        for (int i = 0; i < total; i++)
                        {
                            var person = new Person() { FirstName = GetRandomString(_firstNames), LastName = GetRandomString(_lastNames), IsActive = _rnd.NextDouble() < 0.9 };
                            db.Person.Add(person);
                        }
                        await db.SaveChangesAsync();
                    });
                    message.Text = "";
                }
                #endregion
                var data = new C1EntityFrameworkCoreVirtualDataCollection<Person>(db);
                data.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(C1EntityFrameworkCoreVirtualDataCollection<Person>.IsLoading))
                        message.Text = data.IsLoading ? AppResources.LoadingMessage : "";
                };
                await data.LoadAsync(0, 0);
                c1DataFilter.ItemsSource = data;
                flexGrid.ItemsSource = data;
            }
            catch (SqliteException) { throw; }
            finally
            {
                LayoutRoot.IsEnabled = true;
            }

        }

        static string GetRandomString(string[] arr)
        {
            return arr[_rnd.Next(arr.Length)];
        }

        private void FlexGridOnColumnFilterLoading(object sender, C1.WPF.Grid.GridColumnFilterLoadingEventArgs e)
        {
            // Sample how to change the initial filter operation for column's conditional filters.
            foreach (var filter in e.DataFilter.Filters)
            {
                if (filter is TextFilter textFilter)
                {
                    textFilter.DefaultFilterOperation = FilterOperation.StartsWith;
                }
            }
        }
    }
}
