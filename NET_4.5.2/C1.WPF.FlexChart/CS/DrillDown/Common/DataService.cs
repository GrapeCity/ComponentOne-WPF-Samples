using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace DrillDown
{
    public class DataService
    {
        Random rnd = new Random();
        static DataService _default;
        static IEnumerable<Item> _rawData;

        private DataService()
        {

        }

        public static DataService Instance
        {
            get
            {
                if (_default == null)
                {
                    _default = new DataService();
                }

                return _default;
            }
        }

        public List<DataItem> GetSunburstData()
        {
            var list = new List<DataItem>();
            List<string> years = new List<string>();
            List<List<string>> times = new List<List<string>>()
            {
                new List<string>() { "Jan", "Feb", "Mar"},
                new List<string>() { "Apr", "May", "June"},
                new List<string>() { "Jul", "Aug", "Sep"},
                new List<string>() { "Oct", "Nov", "Dec" }
            };

            var yearLen = Math.Max((int)Math.Round(Math.Abs(5 - Instance.rnd.NextDouble() * 10)), 3);
            int currentYear = DateTime.Now.Year;
            for (int i = yearLen; i > 0; i--)
            {
                years.Add((currentYear - i).ToString());
            }
            var quarterAdded = false;

            years.ForEach(y =>
            {
                var i = years.IndexOf(y);
                var addQuarter = Instance.rnd.NextDouble() > 0.5;
                if (!quarterAdded && i == years.Count - 1)
                {
                    addQuarter = true;
                }
                var year = new DataItem() { Year = y };
                if (addQuarter)
                {
                    quarterAdded = true;
                    times.ForEach(q =>
                    {
                        var addMonth = Instance.rnd.NextDouble() > 0.5;
                        int idx = times.IndexOf(q);
                        var quar = "Q" + (idx + 1);
                        var quarters = new DataItem() { Year = y, Quarter = quar };
                        if (addMonth)
                        {
                            q.ForEach(m =>
                            {
                                quarters.Items.Add(new DataItem()
                                {
                                    Year = y,
                                    Quarter = quar,
                                    Month = m,
                                    Value = rnd.Next(20, 30)
                                });
                            });
                        }
                        else
                        {
                            quarters.Value = rnd.Next(80, 100);
                        }
                        year.Items.Add(quarters);
                    });
                }
                else
                {
                    year.Value = rnd.Next(80, 100);
                }
                list.Add(year);
            });

            return list;
        }

        public List<Item> GetData(int count)
        {
            List<Item> data = new List<Item>();
            var countries = "US,Germany,UK,Japan,Italy,Greece".Split(',');
            var years = new int[] { 2010, 2011, 2012, 2013, 2014 };
            var countriesLen = countries.Length;
            var citiesByCountry = new Dictionary<string, string[]>();
            citiesByCountry.Add("US", new string[] { "New York", "Los Angeles", "Chicago", "Phoenix", "Dallas" });
            citiesByCountry.Add("Germany", new string[] { "Berlin", "Hamburg", "Munich", "Cologne", "Frankfurt" });
            citiesByCountry.Add("UK", new string[] { "London", "Birmingham", "Leeds", "Glasgow", "Sheffield" });
            citiesByCountry.Add("Japan", new string[] { "Tokyo", "Kanagawa", "Osaka", "Aichi", "Hokkaido" });
            citiesByCountry.Add("Italy", new string[] { "Rome", "Milan", "Naples", "Turin", "Palermo" });
            citiesByCountry.Add("Greece", new string[] { "Athens", "Thessaloniki", "Patras", "Heraklion", "Larissa" });
            for (int i = 0; i < count; i++)
            {
                var yearIndex = rnd.Next(0, 5);
                var cityIndex = rnd.Next(0, 5);
                int countryIndex = i % countriesLen;
                var country = countries[countryIndex];
                data.Add(new Item()
                {
                    ID = i,
                    Country = country,
                    City = citiesByCountry[country][cityIndex],
                    Year = years[yearIndex].ToString(),
                    Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(i % 12 + 1),
                    Day = (i % 27 + 1),
                    Amount = Math.Floor(rnd.NextDouble() * 10000)
                });
            }

            return data;
        }

        public IEnumerable<FlexPoint> GetGroupedData(string currentPath, object currentPathValue = null, string newPath = null)
        {
            if (_rawData == null)
            {
                _rawData = Instance.GetData(5000);
            }

            var initialDataSource = _rawData;
            var summarizedData = new List<FlexPoint>();
            if (currentPathValue != null && newPath != null)
            {
                Thread.Sleep(5000);

                summarizedData = initialDataSource
                     .Where(p => Instance.GetProperty(p, currentPath).Equals(currentPathValue))
                     .GroupBy(p1 => Instance.GetProperty(p1, newPath))
                     .Select(p2 =>
                        new FlexPoint
                        {
                            XValue = p2.Key.ToString(),
                            YValue = p2.Sum(k => k.Amount)
                        }).OrderBy(x => x.XValue).ToList();
                if (currentPath == "Year")
                {
                    var mNames = DateTimeFormatInfo.CurrentInfo.MonthNames.ToList();
                    summarizedData = summarizedData.OrderBy(x => mNames.IndexOf(x.XValue)).ToList();
                }
                if (currentPath == "Month")
                {
                    summarizedData = summarizedData.OrderBy(x => Int32.Parse(x.XValue)).ToList();
                }
            }
            else
            {
                summarizedData = initialDataSource
                    .GroupBy(p => Instance.GetProperty(p, currentPath))
                    .Select(p =>
                        new FlexPoint
                        {
                            XValue = p.Key.ToString(),
                            YValue = p.Sum(k => k.Amount)
                        }).OrderBy(x => x.XValue).ToList();
            }
            return summarizedData;
        }

        private object GetProperty(object obj, string path)
        {
            return obj.GetType().GetProperty(path).GetValue(obj,null);
        }


        int Rand()
        {
            return rnd.Next(10, 100);
        }
        public Leaf[] GetTreemapData()
        {
            var data = new Leaf[] {
                new Leaf {  Type = "Music",
                    Items = new [] {
                        new Leaf {  Type = "Country",
                            Items = new [] {
                                new Leaf {  Type= "Classic Country",    Sales= Rand()   },
                                new Leaf {  Type= "Cowboy Country",     Sales= Rand()   },
                                new Leaf {  Type= "Outlaw Country",     Sales= Rand()   },
                                new Leaf {  Type= "Western Swing",      Sales= Rand()   },
                                new Leaf {  Type= "Roadhouse Country",  Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Rock",
                            Items= new [] {
                                new Leaf {  Type= "Hard Rock",          Sales= Rand()   },
                                new Leaf {  Type= "Blues Rock",         Sales= Rand()   },
                                new Leaf {  Type= "Funk Rock",          Sales= Rand()   },
                                new Leaf {  Type= "Rap Rock",           Sales= Rand()   },
                                new Leaf {  Type= "Guitar Rock",        Sales= Rand()   },
                                new Leaf {  Type= "Progressive Rock",   Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Classical",
                            Items= new [] {
                                new Leaf {  Type= "Symphonies",         Sales= Rand()   },
                                new Leaf {  Type= "Chamber Music",      Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Soundtracks",
                            Items = new [] {
                                new Leaf {  Type= "Movie Soundtracks",  Sales= Rand()   },
                                new Leaf {  Type= "Musical Soundtracks",Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Jazz",
                            Items= new [] {
                                new Leaf {  Type= "Smooth Jazz",        Sales= Rand()   },
                                new Leaf {  Type= "Vocal Jazz",         Sales= Rand()   },
                                new Leaf {  Type= "Jazz Fusion",        Sales= Rand()   },
                                new Leaf {  Type= "Swing Jazz",         Sales= Rand()   },
                                new Leaf {  Type= "Cool Jazz",          Sales= Rand()   },
                                new Leaf {  Type= "Traditional Jazz",   Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Electronic",
                            Items = new [] {
                                new Leaf {  Type= "Electronica",        Sales= Rand()   },
                                new Leaf {  Type= "Disco",              Sales= Rand()   },
                                new Leaf {  Type= "House",              Sales= Rand()   }
                            }
                        }
                    }
                },
                new Leaf {  Type= "Video",
                    Items = new [] {
                        new Leaf {  Type= "Movie",
                            Items= new [] {
                                new Leaf {  Type= "Kid & Family",       Sales= Rand()   },
                                new Leaf {  Type= "Action & Adventure", Sales= Rand()   },
                                new Leaf {  Type= "Animation",          Sales= Rand()   },
                                new Leaf {  Type= "Comedy",             Sales= Rand()   },
                                new Leaf {  Type= "Drama",              Sales= Rand()   },
                                new Leaf {  Type= "Romance",            Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "TV",
                            Items= new [] {
                                new Leaf {  Type= "Science Fiction",    Sales= Rand()   },
                                new Leaf {  Type= "Documentary",        Sales= Rand()   },
                                new Leaf {  Type= "Fantasy",            Sales= Rand()   },
                                new Leaf {  Type= "Military & War",     Sales= Rand()   },
                                new Leaf {  Type= "Horror",             Sales= Rand()   }
                            }
                        }
                    }
                },
                new Leaf {  Type= "Books",
                    Items= new [] {
                        new Leaf {  Type= "Arts & Photography",
                            Items= new [] {
                                new Leaf {  Type= "Architecture",       Sales= Rand()   },
                                new Leaf {  Type= "Graphic Design",     Sales= Rand()   },
                                new Leaf {  Type= "Drawing",            Sales= Rand()   },
                                new Leaf {  Type= "Photography",        Sales= Rand()   },
                                new Leaf {  Type= "Performing Arts",    Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Children's Books",
                            Items= new [] {
                                new Leaf {  Type= "Beginning Readers",  Sales= Rand()   },
                                new Leaf {  Type= "Board Books",        Sales= Rand()   },
                                new Leaf {  Type= "Chapter Books",      Sales= Rand()   },
                                new Leaf {  Type= "Coloring Books",     Sales= Rand()   },
                                new Leaf {  Type= "Picture Books",      Sales= Rand()   },
                                new Leaf {  Type= "Sound Books",        Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "History",
                            Items= new [] {
                                new Leaf {  Type= "Ancient",            Sales= Rand()   },
                                new Leaf {  Type= "Medieval",           Sales= Rand()   },
                                new Leaf {  Type= "Renaissance",        Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Mystery",
                            Items= new [] {
                                new Leaf {  Type= "Mystery",            Sales= Rand()   },
                                new Leaf {  Type= "Thriller & Suspense",Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Romance",
                            Items= new [] {
                                new Leaf {  Type= "Action & Adventure", Sales= Rand()   },
                                new Leaf {  Type= "Holidays",           Sales= Rand()   },
                                new Leaf {  Type= "Romantic Comedy",    Sales= Rand()   },
                                new Leaf {  Type= "Romantic Suspense",  Sales= Rand()   },
                                new Leaf {  Type= "Western",            Sales= Rand()   },
                                new Leaf {  Type= "Historical",         Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Sci-Fi & Fantasy",
                            Items= new [] {
                                new Leaf {  Type= "Fantasy",            Sales= Rand()   },
                                new Leaf {  Type= "Gaming",             Sales= Rand()   },
                                new Leaf {  Type= "Science Fiction",    Sales= Rand()   }
                            }
                        }
                    }
                },
                new Leaf {  Type= "Electronics",
                    Items= new Leaf [] {
                        new Leaf {  Type= "Camera",
                            Items= new Leaf[] {
                                new Leaf {  Type= "Digital Cameras",    Sales= Rand()   },
                                new Leaf {  Type= "Film Photography",   Sales= Rand()   },
                                new Leaf {  Type= "Lenses",             Sales= Rand()   },
                                new Leaf {  Type= "Video",              Sales= Rand()   },
                                new Leaf {  Type= "Accessories",        Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Headphones",
                            Items= new Leaf[] {
                                new Leaf {  Type= "Earbud headphones",  Sales= Rand()   },
                                new Leaf {  Type= "Over-ear headphones",Sales= Rand()   },
                                new Leaf {  Type= "On-ear headphones",  Sales= Rand()   },
                                new Leaf {  Type= "Bluetooth headphones",Sales= Rand()  },
                                new Leaf {  Type= "Noise-cancelling headphones",Sales= Rand()   },
                                new Leaf {  Type= "Audiophile headphones",      Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Cell Phones",
                            Items= new Leaf [] {
                                new Leaf {  Type= "Cell Phones",        Sales= Rand()   },
                                new Leaf {  Type= "Accessories",
                                    Items = new Leaf[] {
                                        new Leaf {  Type= "Batteries",          Sales= Rand()   },
                                        new Leaf {  Type= "Bluetooth Headsets", Sales= Rand()   },
                                        new Leaf {  Type= "Bluetooth Speakers", Sales= Rand()   },
                                        new Leaf {  Type= "Chargers",           Sales= Rand()   },
                                        new Leaf {  Type= "Screen Protectors",  Sales= Rand()   }
                                    }
                                }
                            }
                        },
                        new Leaf {  Type= "Wearable Technology",
                            Items= new Leaf[] {
                                new Leaf {  Type= "Activity Trackers",          Sales= Rand()   },
                                new Leaf {  Type= "Smart Watches",              Sales= Rand()   },
                                new Leaf {  Type= "Sports & GPS Watches",       Sales= Rand()   },
                                new Leaf {  Type= "Virtual Reality Headsets",   Sales= Rand()   },
                                new Leaf {  Type= "Wearable Cameras",           Sales= Rand()   },
                                new Leaf {  Type= "Smart Glasses",              Sales= Rand()   }
                            }
                        }
                    }
                },
                new Leaf {  Type= "Computers & Tablets",
                    Items= new [] {
                        new Leaf {  Type= "Desktops",
                            Items= new Leaf[] {
                                new Leaf {  Type= "All-in-ones",    Sales= Rand()   },
                                new Leaf {  Type= "Minis",          Sales= Rand()   },
                                new Leaf {  Type= "Towers",         Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Laptops",
                            Items = new Leaf[] {
                                new Leaf {  Type= "2 in 1 laptops",         Sales= Rand()   },
                                new Leaf {  Type= "Traditional laptops",    Sales= Rand()   }
                            }
                        },
                        new Leaf {  Type= "Tablets",
                            Items= new [] {
                                new Leaf {  Type= "iOS",    Sales= Rand()   },
                                new Leaf {  Type= "Andriod",Sales= Rand()   },
                                new Leaf {  Type= "Fire os",Sales= Rand()   },
                                new Leaf {  Type= "Windows",Sales= Rand()   }
                            }
                        }
                    }
                }
            };
            return data;
        }
    }
}
