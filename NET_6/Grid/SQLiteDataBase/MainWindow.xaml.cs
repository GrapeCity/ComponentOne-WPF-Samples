﻿using C1.DataCollection.EntityFrameworkCore;
using C1.WPF.DataFilter;
using C1.WPF.Grid;
using Microsoft.Data.Sqlite;
using SQLiteDataBase.Resources;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SQLiteDataBase
{
    public partial class MainWindow : Window
    {
        static Random _rnd = new Random();
        static string[] _firstNames = "Andy|Ben|Charlie|Dan|Ed|Fred|Gil|Herb|Jack|Karl|Larry|Mark|Noah|Oprah|Paul|Quince|Rich|Steve|Ted|Ulrich|Vic|Xavier|Zeb".Split('|');
        static string[] _lastNames = "Ambers|Bishop|Cole|Danson|Evers|Frommer|Griswold|Heath|Jammers|Krause|Lehman|Myers|Neiman|Orsted|Paulson|Quaid|Richards|Stevens|Trask|Ulam".Split('|');

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        private async void Load()
        {
            try
            {
                var db = new PeopleContext();
                #region ** creates testing data
                //await db.Database.EnsureDeletedAsync();
                await db.Database.EnsureCreatedAsync();
                var count = db.Person.Count();
                if (count == 0)
                {
                    var total = 100_000;
                    message.Text = $"Creating {total} records...";
                    await Task.Delay(100);
                    for (int i = 0; i < total; i++)
                    {
                        var person = new Person() { FirstName = GetRandomString(_firstNames), LastName = GetRandomString(_lastNames) };
                        db.Person.Add(person);
                    }
                    await db.SaveChangesAsync();
                    message.Text = "";
                }
                message.Visibility = Visibility.Collapsed;
                #endregion
                var collection = new C1EntityFrameworkCoreVirtualDataCollection<Person>(db);
                collection.PropertyChanged += (s, e) =>
                  {
                      if (e.PropertyName == nameof(C1EntityFrameworkCoreVirtualDataCollection<Person>.IsLoading))
                          LoadingMessage.Text = collection.IsLoading ? AppResources.LoadingMessage : "";
                  };
                grid.ItemsSource = collection;
            }
            catch (SqliteException) { throw; }
        }

        static string GetRandomString(string[] arr)
        {
            return arr[_rnd.Next(arr.Length)];
        }

        private void OnAutoGeneratingColumn(object sender, GridAutoGeneratingColumnEventArgs e)
        {
            if (e.Property.Name == "FirstName" || e.Property.Name == "LastName")
            {
                e.Column.Width = new GridLength(1, GridUnitType.Star);
                e.Column.FilterLoading += OnFilterLoading;
            }
        }

        private void OnFilterLoading(object sender, GridColumnFilterLoadingEventArgs e)
        {
            e.DataFilter.Filters.Clear();
            e.DataFilter.Filters.Add(new FullTextFilter() { PropertyName = e.Column.Binding, ShowMatchWholeWord = false });
        }
    }

}
