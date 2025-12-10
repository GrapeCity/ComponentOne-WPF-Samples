using C1.WPF.Maps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace MapsExplorer
{
    public partial class Factories : UserControl
    {
        const double minStoreZoom = 10.5;
        DataBase database;


        public Factories()
        {
            InitializeComponent();
            Tag = Properties.Resources.Factories;
            this.Loaded += Factories_Loaded;
        }

        private void Factories_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataBase();
            InitializeMapLayers();
        }

        void LoadDataBase()
        {
            var resource = Application.GetResourceStream(new Uri("/" + new AssemblyName(Assembly.GetExecutingAssembly().FullName).Name + ";component/Resources/database.xml", UriKind.Relative));
            if (resource != null)
            {
                var serializer = new XmlSerializer(typeof(DataBase));
                database = (DataBase)serializer.Deserialize(resource.Stream);
            }
            else
            {
                database = new DataBase();
            }
        }

        void InitializeMapLayers()
        {
            map.Layers.Add(new MapItemsLayer
            {
                ItemsSource = database.Factories,
                ItemTemplate = (DataTemplate)Resources["factoryTemplate"]
            });

            map.Layers.Add(new MapItemsLayer
            {
                ItemsSource = database.Offices,
                ItemTemplate = (DataTemplate)Resources["officeTemplate"]
            });

            int storeSlices = (int)Math.Pow(2, minStoreZoom);
            map.Layers.Add(new MapVirtualLayer
            {
                Slices = {
                    new MapSlice(1, 1, 0),
                    new MapSlice(storeSlices, storeSlices, minStoreZoom)
                },
                MapItemsSource = new LocalStoreSource(database),
                ItemTemplate = (DataTemplate)Resources["storeTemplate"]
            });
        }

        public class LocalStoreSource : IMapVirtualSource
        {
            DataBase _dataBase;

            public LocalStoreSource(DataBase localDataBase)
            {
                _dataBase = localDataBase;
            }

            public void Request(double minZoom, double maxZoom, Point lowerLeft, Point upperRight, Action<ICollection> callback)
            {
                if (minZoom < minStoreZoom)
                    return;

                var stores = new List<Store>();

                foreach (var store in _dataBase.Stores)
                {
                    if (store.Latitude > lowerLeft.Y
                      && store.Longitude > lowerLeft.X
                      && store.Latitude <= upperRight.Y
                      && store.Longitude <= upperRight.X)
                    {
                        stores.Add(store);
                    }
                }

                callback(stores);
            }
        }
    }

    public class Entity
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Point Position { get { return new Point(Longitude, Latitude); } }
    }

    public class Office : Entity
    {
        public string Manager { get; set; }
        public int Stores { get; set; }
    }

    public class Factory : Entity
    {
        public double Capacity { get; set; }
    }

    public class Store : Entity
    {
        public List<ProductSale> Sales { get; set; }
    }

    public class ProductSale
    {
        public Product Product { get; set; }
        public int Sales { get; set; }
    }

    public class Product
    {
        public string Name { get; set; }
    }

    public class DataBase
    {
        public List<Factory> Factories { get; set; }
        public List<Office> Offices { get; set; }
        public List<Store> Stores { get; set; }
        public double OfficeStoreDistance { get; set; }

        public DataBase()
        {
            Factories = new List<Factory>();
            Offices = new List<Office>();
            Stores = new List<Store>();
            OfficeStoreDistance = 100000;
        }
    }
}
