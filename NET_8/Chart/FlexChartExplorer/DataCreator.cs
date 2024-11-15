using C1.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlexChartExplorer
{
    class DataCreator
    {
        public delegate double MathActionDouble(double num);
        public delegate double MathActionInt(int num);

        public static List<string> CreateSimpleChartTypes()
        {
            return new List<string> { "Column", "Bar", "Line", "Scatter", "LineSymbols", "Area", "Spline", "SplineArea", "SplineSymbols", "Step", "StepSymbols", "StepArea" };
        }

        public static List<string> CreateChartTypesForErrorBar()
        {
            return new List<string> { "Column", "Line", "Scatter", "LineSymbols", "Area", "Spline", "SplineArea", "SplineSymbols", "Step", "StepSymbols", "StepArea" };
        }

        public static List<string> CreateFinancialChartTypes()
        {
            return new List<string> { "Candlestick", "HighLowOpenClose" };
        }

        public static List<string> CreateQuartileCalculations()
        {
            return new List<string> { "InclusiveMedian", "ExclusiveMedian" };
        }

        public static List<string> CreateErrorAmounts()
        {
            return new List<string> { "FixedValue", "Percentage", "StandardDeviation", "StandardError", "Custom" };
        }

        public static List<string> CreateErrorBarDirections()
        {
            return new List<string> { "Both", "Minus", "Plus" };
        }

        public static List<string> CreateErrorBarEndStyles()
        {
            return new List<string> { "Cap", "NoCap" };
        }

        public static List<ClassScore> CreateSchoolScoreData()
        {
            var result = new List<ClassScore>();
            result.Add(new ClassScore() { ClassName = "English", SchoolA = 46, SchoolB = 53, SchoolC = 66 });
            result.Add(new ClassScore() { ClassName = "Physics", SchoolA = 61, SchoolB = 55, SchoolC = 65 });
            result.Add(new ClassScore() { ClassName = "English", SchoolA = 58, SchoolB = 56, SchoolC = 67 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 58, SchoolB = 51, SchoolC = 64 });
            result.Add(new ClassScore() { ClassName = "English", SchoolA = 63, SchoolB = 53, SchoolC = 45 });
            result.Add(new ClassScore() { ClassName = "English", SchoolA = 63, SchoolB = 50, SchoolC = 65 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 60, SchoolB = 45, SchoolC = 67 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 62, SchoolB = 53, SchoolC = 66 });
            result.Add(new ClassScore() { ClassName = "Physics", SchoolA = 63, SchoolB = 54, SchoolC = 64 });
            result.Add(new ClassScore() { ClassName = "English", SchoolA = 63, SchoolB = 52, SchoolC = 67 });
            result.Add(new ClassScore() { ClassName = "Physics", SchoolA = 69, SchoolB = 66, SchoolC = 71 });
            result.Add(new ClassScore() { ClassName = "Physics", SchoolA = 48, SchoolB = 67, SchoolC = 50 });
            result.Add(new ClassScore() { ClassName = "Physics", SchoolA = 54, SchoolB = 50, SchoolC = 56 });
            result.Add(new ClassScore() { ClassName = "Physics", SchoolA = 60, SchoolB = 56, SchoolC = 64 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 71, SchoolB = 65, SchoolC = 50 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 48, SchoolB = 70, SchoolC = 72 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 53, SchoolB = 40, SchoolC = 80 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 60, SchoolB = 56, SchoolC = 67 });
            result.Add(new ClassScore() { ClassName = "Math", SchoolA = 61, SchoolB = 56, SchoolC = 45 });
            result.Add(new ClassScore() { ClassName = "English", SchoolA = 63, SchoolB = 58, SchoolC = 64 });
            result.Add(new ClassScore() { ClassName = "Physics", SchoolA = 59, SchoolB = 54, SchoolC = 65 });

            return result;
        }


        public static List<DataPoint> Create(MathActionDouble function, double from, double to, double step)
        {
            var result = new List<DataPoint>();
            var count = (to - from) / step;

            for (double r = from; r < to; r += step)
            {
                result.Add(new DataPoint()
                {
                    XVals = r,
                    YVals = function(r)
                });
            }
            return result;
        }

        public static List<DataPoint> Create(MathActionInt function, int from, int to, int step)
        {
            var result = new List<DataPoint>();
            var count = (to - from) / step;

            for (int r = from; r < to; r += step)
            {
                result.Add(new DataPoint()
                {
                    XVals = r,
                    YVals = function(r)
                });
            }
            return result;
        }

        public static List<DataPoint> Create(MathActionDouble functionX, MathActionDouble functionY, int ptsCount)
        {
            var result = new List<DataPoint>();

            for (double i = 0; i < ptsCount; i++)
            {
                result.Add(new DataPoint()
                {
                    XVals = functionX(i),
                    YVals = functionY(i)
                });
            }
            return result;
        }

        public static List<DataPoint> Create(string[] coef, int ptsCount)
        {
            var result = new List<DataPoint>();

            double[] x = new double[ptsCount];
            double[] y = new double[ptsCount];
            Random rnd = new Random();
            double[] c = StringToCoeff(coef[rnd.Next(coef.Length)]);

            for (int i = 1; i < ptsCount; i++)
            {
                DataPoint pt = Iterate(x[i - 1], y[i - 1], c);
                result.Add(pt);
                x[i] = pt.XVals; y[i] = pt.YVals;
            }

            return result;
        }

        public static List<FruitDataItem> CreateFruit()
        {
            var fruits = new string[] { "Oranges", "Apples", "Pears", "Bananas" };
            var count = fruits.Length;
            var result = new List<FruitDataItem>();
            var rnd = new Random();
            for (var i = 0; i < count; i++)
                result.Add(new FruitDataItem()
                {
                    Fruit = fruits[i],
                    March = rnd.Next(20),
                    April = rnd.Next(20),
                    May = rnd.Next(20),
                });
            return result;
        }

        public static List<FruitsDataItem> CreateFruits()
        {
            var fruits = new string[] { "Oranges", "Apples", "Pears", "Bananas" };
            var count = fruits.Length;
            var result = new List<FruitsDataItem>();
            var rnd = new Random();
            for (var i = 0; i < count; i++)
                result.Add(new FruitsDataItem()
                {
                    Fruit = fruits[i],
                    March = 1 + rnd.Next(20),
                    April = 1 + rnd.Next(20),
                    May = 1 + rnd.Next(20),
                    June = 1 + rnd.Next(20),
                    July = 1 + rnd.Next(20),
                });
            return result;
        }

        public static List<WaterfallItem> CreateWaterfallData()
        {
            Random rnd = new Random();
            List<WaterfallItem> items = new List<WaterfallItem>();
            string[] names = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            foreach (var name in names)
            {
                items.Add(new WaterfallItem()
                {
                    Name = name,
                    Value = Math.Round((0.5 - rnd.NextDouble()) * 1000)
                });
            }
            return items;
        }


        public static PlotAreasSampleDataItem[] CreateForPlotAreas()
        {
            var data = new PlotAreasSampleDataItem[20];
            for (int i = 0; i < 20; i++)
            {
                var ac = i;
                var ve = ac * i;
                var di = 0.5 * ac * i * i;
                data[i] = new PlotAreasSampleDataItem()
                {
                    Acceleration = ac,
                    Velocity = ve,
                    Distance = di,
                    Time = i
                };
            }

            return data;
        }

        public static double Sinus(double x)
        {
            return Math.Sin(x);
        }

        public static double Test1(double x)
        {
            return (Math.Pow(-x, 2 / 3) + Math.Sqrt(Math.Pow(x, 4 / 3) - 4 * Math.Pow(x, 2) + 4)) / 2;
        }

        static DataPoint Iterate(double x, double y, double[] c)
        {
            double x1 = c[0] + c[1] * x + c[2] * x * x + c[3] * x * y + c[4] * y + c[5] * y * y;
            double y1 = c[6] + c[7] * x + c[8] * x * x + c[9] * x * y + c[10] * y + c[11] * y * y;

            return new DataPoint() { XVals = x1, YVals = y1 };
        }

        static double[] StringToCoeff(string s)
        {
            int len = s.Length;
            double[] c = new double[len];

            for (int i = 0; i < len; i++)
            {
                c[i] = -1.2 + 0.1 * (s[i] - 'A');
            }

            return c;
        }

        public static Sale[] CreateSales()
        {
            var countries = new string[] { "US", "Germany", "UK", "Japan", "Italy", "Greece", "China", "France", "Russia" };
            var count = countries.Length;
            var result = new Sale[count];
            var rnd = new Random();
            for (int i = 0; i < count; i++)
            {
                result[i] = new Sale()
                {
                    Country = countries[i],
                    Sales = rnd.Next(60)
                };
            }

            return result;
        }

        public static object[] CreateHierarchicalData()
        {
            var data = new object[] {
             new {
              type = "Music",
               items = new [] {
                new {
                 type = "Country",
                  items = new [] {
                   new {
                    type = "Classic Country",
                     sales = rand()
                   }, new {
                    type = "Cowboy Country",
                     sales = rand()
                   }, new {
                    type = "Outlaw Country",
                     sales = rand()
                   }, new {
                    type = "Western Swing",
                     sales = rand()
                   }, new {
                    type = "Roadhouse Country",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Rock",
                  items = new [] {
                   new {
                    type = "Hard Rock",
                     sales = rand()
                   }, new {
                    type = "Blues Rock",
                     sales = rand()
                   }, new {
                    type = "Funk Rock",
                     sales = rand()
                   }, new {
                    type = "Rap Rock",
                     sales = rand()
                   }, new {
                    type = "Guitar Rock",
                     sales = rand()
                   }, new {
                    type = "Progressive Rock",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Classical",
                  items = new [] {
                   new {
                    type = "Symphonies",
                     sales = rand()
                   }, new {
                    type = "Chamber Music",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Soundtracks",
                  items = new [] {
                   new {
                    type = "Movie Soundtracks",
                     sales = rand()
                   }, new {
                    type = "Musical Soundtracks",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Jazz",
                  items = new [] {
                   new {
                    type = "Smooth Jazz",
                     sales = rand()
                   }, new {
                    type = "Vocal Jazz",
                     sales = rand()
                   }, new {
                    type = "Jazz Fusion",
                     sales = rand()
                   }, new {
                    type = "Swing Jazz",
                     sales = rand()
                   }, new {
                    type = "Cool Jazz",
                     sales = rand()
                   }, new {
                    type = "Traditional Jazz",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Electronic",
                  items = new [] {
                   new {
                    type = "Electronica",
                     sales = rand()
                   }, new {
                    type = "Disco",
                     sales = rand()
                   }, new {
                    type = "House",
                     sales = rand()
                   }
                  }
                }
               }
             }, new {
              type = "Video",
               items = new [] {
                new {
                 type = "Movie",
                  items = new [] {
                   new {
                    type = "Kid & Family",
                     sales = rand()
                   }, new {
                    type = "Action & Adventure",
                     sales = rand()
                   }, new {
                    type = "Animation",
                     sales = rand()
                   }, new {
                    type = "Comedy",
                     sales = rand()
                   }, new {
                    type = "Drama",
                     sales = rand()
                   }, new {
                    type = "Romance",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "TV",
                  items = new [] {
                   new {
                    type = "Science Fiction",
                     sales = rand()
                   }, new {
                    type = "Documentary",
                     sales = rand()
                   }, new {
                    type = "Fantasy",
                     sales = rand()
                   }, new {
                    type = "Military & War",
                     sales = rand()
                   }, new {
                    type = "Horror",
                     sales = rand()
                   }
                  }
                }
               }
             }, new {
              type = "Books",
               items = new [] {
                new {
                 type = "Arts & Photography",
                  items = new [] {
                   new {
                    type = "Architecture",
                     sales = rand()
                   }, new {
                    type = "Graphic Design",
                     sales = rand()
                   }, new {
                    type = "Drawing",
                     sales = rand()
                   }, new {
                    type = "Photography",
                     sales = rand()
                   }, new {
                    type = "Performing Arts",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Children's Books",
                  items = new [] {
                   new {
                    type = "Beginning Readers",
                     sales = rand()
                   }, new {
                    type = "Board Books",
                     sales = rand()
                   }, new {
                    type = "Chapter Books",
                     sales = rand()
                   }, new {
                    type = "Coloring Books",
                     sales = rand()
                   }, new {
                    type = "Picture Books",
                     sales = rand()
                   }, new {
                    type = "Sound Books",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "History",
                  items = new [] {
                   new {
                    type = "Ancient",
                     sales = rand()
                   }, new {
                    type = "Medieval",
                     sales = rand()
                   }, new {
                    type = "Renaissance",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Mystery",
                  items = new [] {
                   new {
                    type = "Mystery",
                     sales = rand()
                   }, new {
                    type = "Thriller & Suspense",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Romance",
                  items = new [] {
                   new {
                    type = "Action & Adventure",
                     sales = rand()
                   }, new {
                    type = "Holidays",
                     sales = rand()
                   }, new {
                    type = "Romantic Comedy",
                     sales = rand()
                   }, new {
                    type = "Romantic Suspense",
                     sales = rand()
                   }, new {
                    type = "Western",
                     sales = rand()
                   }, new {
                    type = "Historical",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Sci-Fi & Fantasy",
                  items = new [] {
                   new {
                    type = "Fantasy",
                     sales = rand()
                   }, new {
                    type = "Gaming",
                     sales = rand()
                   }, new {
                    type = "Science Fiction",
                     sales = rand()
                   }
                  }
                }
               }
             }, new {
              type = "Electronics",
               items = new object[] {
                new {
                 type = "Camera",
                  items = new [] {
                   new {
                    type = "Digital Cameras",
                     sales = rand()
                   }, new {
                    type = "Film Photography",
                     sales = rand()
                   }, new {
                    type = "Lenses",
                     sales = rand()
                   }, new {
                    type = "Video",
                     sales = rand()
                   }, new {
                    type = "Accessories",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Headphones",
                  items = new [] {
                   new {
                    type = "Earbud headphones",
                     sales = rand()
                   }, new {
                    type = "Over-ear headphones",
                     sales = rand()
                   }, new {
                    type = "On-ear headphones",
                     sales = rand()
                   }, new {
                    type = "Bluetooth headphones",
                     sales = rand()
                   }, new {
                    type = "Noise-cancelling headphones",
                     sales = rand()
                   }, new {
                    type = "Audiophile headphones",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Cell Phones",
                  items = new object[] {
                   new {
                    type = "Cell Phones",
                     sales = rand()
                   }, new {
                    type = "Accessories",
                     items = new [] {
                      new {
                       type = "Batteries",
                        sales = rand()
                      }, new {
                       type = "Bluetooth Headsets",
                        sales = rand()
                      }, new {
                       type = "Bluetooth Speakers",
                        sales = rand()
                      }, new {
                       type = "Chargers",
                        sales = rand()
                      }, new {
                       type = "Screen Protectors",
                        sales = rand()
                      }
                     }
                   }
                  }
                }, new {
                 type = "Wearable Technology",
                  items = new [] {
                   new {
                    type = "Activity Trackers",
                     sales = rand()
                   }, new {
                    type = "Smart Watches",
                     sales = rand()
                   }, new {
                    type = "Sports & GPS Watches",
                     sales = rand()
                   }, new {
                    type = "Virtual Reality Headsets",
                     sales = rand()
                   }, new {
                    type = "Wearable Cameras",
                     sales = rand()
                   }, new {
                    type = "Smart Glasses",
                     sales = rand()
                   }
                  }
                }
               }
             }, new {
              type = "Computers & Tablets",
               items = new [] {
                new {
                 type = "Desktops",
                  items = new [] {
                   new {
                    type = "All-in-ones",
                     sales = rand()
                   }, new {
                    type = "Minis",
                     sales = rand()
                   }, new {
                    type = "Towers",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Laptops",
                  items = new [] {
                   new {
                    type = "2 in 1 laptops",
                     sales = rand()
                   }, new {
                    type = "Traditional laptops",
                     sales = rand()
                   }
                  }
                }, new {
                 type = "Tablets",
                  items = new [] {
                   new {
                    type = "iOS",
                     sales = rand()
                   }, new {
                    type = "Andriod",
                     sales = rand()
                   }, new {
                    type = "Fire os",
                     sales = rand()
                   }, new {
                    type = "Windows",
                     sales = rand()
                   }
                  }
                }
               }
             }
            };
            return data;
        }

        static Random rnd = new Random();

        static int rand()
        {
            return rnd.Next(10, 100);
        }

        private static List<TemperatureDiff> temperatureDiffs;

        public static List<TemperatureDiff> GetTemperatureDifferenceData()
        {
            if (temperatureDiffs == null)
            {
                var list = new List<TemperatureDiff>();
                var asm = Assembly.GetExecutingAssembly();

                using (var stream = asm.GetManifestResourceStream(asm.GetName().Name + ".Resources.tempNY-SF.csv"))
                {
                    using (var sr = new StreamReader(stream))
                    {
                        for (var line = sr.ReadLine(); line != null; line = sr.ReadLine())
                        {
                            if (line.StartsWith("date"))
                                continue;

                            var fields = line.Split(',');
                            if (fields.Length == 3)
                            {
                                list.Add(new TemperatureDiff()
                                {
                                    Date = DateTime.ParseExact(fields[0], "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    Temp1 = double.Parse(fields[1], CultureInfo.InvariantCulture),
                                    Temp2 = double.Parse(fields[2], CultureInfo.InvariantCulture),
                                });
                            }
                        }
                    }
                }

                temperatureDiffs = list;
            }

            return temperatureDiffs;
        }

    }

    public class Sale
    {
        public string Country { get; set; }
        public int Sales { get; set; }
    }

    public class FruitDataItem
    {
        public string Fruit { get; set; }
        public double March { get; set; }
        public double April { get; set; }
        public double May { get; set; }
    }

    public class FruitsDataItem
    {
        public string Fruit { get; set; }
        public double March { get; set; }
        public double April { get; set; }
        public double May { get; set; }
        public double June { get; set; }
        public double July { get; set; }
    }

    public class DataPoint
    {
        public double XVals { get; set; }
        public double YVals { get; set; }
    }

    public class WaterfallItem
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }

    public class ClassScore
    {
        public string ClassName { get; set; }
        public double SchoolA { get; set; }
        public double SchoolB { get; set; }
        public double SchoolC { get; set; }
    }

    public class PlotAreasSampleDataItem
    {
        public double Acceleration { get; set; }
        public double Velocity { get; set; }
        public double Distance { get; set; }
        public int Time { get; set; }
    }

    public class TemperatureDiff
    {
        public DateTime Date { get; set; }
        public double Temp1 { get; set; }
        public double Temp2 { get; set; }
        public double Diff => Temp2 - Temp1;
    }
}
