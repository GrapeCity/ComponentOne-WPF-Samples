using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using C1.Chart;
using System.Windows.Media;

namespace FlexChartExplorer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TreeMapNodeColor : UserControl
    {

        SolidColorBrush _posBrush = new SolidColorBrush(Color.FromArgb(90, 0, 80, 0));
        SolidColorBrush _negBrush = new SolidColorBrush(Color.FromArgb(120, 255, 0, 0));
        SolidColorBrush _titleBrush = new SolidColorBrush(Color.FromArgb(140, 0, 80, 0));
        SolidColorBrush _borderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        List<int> _maxDepths;

        public TreeMapNodeColor()
        {
            this.InitializeComponent();
            treeMap.DataLabel.Style.Stroke = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            treeMap.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }

        public List<int> MaxDepths
        {
            get
            {
                if (_maxDepths == null)
                    _maxDepths = new List<int>() { 1, 2, 3, 4 };
                return _maxDepths;
            }
        }

        private void treeMap_NodeRendering(object sender, C1.WPF.Chart.RenderNodeEventArgs e)
        {
            var dataItem = e.Item as Item;
            if (dataItem == null)
                return;
            SolidColorBrush fill = null;
            e.Engine.SetStroke(_borderBrush);

            if (e.IsTitle)
                fill = _titleBrush;
            else
                fill = dataItem.CurrentSales >= dataItem.PreviousSales ? _posBrush : _negBrush;
            e.Engine.SetFill(fill);
        }

        Random rnd = new Random();

        int rand()
        {
            return rnd.Next(10, 100);
        }

        public Item[] Data
        {
            get
            {
                var data = new Item[] { new Item {
            Type = "Music",
            Items = new Item [] { new Item {
                Type = "Country",
                Items= new Item [] { new Item {
                    Type= "Classic Country",
                    CurrentSales = rand(),
                    PreviousSales = rand()
                }, new Item {
                    Type= "Cowboy Country",
                    CurrentSales = rand(),
                    PreviousSales = rand()
                 }, new Item {
                    Type= "Outlaw Country",
                    CurrentSales = rand(),
                     PreviousSales = rand()
                 }, new Item {
                    Type= "Western Swing",
                    CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Roadhouse Country",
                    CurrentSales = rand(),PreviousSales = rand()
                 } }
            }, new Item {
                Type= "Rock",
                Items= new Item [] { new Item {
                    Type= "Hard Rock",
                    CurrentSales = rand(),PreviousSales = rand()
                }, new Item {
                    Type= "Blues Rock",
                    CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Funk Rock",
                    CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Rap Rock",
                    CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Guitar Rock",
                    CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Progressive Rock",
                    CurrentSales = rand(),PreviousSales = rand()
                 } }
            }, new Item {
                Type= "Classical",
                Items= new Item [] { new Item {
                    Type= "Symphonies",
                    CurrentSales = rand(),PreviousSales = rand()
                    }, new Item {
                    Type= "Chamber Music",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Soundtracks",
        Items = new Item [] { new Item {
                    Type= "Movie Soundtracks",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Musical Soundtracks",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Jazz",
        Items= new Item [] { new Item {
                    Type= "Smooth Jazz",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Vocal Jazz",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Jazz Fusion",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Swing Jazz",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Cool Jazz",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Traditional Jazz",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Electronic",
        Items = new Item [] { new Item {
                    Type= "Electronica",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Disco",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "House",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      } }
    }, new Item {
            Type= "Video",
      Items = new Item [] { new Item {
                Type= "Movie",
        Items= new Item [] { new Item {
                    Type= "Kid & Family",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Action & Adventure",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Animation",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Comedy",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Drama",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Romance",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "TV",
        Items= new Item [] { new Item {
                    Type= "Science Fiction",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Documentary",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Fantasy",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Military & War",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Horror",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      } }
    }, new Item {
            Type= "Books",
      Items= new Item [] { new Item {
                Type= "Arts & Photography",
        Items= new Item [] { new Item {
                    Type= "Architecture",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Graphic Design",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Drawing",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Photography",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Performing Arts",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Children's Books",
        Items= new Item [] { new Item {
                    Type= "Beginning Readers",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Board Books",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Chapter Books",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Coloring Books",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Picture Books",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Sound Books",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "History",
        Items= new Item [] { new Item {
                    Type= "Ancient",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Medieval",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Renaissance",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Mystery",
        Items= new Item [] { new Item {
                    Type= "Mystery",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Thriller & Suspense",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Romance",
        Items= new Item [] {  new Item {
                    Type= "Action & Adventure",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Holidays",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Romantic Comedy",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Romantic Suspense",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Western",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Historical",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Sci-Fi & Fantasy",
        Items= new Item [] { new Item {
                    Type= "Fantasy",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Gaming",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Science Fiction",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      } }
    }, new Item {
            Type= "Electronics",
      Items= new Item  [] { new Item {
                Type= "Camera",
        Items= new Item [] { new Item {
                    Type= "Digital Cameras",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Film Photography",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Lenses",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Video",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Accessories",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Headphones",
        Items= new Item [] { new Item {
                    Type= "Earbud headphones",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Over-ear headphones",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "On-ear headphones",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Bluetooth headphones",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Noise-cancelling headphones",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Audiophile headphones",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Cell Phones",
        Items= new Item  [] { new Item {
                    Type= "Cell Phones",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Accessories",
          Items= new Item [] { new Item {
                        Type= "Batteries",
            CurrentSales = rand(),PreviousSales = rand()
          }, new Item {
                        Type= "Bluetooth Headsets",
            CurrentSales = rand(),PreviousSales = rand()
                     }, new Item {
                        Type= "Bluetooth Speakers",
            CurrentSales = rand(),PreviousSales = rand()
                     }, new Item {
                        Type= "Chargers",
            CurrentSales = rand(),PreviousSales = rand()
                     }, new Item {
                        Type= "Screen Protectors",
            CurrentSales = rand(),PreviousSales = rand()
                     } }
        } }
      }, new Item {
                Type= "Wearable Technology",
        Items= new Item [] { new Item {
                    Type= "Activity Trackers",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Smart Watches",
          CurrentSales = rand(),PreviousSales = rand()
                 },  new Item {
                    Type= "Sports & GPS Watches",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Virtual Reality Headsets",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Wearable Cameras",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Smart Glasses",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      } }
    }, new Item {
            Type= "Computers & Tablets",
      Items= new Item [] { new Item {
                Type= "Desktops",
        Items= new Item [] { new Item {
                    Type= "All-in-ones",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Minis",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Towers",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Laptops",
        Items = new Item [] { new Item  {
                    Type= "2 in 1 laptops",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Traditional laptops",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      }, new Item {
                Type= "Tablets",
        Items= new Item [] { new Item {
                    Type= "iOS",
          CurrentSales = rand(),PreviousSales = rand()
        }, new Item {
                    Type= "Andriod",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Fire os",
          CurrentSales = rand(),PreviousSales = rand()
                 }, new Item {
                    Type= "Windows",
          CurrentSales = rand(),PreviousSales = rand()
                 } }
      } }
    } };
                return data;
            }
        }
    }

    public class Item
    {
        private double _currentSales, _previousSales;

        public string Type { get; set; }
        public Item[] Items { get; set; }
        public double CurrentSales
        {
            get
            {
                if (Items != null)
                    _currentSales = Items.Sum(x => x.CurrentSales);
                return _currentSales;
            }
            set
            {
                if (Items == null)
                    _currentSales = value;
            }
        }
        public double PreviousSales
        {
            get
            {
                if (Items != null)
                    _previousSales = Items.Sum(x => x.PreviousSales);
                return _previousSales;
            }
            set
            {
                if (Items == null)
                    _previousSales = value;
            }
        }

    }
}